using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class GameMain : Game
    {
        private GameState state; // Game state
        public int maxchar = 5; // Character list size
        private TcpClient client = null;
        private Network network = null;
        private GraphicsDeviceManager graphics = null;
        private SpriteBatch spriteBatch = null;
        private Texture2D mainBackground = null;
        private Texture2D loadingBackground = null;
        private Manager manager = null;
        // GUIs
        public GUIMessageDialog messageDialog = null;
        private GUILogin pageLogin = null;
        private GUICharacterSelector pageCharacterSelector = null;
        private GUICharacterCreator pageCharacterCreator = null;
        private GUIGameAttribute guiGameAttribute = null;
        private GUIGameInventory guiGameInventory = null;
        private GUIGameEquipment guiGameEquipment = null;
        private GUIGameChat guiGameChat = null;
        private GUIGameMenu guiGameMenu = null;
        private GUIGameNPC guiGameNPC = null;
        private GUIGameEnvironment guiGameEnvironment = null;
        // Handler
        private GameHandler gameHandler = null; // Game Handler, Handling game when game state start
        private GameCamera gameCamera = null; // Camera using when game state start
        private GameModelBank modelBank = null; // Model bank which is using to get model path by key
        private GameItemBank itemBank = null; // Item bank which is using to get item data by key
        private Configuration config = null; // Configuration parser
        private Dictionary<short, MapEntity> mapEntities = null; // Map entity use when game state start
        private Dictionary<short, GameClassConfig> classConfigs = null; // All Character Class Information
        private GameModelNode charEntity = null; // Character entity for character select/create pages
        private ServerAddressConfig serverConfig = null; // Server configuration using for set where to connecting with server...
        private int SelectingCharacterID = 0;
        private short currentSelectedClassModelID = -1;
        private long startPing = 0;
        //private ShadowMapHandler shadow; It's not work :(
        // Audio system
        private AudioSystem audioSystem;
        // Audio objects for BGM
        private Cue cue;
        // Particle system
        private ParticlePreset particle;
        // Bloom post process
        private BloomComponent bloom;
        // Way to go Effect
        private GameModel wayToGoEffect;
        public GameMain()
        {
            state = GameState.game_login;
            mapEntities = new Dictionary<short, MapEntity>();
            config = new Configuration();
            config.parsing();
            // Load all class
            classConfigs = config.getClassList();
            // Network
            client = new TcpClient();
            network = new Network(client);
            // Graphic
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 1024;
            //graphics.PreferredBackBufferHeight = 768;
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            manager = new Manager(this, graphics, "Default");
            startPing = DateTime.Now.Ticks;
            //shadow = new ShadowMapHandler(Content, graphics);
            // Audio system
            audioSystem = new AudioSystem();
            // Bloom post process
            bloom = new BloomComponent(this);
            Components.Add(bloom);
            this.Exiting += new EventHandler<System.EventArgs>(GameMain_Exiting);
        }

        void GameMain_Exiting(object sender, System.EventArgs e)
        {
            if (cue != null && cue.IsPlaying)
                cue.Stop(AudioStopOptions.Immediate);
            while (gameHandler != null && gameHandler.receiveThread != null && gameHandler.receiveThread.IsAlive)
            {
                if (gameHandler.currentState() == GameState.none || gameHandler.currentState() == GameState.map_warp)
                    gameHandler.currentState(GameState.map_backtologin);
                gameHandler.receiveThread.Abort();
            }
            long startExit = DateTime.Now.Ticks;
            network.Send("LOGOUT:0;");
            bool isLoggedOut = false;
            while (!isLoggedOut && network.isConnected())
            {
                String responseLogoutMsg = "";
                while (responseLogoutMsg.Length <= 0)
                    responseLogoutMsg = network.Receive();
                String[] LogoutLine = responseLogoutMsg.Split(';');
                for (int l = 0; l < LogoutLine.Length; ++l)
                {
                    String[] LogoutMsg = LogoutLine[l].Split(':');
                    if (LogoutMsg[0].Equals("LOGOUT") && LogoutMsg.Length == 2)
                    {
                        isLoggedOut = true;
                        break;
                    }
                }
            }
            network.Close();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            manager.Initialize();
            // Add Login window
            pageLogin = new GUILogin(manager, network, this);
            manager.Add(pageLogin);
            // Add Character selector window
            pageCharacterSelector = new GUICharacterSelector(manager, network, this);
            manager.Add(pageCharacterSelector);
            pageCharacterSelector.Visible = false;
            // Add Character creator window
            pageCharacterCreator = new GUICharacterCreator(manager, network, this, classConfigs);
            manager.Add(pageCharacterCreator);
            pageCharacterCreator.Visible = false;
            // Add Message dialog
            messageDialog = new GUIMessageDialog(manager, "", "");
            manager.Add(messageDialog);
            messageDialog.Visible = false;
            // game gui
            guiGameAttribute = new GUIGameAttribute(manager, network);
            manager.Add(guiGameAttribute);
            guiGameAttribute.Visible = false;
            guiGameInventory = new GUIGameInventory(manager, network, Content, graphics, config.getItemList());
            manager.Add(guiGameInventory);
            guiGameInventory.Visible = false;
            guiGameEquipment = new GUIGameEquipment(manager, network, Content, graphics, config.getItemList());
            manager.Add(guiGameEquipment);
            guiGameEquipment.Visible = false;
            guiGameChat = new GUIGameChat(manager, network);
            manager.Add(guiGameChat);
            guiGameChat.Visible = false;
            guiGameMenu = new GUIGameMenu(manager, network, gameHandler, this);
            manager.Add(guiGameMenu);
            guiGameMenu.Visible = false;
            guiGameNPC = new GUIGameNPC(manager, network, Content, config.getDialogList());
            manager.Add(guiGameNPC);
            guiGameNPC.Visible = false;
            guiGameEnvironment = new GUIGameEnvironment(manager, guiGameAttribute, null, guiGameEquipment, guiGameInventory, guiGameMenu);
            manager.Add(guiGameEnvironment);
            guiGameEnvironment.Visible = false;
            // Init Inventory & Equipment System
            guiGameInventory.init(guiGameEquipment);
            guiGameEquipment.init(guiGameInventory);
            // Shadow map
            //shadow.Initialize();
            // SFX
            audioSystem.Initialize();
            // BGM
            cue = audioSystem.getSoundBank().GetCue("01");
            cue.Play();
            //Debug.WriteLine("Initialized");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Main Background
            mainBackground = Content.Load<Texture2D>("mainbg");
            // Loading Background
            loadingBackground = Content.Load<Texture2D>("loadingbg");
            // TODO: use this.Content to load your game content here
            // Initializing model bank which using for storing model file path
            modelBank = new GameModelBank(Content);
            // Initializing item bank which using for storing item data path
            itemBank = new GameItemBank(Content);
            // Load all model path here.
            Dictionary<short, GameModelConfig> modellist = config.getModelList();
            foreach (short id in modellist.Keys)
            {
                modelBank.Load(id, modellist[id]);
                // Cache
                GameModelNode cache = new GameModelNode(Content, modelBank);
                cache.Load(modelBank.getModelPath(id));
            }
            Dictionary<int, GameItemConfig> itemlist = config.getItemList();
            foreach (short id in itemlist.Keys)
            {
                itemBank.Load(id, itemlist[id]);
            }
            // Load all map
            Dictionary<short, GameMapConfig> maplist = config.getMapList();
            foreach (short id in maplist.Keys)
            {
                mapEntities.Add(id, new MapEntity(id, maplist[id], Content));
            }
            // Server configuration
            serverConfig = config.getServerList()[0];
            // Initializing game camera
            gameCamera = new GameCamera(graphics);
            // Particle system
            particle = new ParticlePreset(this, graphics, gameCamera);
            particle.LoadContent();
            // Game Event handler object
            gameHandler = new GameHandler(manager, network, this, Content, graphics, spriteBatch, modelBank, itemBank, classConfigs, gameCamera, audioSystem, particle);
            // Shadow map
            //shadow.LoadContent(spriteBatch, gameCamera);
            // Bloom post process
            bloom.Settings = BloomSettings.PresetSettings[0];
            // Way to go Effect
            wayToGoEffect = new GameModel(Content);
            wayToGoEffect.Load("waytogo");
            //Debug.WriteLine("LoadedContent");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            // TODO: Add your update logic here
            ping();
            switch (state)
            {
                case GameState.game_login:
                    switch (pageLogin.currentState())
                    {
                        case GameState.login_loggedin:
                            pageLogin.Visible = false;
                            pageCharacterSelector.init(pageLogin.getUserId());
                            pageCharacterSelector.Visible = true;
                            pageLogin.reset();
                            reset();
                            startPing = gameTime.ElapsedGameTime.Milliseconds;
                            state = GameState.game_characterselector;
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.game_characterselector:
                    int selectcharid = pageCharacterSelector.getSelectingCharacterID();
                    if (SelectingCharacterID != selectcharid && selectcharid > 0)
                    {
                        SelectingCharacterID = selectcharid;
                        currentSelectedClassModelID = classConfigs[pageCharacterSelector.getSelectedClassId()].modelID;
                        charEntity = new GameModelNode(Content, modelBank);
                        charEntity.Load(currentSelectedClassModelID);
                        charEntity.Rotation = modelBank.getModelRotation(currentSelectedClassModelID);
                        charEntity.Rotation.Y += 90;
                        charEntity.Position = modelBank.getModelPosition(currentSelectedClassModelID);
                        charEntity.Position.Y -= 7;
                        charEntity.Scale = modelBank.getModelScale(currentSelectedClassModelID);
                        charEntity.playClip((short)GameState.anim_idle, true);
                    }
                    if (selectcharid <= 0)
                    {
                        charEntity = null;
                    }
                    if (charEntity != null && currentSelectedClassModelID > -1)
                    {
                        charEntity.Update(gameTime, Matrix.Identity);
                    }
                    switch (pageCharacterSelector.currentState())
                    {
                        case GameState.character_backtologin:
                            network.Send("LOGOUT:0;");
                            bool isLoggedOut = false;
                            while (!isLoggedOut && network.isConnected())
                            {
                                String responseLogoutMsg = "";
                                while (responseLogoutMsg.Length <= 0)
                                    responseLogoutMsg = network.Receive();
                                String[] LogoutLine = responseLogoutMsg.Split(';');
                                for (int l = 0; l < LogoutLine.Length; ++l)
                                {
                                    String[] LogoutMsg = LogoutLine[l].Split(':');
                                    if (LogoutMsg[0].Equals("LOGOUT") && LogoutMsg.Length == 2)
                                    {
                                        isLoggedOut = true;
                                        break;
                                    }
                                }
                            }
                            network.Close();
                            pageCharacterSelector.Visible = false;
                            pageLogin.Visible = true;
                            pageCharacterSelector.reset();
                            reset();
                            state = GameState.game_login;
                            break;
                        case GameState.character_newcharacter:
                            pageCharacterSelector.Visible = false;
                            pageCharacterCreator.init(pageCharacterSelector.getUserId());
                            pageCharacterCreator.Visible = true;
                            pageCharacterSelector.reset();
                            reset();
                            state = GameState.game_charactercreator;
                            break;
                        case GameState.character_selectedcharacter:
                            pageCharacterSelector.Visible = false;
                            gameHandler.init(pageCharacterSelector.getUserId(), pageCharacterSelector.getSelectedCharInfo());
                            pageCharacterSelector.reset();
                            reset();
                            state = GameState.game_map;
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.game_charactercreator:
                    short charclass_modelid = classConfigs[pageCharacterCreator.getSelectedClassId()].modelID;
                    if (currentSelectedClassModelID != charclass_modelid)
                    {
                        currentSelectedClassModelID = charclass_modelid;
                        charEntity = new GameModelNode(Content, modelBank);
                        charEntity.Load(currentSelectedClassModelID);
                        charEntity.Rotation = modelBank.getModelRotation(currentSelectedClassModelID);
                        charEntity.Rotation.Y += 90;
                        charEntity.Position = modelBank.getModelPosition(currentSelectedClassModelID);
                        charEntity.Position.Y -= 7;
                        charEntity.Scale = modelBank.getModelScale(currentSelectedClassModelID);
                        charEntity.playClip((short)GameState.anim_idle, true);
                    }
                    if (charEntity != null && currentSelectedClassModelID > -1)
                    {
                        charEntity.Update(gameTime, Matrix.Identity);
                    }
                    switch (pageCharacterCreator.currentState())
                    {
                        case GameState.character_newcharacterend:
                            pageCharacterCreator.Visible = false;
                            pageCharacterSelector.init(pageCharacterCreator.getUserId());
                            pageCharacterSelector.Visible = true;
                            pageCharacterCreator.reset();
                            reset();
                            state = GameState.game_characterselector;
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.game_map:
                    switch (gameHandler.currentState())
                    {
                        case GameState.map_warping:
                            gameHandler.initToWarp();
                            break;
                        case GameState.map_warp:
                            cue.Stop(AudioStopOptions.Immediate);
                            gameHandler.warping();
                            short mapid = gameHandler.getCurrentMapID();
                            if (mapid > 0 && mapEntities.ContainsKey(mapid))
                            {
                                cue = audioSystem.getSoundBank().GetCue(mapEntities[mapid].getBGM());
                            }
                            cue.Play();
                            break;
                        case GameState.map_backtologin:
                            network.Send("LOGOUT:0;");
                            bool isLoggedOut = false;
                            while (!isLoggedOut && network.isConnected())
                            {
                                String responseLogoutMsg = "";
                                while (responseLogoutMsg.Length <= 0)
                                    responseLogoutMsg = network.Receive();
                                String[] LogoutLine = responseLogoutMsg.Split(';');
                                for (int l = 0; l < LogoutLine.Length; ++l)
                                {
                                    String[] LogoutMsg = LogoutLine[l].Split(':');
                                    if (LogoutMsg[0].Equals("LOGOUT") && LogoutMsg.Length == 2)
                                    {
                                        isLoggedOut = true;
                                        break;
                                    }
                                }
                            }
                            network.Close();
                            // Unload game connect
                            pageLogin.Visible = true;
                            guiGameAttribute.Visible = false;
                            guiGameChat.Visible = false;
                            guiGameMenu.Visible = false;
                            guiGameNPC.Visible = false;
                            guiGameEnvironment.Visible = false;
                            guiGameInventory.Visible = false;
                            guiGameInventory.clearInventoryData();
                            guiGameEquipment.Visible = false;
                            guiGameEquipment.clearEquipmentData();
                            cue.Stop(AudioStopOptions.Immediate);
                            cue = audioSystem.getSoundBank().GetCue("01");
                            cue.Play();
                            gameHandler.reset();
                            reset();
                            state = GameState.game_login;
                            break;
                        case GameState.none:
                            gameHandler.ProcessKeyboard();
                            gameHandler.Update(gameTime, Matrix.Identity);
                            gameCamera.Update();
                            particle.Update(gameTime, 1);
                            if (IsActive)
                            {
                                gameCamera.ProcessMouse();
                                if (noCursorInsideWindows())
                                    gameHandler.ProcessMouse();
                            }
                            //shadow.Update();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            audioSystem.update();
            base.Update(gameTime);
            manager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                manager.BeginDraw(gameTime);
                bloom.BeginDraw();
                GraphicsDevice.Clear(Color.Black);
                // Draw any thing
                switch (state)
                {
                    case GameState.game_login:
                        spriteBatch.Begin();
                        spriteBatch.Draw(mainBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                        spriteBatch.End();
                        break;
                    case GameState.game_characterselector:
                        spriteBatch.Begin();
                        spriteBatch.Draw(mainBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                        spriteBatch.End();
                        if (charEntity != null)
                        {
                            // reset GraphicDeviceSettings to draw3D meshes
                            GraphicsDevice.BlendState = BlendState.Opaque;
                            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                            charEntity.Draw(Matrix.CreateLookAt(new Vector3(0.0f, 5.0f, 20.0f), Vector3.Zero, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f));
                        }
                        break;
                    case GameState.game_charactercreator:
                        spriteBatch.Begin();
                        spriteBatch.Draw(mainBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                        spriteBatch.End();
                        if (charEntity != null)
                        {
                            // reset GraphicDeviceSettings to draw3D meshes
                            GraphicsDevice.BlendState = BlendState.Opaque;
                            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                            charEntity.Draw(Matrix.CreateLookAt(new Vector3(0.0f, 5.0f, 20.0f), Vector3.Zero, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f));
                        }
                        break;
                    case GameState.game_map:
                        switch (gameHandler.currentState())
                        {
                            case GameState.map_warping:
                                // Loading screen...
                                spriteBatch.Begin();
                                spriteBatch.Draw(loadingBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                                spriteBatch.End();
                                break;
                            case GameState.map_warp:
                                // Loading screen...
                                spriteBatch.Begin();
                                spriteBatch.Draw(loadingBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                                spriteBatch.End();
                                break;
                            case GameState.none:
                                spriteBatch.Begin();
                                // reset GraphicDeviceSettings to draw3D meshes
                                graphics.GraphicsDevice.BlendState = BlendState.Opaque;
                                graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                                short mapid = gameHandler.getCurrentMapID();
                                if (mapid > 0 && mapEntities.ContainsKey(mapid))
                                {
                                    mapEntities[mapid].Draw(gameCamera.getView(), gameCamera.getProjection());
                                    //shadow.setMap(mapEntities[mapid].getModel());
                                    //shadow.Draw(gameTime);
                                }
                                spriteBatch.End();
                                // Drawing 3d Meshes
                                gameHandler.Draw(gameTime);
                                // Drawing particle
                                spriteBatch.Begin();
                                graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                                graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                                particle.Draw(gameTime);
                                spriteBatch.End();
                                // Draw particle additive
                                spriteBatch.Begin();
                                graphics.GraphicsDevice.BlendState = BlendState.Additive;
                                graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                                particle.DrawAdditive(gameTime);
                                spriteBatch.End();
                                break;
                        }
                        break;
                    default:
                        break;
                }
                base.Draw(gameTime);
                manager.EndDraw();
                // Draw selected Inventory Item
                if (guiGameInventory.getSelectedItemIndex() >= 0)
                {
                    guiGameInventory.drawSelectedItemTexturePosition(spriteBatch);
                }
                if (guiGameEquipment.getSelectedItemIndex() >= 0)
                {
                    guiGameEquipment.drawSelectedItemTexturePosition(spriteBatch);
                }
            }
            catch (Exception e)
            {
                manager.EndDraw();
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void hideMessageDialog()
        {
            messageDialog.setTitle("");
            messageDialog.setMessage("");
            messageDialog.Visible = false;
            messageDialog.CloseButtonVisible = false;
        }

        public void reset()
        {
            charEntity = null;
            SelectingCharacterID = 0;
            currentSelectedClassModelID = -1;
        }
        public void ping()
        {
            if (network.isConnected() && (DateTime.Now.Ticks - startPing) / 10000000 >= 5)
            {
                network.Send("PING:0;");
                startPing = DateTime.Now.Ticks;
            }
        }
        public GUIGameAttribute getGuiGameAttribute()
        {
            return guiGameAttribute;
        }
        public GUIGameInventory getGuiGameInventory()
        {
            return guiGameInventory;
        }
        public GUIGameEquipment getGuiGameEquipment()
        {
            return guiGameEquipment;
        }
        public GUIGameChat getGuiGameChat()
        {
            return guiGameChat;
        }
        public GUIGameMenu getGuiGameMenu()
        {
            return guiGameMenu;
        }
        public GUIGameNPC getGuiGameNPC()
        {
            return guiGameNPC;
        }
        public GUIGameEnvironment getGuiGameEnvironment()
        {
            return guiGameEnvironment;
        }
        public ServerAddressConfig getServerConfig()
        {
            return serverConfig;
        }
        public Boolean isCursorInsideWindow(Control c)
        {
            MouseState pos = Mouse.GetState();
            return (c.Visible && (pos.X >= c.AbsoluteLeft) && (pos.X <= c.AbsoluteLeft + c.Width) && (pos.Y >= c.AbsoluteTop) && (pos.Y <= c.AbsoluteTop + c.Height));
        }
        public Boolean noCursorInsideWindows()
        {
            return !isCursorInsideWindow(guiGameAttribute) && !isCursorInsideWindow(guiGameInventory) && !isCursorInsideWindow(guiGameEquipment) && !isCursorInsideWindow(guiGameChat) && !isCursorInsideWindow(guiGameMenu) && !isCursorInsideWindow(guiGameNPC) && !isCursorInsideWindow(guiGameEnvironment);
        }
        public GameModel getWayToGoEffect()
        {
            return wayToGoEffect;
        }
    }
}

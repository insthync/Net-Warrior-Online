using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    public class GameHandler
    {
        private Dictionary<String, UnitEntity> monstersEntity;
        private Dictionary<String, UnitEntity> npcsEntity;
        private Dictionary<String, UnitEntity> playersEntity;
        private Dictionary<String, WarpEntity> warpsEntity;
        private Dictionary<String, ItemEntity> itemsEntity;
        private UnitEntity playerEntity;
        // Keystate
        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;
        // Mousestate
        private MouseState currentMouseState;
        private MouseState prevMouseState;
        // Important Field
        private GameState state = GameState.none;
        private int LoggedUserID = 0;
        private int SelectedCharacterID = 0;
        private short CurrentMapID = 0;
        private Network network = null;
        private GameMain game = null;
        private Manager manager = null;
        private ContentManager content = null;
        private GraphicsDeviceManager graphics = null;
        private GameModelBank modelBank = null;
        private GameItemBank itemBank = null;
        private Dictionary<short, GameClassConfig> classConfigs;
        private GameCamera camera = null;
        public Thread receiveThread = null;
        private UnitEntity targetNode = null;
        private static Vector3 normalTargetPos = new Vector3(-1);
        private Vector3 targetPos = normalTargetPos;
        // Contents
        private AudioSystem audioSystem = null;
        private ParticlePreset particleManager = null;
        private SpriteBatch spriteBatch = null;
        private SpriteFont entityNameFont = null;
        private SpriteFont entityDamageFont = null;
        private Texture2D whiteRect = null;
        // 
        private Boolean npcVisibleBeforeWarp = false;
        public GameHandler(Manager manager, Network network, GameMain game, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameModelBank modelBank, GameItemBank itemBank, Dictionary<short, GameClassConfig> classConfigs, GameCamera camera, AudioSystem audioSystem, ParticlePreset particleManager)
        {
            this.manager = manager;
            this.game = game;
            this.content = content;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            // Bank & Config
            this.modelBank = modelBank;
            this.itemBank = itemBank;
            this.classConfigs = classConfigs;
            // System handler
            this.camera = camera;
            this.network = network;
            // Entities
            this.monstersEntity = new Dictionary<String, UnitEntity>();
            this.npcsEntity = new Dictionary<String, UnitEntity>();
            this.playersEntity = new Dictionary<String, UnitEntity>();
            this.warpsEntity = new Dictionary<String, WarpEntity>();
            this.itemsEntity = new Dictionary<String, ItemEntity>();
            this.playerEntity = null;
            // Effects
            this.audioSystem = audioSystem;
            this.particleManager = particleManager;
            // Load font
            entityNameFont = content.Load<SpriteFont>("Fonts/EntityName");
            entityDamageFont = content.Load<SpriteFont>("Fonts/EntityDamage");
            // White rect
            whiteRect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            whiteRect.SetData(new[] { Color.White });
        }

        public void ProcessKeyboard()
        {
            prevKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            if (prevKeyboardState != null && !game.getGuiGameChat().isFocus())
            {
                if (prevKeyboardState.IsKeyDown(Keys.A) && currentKeyboardState.IsKeyUp(Keys.A))
                    game.getGuiGameAttribute().Visible = !game.getGuiGameAttribute().Visible;
                if (prevKeyboardState.IsKeyDown(Keys.I) && currentKeyboardState.IsKeyUp(Keys.I))
                    game.getGuiGameInventory().Visible = !game.getGuiGameInventory().Visible;
                if (prevKeyboardState.IsKeyDown(Keys.E) && currentKeyboardState.IsKeyUp(Keys.E))
                    game.getGuiGameEquipment().Visible = !game.getGuiGameEquipment().Visible;
                if (prevKeyboardState.IsKeyDown(Keys.X) && currentKeyboardState.IsKeyUp(Keys.X))
                    game.getGuiGameMenu().Visible = !game.getGuiGameMenu().Visible;
            }
        }

        public void ProcessMouse()
        {
            try
            {
                prevMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();
                Vector3 Cursor3D = camera.getCursor3D();
                Ray Cursor3DRay = camera.getCursor3DRay();
                foreach (UnitEntity ent in monstersEntity.Values)
                {
                    if (ent != targetNode)
                    {
                        if (ent != null)
                        {
                            ent.drawHP = false;
                        }
                    }
                    if (ent != null && ent.RayIntersectsModel(Cursor3DRay))
                    {
                        ent.drawName = true;
                        if (prevMouseState != null && prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                        {
                            targetPos = normalTargetPos;
                            targetNode = ent;
                            ent.drawHP = true;
                            network.Send("TARGETMONSTERNODE:" + ent.getID() + ";");
                            return;
                        }
                    }
                    else
                    {
                        if (ent != null)
                        {
                            ent.drawName = false;
                        }
                    }
                }
                foreach (UnitEntity ent in npcsEntity.Values)
                {
                    if (ent != null && ent.RayIntersectsModel(Cursor3DRay))
                    {
                        ent.drawName = true;
                        if (prevMouseState != null && prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                        {
                            targetPos = normalTargetPos;
                            targetNode = ent;
                            network.Send("TARGETNPCNODE:" + ent.getID() + ";");
                            return;
                        }
                    }
                    else
                    {
                        if (ent != null)
                        {
                            ent.drawName = false;
                        }
                    }
                }
                foreach (UnitEntity ent in playersEntity.Values)
                {
                    if (ent != null && ent.RayIntersectsModel(Cursor3DRay))
                    {
                        ent.drawName = true;
                        if (prevMouseState != null && prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                        {
                            targetPos = normalTargetPos;
                            targetNode = ent;
                            network.Send("TARGETPLAYERNODE:" + ent.getID() + ";");
                            return;
                        }
                    }
                    else
                    {
                        if (ent != null)
                        {
                            ent.drawName = false;
                        }
                    }
                }
                foreach (WarpEntity ent in warpsEntity.Values)
                {
                    if (ent != null && ent.RayIntersectsModel(Cursor3DRay))
                    {
                        if (prevMouseState != null && prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                        {
                            targetNode = null;
                            targetPos = ent.getPosition();
                            network.Send("TARGETWARPNODE:" + ent.getID() + ";");
                            return;
                        }
                    }
                }
                foreach (ItemEntity ent in itemsEntity.Values)
                {
                    if (ent != null && ent.RayIntersectsModel(Cursor3DRay))
                    {
                        if (prevMouseState != null && prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                        {
                            targetNode = null;
                            targetPos = ent.getPosition();
                            network.Send("TARGETITEMNODE:" + ent.getID() + ";");
                            return;
                        }
                    }
                }
                if (Cursor3D != null)
                {
                    if (prevMouseState != null && prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                    {
                        targetNode = null;
                        targetPos = new Vector3(Cursor3D.X, 0, Cursor3D.Z);
                        network.Send("TARGETPOSITION:" + Cursor3D.X + " " + Cursor3D.Z + ";");
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void receiveMsg()
        {
            // What event happening, receive from server.
            while (state == GameState.none || state == GameState.map_warp)
            {
                try
                {
                    String responseMsg = network.Receive();
                    //Debug.WriteLine(responseMsg);
                    String[] line = responseMsg.Split(';');
                    for (int i = 0; i < line.Length; ++i)
                    {
                        String[] msg = line[i].Split(':');
                        // Get character info
                        if (msg[0].Equals("CHARINFOSELF") && msg.Length == 2)
                        {
                            // Split message value
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 24)
                            {
                                String statusMsg = value[0];
                                if (statusMsg.Equals("OK"))
                                {
                                    // if status is OK
                                    CharacterInformation charInfo = new CharacterInformation(SelectedCharacterID, value[1]/*name*/,
                                        Convert.ToInt16(value[2])/*class*/, Convert.ToInt16(value[3])/*level*/, Convert.ToInt32(value[4])/*exp*/, Convert.ToInt32(value[5])/*curexp*/, Convert.ToInt32(value[6])/*gold*/,
                                        Convert.ToInt32(value[7])/*hp*/, Convert.ToInt32(value[8])/*cur_hp*/, Convert.ToInt32(value[9])/*sp*/, Convert.ToInt32(value[10])/*cur_sp*/, Convert.ToInt32(value[11])/*stpoint*/, Convert.ToInt32(value[12])/*skpoint*/,
                                        Convert.ToInt16(value[13])/*str*/, Convert.ToInt16(value[14])/*dex*/, Convert.ToInt16(value[15])/*int*/, Convert.ToInt16(value[16])/*hair*/,
                                        Convert.ToInt16(value[17])/*face*/, Convert.ToInt16(value[18])/*curmap*/, Convert.ToSingle(value[19])/*curmapx*/, Convert.ToSingle(value[20])/*curmapy*/,
                                        Convert.ToInt16(value[21])/*savmap*/, Convert.ToSingle(value[22])/*savmapx*/, Convert.ToSingle(value[23])/*savmapy*/);
                                    game.getGuiGameAttribute().setValues(charInfo);
                                    game.getGuiGameEnvironment().setValues(charInfo);
                                    if (charInfo.getChar_curmap() != CurrentMapID)
                                    {
                                        CurrentMapID = charInfo.getChar_curmap();
                                        state = GameState.map_warping;
                                    }
                                }
                            }
                        }
                        if (msg[0].Equals("NPCDIALOG") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 3)
                            {
                                int npcid = Convert.ToInt32(value[0]);
                                String txtmsg = value[1];
                                Dictionary<String, String> txtmenu = new Dictionary<String, String>();
                                String[] menus = value[2].Split(',');
                                for (int idx = 0; idx < menus.Length; ++idx)
                                {
                                    if ((idx + 1) % 2 == 0)
                                    {
                                        txtmenu.Add(menus[idx - 1], menus[idx]);
                                    }
                                }
                                game.getGuiGameNPC().setDialog(npcid, txtmsg, txtmenu);
                            }
                        }
                        // Walk target
                        if (msg[0].Equals("TARGETPOSITION") && msg.Length == 2)
                        {
                            // Split message value
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 2)
                            {
                                float posx = Convert.ToSingle(value[0]);
                                float posy = Convert.ToSingle(value[1]);
                            }
                        }
                        // Action target
                        if (msg[0].Equals("TARGETPLAYERNODE") && msg.Length == 2)
                        {
                            // Split message value
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 1)
                            {
                                int id = Convert.ToInt32(value[0]);
                            }
                        }
                        if (msg[0].Equals("TARGETMONSTERNODE") && msg.Length == 2)
                        {
                            // Split message value
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 1)
                            {
                                int id = Convert.ToInt32(value[0]);
                            }
                        }
                        if (msg[0].Equals("TARGETNPCNODE") && msg.Length == 2)
                        {
                            // Split message value
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 1)
                            {
                                int id = Convert.ToInt32(value[0]);
                            }
                        }
                        if (msg[0].Equals("TARGETWARPNODE") && msg.Length == 2)
                        {
                            // Split message value
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 1)
                            {
                                int id = Convert.ToInt32(value[0]);
                            }
                        }
                        // Requesting (Player, Monster, NPC, Warp, Item, ... etc)
                        // START REQUEST -> Player node
                        if (msg[0].Equals("LISTPLAYER") && msg.Length == 2)
                        {
                            List<String> value = new List<String>(msg[1].Split(' '));
                            for (int j = 0; j < value.Count; ++j)
                            {
                                if (SelectedCharacterID != Convert.ToInt32(value[j]) && !playersEntity.ContainsKey(value[j]))
                                    network.Send("NODEPLAYER:" + value[j] + ";");
                            }
                            foreach (String key in playersEntity.Keys)
                            {
                                if (!value.Contains(key))
                                {
                                    // May play any effect before remove...
                                    playersEntity.Remove(key);
                                    break;
                                }
                            }
                        }
                        if (msg[0].Equals("NODEPLAYER") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 7 && !playersEntity.ContainsKey(value[0]))
                            {
                                playersEntity.Add(value[0], new UnitEntity(Convert.ToInt32(value[0]), value[1], classConfigs[Convert.ToInt16(value[2])], Convert.ToInt32(value[3]), Convert.ToInt32(value[4]), Convert.ToInt32(value[5]), Convert.ToInt32(value[6]), game, content, graphics, spriteBatch, entityNameFont, entityDamageFont, whiteRect, modelBank, itemBank, audioSystem, particleManager));
                            }
                        }
                        // END REQUEST -> Player node
                        // START REQUEST -> Monster node
                        if (msg[0].Equals("LISTMONSTER") && msg.Length == 2)
                        {
                            List<String> value = new List<String>(msg[1].Split(' '));
                            for (int j = 0; j < value.Count; ++j)
                            {
                                if (!monstersEntity.ContainsKey(value[j]))
                                    network.Send("NODEMONSTER:" + value[j] + ";");
                            }
                            foreach (String key in monstersEntity.Keys)
                            {
                                if (!value.Contains(key))
                                {
                                    // May play any effect before remove...
                                    monstersEntity.Remove(key);
                                    break;
                                }
                            }
                        }
                        if (msg[0].Equals("NODEMONSTER") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 7 && !monstersEntity.ContainsKey(value[0]))
                            {
                                monstersEntity.Add(value[0], new UnitEntity(Convert.ToInt32(value[0]), value[1], classConfigs[Convert.ToInt16(value[2])], Convert.ToInt32(value[3]), Convert.ToInt32(value[4]), Convert.ToInt32(value[5]), Convert.ToInt32(value[6]), game, content, graphics, spriteBatch, entityNameFont, entityDamageFont, whiteRect, modelBank, itemBank, audioSystem, particleManager));
                            }
                        }
                        // END REQUEST -> Monster node
                        // START REQUEST -> NPC node
                        if (msg[0].Equals("LISTNPC") && msg.Length == 2)
                        {
                            List<String> value = new List<String>(msg[1].Split(' '));
                            for (int j = 0; j < value.Count; ++j)
                            {
                                if (!npcsEntity.ContainsKey(value[j]))
                                {
                                    network.Send("NODENPC:" + value[j] + ";");
                                }
                            }
                            foreach (String key in npcsEntity.Keys)
                            {
                                if (!value.Contains(key))
                                {
                                    // May play any effect before remove...
                                    npcsEntity.Remove(key);
                                    break;
                                }
                            }
                        }
                        if (msg[0].Equals("NODENPC") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 7 && !npcsEntity.ContainsKey(value[0]))
                            {
                                npcsEntity.Add(value[0], new UnitEntity(Convert.ToInt32(value[0]), value[1], classConfigs[Convert.ToInt16(value[2])], Convert.ToInt32(value[3]), Convert.ToInt32(value[4]), Convert.ToInt32(value[5]), Convert.ToInt32(value[6]), game, content, graphics, spriteBatch, entityNameFont, entityDamageFont, whiteRect, modelBank, itemBank, audioSystem, particleManager));
                            }
                        }
                        // END REQUEST -> NPC node
                        // START REQUEST -> Warp node
                        if (msg[0].Equals("LISTWARP") && msg.Length == 2)
                        {
                            List<String> value = new List<String>(msg[1].Split(' '));
                            for (int j = 0; j < value.Count; ++j)
                            {
                                if (!warpsEntity.ContainsKey(value[j]))
                                {
                                    network.Send("NODEWARP:" + value[j] + ";");
                                }
                            }
                            foreach (String key in warpsEntity.Keys)
                            {
                                if (!value.Contains(key))
                                {
                                    // May play any effect before remove...
                                    warpsEntity.Remove(key);
                                    break;
                                }
                            }
                        }
                        if (msg[0].Equals("NODEWARP") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 3 && !warpsEntity.ContainsKey(value[0]))
                            {
                                warpsEntity.Add(value[0], new WarpEntity(Convert.ToInt32(value[0]), Convert.ToSingle(value[1]), Convert.ToSingle(value[2]), content));
                            }
                        }
                        // END REQUEST -> Warp node
                        // START REQUEST -> Item node
                        if (msg[0].Equals("LISTITEM") && msg.Length == 2)
                        {
                            List<String> value = new List<String>(msg[1].Split(' '));
                            for (int j = 0; j < value.Count; ++j)
                            {
                                if (!itemsEntity.ContainsKey(value[j]))
                                {
                                    network.Send("NODEITEM:" + value[j] + ";");
                                }
                            }
                            foreach (String key in itemsEntity.Keys)
                            {
                                if (!value.Contains(key))
                                {
                                    // May play any effect before remove...
                                    itemsEntity.Remove(key);
                                    break;
                                }
                            }
                        }
                        if (msg[0].Equals("NODEITEM") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 3 && !itemsEntity.ContainsKey(value[0]))
                            {
                                itemsEntity.Add(value[0], new ItemEntity(Convert.ToInt32(value[0]), Convert.ToInt16(value[1]), content, graphics, spriteBatch, entityNameFont, whiteRect, itemBank));
                            }
                        }
                        // END REQUEST -> Item node
                        // UPDATE NODE
                        if (msg[0].Equals("NODEPLAYERUPDATE") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 9)
                            {
                                UnitEntity ent = null;
                                if (SelectedCharacterID == Convert.ToInt32(value[0]))
                                {
                                    ent = playerEntity;
                                }
                                else
                                {
                                    playersEntity.TryGetValue(value[0], out ent);
                                }
                                if (ent != null)
                                {
                                    float posx = Convert.ToSingle(value[1]);
                                    float posy = Convert.ToSingle(value[2]);
                                    Vector3 pos = new Vector3(posx, 0, posy);
                                    if (ent.Equals(playerEntity))
                                    {
                                        camera.target = pos;
                                    }
                                    ent.setPosition(pos);
                                    ent.currentState(GameState.anim_anim + 1 + Convert.ToInt32(value[3]));
                                    ent.curHP = Convert.ToInt32(value[4]);
                                    ent.maxHP = Convert.ToInt32(value[5]);
                                    ent.curSP = Convert.ToInt32(value[6]);
                                    ent.maxSP = Convert.ToInt32(value[7]);
                                    float roty = Convert.ToSingle(value[8]);
                                    ent.setRotation(new Vector3(0, roty, 0));
                                }
                            }
                        }
                        if (msg[0].Equals("NODEMONSTERUPDATE") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 9)
                            {
                                UnitEntity ent = null;
                                if (monstersEntity.TryGetValue(value[0], out ent))
                                {
                                    float posx = Convert.ToSingle(value[1]);
                                    float posy = Convert.ToSingle(value[2]);
                                    ent.setPosition(new Vector3(posx, 0, posy));
                                    ent.currentState(GameState.anim_anim + 1 + Convert.ToInt32(value[3]));
                                    ent.curHP = Convert.ToInt32(value[4]);
                                    ent.maxHP = Convert.ToInt32(value[5]);
                                    ent.curSP = Convert.ToInt32(value[6]);
                                    ent.maxSP = Convert.ToInt32(value[7]);
                                    float roty = Convert.ToSingle(value[8]);
                                    ent.setRotation(new Vector3(0, roty, 0));
                                }
                            }
                        }
                        if (msg[0].Equals("NODENPCUPDATE") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 9)
                            {
                                UnitEntity ent = null;
                                if (npcsEntity.TryGetValue(value[0], out ent))
                                {
                                    float posx = Convert.ToSingle(value[1]);
                                    float posy = Convert.ToSingle(value[2]);
                                    ent.setPosition(new Vector3(posx, 0, posy));
                                    ent.currentState(GameState.anim_anim + 1 + Convert.ToInt32(value[3]));
                                    ent.curHP = Convert.ToInt32(value[4]);
                                    ent.maxHP = Convert.ToInt32(value[5]);
                                    ent.curSP = Convert.ToInt32(value[6]);
                                    ent.maxSP = Convert.ToInt32(value[7]);
                                    float roty = Convert.ToSingle(value[8]);
                                    ent.setRotation(new Vector3(0, roty, 0));
                                }
                            }
                        }
                        // Damage
                        if (msg[0].Equals("PLAYERNODEDAMAGE") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 2 && SelectedCharacterID > 0)
                            {
                                UnitEntity ent = null;
                                if (SelectedCharacterID == Convert.ToInt32(value[0]))
                                {
                                    ent = playerEntity;
                                }
                                else
                                {
                                    playersEntity.TryGetValue(value[0], out ent);
                                }
                                if (ent != null)
                                {
                                    // Show damage
                                    ent.recieveDamage(Convert.ToInt32(value[1]));
                                }
                            }
                        }
                        if (msg[0].Equals("MONSTERNODEDAMAGE") && msg.Length == 2)
                        {

                            String[] value = msg[1].Split(' ');
                            if (value.Length == 2 && SelectedCharacterID > 0)
                            {
                                UnitEntity ent = null;
                                if (monstersEntity.TryGetValue(value[0], out ent))
                                {
                                    // Show damage
                                    ent.recieveDamage(Convert.ToInt32(value[1]));
                                }
                            }
                        }
                        if (msg[0].Equals("NPCNODEDAMAGE") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 2)
                            {
                                UnitEntity ent = null;
                                if (npcsEntity.TryGetValue(value[0], out ent))
                                {
                                    // Show damage
                                    ent.recieveDamage(Convert.ToInt32(value[1]));
                                }
                            }
                        }

                        // Inventory list
                        if (msg[0].Equals("INVENTORYINFO") && msg.Length == 2)
                        {
                            GUIGameInventory inven = game.getGuiGameInventory();
                            GUIGameEquipment equipm = game.getGuiGameEquipment();
                            // Split message value
                            List<String> value = new List<String>(msg[1].Split(' '));
                            for (int j = 0; j < value.Count; ++j)
                            {
                                String[] itemData = value[j].Split('&');
                                if (itemData.Length == 10 && SelectedCharacterID > 0)
                                {
                                    int inventoryidx = Convert.ToInt32(itemData[0]);
                                    int itemid = Convert.ToInt32(itemData[1]);
                                    short amount = Convert.ToInt16(itemData[2]);
                                    short equip = Convert.ToInt16(itemData[3]);
                                    short refine = Convert.ToInt16(itemData[4]);
                                    int attributeid = Convert.ToInt32(itemData[5]);
                                    int slot1 = Convert.ToInt32(itemData[6]);
                                    int slot2 = Convert.ToInt32(itemData[7]);
                                    int slot3 = Convert.ToInt32(itemData[8]);
                                    int slot4 = Convert.ToInt32(itemData[9]);
                                    if (equip != 0)
                                    {
                                        InventoryItemData newItemData = new InventoryItemData(itemid, amount, refine, attributeid, slot1, slot2, slot3, slot4);

                                        if (!equipm.isSameItemData(equip, newItemData))
                                        {
                                            equipm.setItemData(equip, newItemData);
                                        }
                                    }
                                    else
                                    {
                                        InventoryItemData newItemData = new InventoryItemData(itemid, amount, refine, attributeid, slot1, slot2, slot3, slot4);

                                        if (!inven.isSameItemData(inventoryidx, newItemData))
                                        {
                                            inven.setItemData(inventoryidx, newItemData);
                                        }
                                    }
                                }
                            }
                        }
                        // Equipment
                        if (msg[0].Equals("NODEPLAYEREQUIPMENT") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 7 && SelectedCharacterID > 0)
                            {
                                int id = Convert.ToInt32(value[0]);
                                int itemid_head = Convert.ToInt32(value[1]);
                                int itemid_body = Convert.ToInt32(value[2]);
                                int itemid_hand = Convert.ToInt32(value[3]);
                                int itemid_foot = Convert.ToInt32(value[4]);
                                int itemid_weaponR = Convert.ToInt32(value[5]);
                                int itemid_weaponL = Convert.ToInt32(value[6]);
                                if (id == SelectedCharacterID)
                                {
                                    playerEntity.setEquipmentItemID(itemid_head, itemid_body, itemid_hand, itemid_foot, itemid_weaponR, itemid_weaponL);
                                }
                                else
                                {
                                    if (playersEntity.ContainsKey("" + id))
                                    {
                                        playersEntity["" + id].setEquipmentItemID(itemid_head, itemid_body, itemid_hand, itemid_foot, itemid_weaponR, itemid_weaponL);
                                    }
                                }
                            }
                        }
                        if (msg[0].Equals("NODEMONSTEREQUIPMENT") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 7 && SelectedCharacterID > 0)
                            {
                                int id = Convert.ToInt32(value[0]);
                                int itemid_head = Convert.ToInt32(value[1]);
                                int itemid_body = Convert.ToInt32(value[2]);
                                int itemid_hand = Convert.ToInt32(value[3]);
                                int itemid_foot = Convert.ToInt32(value[4]);
                                int itemid_weaponR = Convert.ToInt32(value[5]);
                                int itemid_weaponL = Convert.ToInt32(value[6]);
                                if (monstersEntity.ContainsKey("" + id))
                                {
                                    monstersEntity["" + id].setEquipmentItemID(itemid_head, itemid_body, itemid_hand, itemid_foot, itemid_weaponR, itemid_weaponL);
                                }
                            }
                        }
                        if (msg[0].Equals("NODENPCEQUIPMENT") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length == 7 && SelectedCharacterID > 0)
                            {
                                int id = Convert.ToInt32(value[0]);
                                int itemid_head = Convert.ToInt32(value[1]);
                                int itemid_body = Convert.ToInt32(value[2]);
                                int itemid_hand = Convert.ToInt32(value[3]);
                                int itemid_foot = Convert.ToInt32(value[4]);
                                int itemid_weaponR = Convert.ToInt32(value[5]);
                                int itemid_weaponL = Convert.ToInt32(value[6]);
                                if (npcsEntity.ContainsKey("" + id))
                                {
                                    npcsEntity["" + id].setEquipmentItemID(itemid_head, itemid_body, itemid_hand, itemid_foot, itemid_weaponR, itemid_weaponL);
                                }
                            }
                        }
                        if (msg[0].Equals("CHAT") && msg.Length == 2)
                        {
                            String[] value = msg[1].Split(' ');
                            if (value.Length > 0 && SelectedCharacterID > 0)
                            {
                                byte msgType = Convert.ToByte(value[0]);
                                String charid = value[1];
                                String recMessage = "";
                                String name = "";
                                switch (msgType)
                                {
                                    case 0:
                                        if (charid.Equals("" + playerEntity.getID()))
                                            name = playerEntity.getName();
                                        else
                                            name = playersEntity[charid].getName();
                                        for (int index = 2; index < value.Length; ++index)
                                            recMessage += (value[index] + " ");
                                        game.getGuiGameChat().InsertMessage(msgType, name, recMessage);
                                        break;
                                    case 1:
                                        if (charid.Equals("" + playerEntity.getID()))
                                            name = playerEntity.getName();
                                        else
                                            name = playersEntity[charid].getName();
                                        recMessage += (" [" + name + "]");
                                        for (int index = 2; index < value.Length; ++index)
                                            recMessage += (value[index] + " ");
                                        game.getGuiGameChat().InsertMessage(msgType, name, recMessage);
                                        break;
                                    case 2:
                                        break;
                                    case 3:
                                        break;
                                }
                            }
                        }
                    } // For scope
                } // Try scope
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
                // Sleep 50ms avoiding 100% CPU
                Thread.Sleep(50);
            }
        }

        private void sendMsg()
        {
            // What player doing, send to server.
            network.Send("CHARINFOSELF:1;");
        }

        public void Update(GameTime gameTime, Matrix rootTransform)
        {
            try
            {
                // Update all entity
                foreach (UnitEntity ent in monstersEntity.Values)
                {
                    ent.Update(gameTime, rootTransform);
                }
                foreach (UnitEntity ent in npcsEntity.Values)
                {
                    ent.Update(gameTime, rootTransform);
                }
                foreach (UnitEntity ent in playersEntity.Values)
                {
                    ent.Update(gameTime, rootTransform);
                }
                if (playerEntity != null)
                    playerEntity.Update(gameTime, rootTransform);
                sendMsg();
                GUIGameMenu guiGameMenu = game.getGuiGameMenu();
                if (playerEntity.currentState() == GameState.anim_die)
                {
                    guiGameMenu.enableRebornButton(true);
                    if (!guiGameMenu.showRebornOnce)
                    {
                        guiGameMenu.Visible = true;
                        guiGameMenu.showRebornOnce = true;
                    }
                }
                else
                {
                    guiGameMenu.enableRebornButton(false);
                    if (guiGameMenu.showRebornOnce)
                        guiGameMenu.showRebornOnce = false;
                }
                audioSystem.setListenerPosition(this.getPlayerPosition());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void Draw(GameTime gameTime)
        {
            Matrix view = camera.getView();
            Matrix projection = camera.getProjection();
            try
            {
                if (playerEntity != null)
                {
                    if (!targetPos.Equals(normalTargetPos))
                    {
                        game.getWayToGoEffect().Draw(view, projection, targetPos, Vector3.Zero, 1.0f);
                    }
                    if (targetNode != null)
                    {
                        game.getWayToGoEffect().Draw(view, projection, targetNode.getPosition(), Vector3.Zero, 1.0f);
                    }
                    foreach (WarpEntity ent in warpsEntity.Values)
                    {
                        if (ent != null)
                        {
                            ent.Draw(view, projection);
                        }
                    }
                    foreach (ItemEntity ent in itemsEntity.Values)
                    {
                        if (ent != null)
                        {
                            ent.Draw(view, projection);
                        }
                    }
                    // Draw all entity
                    foreach (UnitEntity ent in monstersEntity.Values)
                    {
                        if (ent != null)
                        {
                            if (playerEntity.getDistanceFrom(ent) <= 256)
                            {
                                ent.Draw(view, projection);
                            }
                            if (playerEntity.getDistanceFrom(ent) <= 128)
                            {
                                ent.DrawInfo(view, projection);
                                ent.DrawDamage(view, projection);
                            }
                        }
                    }
                    foreach (UnitEntity ent in npcsEntity.Values)
                    {
                        if (ent != null)
                        {
                            if (playerEntity.getDistanceFrom(ent) <= 256)
                            {
                                ent.Draw(view, projection);
                            }
                            if (playerEntity.getDistanceFrom(ent) <= 128)
                            {
                                ent.DrawInfo(view, projection);
                                ent.DrawDamage(view, projection);
                            }
                        }
                    }
                    foreach (UnitEntity ent in playersEntity.Values)
                    {
                        if (ent != null)
                        {
                            if (playerEntity.getDistanceFrom(ent) <= 256)
                            {
                                ent.Draw(view, projection);
                            }
                            if (playerEntity.getDistanceFrom(ent) <= 128)
                            {
                                ent.DrawInfo(view, projection);
                                ent.DrawDamage(view, projection);
                            }
                        }
                    }
                    playerEntity.Draw(view, projection);
                    playerEntity.DrawInfo(view, projection);
                    playerEntity.DrawDamage(view, projection);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void init(int loginid, CharacterInformation info)
        {
            LoggedUserID = loginid;
            SelectedCharacterID = info.getCharid();
            CurrentMapID = info.getChar_curmap();

            this.playerEntity = null;
            this.playerEntity = new UnitEntity(info.getCharid(), info.getChar_name(), classConfigs[info.getChar_class()], info.getChar_hair(), info.getChar_face(), info.getChar_hp(), info.getChar_sp(), game, content, graphics, spriteBatch, entityNameFont, entityDamageFont, whiteRect, modelBank, itemBank, audioSystem, particleManager);
            
            state = GameState.map_warp;
        }

        public void initToWarp()
        {
            targetPos = normalTargetPos;
            targetNode = null;
            npcVisibleBeforeWarp = game.getGuiGameNPC().Visible;
            game.getGuiGameAttribute().Visible = false;
            game.getGuiGameChat().Visible = false;
            game.getGuiGameMenu().Visible = false;
            game.getGuiGameNPC().Visible = false;
            game.getGuiGameEnvironment().Visible = false;
            game.getGuiGameInventory().Visible = false;
            game.getGuiGameEquipment().Visible = false;
            currentState(GameState.map_warp);
        }

        public void warping()
        {
            while (receiveThread != null && receiveThread.IsAlive)
                receiveThread.Abort();

            this.monstersEntity.Clear();
            this.npcsEntity.Clear();
            this.playersEntity.Clear();

            receiveThread = new Thread(new ThreadStart(receiveMsg));
            receiveThread.Name = "gameHandler:RecieveMessageThread";
            receiveThread.Start();

            Thread.Sleep(3000);

            state = GameState.none;
            game.getGuiGameChat().Visible = true;
            game.getGuiGameEnvironment().Visible = true;
            if (npcVisibleBeforeWarp)
                game.getGuiGameNPC().Visible = true;
        }

        public void reset()
        {
            LoggedUserID = 0;
            SelectedCharacterID = 0;
            state = GameState.none;

            this.monstersEntity.Clear();
            this.npcsEntity.Clear();
            this.playersEntity.Clear();

            targetPos = normalTargetPos;
            targetNode = null;

            npcVisibleBeforeWarp = false;

            while (receiveThread.IsAlive)
                receiveThread.Abort();
        }

        public void currentState(GameState state)
        {
            this.state = state;
        }

        public GameState currentState()
        {
            return state;
        }

        public short getCurrentMapID()
        {
            return CurrentMapID;
        }

        public Vector3 getPlayerPosition()
        {
            if (playerEntity != null)
                return playerEntity.getPosition();
            return Vector3.Zero;
        }
    }
}

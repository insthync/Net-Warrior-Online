using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    class GUICharacterCreator : Window
    {
        private GameState state = GameState.none;
        private int LoggedUserID = 0;
        private Network network = null;
        private Label nameLabel;
        private Label hairLabel;
        private Label faceLabel;
        private Label classLabel;
        private TextBox char_name;
        private ComboBox char_hair;
        private ComboBox char_face;
        private ComboBox char_class;
        private Button createButton;
        private Button backButton;
        private GameMain game;
        private Manager manager;
        private Dictionary<short, GameClassConfig> classConfigs;
        private List<short> classIdPair;
        public GUICharacterCreator(Manager manager, Network network, GameMain game, Dictionary<short, GameClassConfig> classConfigs)
            : base(manager)
        {
            this.manager = manager;
            this.game = game;
            this.network = network;
            this.classConfigs = classConfigs;

            // Class config pair for selecting class method
            classIdPair = new List<short>();

            // Define window property
            Init();
            Text = "New Character";
            Top = manager.ScreenHeight - 520;
            Left = manager.ScreenWidth - 220;
            Width = 200;
            Height = 500;
            CloseButtonVisible = false;
            Resizable = false;

            nameLabel = new Label(manager);
            nameLabel.Init();
            nameLabel.Text = "Character name";
            nameLabel.Width = 165;
            nameLabel.Height = 25;
            nameLabel.Top = 10;
            nameLabel.Left = 10;
            nameLabel.Parent = this;
            Add(nameLabel);

            char_name = new TextBox(manager);
            char_name.Init();
            char_name.Width = 165;
            char_name.Height = 25;
            char_name.Top = 35;
            char_name.Left = 10;
            char_name.Parent = this;
            Add(char_name);

            classLabel = new Label(manager);
            classLabel.Init();
            classLabel.Text = "Class";
            classLabel.Width = 165;
            classLabel.Height = 25;
            classLabel.Top = 60;
            classLabel.Left = 10;
            classLabel.Parent = this;
            Add(classLabel);

            char_class = new ComboBox(manager);
            char_class.Init();
            char_class.ReadOnly = true;
            char_class.Width = 165;
            char_class.Top = 85;
            char_class.Left = 10;
            char_class.Parent = this;
            Add(char_class);
            
            hairLabel = new Label(manager);
            hairLabel.Init();
            hairLabel.Text = "Hair";
            hairLabel.Width = 165;
            hairLabel.Height = 25;
            hairLabel.Top = 110;
            hairLabel.Left = 10;
            hairLabel.Parent = this;
            Add(hairLabel);

            char_hair = new ComboBox(manager);
            char_hair.Init();
            char_hair.Items.Add("Style 1");
            char_hair.ItemIndex = 0;
            char_hair.ReadOnly = true;
            char_hair.Width = 165;
            char_hair.Top = 135;
            char_hair.Left = 10;
            char_hair.Parent = this;
            Add(char_hair);

            faceLabel = new Label(manager);
            faceLabel.Init();
            faceLabel.Text = "Face";
            faceLabel.Width = 165;
            faceLabel.Height = 25;
            faceLabel.Top = 160;
            faceLabel.Left = 10;
            faceLabel.Parent = this;
            Add(faceLabel);

            char_face = new ComboBox(manager);
            char_face.Init();
            char_face.Items.Add("Style 1");
            char_face.ItemIndex = 0;
            char_face.ReadOnly = true;
            char_face.Width = 165;
            char_face.Top = 185;
            char_face.Left = 10;
            char_face.Parent = this;
            Add(char_face);

            createButton = new Button(manager);
            createButton.Init();
            createButton.Text = "Create";
            createButton.Width = 80;
            createButton.Height = 25;
            createButton.Top = Height - 65;
            createButton.Left = 10;
            createButton.Parent = this;
            Add(createButton);

            backButton = new Button(manager);
            backButton.Init();
            backButton.Text = "Back";
            backButton.Width = 80;
            backButton.Height = 25;
            backButton.Top = Height - 65;
            backButton.Left = 95;
            backButton.Parent = this;
            Add(backButton);

            createButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnCreate_Click);
            backButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnBack_Click);
        }

        public void load()
        {
            char_class.Items.Clear();
            classIdPair.Clear();
            // Load character data
            createButton.Enabled = false;
            backButton.Enabled = false;
            game.messageDialog.setTitle("");
            game.messageDialog.setMessage("Please wait...");
            game.messageDialog.Visible = true;
            game.messageDialog.CloseButtonVisible = false;
            String responseMsg = "";
            try
            {
                // Try connect to server
                //if (!network.isConnected())
                //    network.Connect("127.0.0.1", 5000);
                // Send login message
                network.Send("CHARCREATEABLE:" + LoggedUserID + ";"); // Get character from index
                // Receive message
                while (responseMsg.Length <= 0)
                    responseMsg = network.Receive();
            }
            catch (Exception e)
            {
                // Has any error come here
                Debug.WriteLine(e.ToString());
                game.messageDialog.setTitle("ERROR!!");
                game.messageDialog.setMessage("Can't connect to server.");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = true;
                state = GameState.none;
            }
            // Split message from end of line
            String[] line = responseMsg.Split(';');
            for (int i = 0; i < line.Length; ++i)
            {
                // If has message
                // CHARCREATEAVAILABLE:{ID1} {ID2} {ID3} ... {ID4};
                String[] msg = line[i].Split(':');
                if (msg[0].Equals("CHARCREATEABLE") && msg.Length == 2)
                {
                    // Split message value
                    List<String> value = new List<String>(msg[1].Split(' '));
                    for (int j = 0; j < value.Count; ++j)
                    {
                        short classid = Convert.ToInt16(value[j]);
                        GameClassConfig cfg = null;
                        if (classConfigs.TryGetValue(classid, out cfg))
                        {
                            char_class.Items.Add(cfg.name);
                            classIdPair.Add(classid);
                        }
                    }
                }
            }
            char_class.ItemIndex = 0;
            game.hideMessageDialog();
            createButton.Enabled = true;
            backButton.Enabled = true;
        }

        private void createCharThread()
        {
            String responseMsg = "";
            String statusMsg = "";
            String idMsg = "";
            try
            {
                // Try connect to server
                //if (!network.isConnected())
                //    network.Connect("127.0.0.1", 5000);
                // Send login message
                network.Send("CHARCREATE:" + LoggedUserID + " " + char_name.Text + " " + classIdPair[char_class.ItemIndex] + " " + char_hair.ItemIndex + " " + char_face.ItemIndex + ";");
                // Receive message
                while (responseMsg.Length <= 0)
                    responseMsg = network.Receive();
            }
            catch (Exception e)
            {
                // Has any error come here
                Debug.WriteLine(e.ToString());
                createButton.Enabled = true;
                backButton.Enabled = true;
                game.messageDialog.setTitle("ERROR!!");
                game.messageDialog.setMessage("Can't connect to server.");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = true;
                state = GameState.none;
            }
            // Split message from end of line
            String[] line = responseMsg.Split(';');
            for (int i = 0; i < line.Length; ++i)
            {
                // If has message
                // Split message name(CHARCREATE) and value({STATUS})
                // CHARCREATE:{STATUS};
                String[] msg = line[i].Split(':');
                if (msg[0].Equals("CHARCREATE") && msg.Length == 2)
                {
                    // Split message value
                    String[] value = msg[1].Split(' ');
                    if (value.Length == 2)
                    {
                        statusMsg = value[0];
                        idMsg = value[1];
                    }
                    if (statusMsg.Equals("OK"))
                    {
                        // if status is OK
                        try
                        {
                            createButton.Enabled = true;
                            backButton.Enabled = true;
                            game.hideMessageDialog();
                            state = GameState.character_newcharacterend;
                        }
                        catch (Exception e)
                        {
                            // Has any error come here
                            Debug.WriteLine(e.ToString());
                            createButton.Enabled = true;
                            backButton.Enabled = true;
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("Socket error occur when creating character.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                            state = GameState.none;
                        }
                    }
                    else
                    {
                        // if status isn't OK
                        createButton.Enabled = true;
                        backButton.Enabled = true;
                        if (idMsg.Equals(0))
                        {
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("Socket error occur when creating character.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                        }
                        else
                        {
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("This character name has been used.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                        }
                        state = GameState.none;
                    }
                }
            }
        }

        void btnCreate_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (!char_name.Text.Equals("") && !char_name.Text.Contains(" "))
            {
                createButton.Enabled = false;
                backButton.Enabled = false;
                game.messageDialog.setTitle("");
                game.messageDialog.setMessage("Please wait...");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = false;
                Thread threader = new Thread(new ThreadStart(createCharThread));
                threader.Start();
            }
            else
            {
                game.messageDialog.setTitle("ERROR!!");
                game.messageDialog.setMessage("Please enter your character name, space not allow.");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = true;
            }
        }

        void btnBack_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            createButton.Enabled = false;
            state = GameState.character_newcharacterend;
        }

        public int getUserId()
        {
            return LoggedUserID;
        }

        public void init(int id)
        {
            // Init character list by loginid
            LoggedUserID = id;
            load();
        }

        public void reset()
        {
            createButton.Enabled = true;
            backButton.Enabled = true;
            LoggedUserID = 0;
            state = GameState.none;
        }

        public void currentState(GameState state)
        {
            this.state = state;
        }

        public GameState currentState()
        {
            return state;
        }

        public short getSelectedClassId()
        {
            return (short)classIdPair[char_class.ItemIndex];
        }
    }
}

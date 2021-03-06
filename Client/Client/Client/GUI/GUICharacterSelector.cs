﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    class GUICharacterSelector : Window
    {
        private GameState state = GameState.none;
        private int LoggedUserID = 0;
        private int SelectedCharacterID = 0;
        private int SelectingCharacterID = 0;
        private Network network = null;
        private Button createButton;
        private Button startButton;
        private Button backButton;
        private Label[] character_name;
        private Label[] character_level;
        private Button[] character_selectButton;
        private Button[] character_deleteButton;
        private Panel[] character_panel;
        private GameMain game;
        private Manager manager;
        private Dictionary<int, CharacterInformation> charInfo;
        private Dictionary<int, int> listId;
        public GUICharacterSelector(Manager manager, Network network, GameMain game)
            : base(manager)
        {
            this.manager = manager;
            this.game = game;
            this.network = network;

            // Character Information
            charInfo = new Dictionary<int, CharacterInformation>();

            // Index, Id
            listId = new Dictionary<int, int>();

            // Define window property
            Init();
            Text = "Your Characters";
            Top = manager.ScreenHeight - 520;
            Left = manager.ScreenWidth - 220;
            Width = 200;
            Height = 500;
            CloseButtonVisible = false;
            Resizable = false;

            createButton = new Button(manager);
            createButton.Init();
            createButton.Text = "New Character";
            createButton.Width = 165;
            createButton.Height = 25;
            createButton.Top = Height - 95;
            createButton.Left = 10;
            createButton.Parent = this;
            Add(createButton);

            startButton = new Button(manager);
            startButton.Init();
            startButton.Text = "Start";
            startButton.Width = 80;
            startButton.Height = 25;
            startButton.Top = Height - 65;
            startButton.Left = 10;
            startButton.Parent = this;
            Add(startButton);

            backButton = new Button(manager);
            backButton.Init();
            backButton.Text = "Back";
            backButton.Width = 80;
            backButton.Height = 25;
            backButton.Top = Height - 65;
            backButton.Left = 95;
            backButton.Parent = this;
            Add(backButton);

            character_name = new Label[game.maxchar];
            character_level = new Label[game.maxchar];
            character_selectButton = new Button[game.maxchar];
            character_deleteButton = new Button[game.maxchar];
            character_panel = new Panel[game.maxchar];

            for (int i = 0; i < game.maxchar; ++i)
            {
                character_panel[i] = new Panel(manager);
                character_panel[i].Init();
                character_panel[i].Width = 165;
                character_panel[i].Height = 70;
                character_panel[i].Top = 10 + (75*i);
                character_panel[i].Left = 10;
                character_panel[i].Parent = this;
                Add(character_panel[i]);

                character_name[i] = new Label(manager);
                character_name[i].Init();
                character_name[i].Text = "";
                character_name[i].Width = 155;
                character_name[i].Height = 15;
                character_name[i].Top = 5;
                character_name[i].Left = 5;
                character_name[i].Parent = character_panel[i];
                character_panel[i].Add(character_name[i]);

                character_level[i] = new Label(manager);
                character_level[i].Init();
                character_level[i].Text = "";
                character_level[i].Width = 155;
                character_level[i].Height = 15;
                character_level[i].Top = 25;
                character_level[i].Left = 5;
                character_level[i].Parent = character_panel[i];
                character_panel[i].Add(character_level[i]);

                character_selectButton[i] = new Button(manager);
                character_selectButton[i].Init();
                character_selectButton[i].Text = "Select";
                character_selectButton[i].Width = 76;
                character_selectButton[i].Height = 20;
                character_selectButton[i].Top = 45;
                character_selectButton[i].Left = 5;
                character_selectButton[i].Parent = character_panel[i];
                character_selectButton[i].Enabled = false;
                character_panel[i].Add(character_selectButton[i]);

                character_deleteButton[i] = new Button(manager);
                character_deleteButton[i].Init();
                character_deleteButton[i].Text = "Delete";
                character_deleteButton[i].Width = 76;
                character_deleteButton[i].Height = 20;
                character_deleteButton[i].Top = 45;
                character_deleteButton[i].Left = 84;
                character_deleteButton[i].Parent = character_panel[i];
                character_deleteButton[i].Enabled = false;
                character_panel[i].Add(character_deleteButton[i]);

                character_selectButton[i].Click += new TomShane.Neoforce.Controls.EventHandler(btnSelect_Click);
                character_deleteButton[i].Click += new TomShane.Neoforce.Controls.EventHandler(btnDelete_Click);

                character_panel[i].Visible = false;
            }

            createButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnCreate_Click);
            startButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnStart_Click);
            backButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnBack_Click);
        }

        public void load()
        {
            // Load character data
            createButton.Enabled = false;
            startButton.Enabled = false;
            game.messageDialog.setTitle("");
            game.messageDialog.setMessage("Please wait...");
            game.messageDialog.Visible = true;
            game.messageDialog.CloseButtonVisible = false;
            Boolean end = false;
            for (int i = 0; i < game.maxchar; ++i)
            {
                String responseMsg = "";
                String statusMsg = "";
                try
                {
                    // Try connect to server
                    //if (!network.isConnected())
                    //    network.Connect("127.0.0.1", 5000);
                    // Send login message
                    network.Send("CHARGET:" + LoggedUserID + " " + i + ";"); // Get character from index
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
                for (int j = 0; j < line.Length; ++j)
                {
                    // If has message
                    // Split message name(CHARGET) and value({STATUS}|{CHARID})
                    // CHARGET:{STATUS}|{CHARID};
                    String[] msg = line[j].Split(':');
                    if (msg[0].Equals("CHARGET") && msg.Length == 2)
                    {
                        // Split message value
                        String[] value = msg[1].Split(' ');
                        if (value.Length == 2)
                            statusMsg = value[0];
                        if (statusMsg.Equals("OK"))
                        {
                            // if status is OK
                            try
                            {
                                // If character found show panel
                                CharacterInformation chara = getCharInfo(Convert.ToInt32(value[1]));
                                if (chara != null)
                                {
                                    if (!charInfo.ContainsKey(chara.getCharid()))
                                    {
                                        charInfo.Add(chara.getCharid(), chara);
                                        listId.Add(i, chara.getCharid());
                                        character_name[i].Text = ("Name: " + charInfo[listId[i]].getChar_name());
                                        character_level[i].Text = ("LV: " + charInfo[listId[i]].getChar_level());
                                        character_selectButton[i].Enabled = true;
                                        character_deleteButton[i].Enabled = true;
                                        character_panel[i].Visible = true;
                                        state = GameState.none;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                // Has any error come here
                                Debug.WriteLine(e.ToString());
                                game.messageDialog.setTitle("ERROR!!");
                                game.messageDialog.setMessage("There has something error.");
                                game.messageDialog.Visible = true;
                                game.messageDialog.CloseButtonVisible = true;
                                state = GameState.none;
                            }
                            break;
                        }
                        else
                        {
                            // if status isn't OK
                            state = GameState.none;
                            end = true;
                            break;
                        }
                    }
                }
                // If Readed all character data
                if (end)
                    break;
            }
            game.hideMessageDialog();
            createButton.Enabled = true;
            startButton.Enabled = true;
        }

        private void selectCharThread()
        {
            String responseMsg = "";
            String statusMsg = "";
            String idMsg = "";
            try
            {
                // Try connect to server
                //if (!network.isConnected())
                //    network.Connect("127.0.0.1", 5000);
                // Starting ping...
                // Send login message
                network.Send("CHARSELECT:" + LoggedUserID + " " + SelectingCharacterID + ";");
                // Receive message
                while (responseMsg.Length <= 0)
                    responseMsg = network.Receive();
            }
            catch (Exception e)
            {
                // Has any error come here
                Debug.WriteLine(e.ToString());
                // Enabling some buttons...
                game.messageDialog.setTitle("ERROR!!");
                game.messageDialog.setMessage("Can't connect to server.");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = true;
                LoggedUserID = 0;
                state = GameState.none;
            }
            // Split message from end of line
            String[] line = responseMsg.Split(';');
            for (int i = 0; i < line.Length; ++i)
            {
                // If has message
                // Split message name(CHARSELECT) and value({STATUS}|{CHARID})
                // CHARSELECT:{STATUS}|{CHARID};
                String[] msg = line[i].Split(':');
                if (msg[0].Equals("CHARSELECT") && msg.Length == 2)
                {
                    // Split message value
                    String[] value = msg[1].Split(' ');
                    if (value.Length == 2)
                    {
                        statusMsg = value[0];
                        idMsg = value[1];
                    }
                    if (statusMsg.Equals("OK") && !idMsg.Equals("0"))
                    {
                        // if status is OK
                        try
                        {
                            // Enabling some buttons...
                            game.hideMessageDialog();
                            SelectedCharacterID = Convert.ToInt32(idMsg);
                            state = GameState.character_selectedcharacter;
                        }
                        catch (Exception e)
                        {
                            // Has any error come here
                            // I was expect that NumberFormatException can occur
                            Debug.WriteLine(e.ToString());
                            // Enabling some buttons...
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("Error occur when selecting this character.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                            SelectedCharacterID = 0;
                            state = GameState.none;
                        }
                    }
                    else
                    {
                        // if status isn't OK
                        // Enabling some buttons...
                        game.messageDialog.setTitle("ERROR!!");
                        game.messageDialog.setMessage("Wrong username or password.");
                        game.messageDialog.Visible = true;
                        game.messageDialog.CloseButtonVisible = true;
                        SelectedCharacterID = 0;
                        state = GameState.none;
                    }
                }
            }
        }

        void btnSelect_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            for (int i = 0; i < game.maxchar; ++i)
            {
                if (sender.Equals(character_selectButton[i]))
                {
                    // Set current character ID
                    if (listId.ContainsKey(i))
                    {
                        SelectingCharacterID = listId[i];
                    }
                    // Show it's model here
                }
            }
        }

        void btnDelete_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            game.messageDialog.setTitle("");
            game.messageDialog.setMessage("Please wait...");
            game.messageDialog.Visible = true;
            game.messageDialog.CloseButtonVisible = false;
            for (int i = 0; i < game.maxchar; ++i)
            {
                if (sender.Equals(character_deleteButton[i]))
                {
                    // Set current character ID
                    if (listId.ContainsKey(i))
                    {
                        String responseMsg = "";
                        try
                        {
                            network.Send("CHARDELETE:" + this.LoggedUserID + " " + listId[i] + ";");
                            // Receive message
                            while (responseMsg.Length <= 0)
                                responseMsg = network.Receive();
                        }
                        catch (Exception ex)
                        {
                            // Has any error come here
                            Debug.WriteLine(ex.ToString());
                            // Enabling some buttons...
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("Can't connect to server.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                            this.LoggedUserID = 0;
                            state = GameState.none;
                        }
                    }
                }
            }
            int LoggedUserID = this.LoggedUserID;
            this.reset();
            this.init(LoggedUserID);
        }

        void btnCreate_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            createButton.Enabled = false;
            startButton.Enabled = false;
            state = GameState.character_newcharacter;
        }

        void btnStart_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (SelectingCharacterID != 0)
            {
                // Disabling some buttons...
                // Loading dialog...
                game.messageDialog.setTitle("");
                game.messageDialog.setMessage("Please wait...");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = false;
                Thread threader = new Thread(new ThreadStart(selectCharThread));
                threader.Start();
            }
            else
            {
                game.messageDialog.setTitle("ERROR!!");
                game.messageDialog.setMessage("Please select your character.");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = true;
                state = GameState.none;
            }
        }

        void btnBack_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            createButton.Enabled = false;
            startButton.Enabled = false;
            game.hideMessageDialog();
            state = GameState.character_backtologin;
        }

        public int getUserId()
        {
            return LoggedUserID;
        }

        public int getCharId()
        {
            return SelectedCharacterID;
        }

        public CharacterInformation getSelectedCharInfo()
        {
            return charInfo[SelectedCharacterID];
        }


        private CharacterInformation getCharInfo(int charid)
        {
            CharacterInformation returnValue = null;
            String responseMsg = "";
            String statusMsg = "";
            try
            {
                // Try connect to server
                //if (!network.isConnected())
                //    network.Connect("127.0.0.1", 5000);
                // Send login message
                network.Send("CHARINFO:" + LoggedUserID + " " + charid + ";");
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
                // Split message name(CHARINFO) and values(TOO MUCH!!)
                String[] msg = line[i].Split(':');
                if (msg[0].Equals("CHARINFO") && msg.Length == 2)
                {
                    // Split message value
                    String[] value = msg[1].Split(' ');
                    if (value.Length == 24)
                        statusMsg = value[0];
                    if (statusMsg.Equals("OK"))
                    {
                        // if status is OK
                        try
                        {
                            returnValue = new CharacterInformation(charid, value[1]/*name*/,
                                    Convert.ToInt16(value[2])/*class*/, Convert.ToInt16(value[3])/*level*/, Convert.ToInt32(value[4])/*exp*/, Convert.ToInt32(value[5])/*curexp*/, Convert.ToInt32(value[6])/*gold*/,
                                    Convert.ToInt32(value[7])/*hp*/, Convert.ToInt32(value[8])/*cur_hp*/, Convert.ToInt32(value[9])/*sp*/, Convert.ToInt32(value[10])/*cur_sp*/, Convert.ToInt32(value[11])/*stpoint*/, Convert.ToInt32(value[12])/*skpoint*/,
                                    Convert.ToInt16(value[13])/*str*/, Convert.ToInt16(value[14])/*dex*/, Convert.ToInt16(value[15])/*int*/, Convert.ToInt16(value[16])/*hair*/,
                                    Convert.ToInt16(value[17])/*face*/, Convert.ToInt16(value[18])/*curmap*/, Convert.ToSingle(value[19])/*curmapx*/, Convert.ToSingle(value[20])/*curmapy*/,
                                    Convert.ToInt16(value[21])/*savmap*/, Convert.ToSingle(value[22])/*savmapx*/, Convert.ToSingle(value[23])/*savmapy*/);
                            state = GameState.none;
                        }
                        catch (Exception e)
                        {
                            // Has any error come here
                            Debug.WriteLine(e.ToString());
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("There has something error.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                            state = GameState.none;
                        }
                    }
                    else
                    {
                        // if status isn't OK
                        game.messageDialog.setTitle("ERROR!!");
                        game.messageDialog.setMessage("This character not found.");
                        game.messageDialog.Visible = true;
                        game.messageDialog.CloseButtonVisible = true;
                        state = GameState.none;
                    }
                }
            }
            return returnValue;
        }

        public void init(int id)
        {
            // Init character list by loginid
            LoggedUserID = id;
            load();
        }

        public void reset()
        {
            charInfo.Clear();
            listId.Clear();
            for (int i = 0; i < game.maxchar; ++i)
            {
                character_selectButton[i].Enabled = false;
                character_deleteButton[i].Enabled = false;
                character_panel[i].Visible = false;
            }
            createButton.Enabled = true;
            startButton.Enabled = true;
            LoggedUserID = 0;
            state = GameState.none;
            SelectedCharacterID = 0;
            SelectingCharacterID = 0;
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
            if (SelectingCharacterID > 0)
                return charInfo[SelectingCharacterID].getChar_class();
            return -1;
        }

        public int getSelectingCharacterID()
        {
            return SelectingCharacterID;
        }
    }
}

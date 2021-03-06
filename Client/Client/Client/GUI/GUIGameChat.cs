﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    public class GUIGameChat : Window
    {
        private Console console;
        private Manager manager;
        private TextBox txtMain = null;
        private ComboBox cmbMain = null;
        private Network network;
        public GUIGameChat(Manager manager, Network network)
            : base(manager)
        {
            this.manager = manager;
            this.network = network;
            // Define window property
            Init();
            Text = "Chat";
            Width = 512;
            Height = 128;
            Top = manager.ScreenHeight - Height;
            Left = (manager.ScreenWidth - Width) / 2;
            MinimumHeight = 128;
            MinimumWidth = 384;
            MaximumHeight = 384;
            MaximumWidth = 800;
            Alpha = 220;
            CloseButtonVisible = false;

            // Chat console
            console = new Console(manager);
            console.Init();
            console.Channels.Add(new ConsoleChannel(0, "General", Color.White));
            console.Channels.Add(new ConsoleChannel(1, "Whisper", Color.Blue));
            console.Channels.Add(new ConsoleChannel(2, "Party", Color.Green));
            console.Channels.Add(new ConsoleChannel(3, "Guild", Color.Orange));
            //console.Channels.Add(new ConsoleChannel(4, "System", Color.Yellow));
            console.MessageFormat = ConsoleMessageFormats.None;
            console.Left = 1;
            console.Top = 1;
            console.Width = Width - 16;
            console.Height = Height - 60;
            console.Anchor = Anchors.All;
            console.SelectedChannel = 0;
            console.TextBoxVisible = false;
            console.ChannelsVisible = false;
            Add(console);

            txtMain = new TextBox(manager);
            txtMain.Init();
            txtMain.Top = 98;
            txtMain.Left = 129;
            txtMain.Width = Width - txtMain.Left - 8;
            txtMain.Anchor = Anchors.Left | Anchors.Bottom | Anchors.Right;
            txtMain.Detached = false;
            txtMain.KeyDown += new KeyEventHandler(txtMain_KeyDown);
            txtMain.GamePadDown += new GamePadEventHandler(txtMain_GamePadDown);
            txtMain.FocusGained += new EventHandler(txtMain_FocusGained);
            Add(txtMain, false);

            cmbMain = new ComboBox(manager);
            cmbMain.Init();
            cmbMain.Top = 98;
            cmbMain.Left = 8;
            cmbMain.Width = 120;
            cmbMain.Anchor = Anchors.Left | Anchors.Bottom;
            cmbMain.Detached = false;
            cmbMain.DrawSelection = false;
            foreach (ConsoleChannel c in console.Channels)
            {
                cmbMain.Items.Add(c.Name);
            }
            cmbMain.ItemIndex = 0;
            cmbMain.ItemIndexChanged += new EventHandler(txtMain_FocusGained);
            Add(cmbMain, false);
            
        }
        ////////////////////////////////////////////////////////////////////////////     
        void txtMain_FocusGained(object sender, EventArgs e)
        {
            ConsoleChannel ch = console.Channels[cmbMain.ItemIndex];
            if (ch != null) txtMain.TextColor = ch.Color;
        }
        ////////////////////////////////////////////////////////////////////////////     

        ////////////////////////////////////////////////////////////////////////////     
        void txtMain_KeyDown(object sender, KeyEventArgs e)
        {
            SendMessage(e);
        }
        ////////////////////////////////////////////////////////////////////////////    

        ////////////////////////////////////////////////////////////////////////////        
        void txtMain_GamePadDown(object sender, GamePadEventArgs e)
        {
            SendMessage(e);
        }
        ////////////////////////////////////////////////////////////////////////////        

        ////////////////////////////////////////////////////////////////////////////        
        private void SendMessage(EventArgs x)
        {
            KeyEventArgs k = new KeyEventArgs();
            GamePadEventArgs g = new GamePadEventArgs(PlayerIndex.One);

            if (x is KeyEventArgs) k = x as KeyEventArgs;
            else if (x is GamePadEventArgs) g = x as GamePadEventArgs;

            ConsoleChannel ch = console.Channels[cmbMain.ItemIndex];
            if (ch != null)
            {
                txtMain.TextColor = ch.Color;

                string message = txtMain.Text;
                if ((k.Key == Microsoft.Xna.Framework.Input.Keys.Enter || g.Button == GamePadActions.Press) && message != null && message != "")
                {
                    x.Handled = true;
                    // Send chat message
                    if (network.isConnected())
                    {
                        string chatMsg = txtMain.Text;
                        chatMsg = chatMsg.Replace("'", "'39'");
                        chatMsg = chatMsg.Replace(" ", "'32'");
                        chatMsg = chatMsg.Replace(":", "'58'");
                        chatMsg = chatMsg.Replace(";", "'59'");
                        network.Send("CHAT:" + cmbMain.ItemIndex + " " + chatMsg + ";");
                    }
                    txtMain.Text = "";
                    ClientArea.Invalidate();
                }
            }
        }

        public void InsertMessage(byte channel, string typer, string message)
        {
            message = message.Replace("'58'", ":");
            message = message.Replace("'59'", ";");
            message = message.Replace("'32'", " ");
            message = message.Replace("'39'", "'");
            console.MessageBuffer.Add(new ConsoleMessage(" (" + console.Channels[channel].Name + ")" + typer + ": " + message, channel));
        }

        public bool isFocus()
        {
            return txtMain.Focused;
        }
    }
}

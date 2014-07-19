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
    public class GUIGameMenu : Window
    {
        public Boolean showRebornOnce = false;
        private Button rebornBtn;
        private Button optionBtn;
        private Button relogBtn;
        private Button exitBtn;
        private Button closeBtn;
        private Manager manager;
        private GameHandler handler;
        private Network network;
        private GameMain game;
        public GUIGameMenu(Manager manager, Network network, GameHandler handler, GameMain game)
            : base(manager)
        {
            this.manager = manager;
            this.handler = handler;
            this.network = network;
            this.game = game;
            Init();
            Text = "System Menu";
            Width = 170;
            Height = 195;
            Left = (manager.ScreenWidth - Width) / 2;
            Top = (manager.ScreenHeight - Height) / 2;
            Alpha = 220;
            Resizable = false;
            CloseButtonVisible = false;

            rebornBtn = new Button(manager);
            rebornBtn.Init();
            rebornBtn.Text = "Reborn";
            rebornBtn.Enabled = false;
            rebornBtn.Parent = this;
            rebornBtn.Left = 10;
            rebornBtn.Top = 10;
            rebornBtn.Width = 140;
            rebornBtn.Click += new TomShane.Neoforce.Controls.EventHandler(rebornBtn_Click);
            Add(rebornBtn);

            optionBtn = new Button(manager);
            optionBtn.Init();
            optionBtn.Text = "Option";
            optionBtn.Enabled = false;
            optionBtn.Parent = this;
            optionBtn.Left = 10;
            optionBtn.Top = 40;
            optionBtn.Width = 140;
            Add(optionBtn);

            relogBtn = new Button(manager);
            relogBtn.Init();
            relogBtn.Text = "Re-Login";
            relogBtn.Parent = this;
            relogBtn.Left = 10;
            relogBtn.Top = 70;
            relogBtn.Width = 140;
            relogBtn.Click += new TomShane.Neoforce.Controls.EventHandler(relogBtn_Click);
            Add(relogBtn);

            exitBtn = new Button(manager);
            exitBtn.Init();
            exitBtn.Text = "Exit Game";
            exitBtn.Parent = this;
            exitBtn.Left = 10;
            exitBtn.Top = 100;
            exitBtn.Width = 140;
            exitBtn.Click += new TomShane.Neoforce.Controls.EventHandler(exitBtn_Click);
            Add(exitBtn);

            closeBtn = new Button(manager);
            closeBtn.Init();
            closeBtn.Text = "Close";
            closeBtn.Parent = this;
            closeBtn.Left = 10;
            closeBtn.Top = 130;
            closeBtn.Width = 140;
            closeBtn.Click += new TomShane.Neoforce.Controls.EventHandler(closeBtn_Click);
            Add(closeBtn);
        }

        public Boolean enableRebornButton()
        {
            return rebornBtn.Enabled;
        }

        public void enableRebornButton(Boolean b)
        {
            rebornBtn.Enabled = b;
        }

        void rebornBtn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            this.Visible = false;
            network.Send("REBORN:0;");
        }

        void relogBtn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            this.Visible = false;
            handler.currentState(GameState.map_backtologin);
        }

        void exitBtn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            this.Visible = false;
            game.Exit();
        }

        void closeBtn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            this.Visible = false;
        }
    }
}

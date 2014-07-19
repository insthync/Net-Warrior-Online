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
    class GUILogin : Window
    {
        private GameState state = GameState.none;
        private int LoggedUserID = 0;
        private Network network = null;
        private Label usernameLabel;
        private Label passwordLabel;
        private TextBox username;
        private TextBox password;
        private Button loginButton;
        private Button exitButton;
        private GameMain game;
        private Manager manager;
        private ServerAddressConfig serverConfig;
        public GUILogin(Manager manager, Network network, GameMain game)
            : base(manager)
        {
            this.manager = manager;
            this.game = game;
            this.network = network;

            // Define window property
            Init();
            Text = "Login";
            Width = 350;
            Height = 150;
            Center();
            Top = manager.ScreenHeight - 250;
            CloseButtonVisible = false;
            Resizable = false;

            usernameLabel = new Label(manager);
            usernameLabel.Init();
            usernameLabel.Text = "Username:";
            usernameLabel.Height = 25;
            usernameLabel.Top = 10;
            usernameLabel.Left = 10;
            usernameLabel.Parent = this;
            Add(usernameLabel);

            passwordLabel = new Label(manager);
            passwordLabel.Init();
            passwordLabel.Text = "Password:";
            passwordLabel.Height = 25;
            passwordLabel.Top = 40;
            passwordLabel.Left = 10;
            passwordLabel.Parent = this;
            Add(passwordLabel);

            username = new TextBox(manager);
            username.Init();
            username.Text = "";
            username.Width = 250;
            username.Height = 25;
            username.Top = 10;
            username.Left = 75;
            username.Parent = this;
            Add(username);

            password = new TextBox(manager);
            password.Init();
            password.Text = "";
            password.Width = 250;
            password.Height = 25;
            password.Top = 40;
            password.Left = 75;
            password.Parent = this;
            password.KeyDown += new KeyEventHandler(password_KeyDown);
            Add(password);

            loginButton = new Button(manager);
            loginButton.Init();
            loginButton.Text = "Login";
            loginButton.Height = 25;
            loginButton.Top = 80;
            loginButton.Left = 170;
            loginButton.Parent = this;
            Add(loginButton);

            exitButton = new Button(manager);
            exitButton.Init();
            exitButton.Text = "Exit";
            exitButton.Height = 25;
            exitButton.Top = 80;
            exitButton.Left = 250;
            exitButton.Parent = this;
            Add(exitButton);
            
            loginButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnLogin_Click);
            exitButton.Click += new TomShane.Neoforce.Controls.EventHandler(btnExit_Click);

            serverConfig = this.game.getServerConfig();
        }

        void password_KeyDown(object sender, KeyEventArgs e)
        {
            password.Mode = TextBoxMode.Normal;
            password.Mode = TextBoxMode.Password;
        }

        private void loginThread()
        {
            String responseMsg = "";
            String statusMsg = "";
            String idMsg = "";
            try
            {
                // Try connect to server
                if (!network.isConnected())
                    network.Connect(serverConfig.getIP(), serverConfig.getPort());
                // Starting ping...
                // Send login message
                network.Send("LOGIN:" + username.Text + " " + password.Text + ";");
                // Receive message
                while (responseMsg.Length <= 0)
                    responseMsg = network.Receive();
            }
            catch (Exception e)
            {
                // Has any error come here
                Debug.WriteLine(e.ToString());
                username.Enabled = true;
                password.Enabled = true;
                loginButton.Enabled = true;
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
                // Split message name(LOGIN) and value({STATUS}|{LOGINID})
                // LOGIN:{STATUS}|{LOGINID};
                String[] msg = line[i].Split(':');
                if (msg[0].Equals("LOGIN") && msg.Length == 2)
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
                            username.Enabled = true;
                            password.Enabled = true;
                            loginButton.Enabled = true;
                            game.hideMessageDialog();
                            LoggedUserID = Convert.ToInt32(idMsg);
                            state = GameState.login_loggedin;
                        }
                        catch (Exception e)
                        {
                            // Has any error come here
                            // I was expect that NumberFormatException can occur
                            Debug.WriteLine(e.ToString());
                            username.Enabled = true;
                            password.Enabled = true;
                            loginButton.Enabled = true;
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("Wrong username or password.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                            LoggedUserID = 0;
                            state = GameState.none;
                        }
                    }
                    else
                    {
                        // if status isn't OK
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
                        username.Enabled = true;
                        password.Enabled = true;
                        loginButton.Enabled = true;
                        if (idMsg.Equals(0))
                        {
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("Wrong username or password.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                        }
                        else
                        {
                            game.messageDialog.setTitle("ERROR!!");
                            game.messageDialog.setMessage("This user is already login.");
                            game.messageDialog.Visible = true;
                            game.messageDialog.CloseButtonVisible = true;
                        }
                        LoggedUserID = 0;
                        break;
                    }
                }
            }
        }

        void btnLogin_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (!username.Text.Equals("") && !password.Text.Equals(""))
            {
                username.Enabled = false;
                password.Enabled = false;
                loginButton.Enabled = false;
                game.messageDialog.setTitle("");
                game.messageDialog.setMessage("Please wait...");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = false;
                Thread threader = new Thread(new ThreadStart(loginThread));
                threader.Start();
            }
            else
            {
                game.messageDialog.setTitle("ERROR!!");
                game.messageDialog.setMessage("Please enter your username and password.");
                game.messageDialog.Visible = true;
                game.messageDialog.CloseButtonVisible = true;
            }
        }

        void btnExit_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            game.hideMessageDialog();
            game.Exit();
        }

        public int getUserId()
        {
            return LoggedUserID;
        }

        public void reset()
        {
            username.Enabled = true;
            password.Enabled = true;
            loginButton.Enabled = true;
            state = GameState.none;
            LoggedUserID = 0;
            username.Text = "";
            password.Text = "";
        }

        public void currentState(GameState state)
        {
            this.state = state;
        }

        public GameState currentState()
        {
            return state;
        }
    }
}

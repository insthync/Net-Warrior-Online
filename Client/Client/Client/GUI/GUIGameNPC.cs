using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    public class GUIGameNPC : Window
    {
        private Manager manager;
        private ImageBox msg;
        private ScrollBar vert; // Vertical scrollbar  
        private ScrollBar horz; // Horizontal scrollbar
        private ListBox menu;
        private Button btn;
        private Network network;
        private int npcid;
        private Dictionary<int, String> txtmenu;
        private Dictionary<String, Texture2D> msglist;
        private ContentManager content;
        public GUIGameNPC(Manager manager, Network network, ContentManager content, Dictionary<String, String> msgTextureList)
            : base(manager)
        {
            this.manager = manager;
            this.network = network;
            this.content = content;
            this.txtmenu = new Dictionary<int, String>();
            this.msglist = new Dictionary<String, Texture2D>();
            foreach (String key in msgTextureList.Keys)
                msglist.Add(key, content.Load<Texture2D>(msgTextureList[key]));
            // Define window property
            Init();
            Text = "Message";
            Width = 768;
            Height = 392;
            Top = (manager.ScreenHeight - Height) / 2;
            Left = (manager.ScreenWidth - Width) / 2;
            MinimumHeight = 392;
            MinimumWidth = 768;
            MaximumHeight = 392;
            MaximumWidth = 768;
            Alpha = 220;
            CloseButtonVisible = false;
            Resizable = false;

            // Setup of the vertical scrollbar      
            vert = new ScrollBar(manager, Orientation.Vertical);
            vert.Init();
            vert.Parent = this;
            vert.Top = 0;
            vert.Left = ClientWidth - vert.Width;
            vert.Height = ClientHeight - vert.Width - 98;
            vert.Value = 0;
            vert.Anchor = Anchors.Top | Anchors.Right | Anchors.Bottom;

            // Setup of the horizontal scrollbar
            horz = new ScrollBar(manager, Orientation.Horizontal);
            horz.Init();
            horz.Parent = this;
            horz.Left = 0;
            horz.Top = ClientHeight - horz.Height - 98;
            horz.Width = ClientWidth - vert.Width;
            horz.Value = 0;
            horz.Anchor = Anchors.Left | Anchors.Right | Anchors.Bottom;

            msg = new ImageBox(manager);
            msg.Init();
            msg.Parent = this;
            msg.Left = 0;
            msg.Top = 0;
            msg.Width = ClientWidth - vert.Width;
            msg.Height = ClientHeight - horz.Height - 98;
            msg.Anchor = Anchors.All;
            Add(msg);

            menu = new ListBox(manager);
            menu.Init();
            menu.Parent = this;
            menu.Left = 1;
            menu.Width = 752;
            menu.Top = 260;
            //menu.Visible = false;
            Add(menu);

            btn = new Button(manager);
            btn.Init();
            btn.Parent = this;
            btn.Width = 140;
            btn.Left = (Width - btn.Width) / 2;
            btn.Top = 328;
            btn.Text = "OK";
            btn.Click += new TomShane.Neoforce.Controls.EventHandler(btn_Click);
            //btn.Visible = false;
            Add(btn);

            vert.ValueChanged += new TomShane.Neoforce.Controls.EventHandler(ValueChanged);
            horz.ValueChanged += new TomShane.Neoforce.Controls.EventHandler(ValueChanged);
            Recalc(this, null); // Calculates initial properties of the scrollbars
        }

        void Recalc(object sender, ResizeEventArgs e)
        {
            // Disable scrollbars when there is nothing to scroll
            horz.Enabled = msg.Width < (msg.Image == null ? 0 : msg.Image.Width);
            vert.Enabled = msg.Height < (msg.Image == null ? 0 : msg.Image.Height);

            vert.Range = (msg.Image == null ? 0 : msg.Image.Height);  // Set range to width of the image, one step equals to one pixel of the image
            vert.PageSize = msg.Height; // Size of the slider according to displayed portion of the image

            horz.Range = (msg.Image == null ? 0 : msg.Image.Width); // Same like above, just for horizontal scrollbar
            horz.PageSize = msg.Width;

            // Portion of the image we are actually displaying in the imagebox
            msg.SourceRect = new Rectangle(horz.Value, vert.Value, msg.Width, msg.Height);

            // Make the imagebox redraw
            msg.Invalidate();
        }

        void ValueChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            // For every scrollbar value change we recalt properties
            Recalc(sender, null);
        }

        void btn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (txtmenu.ContainsKey(menu.ItemIndex))
            {
                // Send chat message
                if (network.isConnected())
                    network.Send("NPCDIALOG:" + npcid + " " + txtmenu[menu.ItemIndex] + ";");
                this.Visible = false;
            }
        }

        public void setDialog(int npcid, String txtmsg, Dictionary<String, String> txtmenu)
        {
            this.npcid = npcid;
            horz.Value = 0;
            vert.Value = 0;
            if (msglist.ContainsKey(txtmsg))
            {
                msg.Image = msglist[txtmsg];
            }
            this.txtmenu.Clear();
            menu.Items.Clear();
            int i = 0;
            foreach (String key in txtmenu.Keys)
            {
                String menuStr = txtmenu[key];
                menuStr = menuStr.Replace("'58'", ":");
                menuStr = menuStr.Replace("'59'", ";");
                menuStr = menuStr.Replace("'32'", " ");
                menuStr = menuStr.Replace("'39'", "'");
                menu.Items.Add(menuStr);
                this.txtmenu.Add(i++, key);
            }
            menu.ItemIndex = 0;
            this.Visible = true;
            Recalc(this, null); // Calculates initial properties of the scrollbars
        }
    }
}

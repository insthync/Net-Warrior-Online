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
    public class GUIGameEnvironment : Window
    {
        private Button attributeButton;
        private Button skillButton;
        private Button equipmentButton;
        private Button inventoryButton;
        private Button systemButton;
        private TrackBar hpBar;
        private TrackBar spBar;
        private TrackBar expBar;
        private Manager manager;
        private GUIGameAttribute guiGameAttribute = null;
        private GUIGameSkill guiGameSkill = null;
        private GUIGameEquipment guiGameEquipment = null;
        private GUIGameInventory guiGameInventory = null;
        private GUIGameMenu guiGameMenu = null;
        public GUIGameEnvironment(Manager manager, GUIGameAttribute guiGameAttribute = null, GUIGameSkill guiGameSkill = null, GUIGameEquipment guiGameEquipment = null, GUIGameInventory guiGameInventory = null, GUIGameMenu guiGameMenu = null)
            : base(manager)
        {
            this.manager = manager;
            this.guiGameAttribute = guiGameAttribute;
            this.guiGameSkill = guiGameSkill;
            this.guiGameEquipment = guiGameEquipment;
            this.guiGameInventory = guiGameInventory;
            this.guiGameMenu = guiGameMenu;
            Init();
            Width = 395;
            Height = 80;
            Top = -15;
            Left = (manager.ScreenWidth - Width) / 2;
            Alpha = 220;
            CaptionVisible = false;
            CloseButtonVisible = false;
            Resizable = false;
            Movable = false;

            attributeButton = new Button(manager);
            attributeButton.Init();
            attributeButton.Text = "Attributes";
            attributeButton.Enabled = true;
            attributeButton.Parent = this;
            attributeButton.Width = 70;
            attributeButton.Top = 10;
            attributeButton.Left = 5;
            attributeButton.Click += new TomShane.Neoforce.Controls.EventHandler(attributeButton_Click);
            Add(attributeButton);

            skillButton = new Button(manager);
            skillButton.Init();
            skillButton.Text = "Skills";
            skillButton.Enabled = true;
            skillButton.Parent = this;
            skillButton.Width = 70;
            skillButton.Top = 10;
            skillButton.Left = 80;
            skillButton.Click += new TomShane.Neoforce.Controls.EventHandler(skillButton_Click);
            Add(skillButton);

            equipmentButton = new Button(manager);
            equipmentButton.Init();
            equipmentButton.Text = "Equipment";
            equipmentButton.Enabled = true;
            equipmentButton.Parent = this;
            equipmentButton.Width = 70;
            equipmentButton.Top = 10;
            equipmentButton.Left = 155;
            equipmentButton.Click += new TomShane.Neoforce.Controls.EventHandler(equipmentButton_Click);
            Add(equipmentButton);

            inventoryButton = new Button(manager);
            inventoryButton.Init();
            inventoryButton.Text = "Inventory";
            inventoryButton.Enabled = true;
            inventoryButton.Parent = this;
            inventoryButton.Width = 70;
            inventoryButton.Top = 10;
            inventoryButton.Left = 230;
            inventoryButton.Click += new TomShane.Neoforce.Controls.EventHandler(inventoryButton_Click);
            Add(inventoryButton);

            systemButton = new Button(manager);
            systemButton.Init();
            systemButton.Text = "System";
            systemButton.Enabled = true;
            systemButton.Parent = this;
            systemButton.Width = 70;
            systemButton.Top = 10;
            systemButton.Left = 305;
            systemButton.Click += new TomShane.Neoforce.Controls.EventHandler(systemButton_Click);
            Add(systemButton);

            hpBar = new TrackBar(manager);
            hpBar.Init();
            hpBar.Value = 75;
            hpBar.Range = 100;
            hpBar.Enabled = false;
            hpBar.Parent = this;
            hpBar.Width = 183;
            hpBar.Height = 20;
            hpBar.Top = 35;
            hpBar.Left = 5;
            hpBar.Color = Color.Red;
            hpBar.SliderButtonVisible = false;
            Add(hpBar);

            spBar = new TrackBar(manager);
            spBar.Init();
            spBar.Value = 75;
            spBar.Range = 100;
            spBar.Enabled = false;
            spBar.Parent = this;
            spBar.Width = 183;
            spBar.Height = 20;
            spBar.Top = 35;
            spBar.Left = 192;
            spBar.Color = Color.Blue;
            spBar.SliderButtonVisible = false;
            Add(spBar);

            expBar = new TrackBar(manager);
            expBar.Init();
            expBar.Value = 75;
            expBar.Range = 100;
            expBar.Enabled = false;
            expBar.Parent = this;
            expBar.Width = 370;
            expBar.Height = 10;
            expBar.Top = 53;
            expBar.Left = 5;
            expBar.Color = Color.Yellow;
            expBar.SliderButtonVisible = false;
            Add(expBar);
        }

        void systemButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (guiGameMenu != null)
            {
                guiGameMenu.Visible = !guiGameMenu.Visible;
            }
        }

        void inventoryButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (guiGameInventory != null)
            {
                guiGameInventory.Visible = !guiGameInventory.Visible;
            }
        }

        void equipmentButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (guiGameEquipment != null)
            {
                guiGameEquipment.Visible = !guiGameEquipment.Visible;
            }
        }

        void skillButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (guiGameSkill != null)
            {
                guiGameSkill.Visible = !guiGameSkill.Visible;
            }
        }

        void attributeButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (guiGameAttribute != null)
            {
                guiGameAttribute.Visible = !guiGameAttribute.Visible;
            }
        }


        public void setValues(CharacterInformation charinfo)
        {
            hpBar.Value = charinfo.getChar_curhp();
            spBar.Value = charinfo.getChar_cursp();
            hpBar.Range = charinfo.getChar_hp();
            spBar.Range = charinfo.getChar_sp();
            expBar.Value = charinfo.getChar_curexp();
            expBar.Range = charinfo.getChar_exp();
        }
    }
}

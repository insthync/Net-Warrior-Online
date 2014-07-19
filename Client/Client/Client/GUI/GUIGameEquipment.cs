using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{

    public class GUIGameEquipment : Window
    {
        private enum equip_pos
        {
            equip_head = 2,
            equip_body = 4,
            equip_hand = 8,
            equip_foot = 16,
            equip_weaponR = 32,
            equip_weaponL = 64
        };
        private Manager manager;
        private Network network;
        private ContentManager content;
        private GraphicsDeviceManager graphics;

        private ImageBox equip_head;
        private ImageBox equip_body;
        private ImageBox equip_hand;
        private ImageBox equip_foot;
        private ImageBox equip_weaponR;
        private ImageBox equip_weaponL;

        private Dictionary<int, Texture2D> itemlist;
        private Dictionary<int, InventoryItemData> itemData;

        private int selectedItemIndex = -1;
        private Texture2D selectedItemTexture = null;
        private Texture2D nullTexture = null;
        private Texture2D noItem = null;

        private GUIGameInventory guiGameInventory = null;
        public GUIGameEquipment(Manager manager, Network network, ContentManager content, GraphicsDeviceManager graphics, Dictionary<int, GameItemConfig> itemConfigList)
            : base(manager)
        {
            this.manager = manager;
            this.network = network;
            this.content = content;
            this.graphics = graphics;
            this.nullTexture = new Texture2D(graphics.GraphicsDevice, 32, 32);
            this.noItem = content.Load<Texture2D>("no-item");
            this.itemlist = new Dictionary<int, Texture2D>();
            foreach (short key in itemConfigList.Keys)
                itemlist.Add(key, content.Load<Texture2D>(itemConfigList[key].inventoryTexturePath));
            Init();
            Text = "Equipment";
            Width = 170;
            Height = 340;
            Left = 15;
            ClientMargins = new Margins(0, 50, 0, 0);
            Top = (manager.ScreenHeight - Height) / 2;
            Alpha = 220;
            Resizable = false;

            ImageBox equip_head_bg = new ImageBox(manager);
            equip_head_bg.Init();
            equip_head_bg.Parent = this;
            equip_head_bg.Width = 32;
            equip_head_bg.Height = 32;
            equip_head_bg.Text = "";
            equip_head_bg.Image = this.noItem;
            equip_head_bg.Left = (Width - equip_head_bg.Width) / 2;
            equip_head_bg.Top = 15;

            ImageBox equip_body_bg = new ImageBox(manager);
            equip_body_bg.Init();
            equip_body_bg.Parent = this;
            equip_body_bg.Width = 32;
            equip_body_bg.Height = 32;
            equip_body_bg.Text = "";
            equip_body_bg.Image = this.noItem;
            equip_body_bg.Left = (Width - equip_head_bg.Width) / 2;
            equip_body_bg.Top = 80;

            ImageBox equip_hand_bg = new ImageBox(manager);
            equip_hand_bg.Init();
            equip_hand_bg.Parent = this;
            equip_hand_bg.Width = 32;
            equip_hand_bg.Height = 32;
            equip_hand_bg.Text = "";
            equip_hand_bg.Image = this.noItem;
            equip_hand_bg.Left = 15;
            equip_hand_bg.Top = 145;

            ImageBox equip_foot_bg = new ImageBox(manager);
            equip_foot_bg.Init();
            equip_foot_bg.Parent = this;
            equip_foot_bg.Width = 32;
            equip_foot_bg.Height = 32;
            equip_foot_bg.Text = "";
            equip_foot_bg.Image = this.noItem;
            equip_foot_bg.Left = Width - equip_foot_bg.Width - 15;
            equip_foot_bg.Top = 145;

            ImageBox equip_weaponR_bg = new ImageBox(manager);
            equip_weaponR_bg.Init();
            equip_weaponR_bg.Parent = this;
            equip_weaponR_bg.Width = 32;
            equip_weaponR_bg.Height = 32;
            equip_weaponR_bg.Text = "";
            equip_weaponR_bg.Image = this.noItem;
            equip_weaponR_bg.Left = 15;
            equip_weaponR_bg.Top = 80;

            ImageBox equip_weaponL_bg = new ImageBox(manager);
            equip_weaponL_bg.Init();
            equip_weaponL_bg.Parent = this;
            equip_weaponL_bg.Width = 32;
            equip_weaponL_bg.Height = 32;
            equip_weaponL_bg.Text = "";
            equip_weaponL_bg.Image = this.noItem;
            equip_weaponL_bg.Left = Width - equip_foot_bg.Width - 15;
            equip_weaponL_bg.Top = 80;

            // real pos
            equip_head = new ImageBox(manager);
            equip_head.Init();
            equip_head.Name = "" + (int)equip_pos.equip_head;
            equip_head.Parent = this;
            equip_head.Width = 32;
            equip_head.Height = 32;
            equip_head.Text = "";
            equip_head.Image = this.nullTexture;
            equip_head.Left = (Width - equip_head.Width) / 2;
            equip_head.Top = 15;
            equip_head.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
            equip_head.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);

            equip_body = new ImageBox(manager);
            equip_body.Init();
            equip_body.Name = "" + (int)equip_pos.equip_body;
            equip_body.Parent = this;
            equip_body.Width = 32;
            equip_body.Height = 32;
            equip_body.Text = "";
            equip_body.Image = this.nullTexture;
            equip_body.Left = (Width - equip_head.Width) / 2;
            equip_body.Top = 80;
            equip_body.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
            equip_body.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);

            equip_hand = new ImageBox(manager);
            equip_hand.Init();
            equip_hand.Name = "" + (int)equip_pos.equip_hand;
            equip_hand.Parent = this;
            equip_hand.Width = 32;
            equip_hand.Height = 32;
            equip_hand.Text = "";
            equip_hand.Image = this.nullTexture;
            equip_hand.Left = 15;
            equip_hand.Top = 145;
            equip_hand.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
            equip_hand.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);

            equip_foot = new ImageBox(manager);
            equip_foot.Init();
            equip_foot.Name = "" + (int)equip_pos.equip_foot;
            equip_foot.Parent = this;
            equip_foot.Width = 32;
            equip_foot.Height = 32;
            equip_foot.Text = "";
            equip_foot.Image = this.nullTexture;
            equip_foot.Left = Width - equip_foot.Width - 15;
            equip_foot.Top = 145;
            equip_foot.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
            equip_foot.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);

            equip_weaponR = new ImageBox(manager);
            equip_weaponR.Init();
            equip_weaponR.Name = "" + (int)equip_pos.equip_weaponR;
            equip_weaponR.Parent = this;
            equip_weaponR.Width = 32;
            equip_weaponR.Height = 32;
            equip_weaponR.Text = "";
            equip_weaponR.Image = this.nullTexture;
            equip_weaponR.Left = 15;
            equip_weaponR.Top = 80;
            equip_weaponR.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
            equip_weaponR.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);

            equip_weaponL = new ImageBox(manager);
            equip_weaponL.Init();
            equip_weaponL.Name = "" + (int)equip_pos.equip_weaponL;
            equip_weaponL.Parent = this;
            equip_weaponL.Width = 32;
            equip_weaponL.Height = 32;
            equip_weaponL.Text = "";
            equip_weaponL.Image = this.nullTexture;
            equip_weaponL.Left = Width - equip_foot.Width - 15;
            equip_weaponL.Top = 80;
            equip_weaponL.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
            equip_weaponL.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);

            itemData = new Dictionary<int, InventoryItemData>();
        }

        public void init(GUIGameInventory guiGameInventory)
        {
            this.guiGameInventory = guiGameInventory;
        }

        public void setItemData(int index, InventoryItemData data)
        {
            if (itemData.ContainsKey(index))
                itemData.Remove(index);
            itemData.Add(index, data);
            setItemToIndex(data.getItemID(), index);
        }

        public int getSelectedItemIndex()
        {
            return selectedItemIndex;
        }

        public void drawSelectedItemTexturePosition(SpriteBatch spriteBatch)
        {
            if (selectedItemIndex >= 0 && selectedItemTexture != null)
                spriteBatch.Begin();
            Vector2 pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            spriteBatch.Draw(selectedItemTexture, pos, Color.White);
            spriteBatch.End();
        }

        public InventoryItemData getItemData(int index)
        {
            if (itemData.ContainsKey(index))
                return itemData[index];
            return null;
        }

        public void clearItemData()
        {
            itemData.Clear();
        }

        public bool isSameItemData(int index, InventoryItemData data)
        {
            if (itemData.ContainsKey(index))
                return itemData[index].Equals(data);
            return false;
        }

        void item_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            int equipIdx = Convert.ToInt32(((ImageBox)sender).Name);
            if (guiGameInventory != null)
            {
                if (guiGameInventory.getSelectedItemIndex() < 0)
                {

                    if (selectedItemIndex >= 0)
                    {
                        if (selectedItemIndex == equipIdx)
                        {
                            ((ImageBox)sender).Image = selectedItemTexture;
                            selectedItemTexture = null;
                            selectedItemIndex = -1;
                        }
                    }
                    else
                    {
                        selectedItemTexture = ((ImageBox)sender).Image;
                        ((ImageBox)sender).Image = nullTexture;
                        selectedItemIndex = equipIdx;
                    }
                }
                else
                {
                    // Use selected item
                    if (network.isConnected())
                    {
                        network.Send("INVENTORYUSE:" + guiGameInventory.getSelectedItemIndex() + ";");
                        guiGameInventory.clearSelectedItem();
                    }
                }
            }
        }

        void item_DoubleClick(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            // Unequip
            int equipIdx = Convert.ToInt32(((ImageBox)sender).Name);
            if (guiGameInventory != null)
            {
                if (/*selectedItemIndex < 0 && */guiGameInventory.getSelectedItemIndex() < 0)
                {
                    if (selectedItemIndex >= 0)
                    {
                        clearSelectedItem();
                    }
                    if (equipIdx >= 0 && network.isConnected())
                    {
                        network.Send("UNWEAR:" + equipIdx + ";");
                    }
                }
            }
        }

        public void setItemToIndex(int itemId, int itemIndex)
        {
            // Set Item Texture
            switch (itemIndex)
            {
                case (int)equip_pos.equip_head:
                    if (itemId > 0)
                        equip_head.Image = itemlist[itemId];
                    else
                        equip_head.Image = nullTexture;
                    break;
                case (int)equip_pos.equip_body:
                    if (itemId > 0)
                        equip_body.Image = itemlist[itemId];
                    else
                        equip_body.Image = nullTexture;
                    break;
                case (int)equip_pos.equip_hand:
                    if (itemId > 0)
                        equip_hand.Image = itemlist[itemId];
                    else
                        equip_hand.Image = nullTexture;
                    break;
                case (int)equip_pos.equip_foot:
                    if (itemId > 0)
                        equip_foot.Image = itemlist[itemId];
                    else
                        equip_foot.Image = nullTexture;
                    break;
                case (int)equip_pos.equip_weaponR:
                    if (itemId > 0)
                        equip_weaponR.Image = itemlist[itemId];
                    else
                        equip_weaponR.Image = nullTexture;
                    break;
                case (int)equip_pos.equip_weaponL:
                    if (itemId > 0)
                        equip_weaponL.Image = itemlist[itemId];
                    else
                        equip_weaponL.Image = nullTexture;
                    break;
                default:
                    break;
            }
        }

        public void clearSelectedItem()
        {
            selectedItemTexture = null;
            selectedItemIndex = -1;
        }

        public void clearEquipmentData()
        {
            equip_head.Image = this.nullTexture;
            equip_body.Image = this.nullTexture;
            equip_hand.Image = this.nullTexture;
            equip_foot.Image = this.nullTexture;
            equip_weaponR.Image = this.nullTexture;
            equip_weaponL.Image = this.nullTexture;
            clearItemData();
            selectedItemTexture = null;
            selectedItemIndex = -1;
        }
    }
}

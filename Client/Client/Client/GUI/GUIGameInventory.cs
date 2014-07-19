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
    public class GUIGameInventory : Window
    {
        private Manager manager;
        private Network network;
        private ContentManager content;
        private GraphicsDeviceManager graphics;
        private Label space;

        private List<ImageBox> items;

        private int _x = 0;
        private int _y = 0;

        private Dictionary<int, Texture2D> itemlist;
        private Dictionary<int, InventoryItemData> itemData;

        private int selectedItemIndex = -1;
        private Texture2D selectedItemTexture = null;
        private Texture2D nullTexture = null;
        private Texture2D noItem = null;

        private GUIGameEquipment guiGameEquipment = null;
        public GUIGameInventory(Manager manager, Network network, ContentManager content, GraphicsDeviceManager graphics, Dictionary<int, GameItemConfig> itemConfigList)
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
            Text = "Inventory";
            Width = 170;
            Height = 340;
            Left = manager.ScreenWidth - Width - 15;
            Top = (manager.ScreenHeight - Height) / 2;
            Alpha = 220;
            Resizable = false;

            items = new List<ImageBox>();
            for (int i = 0; i < 120; ++i)
            {
                if (i != 0 && i % 4 == 0)
                {
                    _x = 0;
                    ++_y;
                }
                ImageBox bg = new ImageBox(manager);
                bg.Init();
                bg.Parent = this;
                bg.Width = 32;
                bg.Height = 32;
                bg.Text = "";
                bg.Image = this.noItem;
                bg.Left = 2 + (34 * _x);
                bg.Top = 2 + (34 * _y);

                ImageBox item = new ImageBox(manager);
                item.Init();
                item.Name = "" + i;
                item.Parent = this;
                item.Width = 32;
                item.Height = 32;
                item.Text = "";
                item.Image = this.nullTexture;
                item.Left = 2 + (34 * _x);
                item.Top = 2 + (34 * _y);

                // Event
                item.Click += new TomShane.Neoforce.Controls.EventHandler(item_Click);
                item.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(item_DoubleClick);
                items.Add(item);

                ++_x;
            }

            space = new Label(manager);
            space.Init();
            space.Parent = this;
            space.Width = 34 * 4;
            space.Height = 2;
            space.Text = "";
            space.Left = 2;
            space.Top = 2 + (34 * ++_y);

            itemData = new Dictionary<int, InventoryItemData>();
        }

        public void init(GUIGameEquipment guiGameEquipment)
        {
            this.guiGameEquipment = guiGameEquipment;
        }

        public void setItemData(int index, InventoryItemData data)
        {
            if (itemData.ContainsKey(index))
                itemData.Remove(index);
            itemData.Add(index, data);
            setItemToIndex(data.getItemID(), index);
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
            // Drag Item
            int itemIdx = Convert.ToInt32(((ImageBox)sender).Name);
            if (guiGameEquipment != null)
            {
                if (guiGameEquipment.getSelectedItemIndex() < 0)
                {
                    if (selectedItemIndex >= 0)
                    {
                        if (selectedItemIndex == itemIdx)
                        {
                            ((ImageBox)sender).Image = selectedItemTexture;
                            selectedItemTexture = null;
                            selectedItemIndex = -1;
                        }
                        else
                        {
                            if (network.isConnected())
                            {
                                network.Send("INVENTORYDRAG:" + selectedItemIndex + " " + itemIdx + ";");
                                selectedItemTexture = null;
                                selectedItemIndex = -1;
                            }
                        }
                    }
                    else
                    {
                        selectedItemTexture = ((ImageBox)sender).Image;
                        ((ImageBox)sender).Image = nullTexture;
                        selectedItemIndex = itemIdx;
                    }
                }
                else
                {
                    // Unequip
                    if (network.isConnected())
                    {
                        network.Send("UNWEARDRAGTO:" + guiGameEquipment.getSelectedItemIndex() + " " + itemIdx + ";");
                        guiGameEquipment.clearSelectedItem();
                    }
                }
            }
        }

        void item_DoubleClick(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            // Use Item
            int itemIdx = Convert.ToInt32(((ImageBox)sender).Name);
            if (guiGameEquipment != null)
            {
                if (/*selectedItemIndex < 0 && */guiGameEquipment.getSelectedItemIndex() < 0)
                {
                    if (selectedItemIndex >= 0)
                    {
                        clearSelectedItem();
                    }
                    if (itemIdx >= 0 && network.isConnected())
                    {
                        network.Send("INVENTORYUSE:" + itemIdx + ";");
                    }
                }
            }
        }

        public void setItemToIndex(int itemId, int itemIndex)
        {
            // Set Item Texture
            if (items.Count >= itemIndex)
            {
                if (itemId > 0)
                    items[itemIndex].Image = itemlist[itemId];
                else
                    items[itemIndex].Image = nullTexture;
            }
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

        public void clearSelectedItem()
        {
            selectedItemTexture = null;
            selectedItemIndex = -1;
        }

        public void clearInventoryData()
        {
            for (int i = 0; i < items.Count; ++i)
            {
                items[i].Image = this.nullTexture;
            }
            clearItemData();
            selectedItemTexture = null;
            selectedItemIndex = -1;
        }
    }
}

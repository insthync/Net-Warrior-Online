using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MMORPGCopierClient
{
    public class GameItemBank
    {
        private Dictionary<short, GameItemConfig> itemList;
        public GameItemBank(ContentManager content)
        {
            itemList = new Dictionary<short, GameItemConfig>();
        }

        public void Load(short key, GameItemConfig cfg)
        {
            itemList.Add(key, cfg);
        }

        public String getDropModelPath(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.dropModelPath;
            return null;
        }

        public Vector3 getDropModelRotation(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.dropRotation;
            return Vector3.Zero;
        }

        public Vector3 getDropModelPosition(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.dropPosition;
            return Vector3.Zero;
        }

        public float getDropModelScale(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.dropScale;
            return 0;
        }

        public String getName(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.name;
            return null;
        }

        public String getDescription(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.description;
            return null;
        }

        public short getEquipModelID(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.equipModelID;
            return 0;
        }

        public String getInventoryTexturePath(short key)
        {
            GameItemConfig cfg = null;
            if (itemList.TryGetValue(key, out cfg))
                return cfg.inventoryTexturePath;
            return null;
        }

        public GameItemConfig getItemConfig(short key)
        {
            GameItemConfig cfg = null;
            itemList.TryGetValue(key, out cfg);
            return cfg;
        }
    }
}

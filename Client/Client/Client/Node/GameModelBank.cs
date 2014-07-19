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
    public class GameModelBank
    {
        private Dictionary<short, GameModelConfig> modelList;
        private ContentManager content;
        public GameModelBank(ContentManager content)
        {
            this.content = content;
            modelList = new Dictionary<short, GameModelConfig>();
        }

        public void Load(short key, GameModelConfig cfg)
        {
            modelList.Add(key, cfg);
        }

        public String getModelPath(short key)
        {
            GameModelConfig cfg = null;
            if (modelList.TryGetValue(key, out cfg))
                return cfg.path;
            return null;
        }

        public Vector3 getModelRotation(short key)
        {
            GameModelConfig cfg = null;
            if (modelList.TryGetValue(key, out cfg))
                return cfg.Rotation;
            return Vector3.Zero;
        }

        public Vector3 getModelPosition(short key)
        {
            GameModelConfig cfg = null;
            if (modelList.TryGetValue(key, out cfg))
                return cfg.Position;
            return Vector3.Zero;
        }

        public float getModelScale(short key)
        {
            GameModelConfig cfg = null;
            if (modelList.TryGetValue(key, out cfg))
                return cfg.Scale;
            return 0;
        }

        public String getModelClipName(short key, int state)
        {
            GameModelConfig cfg = null;
            if (modelList.TryGetValue(key, out cfg))
            {
                String clipname = "";
                cfg.stateClip.TryGetValue((short)state, out clipname);
                return clipname;
            }
            return null;
        }

        public GameModelConfig getModelConfig(short key)
        {
            GameModelConfig cfg = null;
            modelList.TryGetValue(key, out cfg);
            return cfg;
        }
    }
}

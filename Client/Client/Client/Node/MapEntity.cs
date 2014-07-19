using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MMORPGCopierClient
{
    public class MapEntity : GameModel
    {
        private int id;
        private Vector3 Position = Vector3.Zero;
        private Vector3 Rotation = Vector3.Zero;
        private float Scale = 1.0f;
        private Matrix worldMatrix = Matrix.Identity;
        private String bgm = "";
        public MapEntity(int id, GameMapConfig cfg, ContentManager content) : base(content)
        {
            this.id = id;
            // Load Model
            this.Load(cfg.path);
            this.Position = cfg.Position;
            this.Rotation = cfg.Rotation;
            this.Scale = cfg.Scale;
            // MAP BGM
            this.bgm = cfg.bgm;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            this.Draw(view, projection, Position, Rotation, Scale);
        }

        public String getBGM()
        {
            return bgm;
        }
    }
}

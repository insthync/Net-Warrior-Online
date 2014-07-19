using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MMORPGCopierClient
{
    public class WarpEntity : GameModel
    {
        private int id;
        private Vector3 Position;
        public WarpEntity(int id, float x, float y, ContentManager content) : base(content)
        {
            this.id = id;
            // Load Model
            this.Load("warp");
            this.Position = new Vector3(x, 0, y);
        }

        public bool RayIntersectsModel(Ray ray)
        {
            return this.RayIntersectsModel(ray, Position, Vector3.Zero, 1.0f);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            this.Draw(view, projection, Position, Vector3.Zero, 1.0f);
        }

        public int getID()
        {
            return id;
        }

        public Vector3 getPosition()
        {
            return Position;
        }

        public void setPosition(Vector3 position)
        {
            Position = position;
        }
    }
}

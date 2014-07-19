using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MMORPGCopierClient
{
    public class GameModelNode : GameModel
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 _Position;
        public Vector3 _Rotation;
        public float Scale;

        private ContentManager content;
        private GameModelBank bank;

        private short modelKey = 0;

        public GameModelNode(ContentManager content, GameModelBank bank) : base(content)
        {
            this.content = content;
            this.bank = bank;
            Position = new Vector3(0, 0, 0);
            _Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);
            _Rotation = new Vector3(0, 0, 0);
            Scale = 1;
        }

        public void Load(short modelKey)
        {
            Load(bank.getModelPath(this.modelKey = modelKey));
        }

        public void playClip(short state, bool isLoop)
        {
            playClip(bank.getModelClipName(this.modelKey, state), isLoop);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Draw(view, projection, Position + _Position, Rotation - _Rotation, Scale);
        }

        public bool RayIntersectsModel(Ray ray)
        {
            return RayIntersectsModel(ray, Position + _Position, Rotation - _Rotation, Scale);
        }
    }
}

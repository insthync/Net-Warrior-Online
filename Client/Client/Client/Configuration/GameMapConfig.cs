using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MMORPGCopierClient
{
    public class GameMapConfig
    {
        public String path = "";
        public String bgm = "";
        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Rotation = new Vector3(0, 0, 0);
        public float Scale = 1;
        public GameMapConfig(String path, String bgm, Vector3 Position, Vector3 Rotation, float Scale)
        {
            this.path = path;
            this.bgm = bgm;
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
        }
    }
}

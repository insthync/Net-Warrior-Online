using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MMORPGCopierClient
{
    public class GameModelConfig
    {
        public String path = "";
        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Rotation = new Vector3(0, 0, 0);
        public float Scale = 1;
        public Dictionary<short, String> stateClip;
        public GameModelConfig(String path, Vector3 Position, Vector3 Rotation, float Scale, Dictionary<short, String> stateClip)
        {
            this.path = path;
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.stateClip = stateClip;
        }
    }
}

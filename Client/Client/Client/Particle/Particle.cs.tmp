using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Based on Particle system by Rachan Neamprasert
// www.hardworkerstudio.com
// Email hwrstudio@gmail.com

namespace MMORPGCopierClient
{
    public struct Particle
    {
        public Vector3 Position;
        public Vector2 Position2d;
        public Vector2 Scale2d;
        public Vector2 Scale2dUp;
        public Vector2 Scale2dUp2;
        public Vector3 Scale;
        public Vector3 Rotation;
        public float Rotation2d;
        public float Rotation2dUp;
        public float Rotation2dUp2;
        public Matrix World;
        public float Alpha;
        public float AlphaUp1;
        public float AlphaUp2;
        public Texture2D texture;
        public Texture2D norm;
        public Model model;
        public Vector3 baseColor;
        public Vector3 baseColorUp1;
        public Vector3 baseColorUp2;
        public int NumEffect;

        public Vector3 PowerUp1;
        public Vector3 ScaleUp1;
        public Vector3 RotationUp1;

        public Vector3 PowerUp2;
        public Vector3 ScaleUp2;
        public Vector3 RotationUp2;
        public SpriteFont Font;
        public Color Color;
        public string Text;

        public bool AdditiveRender;
        public float LifeTime;
        public bool Active;
        public int Technic;
        public int[] ParticleNodes;

        public void Reset()
        {
            Position = Vector3.Zero;
            Position2d = Vector2.Zero;
            Scale2d = Vector2.Zero;
            Scale = Vector3.Zero;
            Rotation2d = 0;
            Rotation2dUp = 0;
            Rotation = Vector3.Zero;
            World = Matrix.Identity;
            Alpha = 0;
            AlphaUp1 = 0;
            AlphaUp2 = 0;
            baseColor = Vector3.Zero;
            baseColorUp1 = Vector3.Zero;
            baseColorUp2 = Vector3.Zero;
            PowerUp1 = Vector3.Zero;
            ScaleUp1 = Vector3.Zero;
            RotationUp1 = Vector3.Zero;
            PowerUp2 = Vector3.Zero;
            ScaleUp2 = Vector3.Zero;
            RotationUp2 = Vector3.Zero;
            Text = "";
            LifeTime = 0;
            AdditiveRender = false;
            Technic = 1;
            Active = false;
        }
    }

}

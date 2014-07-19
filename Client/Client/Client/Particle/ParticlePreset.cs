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
    public class ParticlePreset : ParticleManager
    {
        public ParticlePreset(Game game, GraphicsDeviceManager graphics, GameCamera camera)
            : base(game, graphics, camera)
        {
        }

        public override void LoadContent()
        {
            textureset.Add(0, game.Content.Load<Texture2D>("Particles/smoke"));
            textureset.Add(1, game.Content.Load<Texture2D>("Particles/Particle"));
            textureset.Add(2, game.Content.Load<Texture2D>("Particles/Fire"));
            textureset.Add(3, game.Content.Load<Texture2D>("Particles/Smoke2"));
            textureset.Add(4, game.Content.Load<Texture2D>("Particles/Smoke3"));
            textureset.Add(5, game.Content.Load<Texture2D>("Particles/Attack"));
            textureset.Add(6, game.Content.Load<Texture2D>("Particles/Ring"));
            textureset.Add(7, game.Content.Load<Texture2D>("Particles/Particle"));
            textureset.Add(8, game.Content.Load<Texture2D>("Particles/Attack2"));
            textureset.Add(9, game.Content.Load<Texture2D>("Particles/Ring2"));
            textureset.Add(10, game.Content.Load<Texture2D>("Particles/BlackBlood"));
            textureset.Add(11, game.Content.Load<Texture2D>("Particles/Blood1"));
            textureset.Add(12, game.Content.Load<Texture2D>("Particles/Blood2"));
            textureset.Add(15, game.Content.Load<Texture2D>("Particles/BloodSmoke"));
            textureset.Add(16, game.Content.Load<Texture2D>("Particles/Hit"));
            textureset.Add(17, game.Content.Load<Texture2D>("Particles/EffectNormal2"));
            textureset.Add(18, game.Content.Load<Texture2D>("Particles/EffectNormal"));
            textureset.Add(19, game.Content.Load<Texture2D>("Particles/Smoke1"));
            textureset.Add(20, game.Content.Load<Texture2D>("Particles/Blood3"));
            textureset.Add(21, game.Content.Load<Texture2D>("Particles/Blood4"));
            textureset.Add(22, game.Content.Load<Texture2D>("Particles/Blood5"));
            textureset.Add(23, game.Content.Load<Texture2D>("Particles/HitEffect"));
            textureset.Add(24, game.Content.Load<Texture2D>("Particles/HitEffectDes"));
            textureset.Add(25, game.Content.Load<Texture2D>("Particles/RingDes"));
            textureset.Add(26, game.Content.Load<Texture2D>("Particles/Lumber1"));
            textureset.Add(27, game.Content.Load<Texture2D>("Particles/Lumber2"));
            textureset.Add(28, game.Content.Load<Texture2D>("Particles/Star"));

            texturenorm.Add(2, game.Content.Load<Texture2D>("Particles/Blood1_Norm"));
            texturenorm.Add(3, game.Content.Load<Texture2D>("Particles/Blood2_Norm"));
           
            modelset.Add(0, game.Content.Load<Model>("Particles/plane"));
        }

        public void ParticleGunFlash(Vector3 pos)
        {
            AddParticle(0, 1,
                pos,
                new Vector3(0),
                new Vector3(0),
                new Vector3(rand.Next(5, 10) / 300f),
                new Vector3(rand.Next(1, 5) / 1000f),
                Vector3.Zero,
                new Vector3(0, 0, rand.Next(360) / 10f),
                new Vector3(0, 0, rand.Next(-10, 10) / 500f),
                Vector3.Zero,
                5, -0.2f, 0,
                new Vector3(10),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                2, true, 1);
        }
        
        public void ParticleBloodMini(Vector3 pos, Vector3 Dir)
        {
            AddParticle(0, rand.Next(11, 12),
                pos,
                new Vector3((rand.Next(-15, 85) / 100f) * Dir.X, (rand.Next(-20, 50) / 100f) * Dir.Y, rand.Next(-15, 15) / 100f),
                new Vector3(0, -0.01f, 0),
                new Vector3(rand.Next(5, 10) / 500f),
                new Vector3(rand.Next(1, 5) / 1000f),
                Vector3.Zero,
                new Vector3(0, 0, rand.Next(360) / 10f),
                new Vector3(0, 0, rand.Next(-10, 10) / 500f),
                Vector3.Zero,
                rand.Next(10, 40) / 10f, -0.2f, 0,
                new Vector3(1, 1, 1),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                100, false, 1);
        }

        public void ParticleSmokeBlood(Vector3 pos, Vector3 Dir)
        {
            AddParticle(0, 15,
                pos,
                new Vector3((rand.Next(-15, 45) / 100f) * Dir.X, rand.Next(15) / 100f, rand.Next(-15, 15) / 100f),
                new Vector3(0, -0.01f, 0),
                new Vector3(rand.Next(30) / 1000f),
                new Vector3(0.01f),
                Vector3.Zero,
                new Vector3(0, 0, rand.Next(360) / 10f),
                new Vector3(0, 0, rand.Next(-10, 10) / 500f),
                Vector3.Zero,
                rand.Next(30) / 10f, -0.05f, 0,
                new Vector3(10f, 5, 2),
                new Vector3(0.1f, 0, 0),
                new Vector3(0, 0, 0),
                100, false, 1);
        }

        public void ParticleBlood(Vector3 pos, Vector3 Dir)
        {
            AddParticle(0, rand.Next(20, 22),
                pos,
                new Vector3(0),
                new Vector3(0),
                new Vector3(rand.Next(1, 20) / 1000f),
                new Vector3(0.004f),
                Vector3.Zero,
                new Vector3(0, 0, rand.Next(360) / 10f),
                new Vector3(0),
                Vector3.Zero,
                rand.Next(5, 40) / 10f, -0.2f, 0,
                new Vector3(1, 1, 1),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                100, false, 1);

            for (int i = 0; i < 10; i++)
            {
                AddParticle(0, rand.Next(11, 12),
                    pos,
                    new Vector3((rand.Next(-15, 85) / 100f) * Dir.X, (rand.Next(-20, 100) / 100f) * Dir.Y, rand.Next(-15, 15) / 1000f),
                    new Vector3(0, -0.01f, 0),
                    new Vector3(rand.Next(5, 10) / 500f),
                    new Vector3(rand.Next(1, 5) / 1000f),
                    Vector3.Zero,
                    new Vector3(0, 0, rand.Next(360) / 10f),
                    new Vector3(0, 0, rand.Next(-10, 10) / 500f),
                    Vector3.Zero,
                    rand.Next(10, 40) / 10f, -0.1f, 0,
                    new Vector3(1, 1, 1),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    100, false, 1);
            }
        }

        public void ParticleBloodStar(Vector3 pos, Vector3 Dir)
        {
            for (int i = 0; i < 12; i++)
            {
                AddParticle(0, 28,
                    pos,
                    new Vector3((rand.Next(-15, 85) / 100f) * Dir.X, (rand.Next(-20, 100) / 100f) * Dir.Y, rand.Next(-15, 15) / 1000f),
                    new Vector3(0, -0.01f, 0),
                    new Vector3(rand.Next(5, 10) / 1000f),
                    new Vector3(rand.Next(1, 5) / 1000f),
                    Vector3.Zero,
                    new Vector3(rand.Next(360), rand.Next(360), rand.Next(360)),
                    new Vector3(rand.Next(360), rand.Next(360), rand.Next(360)),
                    Vector3.Zero,
                    2.5f, -0.1f, 0,
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    100, false, 1);
            }
        }

        public void ParticleSmokeGround(Vector3 pos)
        {
            AddParticle(0, 19,
                pos,
                new Vector3(rand.Next(-10, 10), rand.Next(-10, 10), rand.Next(-10, 10)) / 100f,
                new Vector3(0),
                new Vector3(rand.Next(1, 20) / 100f),
                new Vector3(0.005f),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, rand.Next(300) / 100f),
                new Vector3(0, 0, rand.Next(-10, 10) / 1000f),
                Vector3.Zero,
                rand.Next(5, 10) / 10f, -0.01f, 0,
                new Vector3(11, 11, 10),
                new Vector3(0),
                new Vector3(0, 0, 0),
                100, false, 1);
        }

        public void ParticleSmokeGroundBig(Vector3 pos)
        {
            AddParticle(0, 19,
                pos,
                new Vector3(rand.Next(1, 30), rand.Next(1, 10), rand.Next(-10, 10)) / 100f,
                new Vector3(0),
                new Vector3(rand.Next(10, 50) / 100f),
                new Vector3(0.01f),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, rand.Next(300) / 100f),
                new Vector3(0, 0, rand.Next(-10, 10) / 1000f),
                Vector3.Zero,
                rand.Next(8, 20) / 10f, -0.01f, 0,
                new Vector3(2.3f, 3.3f, 3.9f),
                new Vector3(0.1f),
                new Vector3(0, 0, 0),
                100, false, 1);
        }

        public void ParticleHitNormal(Vector3 pos)
        {
            AddParticle(0, 19,
                pos,
                new Vector3(rand.Next(-10, 10), rand.Next(-10, 50), rand.Next(-10, 10)) / 100f,
                new Vector3(0),
                new Vector3(rand.Next(1, 20) / 100f),
                new Vector3(0.005f),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, rand.Next(300) / 100f),
                new Vector3(0, 0, rand.Next(-10, 10) / 1000f),
                Vector3.Zero,
                rand.Next(5, 10) / 10f, -0.05f, 0,
                new Vector3(1),
                new Vector3(0.01f),
                new Vector3(0, 0, 0),
                100, false, 1);
        }
    }
}

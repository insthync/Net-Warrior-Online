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

    public class ParticleManager
    {
        private Particle[] particle;
        private Particle[] particleDis;
        private Particle[] particleMesh;

        public Dictionary<int, Model> modelset;
        private int NumParticle = 0;
        private int NumParticleMesh = 0;

        public Dictionary<int, Texture2D> textureset;
        public Random rand = new Random();
        public Effect Shader;
        public Effect ShaderTail;
        public Dictionary<int, Texture2D> texturenorm;
        protected Game game;
        protected GraphicsDeviceManager graphics;
        protected GameCamera camera;

        public ParticleManager(Game game, GraphicsDeviceManager graphics, GameCamera camera)
        {
            this.game = game;
            this.graphics = graphics;
            this.camera = camera;
            particle = new Particle[1000];
            particleDis = new Particle[20];
            particleMesh = new Particle[20];
            modelset = new Dictionary<int, Model>();
            textureset = new Dictionary<int, Texture2D>();
            texturenorm = new Dictionary<int, Texture2D>();
        }

        public virtual void LoadContent()
        {
        }

        public void AddParticleMesh(
            int Nummodel,
            int[] particleNode,
            Vector3 position,
            Vector3 power1,
            Vector3 power2,
            Vector3 scale,
            Vector3 scaleup1,
            Vector3 scaleup2,
            Vector3 rotation,
            Vector3 rotationup1,
            Vector3 rotationup2,
            int lifeTime)
        {
            NumParticleMesh = (NumParticleMesh + 1) % particleMesh.Length;
            particleMesh[NumParticleMesh].Active = true;
            particleMesh[NumParticleMesh].model = modelset[Nummodel];
            particleMesh[NumParticleMesh].Position = position;
            particleMesh[NumParticleMesh].PowerUp1 = power1;
            particleMesh[NumParticleMesh].PowerUp2 = power2;
            particleMesh[NumParticleMesh].Rotation = rotation;
            particleMesh[NumParticleMesh].RotationUp1 = rotationup1;
            particleMesh[NumParticleMesh].RotationUp2 = rotationup2;
            particleMesh[NumParticleMesh].Scale = scale;
            particleMesh[NumParticleMesh].ScaleUp1 = scaleup1;
            particleMesh[NumParticleMesh].ScaleUp2 = scaleup2;
            particleMesh[NumParticleMesh].LifeTime = lifeTime;
            particleMesh[NumParticleMesh].ParticleNodes = particleNode;
            particleMesh[NumParticleMesh].AdditiveRender = false;
        }
        
        public void AddParticle(
            int Nummodel,
            int Numtexture,
            Vector3 position,
            Vector3 power1,
            Vector3 power2,
            Vector3 scale,
            Vector3 scaleup1,
            Vector3 scaleup2,
            Vector3 rotation,
            Vector3 rotationup1,
            Vector3 rotationup2,
            float alpha,
            float alphaUp1,
            float alphaup2,
            Vector3 basecolor,
            Vector3 basecolorUp1,
            Vector3 basecolorUp2,
            int lifeTime,
            bool add,
            int tech)
        {
            NumParticle = (NumParticle + 1) % particle.Length;
            particle[NumParticle].Active = true;

            particle[NumParticle].model = modelset[Nummodel];
            particle[NumParticle].texture = textureset[Numtexture];
            particle[NumParticle].Position = position;
            particle[NumParticle].PowerUp1 = power1;
            particle[NumParticle].PowerUp2 = power2;
            particle[NumParticle].Rotation = rotation;
            particle[NumParticle].RotationUp1 = rotationup1;
            particle[NumParticle].RotationUp2 = rotationup2;
            particle[NumParticle].Scale = scale;
            particle[NumParticle].ScaleUp1 = scaleup1;
            particle[NumParticle].ScaleUp2 = scaleup2;
            particle[NumParticle].Alpha = alpha;
            particle[NumParticle].AlphaUp1 = alphaUp1;
            particle[NumParticle].AlphaUp2 = alphaup2;
            particle[NumParticle].baseColor = basecolor;
            particle[NumParticle].baseColorUp1 = basecolorUp1;
            particle[NumParticle].baseColorUp2 = basecolorUp2;
            particle[NumParticle].AdditiveRender = add;
            particle[NumParticle].LifeTime = lifeTime;
            particle[NumParticle].Technic = tech;
        }
        public void Update(GameTime gameTime, float slowTime)
        {
            for (int i = 0; i < particle.Length; i++)
            {
                if (particle[i].Active)
                {
                    particle[i].Position.X += particle[i].PowerUp1.X * slowTime;
                    particle[i].Position.Y += particle[i].PowerUp1.Y * slowTime;
                    particle[i].Position.Z += particle[i].PowerUp1.Z * slowTime;

                    particle[i].Rotation.X += particle[i].RotationUp1.X * slowTime;
                    particle[i].Rotation.Y += particle[i].RotationUp1.Y * slowTime;
                    particle[i].Rotation.Z += particle[i].RotationUp1.Z * slowTime;

                    particle[i].Scale.X += particle[i].ScaleUp1.X * slowTime;
                    particle[i].Scale.Y += particle[i].ScaleUp1.Y * slowTime;
                    particle[i].Scale.Z += particle[i].ScaleUp1.Z * slowTime;

                    particle[i].Alpha += particle[i].AlphaUp1 * slowTime;

                    particle[i].PowerUp1.X += particle[i].PowerUp2.X * slowTime;
                    particle[i].PowerUp1.Y += particle[i].PowerUp2.Y * slowTime;
                    particle[i].PowerUp1.Z += particle[i].PowerUp2.Z * slowTime;


                    particle[i].RotationUp1.X += particle[i].RotationUp2.X * slowTime;
                    particle[i].RotationUp1.Y += particle[i].RotationUp2.Y * slowTime;
                    particle[i].RotationUp1.Z += particle[i].RotationUp2.Z * slowTime;


                    particle[i].ScaleUp1.X += particle[i].ScaleUp2.X * slowTime;
                    particle[i].ScaleUp1.Y += particle[i].ScaleUp2.Y * slowTime;
                    particle[i].ScaleUp1.Z += particle[i].ScaleUp2.Z * slowTime;


                    particle[i].AlphaUp1 += particle[i].AlphaUp2 * slowTime;

                    particle[i].baseColor.X += particle[i].baseColorUp1.X * slowTime;
                    particle[i].baseColor.Y += particle[i].baseColorUp1.Y * slowTime;
                    particle[i].baseColor.Z += particle[i].baseColorUp1.Z * slowTime;

                    particle[i].baseColorUp1.X += particle[i].baseColorUp2.X * slowTime;
                    particle[i].baseColorUp1.Y += particle[i].baseColorUp2.Y * slowTime;
                    particle[i].baseColorUp1.Z += particle[i].baseColorUp2.Z * slowTime;

                    particle[i].World = Matrix.CreateScale(particle[i].Scale) *
                        Matrix.CreateRotationZ(particle[i].Rotation.Z) *
                        Matrix.CreateBillboard(particle[i].Position, camera.Position, Vector3.Up, Vector3.Forward);
                    particle[i].LifeTime -= 1 * slowTime;
                    if (particle[i].LifeTime <= 0 || particle[i].Alpha <= 0.01f)
                    {
                        particle[i].Reset();
                    }

                }
                else
                {
                    particle[i].Scale = Vector3.Zero;
                    particle[i].World = Matrix.CreateScale(particle[i].Scale) *
                        Matrix.CreateRotationZ(particle[i].Rotation.Z) *
                        Matrix.CreateBillboard(particle[i].Position, camera.Position, Vector3.Up, Vector3.Forward);
                }

            }


            for (int i = 0; i < particleMesh.Length; i++)
            {
                if (particleMesh[i].Active)
                {

                    particleMesh[i].Position.X += particleMesh[i].PowerUp1.X * slowTime;
                    particleMesh[i].Position.Y += particleMesh[i].PowerUp1.Y * slowTime;
                    particleMesh[i].Position.Z += particleMesh[i].PowerUp1.Z * slowTime;
                    
                    particleMesh[i].Rotation.X += particleMesh[i].RotationUp1.X * slowTime;
                    particleMesh[i].Rotation.Y += particleMesh[i].RotationUp1.Y * slowTime;
                    particleMesh[i].Rotation.Z += particleMesh[i].RotationUp1.Z * slowTime;

                    particleMesh[i].Scale.X += particleMesh[i].ScaleUp1.X * slowTime;
                    particleMesh[i].Scale.Y += particleMesh[i].ScaleUp1.Y * slowTime;
                    particleMesh[i].Scale.Z += particleMesh[i].ScaleUp1.Z * slowTime;

                    particleMesh[i].PowerUp1.X += particleMesh[i].PowerUp2.X * slowTime;
                    particleMesh[i].PowerUp1.Y += particleMesh[i].PowerUp2.Y * slowTime;
                    particleMesh[i].PowerUp1.Z += particleMesh[i].PowerUp2.Z * slowTime;


                    particleMesh[i].RotationUp1.X += particleMesh[i].RotationUp2.X * slowTime;
                    particleMesh[i].RotationUp1.Y += particleMesh[i].RotationUp2.Y * slowTime;
                    particleMesh[i].RotationUp1.Z += particleMesh[i].RotationUp2.Z * slowTime;


                    particleMesh[i].ScaleUp1.X += particleMesh[i].ScaleUp2.X * slowTime;
                    particleMesh[i].ScaleUp1.Y += particleMesh[i].ScaleUp2.Y * slowTime;
                    particleMesh[i].ScaleUp1.Z += particleMesh[i].ScaleUp2.Z * slowTime;

                    particleMesh[i].World = Matrix.CreateScale(particleMesh[i].Scale) *
                        Matrix.CreateRotationZ(particleMesh[i].Rotation.Z) *
                        Matrix.CreateBillboard(particleMesh[i].Position, camera.Position, Vector3.Up, Vector3.Forward);
                    particleMesh[i].LifeTime -= 1 * slowTime;
                    if (particleMesh[i].LifeTime <= 0)
                    {
                        particleMesh[i].Reset();
                    }
                }
            }


            for (int i = 0; i < particleDis.Length; i++)
            {
                if (particleDis[i].Active)
                {
                    particleDis[i].Position.X += particleDis[i].PowerUp1.X * slowTime;
                    particleDis[i].Position.Y += particleDis[i].PowerUp1.Y * slowTime;
                    particleDis[i].Position.Z += particleDis[i].PowerUp1.Z * slowTime;

                    particleDis[i].Rotation.X += particleDis[i].RotationUp1.X * slowTime;
                    particleDis[i].Rotation.Y += particleDis[i].RotationUp1.Y * slowTime;
                    particleDis[i].Rotation.Z += particleDis[i].RotationUp1.Z * slowTime;

                    particleDis[i].Scale.X += particleDis[i].ScaleUp1.X * slowTime;
                    particleDis[i].Scale.Y += particleDis[i].ScaleUp1.Y * slowTime;
                    particleDis[i].Scale.Z += particleDis[i].ScaleUp1.Z * slowTime;

                    particleDis[i].Alpha += particleDis[i].AlphaUp1 * slowTime;

                    particleDis[i].PowerUp1.X += particleDis[i].PowerUp2.X * slowTime;
                    particleDis[i].PowerUp1.Y += particleDis[i].PowerUp2.Y * slowTime;
                    particleDis[i].PowerUp1.Z += particleDis[i].PowerUp2.Z * slowTime;


                    particleDis[i].RotationUp1.X += particleDis[i].RotationUp2.X * slowTime;
                    particleDis[i].RotationUp1.Y += particleDis[i].RotationUp2.Y * slowTime;
                    particleDis[i].RotationUp1.Z += particleDis[i].RotationUp2.Z * slowTime;


                    particleDis[i].ScaleUp1.X += particleDis[i].ScaleUp2.X * slowTime;
                    particleDis[i].ScaleUp1.Y += particleDis[i].ScaleUp2.Y * slowTime;
                    particleDis[i].ScaleUp1.Z += particleDis[i].ScaleUp2.Z * slowTime;


                    particleDis[i].AlphaUp1 += particleDis[i].AlphaUp2 * slowTime;

                    particleDis[i].baseColor.X += particleDis[i].baseColorUp1.X * slowTime;
                    particleDis[i].baseColor.Y += particleDis[i].baseColorUp1.Y * slowTime;
                    particleDis[i].baseColor.Z += particleDis[i].baseColorUp1.Z * slowTime;

                    particleDis[i].baseColorUp1.X += particleDis[i].baseColorUp2.X * slowTime;
                    particleDis[i].baseColorUp1.Y += particleDis[i].baseColorUp2.Y * slowTime;
                    particleDis[i].baseColorUp1.Z += particleDis[i].baseColorUp2.Z * slowTime;

                    particleDis[i].World = Matrix.CreateScale(particleDis[i].Scale) *
                        Matrix.CreateRotationZ(particleDis[i].Rotation.Z) *
                        Matrix.CreateBillboard(particleDis[i].Position, camera.Position, Vector3.Up, Vector3.Forward);
                    particleDis[i].LifeTime -= 1 * slowTime;
                    if (particleDis[i].LifeTime <= 0 || particleDis[i].Alpha <= 0.01f)
                    {
                        particleDis[i].Reset();
                    }
                }
            }
        }


        public void Draw(GameTime gameTime)
        {
            //disable depth buffering
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            for (int i = 0; i < particle.Length; i++)
            {
                if (particle[i].Active && !particle[i].AdditiveRender)
                {
                    DrawModel(ref particle[i], true);
                }
            }
        }

        private void DrawModel(ref Particle particle, bool texturepath)
        {
            foreach (ModelMesh mesh in particle.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = camera.getView();
                    effect.Projection = camera.getProjection();
                    effect.World = particle.World;
                    effect.Alpha = particle.Alpha;
                    if (texturepath)
                    {
                        effect.Texture = particle.texture;
                    }
                    effect.TextureEnabled = true;
                    effect.FogEnabled = true;
                    effect.FogEnd = 256;
                    effect.FogStart = 1;
                }
                mesh.Draw();
            }
        }

        public void DrawMesh(GameTime gameTime)
        {
            for (int i = 0; i < particleMesh.Length; i++)
            {
                if (particleMesh[i].Active && !particleMesh[i].AdditiveRender)
                {
                    DrawModel(ref particleMesh[i], false);
                }
            }
        }

        public void DrawAdditive(GameTime gameTime)
        {
            //disable depth buffering
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            for (int i = 0; i < particle.Length; i++)
            {
                if (particle[i].Active && particle[i].AdditiveRender)
                {
                    DrawModel(ref particle[i], true);
                }
            }
        }
    }
}

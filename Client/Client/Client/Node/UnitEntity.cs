﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MMORPGCopierClient
{
    public class UnitEntity
    {
        private int id;
        private String name;
        private GameClassConfig classcfg;
        private GameModelBank modelBank;
        private GameItemBank itemBank;
        private GameModelNode model;
        public int maxHP;
        public int curHP;
        public int maxSP;
        public int curSP;
        //public int modelHair;
        //public int modelFace;
        private GameState state = GameState.anim_idle;
        private GameMain game;
        private ContentManager content;
        private GraphicsDeviceManager graphics;
        private AudioEmitter audioEmitter;
        private AudioSystem audioSystem;
        private ParticlePreset particleManager;
        private SpriteBatch spriteBatch;
        private SpriteFont charNameFont;
        private SpriteFont charDamageFont;
        private Texture2D whiteRect;
        private const int listDamageSize = 10;
        private int[] listDamageValue = new int[listDamageSize]; // dmg
        private long[] listDamageTime = new long[listDamageSize]; // time
        private int numDamage = 0;
        // toggle information
        public bool drawName = false;
        public bool drawHP = false;
        public bool drawDamage = false;
        public UnitEntity(int id, String name, GameClassConfig classcfg, int modelHair, int modelFace, int maxHP, int maxSP, GameMain game, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont charNameFont, SpriteFont charDamageFont, Texture2D whiteRect, GameModelBank modelBank, GameItemBank itemBank, AudioSystem audioSystem, ParticlePreset particleManager)
        {
            this.id = id;
            name = name.Replace("'58'", ":");
            name = name.Replace("'59'", ";");
            name = name.Replace("'32'", " ");
            name = name.Replace("'39'", "'");
            this.name = name;
            this.game = game;
            this.content = content;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.classcfg = classcfg;
            this.modelBank = modelBank;
            this.itemBank = itemBank;
            this.charNameFont = charNameFont;
            this.charDamageFont = charDamageFont;
            this.whiteRect = whiteRect;
            short modelKey = classcfg.modelID;
            model = new GameModelNode(content, modelBank);
            model.Load(modelKey);
            model.Rotation = modelBank.getModelRotation(modelKey);
            model.Position = modelBank.getModelPosition(modelKey);
            model.Scale = modelBank.getModelScale(modelKey);
            model.playClip((short)state, true);
            this.maxHP = this.curHP = maxHP;
            this.maxSP = this.curSP = maxSP;
            this.audioEmitter = new AudioEmitter();
            this.audioSystem = audioSystem;
            this.particleManager = particleManager;
            for (int i = 0; i < listDamageSize; ++i)
            {
                listDamageValue[i] = -1;
                listDamageTime[i] = -1;
            }
        }

        public void currentState(GameState state)
        {
            if (this.state != state)
            {
                bool isLoop = (state == GameState.anim_idle || state == GameState.anim_walk || state == GameState.anim_run || state == GameState.anim_attackidle);
                bool lastIsLoop = (this.state == GameState.anim_idle || this.state == GameState.anim_walk || this.state == GameState.anim_run || this.state == GameState.anim_attackidle);
                bool isAttackState = (state == GameState.anim_attack1 || state == GameState.anim_attack2 || state == GameState.anim_attack3);
                if (!lastIsLoop)
                {
                    if (model.isAnimated())
                    {
                        model.playClip((short)state, isLoop);
                        this.state = state;
                    }
                }
                else
                {
                    model.playClip((short)state, isLoop);
                    this.state = state;
                }
                if (isAttackState)
                {
                    model.playClip((short)state, isLoop);
                    this.state = state;
                    if (model != null) {
                        Random rand = new Random();
                        audioEmitter.Position = model._Position;
                        Cue cue = audioSystem.getSoundBank().GetCue("att-0." + rand.Next(1,2));
                        cue.Apply3D(audioSystem.getAudioListener(), audioEmitter);
                        cue.Play();
                    }
                }
            }
        }

        public GameState currentState()
        {
            return state;
        }

        public int getID()
        {
            return id;
        }

        public String getName()
        {
            return name;
        }

        public Vector3 getPosition()
        {
            return model._Position;
        }

        public void setPosition(Vector3 position)
        {
            model._Position = position;
        }

        public Vector3 getRotation()
        {
            return model._Rotation;
        }

        public void setRotation(Vector3 rotation)
        {
            model._Rotation = rotation;
        }

        public float getDistanceFrom(UnitEntity ent)
        {
            return (float)Math.Sqrt(Math.Pow((int)this.getPosition().X - (int)ent.getPosition().X, 2) + Math.Pow((int)this.getPosition().Z - (int)ent.getPosition().Z, 2));
        }

        public void Update(GameTime gameTime, Matrix rootTransform)
        {
            if (curHP > maxHP)
                curHP = maxHP;
            if (curSP > maxSP)
                curSP = maxSP;
            model.Update(gameTime, rootTransform);
            for (int i = 0; i < listDamageSize; ++i)
            {
                if (DateTime.Now.Ticks - listDamageTime[i] >= 10000000 && listDamageTime[i] >= 0)
                {
                    listDamageValue[i] = -1;
                    listDamageTime[i] = -1;
                }
                //Debug.WriteLine("" + (DateTime.Now.Millisecond - listDamage[i][1]));
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            model.Draw(view, projection);
            spriteBatch.End();
        }

        public void DrawName(Matrix view, Matrix projection)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // Get 3d screen in 2d
            Vector3 screenSpace = graphics.GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view, model.getWorldMatrix());
            // Drawing name
            Vector2 textPosition = new Vector2(screenSpace.X, screenSpace.Y);
            Vector2 stringCenter = charNameFont.MeasureString(name) / 2;
            Vector2 shadowOffset = new Vector2(1, 1);
            // Shadow
            spriteBatch.DrawString(charNameFont, name, textPosition + shadowOffset, Color.Black, 0.0f, stringCenter, 1.0f, SpriteEffects.None, 0.0f);
            // Text
            spriteBatch.DrawString(charNameFont, name, textPosition, Color.White, 0.0f, stringCenter, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public void DrawHP(Matrix view, Matrix projection)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // Get 3d screen in 2d
            Vector3 screenSpace = graphics.GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view, model.getWorldMatrix());
            // Drawing hp
            Vector2 hpBarPosition = new Vector2(screenSpace.X - 30, screenSpace.Y + 10);
            Vector2 hpBarCenter = hpBarPosition / 2;
            spriteBatch.Draw(whiteRect, hpBarPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(60f, 5f), SpriteEffects.None, 0.0f);
            hpBarPosition.X += 1;
            hpBarPosition.Y += 1;
            float hpPercentage = (float)curHP / (float)maxHP * (float)100.0f;
            float hpBarWidth = (float)hpPercentage / (float)100.0f * (float)58;
            spriteBatch.Draw(whiteRect, hpBarPosition, null, Color.Red, 0f, Vector2.Zero, new Vector2(hpBarWidth, 3f), SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public void DrawDamage(Matrix view, Matrix projection)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            // Get 3d screen in 2d
            Vector3 screenSpace = graphics.GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view, model.getWorldMatrix());
            // Drawing each damage
            for (int i = 0; i < listDamageSize; ++i)
            {
                Vector3 screenSpaceTop = graphics.GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view, model.getWorldMatrix());
                Vector2 damageTextPosition = new Vector2(screenSpaceTop.X, screenSpaceTop.Y);
                if (listDamageValue[i] >= 0 && listDamageTime[i] >= 0)
                {
                    long runtime = DateTime.Now.Ticks - listDamageTime[i];
                    damageTextPosition.Y -= (runtime / 50000);
                    String damage = "" + listDamageValue[i];
                    Vector2 dmgStringCenter = charDamageFont.MeasureString(damage);
                    spriteBatch.DrawString(charDamageFont, damage, damageTextPosition, new Color(255f, 255f, 255f, 255f - (runtime / 10000000.0f * 255.0f)), 0.0f, dmgStringCenter, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
            spriteBatch.End();
        }

        public void DrawInfo(Matrix view, Matrix projection)
        {
            if (drawName)
                DrawName(view, projection);
            if (drawHP)
                DrawHP(view, projection);
            if (drawDamage)
                DrawDamage(view, projection);
        }

        public GameModelNode getModelNode()
        {
            return model;
        }

        public bool RayIntersectsModel(Ray ray)
        {
            return model.RayIntersectsModel(ray);
        }

        public void recieveDamage(int dmg)
        {
            if (model != null && dmg > 0)
            {
                Random rand = new Random();
                audioEmitter.Position = model._Position;
                Cue cue = audioSystem.getSoundBank().GetCue("att-1." + rand.Next(1, 5));
                cue.Apply3D(audioSystem.getAudioListener(), audioEmitter);
                cue.Play();
                Vector3 particlePos = getPosition();
                particlePos.Y += 5;
                particleManager.ParticleBloodStar(particlePos, Vector3.One);

                numDamage = (numDamage + 1) % listDamageSize;
                listDamageValue[numDamage] = dmg;
                listDamageTime[numDamage] = DateTime.Now.Ticks;
            }
        }

        public void setEquipmentItemID(int itemid_head, int itemid_body, int itemid_hand, int itemid_foot, int itemid_weaponR, int itemid_weaponL)
        {

        }
    }
}

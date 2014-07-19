using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MMORPGCopierClient
{
    public class ItemEntity : GameModel
    {
        private int id;
        private short itemid;
        private Vector3 _Position = Vector3.Zero;
        private Vector3 _Rotation = Vector3.Zero;
        private Vector3 Position = Vector3.Zero;
        private Vector3 Rotation = Vector3.Zero;
        private float Scale = 1.0f;
        private Matrix worldMatrix = Matrix.Identity;
        private String name = "";
        private GameItemBank bank;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont itemNameFont;
        private Texture2D whiteRect;
        public ItemEntity(int id, short itemid, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont itemNameFont, Texture2D whiteRect, GameItemBank bank) : base(content)
        {
            this.id = id;
            this.itemid = itemid;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.bank = bank;
            this.itemNameFont = itemNameFont;
            this.whiteRect = whiteRect;
            // Load Model
            this.Load(bank.getDropModelPath(itemid));
            this.Position = bank.getDropModelPosition(itemid);
            this.Rotation = bank.getDropModelRotation(itemid);
            this.Scale = bank.getDropModelScale(itemid);
        }

        public void DrawName(Matrix view, Matrix projection)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // Get 3d screen in 2d
            Vector3 screenSpace = graphics.GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view, worldMatrix);
            // Drawing name
            Vector2 textPosition = new Vector2(screenSpace.X, screenSpace.Y);
            Vector2 stringCenter = itemNameFont.MeasureString(name) / 2;
            Vector2 shadowOffset = new Vector2(1, 1);
            // Shadow
            spriteBatch.DrawString(itemNameFont, name, textPosition + shadowOffset, Color.Black, 0.0f, stringCenter, 1.0f, SpriteEffects.None, 0.0f);
            // Text
            spriteBatch.DrawString(itemNameFont, name, textPosition, Color.White, 0.0f, stringCenter, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public void Draw(Matrix view, Matrix projection)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Draw(view, projection, Position + _Position, Rotation - _Rotation, Scale);
            spriteBatch.End();
        }

        public bool RayIntersectsModel(Ray ray)
        {
            return RayIntersectsModel(ray, Position + _Position, Rotation - _Rotation, Scale);
        }

        public int getID()
        {
            return id;
        }

        public int getItemID()
        {
            return itemid;
        }

        public Vector3 getPosition()
        {
            return this._Position;
        }

        public void setPosition(Vector3 position)
        {
            this._Position = position;
        }

        public Vector3 getRotation()
        {
            return this._Rotation;
        }

        public void setRotation(Vector3 rotation)
        {
            this._Rotation = rotation;
        }

        public String getName()
        {
            return this.name;
        }
    }
}

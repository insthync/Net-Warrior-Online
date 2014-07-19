using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MMORPGCopierClient
{
    public class GameCamera
    {
        public const float ROTATION_SPEED = 0.75f;
        public const float DOLLYING_SPEED = 0.5f;
        public const float SCROLLING_SPEED = 1.45f;
        public const float MIN_ARC = -90.0f;
        public const float MAX_ARC = 0.0f;
        public const float MIN_DISTANCE = 10.0f;
        public const float MAX_DISTANCE = 64.0f;
        private Vector3 offset = new Vector3(0, 7, 0);
        public Vector3 target = Vector3.Zero;
        private float cameraArc = -30;

        public float CameraArc
        {
            get { return cameraArc; }
            set { cameraArc = value; }
        }

        private float cameraRotation = 0.0f;

        public float CameraRotation
        {
            get { return cameraRotation; }
            set { cameraRotation = value; }
        }

        private float cameraDistance = 32.0f;

        public float CameraDistance
        {
            get { return cameraDistance; }
            set { cameraDistance = value; }
        }
        private Matrix view;
        private Matrix projection;

        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
        }

        private float nearPlaneDistance = 1.0f;
        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set { nearPlaneDistance = value; }
        }

        private float farPlaneDistance = 10000.0f;
        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set { farPlaneDistance = value; }
        }

        private Quaternion cameraOrientation = Quaternion.Identity;

        public void ProcessMouse()
        {
            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            if (currentMouseState.MiddleButton == ButtonState.Pressed)
            {
                dz = currentMouseState.Y - prevMouseState.Y;
                dz *= DOLLYING_SPEED;
            }

            if (currentMouseState.ScrollWheelValue > prevMouseState.ScrollWheelValue)
                dz -= SCROLLING_SPEED;
            else if (currentMouseState.ScrollWheelValue < prevMouseState.ScrollWheelValue)
                dz += SCROLLING_SPEED;

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                dx = currentMouseState.X - prevMouseState.X;
                dx *= ROTATION_SPEED;

                dy = currentMouseState.Y - prevMouseState.Y;
                dy *= ROTATION_SPEED;

            }
            cameraRotation += dx;
            cameraArc += (-dy);
            cameraDistance += dz;

            // Limit the arc movement.
            if (cameraArc > MAX_ARC)
                cameraArc = MAX_ARC;
            else if (cameraArc < MIN_ARC)
                cameraArc = MIN_ARC;

            // Limit the arc movement.
            if (cameraDistance > MAX_DISTANCE)
                cameraDistance = MAX_DISTANCE;
            else if (cameraDistance < MIN_DISTANCE)
                cameraDistance = MIN_DISTANCE;

            // Get coordinate of space (Cursor2D in 3D Scene)
            //  Unproject the screen space mouse coordinate into model space 
            //  coordinates. Because the world space matrix is identity, this 
            //  gives the coordinates in world space.
            Viewport vp = graphics.GraphicsDevice.Viewport;
            //  Note the order of the parameters! Projection first.
            Vector3 pos1 = vp.Unproject(new Vector3(currentMouseState.X, currentMouseState.Y, 0), projection, view, Matrix.Identity);
            Vector3 pos2 = vp.Unproject(new Vector3(currentMouseState.X, currentMouseState.Y, 1), projection, view, Matrix.Identity);
            Vector3 dir = Vector3.Normalize(pos2 - pos1); // Direction
            // ** Ray pos1 is start position and dir is ray's direction
            mray = new Ray(pos1, dir);
            //  If the mouse ray is aimed parallel with the world plane, then don't 
            //  intersect, because that would divide by zero.
            if (dir.Y != 0)
            {
                ppos = (pos1 - dir * (pos1.Y / dir.Y));
            }
            Update();
        }

        public void Update()
        {
            Vector3 target = this.target + offset;
            view = Matrix.CreateTranslation(-target.X, -target.Y, -target.Z) *
                      Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                      Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                      Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                          Vector3.Zero, Vector3.Up);

            position = Vector3.Transform(Vector3.Zero, Matrix.Invert(view));

            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, nearPlaneDistance, farPlaneDistance);
        }

        // Mousestate
        private MouseState currentMouseState;
        private MouseState prevMouseState;
        // Important Field
        private GraphicsDeviceManager graphics;
        // Cursor3D
        private Vector3 ppos = Vector3.Zero;
        private Ray mray = new Ray();

        public GameCamera(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }
        
        public Matrix getView()
        {
            return view;
        }

        public Matrix getProjection()
        {
            return projection;
        }

        public Vector3 getCursor3D()
        {
            return ppos;
        }

        public Ray getCursor3DRay()
        {
            return mray;
        }
    }
}

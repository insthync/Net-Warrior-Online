using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
/*
namespace MMORPGCopierClient
{
    public class ShadowMapping
    {
        // The size of the shadow map
        // The larger the size the more detail we will have for our entire scene
        const int shadowMapWidthHeight = 2048;

        ContentManager content;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Starting position and direction of our camera
        Vector3 cameraPosition = new Vector3(0, 70, 100);
        Vector3 cameraForward = new Vector3(0, -0.4472136f, -0.8944272f);
        BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);

        // Light direction
        Vector3 lightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);

        // The shadow map render target
        RenderTarget2D shadowRenderTarget;

        // ViewProjection matrix from the lights perspective
        Matrix lightViewProjection;

        public ShadowMapping(ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.content = content;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            // Create floating point render target
            shadowRenderTarget = new RenderTarget2D(graphics.GraphicsDevice,
                                                    shadowMapWidthHeight,
                                                    shadowMapWidthHeight,
                                                    false,
                                                    SurfaceFormat.Single,
                                                    DepthFormat.Depth24);
        }

        public void Draw()
        {
            // Update the lights ViewProjection matrix based on the 
            // current camera frustum
            lightViewProjection = CreateLightViewProjectionMatrix();

            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Render the scene to the shadow map
            CreateShadowMap();

            // Draw the scene using the shadow map
            DrawWithShadowMap();
        }

        /// <summary>
        /// Creates the WorldViewProjection matrix from the perspective of the 
        /// light using the cameras bounding frustum to determine what is visible 
        /// in the scene.
        /// </summary>
        /// <returns>The WorldViewProjection for the light</returns>
        Matrix CreateLightViewProjectionMatrix()
        {
            // Matrix with that will rotate in points the direction of the light
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightDir,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightDir,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

        /// <summary>
        /// Renders the scene to the floating point render target then 
        /// sets the texture for use when drawing the scene.
        /// </summary>
        void CreateShadowMap()
        {
            // Set our render target to our floating point render target
            graphics.GraphicsDevice.SetRenderTarget(shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            //graphics.GraphicsDevice.Clear(Color.White);

            // Draw the model as shadow
            DrawModel(dudeModel, true);

            // Set render target back to the back buffer
            graphics.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Renders the scene using the shadow map to darken the shadow areas
        /// </summary>
        void DrawWithShadowMap()
        {
            //graphics.GraphicsDevice.Clear(Color.Black);

            graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            // Draw the map
            world = Matrix.Identity;
            DrawModel(gridModel, false);

            // Draw the model
            world = Matrix.CreateRotationY(MathHelper.ToRadians(rotateDude));
            DrawModel(dudeModel, false);
        }

    }
}
*/
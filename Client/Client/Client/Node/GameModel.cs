using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SkinnedModel;

namespace MMORPGCopierClient
{
    public class GameModel
    {
        private Model model = null;
        private ContentManager content = null;
        private Matrix[] transforms = null;
        private Matrix[] bones = null;
        private SkinningData skinningData = null;
        private AnimationPlayer animationPlayer = null;
        private Matrix world = Matrix.Identity;
        public GameModel(ContentManager content)
        {
            this.content = content;
        }

        public void Load(String path)
        {
            if (path != null)
            {
                model = content.Load<Model>(path);
                transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                // Look up our custom skinning information.
                skinningData = model.Tag as SkinningData;

                if (skinningData != null)
                {
                    // Create an animation player, and start decoding an animation clip.
                    animationPlayer = new AnimationPlayer(skinningData);
                }
            }
        }

        public void playClip(String clipname, bool isLoop)
        {
            if (skinningData != null && animationPlayer != null)
            {
                if (skinningData.AnimationClips.ContainsKey(clipname))
                {
                    AnimationClip clip = skinningData.AnimationClips[clipname];
                    animationPlayer.StartClip(clip, isLoop);
                    bones = animationPlayer.GetSkinTransforms();
                }
                else
                {
                    Debug.WriteLine("Clip [" + clipname + "] was not found.");
                }
            }
        }

        public void Update(GameTime gameTime, Matrix rootTransform)
        {
            if (animationPlayer != null)
                animationPlayer.Update(gameTime.ElapsedGameTime, true, rootTransform);
        }

        private void DrawStatic(Matrix view, Matrix projection, Vector3 Position, Vector3 Rotation, float Scale)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z))
                        * Matrix.CreateScale(Scale)
                        * Matrix.CreateTranslation(Position);
                    effect.View = view;
                    effect.Projection = projection;

                    /*effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);

                    effect.DirectionalLight1.Direction = new Vector3(-1, -1, -1);
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);

                    effect.DirectionalLight2.Direction = new Vector3(-1, -1, 1);
                    effect.DirectionalLight2.Enabled = true;
                    effect.DirectionalLight2.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);*/

                    effect.FogEnabled = true;
                    effect.FogEnd = 256;
                    effect.FogStart = 8;

                    effect.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);
                    effect.SpecularColor = new Vector3(0.01f, 0.01f, 0.01f);
                    effect.SpecularPower = 20;
                    effect.PreferPerPixelLighting = true;

                    effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        private void DrawDynamic(Matrix view, Matrix projection, Vector3 Position, Vector3 Rotation, float Scale)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    effect.EnableDefaultLighting();

                    effect.World = world = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z))
                        * Matrix.CreateScale(Scale)
                        * Matrix.CreateTranslation(Position);
                    effect.View = view;
                    effect.Projection = projection;

                    /*effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);

                    effect.DirectionalLight1.Direction = new Vector3(-1, -1, -1);
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);

                    effect.DirectionalLight2.Direction = new Vector3(-1, -1, 1);
                    effect.DirectionalLight2.Enabled = true;
                    effect.DirectionalLight2.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);*/

                    effect.FogEnabled = true;
                    effect.FogEnd = 256;
                    effect.FogStart = 8;

                    effect.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);
                    effect.SpecularColor = new Vector3(0.01f, 0.01f, 0.01f);
                    effect.SpecularPower = 20;
                    effect.PreferPerPixelLighting = true;

                    effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        public void Draw(Matrix view, Matrix projection, Vector3 Position, Vector3 Rotation, float Scale)
        {
            // Draw the model. A model can have multiple meshes, so loop.
            if (model != null)
            {
                if (skinningData != null && animationPlayer != null)
                {
                    DrawDynamic(view, projection, Position, Rotation, Scale);
                }
                else
                {
                    DrawStatic(view, projection, Position, Rotation, Scale);
                }
            }
        }

        public Matrix getWorldMatrix()
        {
            return world;
        }

        public bool isAnimated()
        {
            return animationPlayer.isFrameEnd;
        }

        public BoundingBox CalculateBoundingBox()
        {
            // Create variables to hold min and max xyz values for the model. Initialise them to extremes
            Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            if (model.Meshes != null)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    //Create variables to hold min and max xyz values for the mesh. Initialise them to extremes
                    Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                    Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                    // There may be multiple parts in a mesh (different materials etc.) so loop through each
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // The stride is how big, in bytes, one vertex is in the vertex buffer
                        // We have to use this as we do not know the make up of the vertex
                        int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                        byte[] vertexData = new byte[stride * part.NumVertices];
                        part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, 1); // fixed 13/4/11

                        // Find minimum and maximum xyz values for this mesh part
                        // We know the position will always be the first 3 float values of the vertex data
                        Vector3 vertPosition = new Vector3();
                        for (int ndx = 0; ndx < vertexData.Length; ndx += stride)
                        {
                            vertPosition.X = BitConverter.ToSingle(vertexData, ndx);
                            vertPosition.Y = BitConverter.ToSingle(vertexData, ndx + sizeof(float));
                            vertPosition.Z = BitConverter.ToSingle(vertexData, ndx + sizeof(float) * 2);

                            // update our running values from this vertex
                            meshMin = Vector3.Min(meshMin, vertPosition);
                            meshMax = Vector3.Max(meshMax, vertPosition);
                        }
                    }

                    // transform by mesh bone transforms
                    meshMin = Vector3.Transform(meshMin, transforms[mesh.ParentBone.Index]);
                    meshMax = Vector3.Transform(meshMax, transforms[mesh.ParentBone.Index]);

                    // Expand model extents by the ones from this mesh
                    modelMin = Vector3.Min(modelMin, meshMin);
                    modelMax = Vector3.Max(modelMax, meshMax);
                }
            }
            // Create and return the model bounding box
            return new BoundingBox(modelMin, modelMax);

        }

        public bool RayIntersectsModel(Ray ray, Vector3 Position, Vector3 Rotation, float Scale)
        {
            if (model.Meshes != null)
            {
                // Each ModelMesh in a Model has a bounding sphere, so to check for an
                // intersection in the Model, we have to check every mesh.
                foreach (ModelMesh mesh in model.Meshes)
                {
                    // the mesh's BoundingSphere is stored relative to the mesh itself.
                    // (Mesh space). We want to get this BoundingSphere in terms of world
                    // coordinates. To do this, we calculate a matrix that will transform
                    // from coordinates from mesh space into world space....
                    Matrix world = transforms[mesh.ParentBone.Index]
                                * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X))
                                * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y))
                                * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z))
                                * Matrix.CreateScale(Scale)
                                * Matrix.CreateTranslation(Position);

                    // ... and then transform the BoundingSphere using that matrix.
                    BoundingSphere sphere = TransformBoundingSphere(mesh.BoundingSphere, world);

                    // now that the we have a sphere in world coordinates, we can just use
                    // the BoundingSphere class's Intersects function. Intersects returns a
                    // nullable float (float?). This value is the distance at which the ray
                    // intersects the BoundingSphere, or null if there is no intersection.
                    // so, if the value is not null, we have a collision.
                    if (sphere.Intersects(ray) != null)
                    {
                        return true;
                    }
                }

                // if we've gotten this far, we've made it through every BoundingSphere, and
                // none of them intersected the ray. This means that there was no collision,
                // and we should return false.
            }
            return false;
        }

        private BoundingSphere TransformBoundingSphere(BoundingSphere sphere, Matrix transform)
        {
            BoundingSphere transformedSphere;

            // the transform can contain different scales on the x, y, and z components.
            // this has the effect of stretching and squishing our bounding sphere along
            // different axes. Obviously, this is no good: a bounding sphere has to be a
            // SPHERE. so, the transformed sphere's radius must be the maximum of the 
            // scaled x, y, and z radii.

            // to calculate how the transform matrix will affect the x, y, and z
            // components of the sphere, we'll create a vector3 with x y and z equal
            // to the sphere's radius...
            Vector3 scale3 = new Vector3(sphere.Radius, sphere.Radius, sphere.Radius);

            // then transform that vector using the transform matrix. we use
            // TransformNormal because we don't want to take translation into account.
            scale3 = Vector3.TransformNormal(scale3, transform);

            // scale3 contains the x, y, and z radii of a squished and stretched sphere.
            // we'll set the finished sphere's radius to the maximum of the x y and z
            // radii, creating a sphere that is large enough to contain the original 
            // squished sphere.
            transformedSphere.Radius = Math.Max(scale3.X, Math.Max(scale3.Y, scale3.Z));

            // transforming the center of the sphere is much easier. we can just use 
            // Vector3.Transform to transform the center vector. notice that we're using
            // Transform instead of TransformNormal because in this case we DO want to 
            // take translation into account.
            transformedSphere.Center = Vector3.Transform(sphere.Center, transform);

            return transformedSphere;
        }

        public Model getModel()
        {
            return model;
        }
    }
}

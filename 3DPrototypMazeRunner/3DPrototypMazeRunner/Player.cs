using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DPrototypMazeRunner;

namespace _3DPrototypMazeRunner
{
    class Player
    {
        Model pModel;
        // Set the position of the model in world space, and set the rotation.
        public Vector3 pPosition;
        public Vector3 pVelocity = Vector3.Zero;
        public float pRotation = 0.0f;
        private float scale = 0.015f;
        public Matrix modelWorldMatrix = Matrix.Identity;

        public BoundingBox pBox;
        public BoundingBox worldpBox;

        private BasicEffect boxEffect;

        /// <summary>
        /// Constructor
        /// </summary>
        public Player(Vector3 StartPos)
        {
            pPosition = StartPos;

        }

        /// <summary>
        /// Initialize Player Object with the Model and the StartPosition from the current Map object
        /// </summary>
        /// <param Contentmanager object="contentManager"></param>
        /// <param Graphicsdevice object="device"></param>
        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {

            pModel = contentManager.Load<Model>("Models/Schorsch");
            pBox = GetBounds();
            boxEffect = new BasicEffect(device); 
        }

        //creates a BoundingBox for the player
        public BoundingBox GetBounds()
        {
            // Create initial variables to hold min and max xyz values for the mesh
            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            // Set up model data

            Matrix[] transforms = new Matrix[pModel.Bones.Count];
            pModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in pModel.Meshes)
            {

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    // We have to use this as we do not know the make up of the vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                    // Find minimum and maximum xyz values for this mesh part
                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        // update our values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }
                // transform by mesh bone matrix
                meshMin = Vector3.Transform(meshMin, transforms[mesh.ParentBone.Index]);
                meshMax = Vector3.Transform(meshMax, transforms[mesh.ParentBone.Index]);

                // Now expand main bb by this mesh's box
                pBox.Min = Vector3.Min(pBox.Min, meshMin);
                pBox.Max = Vector3.Max(pBox.Max, meshMax);
            }
            

            // Create the bounding box
            return new BoundingBox(meshMin, meshMax);
            
        }

        public void Update(Vector3 inputVelocity)
        {
            pVelocity = inputVelocity;
            // Add velocity to the current position.
            pPosition += pVelocity;

            // Bleed off velocity over time.
            pVelocity *= 0.875f;

            // update the World matrix for the player model
            modelWorldMatrix = Matrix.Identity * Matrix.CreateScale(scale)
            * Matrix.CreateRotationY(pRotation)
            * Matrix.CreateTranslation(pPosition);

            //pBox = GetBounds();

            // extract the corners of the AABB into 8 vectors - 1 for each corner
            Vector3[] obb = new Vector3[8];
            pBox.GetCorners(obb);

            // Transform the vectors by the model's world matrix
            Vector3.Transform(obb, ref modelWorldMatrix, obb);

            // create an AABB in world space from the OBB in world space
            worldpBox = BoundingBox.CreateFromPoints(obb);
        }

        public void DrawbBox(Matrix View, Matrix Projection, Matrix World)
        {
            // Initialize an array of indices for the box. 12 lines require 24 indices
            short[] bBoxIndices = {
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
              };
            Vector3[] corners = worldpBox.GetCorners();
            VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

            // Assign the 8 box vertices
            for (int i = 0; i < corners.Length; i++)
            {
                primitiveList[i] = new VertexPositionColor(corners[i], Color.White);
            }

            /* Set your own effect parameters here */

            boxEffect.World = World;
            boxEffect.View = View;
            boxEffect.Projection = Projection;
            boxEffect.TextureEnabled = false;

            // Draw the box with a LineList
            foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                boxEffect.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList, primitiveList, 0, 8,
                    bBoxIndices, 0, 12);
            }
        }


       
        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
            DrawbBox(view,projection,world);
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[pModel.Bones.Count];
            pModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in pModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    //transforms mesh by scaling then rotating and translating
                    effect.World = transforms[mesh.ParentBone.Index]
                        * modelWorldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}

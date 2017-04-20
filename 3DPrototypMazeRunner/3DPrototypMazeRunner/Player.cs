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
        }

        //creates a BoundingBox for the player
        public BoundingBox GetBounds()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in pModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    int vertexDataSize = vertexBufferSize / sizeof(float);
                    float[] vertexData = new float[vertexDataSize];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexDataSize; i += vertexStride / sizeof(float))
                    {
                        Vector3 vertex = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);
                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);
                    }
                }
            }

            return new BoundingBox(min, max);
        }

        public void Update()
        {
            // Add velocity to the current position.
            pPosition += pVelocity;

            // Bleed off velocity over time.
            pVelocity *= 0.875f;

            // update the World matrix for the player model
            modelWorldMatrix = modelWorldMatrix * Matrix.CreateScale(scale)
            * Matrix.CreateRotationY(pRotation)
            * Matrix.CreateTranslation(pPosition);

            // extract the corners of the AABB into 8 vectors - 1 for each corner
            Vector3[] obb = new Vector3[8];
            pBox.GetCorners(obb);

            // Transform the vectors by the model's world matrix
            Vector3.Transform(obb, ref modelWorldMatrix, obb);


            // create an AABB in world space from the OBB in world space
            pBox = BoundingBox.CreateFromPoints(obb);


        }
       
        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
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
                        * Matrix.CreateScale(scale)
                        * Matrix.CreateRotationY(pRotation)
                        * Matrix.CreateTranslation(pPosition);
                    effect.View = view;
                    effect.Projection = projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}

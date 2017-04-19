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
        private float scale = 0.025f;

        BoundingBox pBox;

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

        }

        public void Update()
        {
            

            // Add velocity to the current position.
            pPosition += pVelocity;

            // Bleed off velocity over time.
            pVelocity *= 0.95f;

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

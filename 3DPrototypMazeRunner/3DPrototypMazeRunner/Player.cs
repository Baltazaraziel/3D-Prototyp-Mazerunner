using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DPrototypMazeRunner;

namespace _3DPrototypMazeRunner
{
    class Player
    {
        Model playerModel;
        // Set the position of the model in world space, and set the rotation.
        public Vector3 playerPosition;
        public float playerRotation = 0.0f;
        private float scale = 0.1f;

        /// <summary>
        /// Constructor
        /// </summary>
        public Player(Vector3 StartPos)
        {
            playerPosition = StartPos;
        }

        /// <summary>
        /// Initialize Player Object with the Model and the StartPosition from the current Map object
        /// </summary>
        /// <param Contentmanager object="contentManager"></param>
        /// <param Graphicsdevice object="device"></param>
        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {
            playerModel = contentManager.Load<Model>("Models/Player");
        }

        public void Update(GameTime gTime)
        {

        }

        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[playerModel.Bones.Count];
            playerModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in playerModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    //transforms mesh by scaling then rotating and translating
                    effect.World = transforms[mesh.ParentBone.Index] 
                        * Matrix.CreateScale(scale) 
                        * Matrix.CreateRotationY(playerRotation)
                        * Matrix.CreateTranslation(playerPosition);
                    effect.View = view;
                    effect.Projection = projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}

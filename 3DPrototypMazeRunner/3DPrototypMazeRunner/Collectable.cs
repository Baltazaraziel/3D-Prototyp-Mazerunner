using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _3DPrototypMazeRunner
{
    class Collectable
    {
        private Model cModel;
        private Vector3 cPosition;
        private Vector3 cForward;
        private float cRotation;
        private float scale;
        private Matrix cWorld;
        public bool isCollected {get {return m_Collected;} set { m_Collected = value;  } }
        private bool m_Collected;

        //Constructor
        public Collectable(Vector3 pos)
        {
            cPosition = pos;
            cForward = Vector3.Forward;
            cWorld = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            m_Collected = false;
            scale = 1;
        }

        //load model
        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {
            cModel = contentManager.Load<Model>("Models/Collectable");
        }

        //Update Collectable
        public void Update(GameTime gTime, Player player)
        {
            if (Math.Abs(player.pPosition.X - cPosition.X) < 5.0f && Math.Abs(player.pPosition.Z - cPosition.Z) < 5.0f)
            {
                isCollected = true;
            }
            else
            {
                //Collectable rotates slowly around it's own axis
                cRotation = 0.7f*(float) gTime.ElapsedGameTime.TotalSeconds;
                cForward = Vector3.Transform(cForward, Matrix.CreateRotationY(cRotation));
                //Collectable (should) hover up and down
                cPosition.Y = 6.0f + (float) Math.Cos(1000*gTime.ElapsedGameTime.TotalSeconds)*2.0f;
                //scale, position, rotation
                cWorld = Matrix.CreateScale(scale)*Matrix.CreateWorld(cPosition, cForward, Vector3.Up);
            }
        }

        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
            foreach (ModelMesh mesh in cModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = cWorld;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}

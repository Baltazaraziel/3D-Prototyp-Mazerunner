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
    class Camera
    {
        private Vector3 m_Position;
        private Vector3 m_LookAt;
        public Vector3 Position { get { return m_Position; } set { m_Position = value; } }
        public Vector3 LookAt { get { return m_LookAt; } set { m_LookAt = value; } }
        private Matrix m_View;
        private Matrix m_Projection;
        public Matrix View { get { return m_View; } set { } }
        public Matrix Projection { get { return m_Projection; } set { } }
        public Vector3 Velocity { get { return m_Velocity; } set { m_Velocity = value; } }
        private Vector3 m_Velocity;
        public float Rotation;

        private Vector3 m_resetPosition;
        private Vector3 m_resetLookat;


        public Camera(Vector3 position, Vector3 lookat, GraphicsDevice device)
        {
            m_Position = position;
            m_resetPosition = position;
            m_LookAt = lookat;
            m_resetLookat = lookat;
            m_View = Matrix.CreateLookAt(position, lookat, Vector3.Up);
            m_Projection = Matrix.CreatePerspectiveFieldOfView(1.65806f,
                device.Viewport.AspectRatio, 0.1f, 500.0f); //MathHelper.PiOver4
        }

        public void Move(float x, float y, float z)
        {
            m_Position = Vector3.Transform(m_Position, Matrix.CreateTranslation(x, y, z));
            m_LookAt = Vector3.Transform(m_LookAt, Matrix.CreateTranslation(x, y, z));
            m_View = Matrix.CreateLookAt(m_Position, m_LookAt, Vector3.Up);
        }

        public void RotateY(float radians)
        {
            Matrix rot = Matrix.CreateTranslation(-m_LookAt)*Matrix.CreateRotationY(radians)*Matrix.CreateTranslation(m_LookAt);
            m_Position = Vector3.Transform(m_Position, rot);
            m_View = Matrix.CreateLookAt(m_Position, m_LookAt, Vector3.Up);
        }

        public void Update(GameTime gTime, Player player)
        {   
           /* this.Move(m_Velocity.X, 0, m_Velocity.Z);
            m_Velocity *= 0.875f;
            */
            if (player.pVelocity.Length() > 0.012)
            {
                Vector3 normPlayer = new Vector3(player.pVelocity.X, player.pVelocity.Y, player.pVelocity.Z);
                normPlayer.Normalize();
                m_Position = (player.pPosition - 7*normPlayer) + new Vector3(0,11, 0);
                m_LookAt = player.pPosition + new Vector3(0, 8, 0);
                m_View = Matrix.CreateLookAt(m_Position,
                    m_LookAt, Vector3.Up);
            }
        }

        public void Reset()
        {
            m_Position = m_resetPosition;
            m_LookAt = m_resetLookat;
            Rotation = 0.0f;
            Velocity = Vector3.Zero;
            m_View = Matrix.CreateLookAt(m_Position, m_LookAt, Vector3.Up);
        }
    }
}

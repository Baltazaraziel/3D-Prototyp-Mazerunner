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
        public Vector3 Position { get { return m_Position; } set { m_Position = value; } }
        public Vector3 LookAt { get { return m_LookAt; } set { m_LookAt = value; } }
        private Vector3 m_Position;
        private Vector3 m_LookAt;
        private Matrix View;
        private Matrix Projection;

        public Camera(Vector3 position, Vector3 lookat, GraphicsDevice device)
        {
            m_Position = position;
            m_LookAt = lookat;
            View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                device.Viewport.AspectRatio, 0.1f, 500.0f);
        }

        public void Move(float x, float y, float z)
        {
            m_Position = Vector3.Transform(m_Position, Matrix.CreateTranslation(x, y, z));
            m_LookAt = Vector3.Transform(m_LookAt, Matrix.CreateTranslation(x, y, z));
            View = Matrix.CreateLookAt(m_Position, m_LookAt, Vector3.Up);
        }

        public void RotateY(float degree)
        {
            Matrix rot = Matrix.CreateTranslation(m_LookAt)*Matrix.CreateRotationY(MathHelper.ToRadians(degree))*Matrix.CreateTranslation(-m_LookAt);
            m_Position = Vector3.Transform(m_Position, rot);
            View = Matrix.CreateLookAt(m_Position, m_LookAt, Vector3.Up);
        }

        public void Update(GameTime gTime)
        {
            
        }
    }
}

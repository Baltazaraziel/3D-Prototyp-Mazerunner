using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _3DPrototypMazeRunner
{
    /// <summary>
    /// Primitive Cuboid based on 8 verticies
    /// </summary>
    class Cuboid
    {
        private VertexPositionColorTexture[] Verts = new VertexPositionColorTexture[8];
        private short[] Indices = new short[6*6];
        private Vector3 Position;
        private Vector3 Dimensions;
        private BasicEffect Effect;
        private Texture2D Texture;
        private VertexBuffer vBuffer;
        private IndexBuffer iBuffer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="center">Center of Cuboid</param>
        /// <param name="x">Dimension along x-Axis</param>
        /// <param name="y">Dimension along y-Axis (height)</param>
        /// <param name="z">Dimension along z-Axis</param>
        public Cuboid(Vector3 center, float x, float y, float z)
        {
            Verts[0].Position = center + Vector3.UnitX * (x / 2) + Vector3.UnitY * (y / 2) + Vector3.UnitZ * (z / 2);
            Verts[1].Position = center + Vector3.UnitX * -(x / 2) + Vector3.UnitY * (y / 2) + Vector3.UnitZ * (z / 2);
            Verts[2].Position = center + Vector3.UnitX * -(x / 2) + Vector3.UnitY * -(y / 2) + Vector3.UnitZ * (z / 2);
            Verts[4].Position = center + Vector3.UnitX * -(x / 2) + Vector3.UnitY * -(y / 2) + Vector3.UnitZ * -(z / 2);
            Verts[3].Position = center + Vector3.UnitX * (x / 2) + Vector3.UnitY * -(y / 2) + Vector3.UnitZ * (z / 2);
            Verts[5].Position = center + Vector3.UnitX * (x / 2) + Vector3.UnitY * -(y / 2) + Vector3.UnitZ * -(z / 2);
            Verts[6].Position = center + Vector3.UnitX * -(x / 2) + Vector3.UnitY * (y / 2) + Vector3.UnitZ * -(z / 2);
            Verts[7].Position = center + Vector3.UnitX * (x / 2) + Vector3.UnitY * (y / 2) + Vector3.UnitZ * -(z / 2);

            Indices[0] = 0; Indices[1] = 3; Indices[2] = 2;
            Indices[3] = 0; Indices[4] = 2; Indices[5] = 1;
            Indices[6] = 2; Indices[7] = 3; Indices[8] = 5;
            Indices[9] = 2; Indices[10] = 5; Indices[11] = 4;
            Indices[12] = 0; Indices[13] = 5; Indices[14] = 3;
            Indices[15] = 0; Indices[16] = 7; Indices[17] = 5;
            Indices[18] = 6; Indices[19] = 1; Indices[20] = 4;
            Indices[21] = 1; Indices[22] = 2; Indices[23] = 4;
            Indices[24] = 6; Indices[25] = 4; Indices[26] = 7;
            Indices[27] = 7; Indices[28] = 4; Indices[29] = 5;
            Indices[30] = 6; Indices[31] = 7; Indices[32] = 1;
            Indices[33] = 7; Indices[34] = 0; Indices[35] = 1;

            Dimensions = new Vector3(x, y, z);
        }

        public Cuboid() : this(Vector3.Zero, 1.0f, 1.0f, 1.0f)
        {
        }

        //Initialize Cuboid
        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {
            Texture = contentManager.Load<Texture2D>("Textures/GrayNoise64");
            Verts[0].TextureCoordinate = new Vector2(1, 0);
            Verts[1].TextureCoordinate = new Vector2(0, 0);
            Verts[2].TextureCoordinate = new Vector2(0, 1);
            Verts[3].TextureCoordinate = new Vector2(1, 1);
            Verts[4].TextureCoordinate = new Vector2(1,1);
            Verts[5].TextureCoordinate = new Vector2(0,1);
            Verts[6].TextureCoordinate = new Vector2(1,0);
            Verts[7].TextureCoordinate = new Vector2(0,0);

            Verts[0].Color = Color.DarkGray;
            Verts[1].Color = Color.DarkGray;
            Verts[2].Color = Color.DarkRed;
            Verts[3].Color = Color.DarkRed;
            Verts[4].Color = Color.DarkRed;
            Verts[5].Color = Color.DarkRed;
            Verts[6].Color = Color.DarkGray;
            Verts[7].Color = Color.DarkGray;

            Effect = new BasicEffect(device);

            // Set up the vertex buffer
            vBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), 8, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColorTexture>(Verts);

            // Set up index Buffer
            iBuffer = new IndexBuffer(device, typeof(short), Indices.Length, BufferUsage.WriteOnly);
            iBuffer.SetData(Indices);
        }

        //Draw Cuboid to Screen
        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
            Effect.Projection = projection;
            Effect.View = view;
            Effect.World = world;

            Effect.TextureEnabled = true;
            Effect.VertexColorEnabled = true;
            Effect.Texture = Texture;

            Effect.GraphicsDevice.SetVertexBuffer(vBuffer);
            Effect.GraphicsDevice.Indices = iBuffer;

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }

    }
}

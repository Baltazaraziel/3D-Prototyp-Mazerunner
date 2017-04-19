using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _3DPrototypMazeRunner
{
    class Plane
    {
        private short[] Indices = new short[6];
        private VertexPositionColorTexture[] Verts = new VertexPositionColorTexture[4];
        private Vector3 Position = Vector3.Zero;
        private Vector3 Normal;
        private Vector3 Up;
        private Vector2 Dimensions;
        private Texture2D Texture;
        private BasicEffect Effect;
        private VertexBuffer vBuffer;
        private IndexBuffer iBuffer;

        public Plane(Vector3 center, Vector3 normal, Vector3 up, float width, float height)
        {
            Position = center;
            Normal = normal;
            Up = up;

            // Calculate the plane corners
            Vector3 Left = Vector3.Cross(normal, up);
            Vector3 uppercenter = (Up * height / 2) + center;
            Verts[1].Position = uppercenter + (Left * width / 2);
            Verts[0].Position = uppercenter - (Left * width / 2);
            Verts[2].Position = Verts[1].Position - (Up * height);
            Verts[3].Position = Verts[0].Position - (Up * height);

            //Set Indices
            Indices[0] = 0;
            Indices[1] = 3;
            Indices[2] = 1;
            Indices[3] = 2;
            Indices[4] = 1;
            Indices[5] = 3;

            Dimensions = new Vector2(width, height);
        }

        public Plane(float width, float height) : this(Vector3.Zero, Vector3.Up, Vector3.UnitX, width, height)
        {
        }

        public Plane() : this(10.0f, 10.0f)
        {
        }

        //Initialize Plane with Texture and set BasicEffect
        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {
            Texture = contentManager.Load<Texture2D>("Textures/FloorTile30");

            Verts[0].TextureCoordinate = new Vector2(0, 0);
            Verts[1].TextureCoordinate = new Vector2(Dimensions.X/3+Dimensions.X%3, 0);
            Verts[2].TextureCoordinate = new Vector2(Dimensions.X / 3 + Dimensions.X % 3, Dimensions.Y / 3 + Dimensions.Y % 3);
            Verts[3].TextureCoordinate = new Vector2(0, Dimensions.Y / 3 + Dimensions.Y % 3);

            Verts[0].Color = Color.Black;
            Verts[1].Color = Color.DeepPink;
            Verts[2].Color = Color.Gold;
            Verts[3].Color = Color.HotPink;

            Effect = new BasicEffect(device);

            // Set up the vertex buffer
            vBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), 4, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColorTexture>(Verts);

            // Set up index Buffer
            iBuffer = new IndexBuffer(device, typeof(short), Indices.Length, BufferUsage.WriteOnly);
            iBuffer.SetData(Indices);
        }

        //Draw Plane to Screen
        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
            Effect.Projection = projection;
            Effect.View = view;
            Effect.World = world;

            Effect.TextureEnabled = true;
            //Effect.VertexColorEnabled = true;
            Effect.Texture = Texture;

            Effect.GraphicsDevice.SetVertexBuffer(vBuffer);
            Effect.GraphicsDevice.Indices = iBuffer;



            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,2);
            }
        }

        public VertexPositionColorTexture[] getEdges()
        {
            return Verts;
        }

    }
}

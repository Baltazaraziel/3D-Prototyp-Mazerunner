using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysDraw = System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DPrototypMazeRunner;

namespace _3DPrototypMazeRunner
{
    /// <summary>
    /// Map reading a Bitmap to generate 3D Version
    /// </summary>
    class Map
    {
        private Plane Ground;
        private List<Cuboid> Walls;
        //private List<Collectables> Collectables;
        public Vector3 StartPos;
        public Vector3 Destination;
        private Vector2 Dimensions;
        private int Level;

        //public static Vector3 Startpos { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="level">Level to build</param>
        public Map(int level)
        {
            Level = level;
        }


        /// <summary>
        /// Initializes members of Map and Loads Content
        /// </summary>
        /// <param name="contentManager">Monogame ContentManager needed to load texture</param>
        /// <param name="device">GraphicsDevice used to render</param>
        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {
            Texture2D tex;
            Dimensions = new Vector2();

            //Load Bitmap of Level specified in the constructor
            switch (Level)
            {
                case 1:
                    tex = contentManager.Load<Texture2D>("Bitmaps/Level1");
                    break;
                default:
                    tex = contentManager.Load<Texture2D>("Bitmaps/Level1");
                    break;
            }

            Dimensions.X = tex.Width;
            Dimensions.Y = tex.Height;

            //Set Ground
            Ground = new Plane(new Vector3(Dimensions.X/2*10, 0, Dimensions.Y/2*10), Vector3.Up, Vector3.UnitX, Dimensions.X*10, Dimensions.Y*10);
            Ground.Initialize(contentManager, device);

            //Initialize Lists
            Walls = new List<Cuboid>();
            //Collectables = new List<Collectables>();

            //Build Level based on Colors used in Bitmap
            Color[] colors1D = new Color[(int)Dimensions.X * (int)Dimensions.Y];
            tex.GetData(colors1D);

            for (int i = 0; i < Dimensions.X; ++i)
            {
                for (int j = 0; j < Dimensions.Y; ++j)
                {
                    Color check = colors1D[(int)Dimensions.X*i + j];

                    if (check == Color.Black)
                    {
                        Walls.Add(new Cuboid(new Vector3(10.0f*i+5.0f, 10, 10.0f*j+5.0f), 10.0f, 20.0f, 10.0f));
                        Walls[Walls.Count-1].Initialize(contentManager, device);
                    }
                    else if (check == Color.Red)
                    {
                        StartPos = new Vector3(i, 0, j);
                    }
                    else if (check == Color.Lime)
                    {
                        Destination = new Vector3(i, 0, j);
                    }
                    else if (check == Color.Yellow)
                    {
                        //Collectables.Add();
                    }
                }
            }
        }

        /// <summary>
        /// Draw Map to Screen
        /// </summary>
        /// <param name="projection">Projection Matrix for Rendering</param>
        /// <param name="view">View Matrix for Rendering</param>
        /// <param name="world">World Matrix for Rendering</param>
        public void Draw(Matrix projection, Matrix view, Matrix world)
        {
            Ground.Draw(projection, view, world);
            foreach (Cuboid c in Walls)
            {
                c.Draw(projection, view, world);
            }
            /*foreach (Collectable col in Collectables)
            {
                col.Draw(projection, view, world);
            }*/
        }

    }
}

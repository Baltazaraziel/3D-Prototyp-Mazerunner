using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DPrototypMazeRunner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Plane p1;
        private Cuboid c1;
        private Map m1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            p1 = new Plane(new Vector3(150,0,150), Vector3.Backward+Vector3.UnitY, Vector3.UnitX, 100, 100);
            p1.Initialize(Content, graphics.GraphicsDevice);
            c1 = new Cuboid();
            c1.Initialize(Content, graphics.GraphicsDevice);
            m1 = new Map(1);
            m1.Initialize(Content, graphics.GraphicsDevice);

            DepthStencilState temp = new DepthStencilState();
            temp.DepthBufferEnable = true;
            temp.DepthBufferWriteEnable = true;

            GraphicsDevice.DepthStencilState = temp;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //slowly zoom out
            height += 0.1f;

            base.Update(gameTime);
        }

        //camera height
        private float height = 100;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio, 0.1f, 500.0f);
            Matrix View =  Matrix.CreateLookAt(new Vector3(100,height, 100), new Vector3(150, 0, 150), Vector3.Up);
            Matrix World = Matrix.Identity;

            //p1.Draw(Projection, View, World);
            //c1.Draw(Projection, View, World);
            m1.Draw(Projection, View, World);

            base.Draw(gameTime);
        }
    }
}

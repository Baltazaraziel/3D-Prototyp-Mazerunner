using System;
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
        private Player player;
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
            player = new Player(m1.StartPos);
            player.Initialize(Content, graphics.GraphicsDevice);

            DepthStencilState temp = new DepthStencilState();
            temp.DepthBufferEnable = true;
            temp.DepthBufferWriteEnable = true;

            GraphicsDevice.DepthStencilState = temp;

            base.Initialize();
        }

        // Brauchen wir die Funktion?
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
            // Get some input.
            UpdateInput();
            player.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            // Get the keyboard state.
            KeyboardState currentKeyState = Keyboard.GetState();
            if (currentKeyState.IsKeyDown(Keys.A))
                player.pRotation += 0.10f;
            else if (currentKeyState.IsKeyDown(Keys.D))
                player.pRotation -= 0.10f;
            else
                // Rotate the model using the left thumbstick, and scale it down
                player.pRotation -= currentState.ThumbSticks.Left.X * 0.10f;

                // Create some velocity if the right trigger is down.
                Vector3 playerVelocityAdd = Vector3.Zero;

                // Find out what direction we should be thrusting, 
                // using rotation.
                playerVelocityAdd.X = -(float)Math.Sin(player.pRotation);
                playerVelocityAdd.Z = -(float)Math.Cos(player.pRotation);

            if (currentKeyState.IsKeyDown(Keys.W))
                playerVelocityAdd *= 0.05f;
            else
                // Now scale our direction by how hard the trigger is down.
                playerVelocityAdd *= currentState.Triggers.Right;

                // Finally, add this vector to our velocity.
                player.pVelocity += playerVelocityAdd;

                GamePad.SetVibration(PlayerIndex.One,
                    currentState.Triggers.Right,
                    currentState.Triggers.Right);


                // In case you get lost, press A or Enter to warp back to the center.
                if (currentState.Buttons.A == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Enter))
                {
                    player.pPosition = m1.StartPos;
                    player.pVelocity = Vector3.Zero;
                    player.pRotation = 0.0f;
                }
        }

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
            Matrix View = Matrix.CreateLookAt(new Vector3( 100, 100, 10), player.pPosition, Vector3.Up);
            //Matrix View =  Matrix.CreateLookAt(new Vector3(100,height, 100), new Vector3(150, 0, 150), Vector3.Up);
            Matrix World = Matrix.Identity;

            //p1.Draw(Projection, View, World);
            //c1.Draw(Projection, View, World);
            m1.Draw(Projection, View, World);
            player.Draw(Projection, View, World);


            base.Draw(gameTime);
        }
    }
}

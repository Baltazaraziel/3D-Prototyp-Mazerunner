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
    class Hud
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private bool gameWon;
        private bool gameLost;
        private GraphicsDevice graphicsDevice;
        private int nbCollectables;
        private TimeSpan timeLeft;
        public Hud()
        {
            gameLost = false;
            gameWon = false;
        }

        public void Initialize(ContentManager contentManager, GraphicsDevice device)
        {
            graphicsDevice = device;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(device);
            font = contentManager.Load<SpriteFont>("Fonts/SpriteFont/SKEWED_SLASH");
        }

        public void Update(Map map)
        {
            gameLost = map.Lost;
            gameWon = map.Won;
            timeLeft = map.Timer;
            nbCollectables = map.NumberCollectablesLeft();
        }

        public void Draw()
        {
            //save states because spritebatch alters these -> 3d rendering broken
            BlendState beforeBlendStateState = graphicsDevice.BlendState;
            DepthStencilState beforeDepthStencilStateState = graphicsDevice.DepthStencilState;
            SamplerState beforeSamplerState = graphicsDevice.SamplerStates[0];

            //draw HUD to screen
            spriteBatch.Begin();
           
            //still need to go and collect
            if (!gameLost && !gameWon)
            {
                if (nbCollectables > 0)
                {
                    spriteBatch.DrawString(font, "You need to Collect another " + nbCollectables + " Collectables",
                        new Vector2(graphicsDevice.DisplayMode.Width/2 - 300, 10), Color.Gray);
                    spriteBatch.DrawString(font, timeLeft.Minutes + "min " + timeLeft.Seconds + "sec left",
                        new Vector2(graphicsDevice.DisplayMode.Width/2 - 300, 50), Color.Gray);
                }
                else
                {
                    spriteBatch.DrawString(font, "Find the End of the maze",
                    new Vector2(graphicsDevice.DisplayMode.Width / 2 - 300, 10), Color.DarkRed);
                    spriteBatch.DrawString(font, timeLeft.Minutes + "min " + timeLeft.Seconds + "sec left",
                        new Vector2(graphicsDevice.DisplayMode.Width / 2 - 300, 50), Color.Gray);
                }
                
            }
            //won the game
            else if (gameWon)
            {
                spriteBatch.DrawString(font, "You beat the Maze with " +
                                             timeLeft.Minutes + "min " + timeLeft.Seconds + "sec left",
                    new Vector2(graphicsDevice.DisplayMode.Width/2 -600, 10), Color.Gray);
            }
            //lost the game
            else
            {
                spriteBatch.DrawString(font, "You lost",
                    new Vector2(graphicsDevice.DisplayMode.Width / 2 - 300, 10), Color.Gray);
            }
            
            spriteBatch.End();

            //restore values for 3d rendering
            graphicsDevice.BlendState = beforeBlendStateState;
            graphicsDevice.DepthStencilState = beforeDepthStencilStateState;
            graphicsDevice.SamplerStates[0] = beforeSamplerState;
        }
    }
}

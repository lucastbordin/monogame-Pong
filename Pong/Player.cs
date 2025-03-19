using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Player
    {   
        Vector2 position;
        Texture2D texture;
        Keys keyUp;
        Keys keyDown;
        Game game;

        public float player_speed = 5F;

        public Player(Game game, Vector2 position, Texture2D texture, Keys keyUp, Keys keyDown)
        {   
            this.game = game;
            this.position = position;
            this.texture = texture;
            this.keyUp = keyUp;
            this.keyDown = keyDown;
        }

        public void Update()
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(keyUp))
            {
                position.Y -= player_speed;
            }
            else if (keyboard.IsKeyDown(keyDown)) 
            {
                position.Y += player_speed;
            }

            var viewport = game.GraphicsDevice.Viewport;

            if (position.Y < 0)
            {
                position.Y = 0;
            }

            if (position.Y + texture.Height > viewport.Height)
            {
                position.Y = viewport.Height - texture.Height;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void HasCollided(Ball ball)
        {
            var ballBounds = ball.GetBounds();
            var playerBounds = GetBounds();

            if (playerBounds.Intersects(ballBounds))
            {
                if (ball.Velocity.X < 0)
                {
                    ball.SetPosition(position.X + texture.Width);
                }
                else
                {
                    ball.SetPosition(position.X - texture.Width);
                }

                ball.InvertVelocity();
            }
        }
    }
}

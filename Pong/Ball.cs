using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Ball
    {
        public const float BALL_VELOCITY = 2.5F;

        Texture2D texture;
        Game game;
        Vector2 position;
        Vector2 velocity;
        bool outOfBounds = false;

        public bool OutOfBounds { get => outOfBounds; }
        public Vector2 Position { get => position; }
        public Vector2 Velocity { get => velocity; }
        public Texture2D Texture { get => texture; }


        public Ball(Game game, Texture2D texture)
        {
            this.game = game;
            this.texture = texture;
        }

        public void SetInStartPosition(Vector2 previousVelocity)
        {
            var viewport = game.GraphicsDevice.Viewport;

            position.Y = (viewport.Height/2) - (texture.Height/2);
            position.X = (viewport.Width/2) - (texture.Width/2);

            Random rnd = new Random();
            int verticalRandom = rnd.Next(1, 15);

            if (previousVelocity.X < 0)
            {
                velocity = new Vector2(BALL_VELOCITY, -BALL_VELOCITY + verticalRandom/10);
            }
            else 
            { 
                velocity = new Vector2(-BALL_VELOCITY, -BALL_VELOCITY + verticalRandom/10);
            }


            outOfBounds = false;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void SetPosition(float x)
        {
            position.X = x;
        }

        public void InvertVelocity()
        {
            Random rnd = new Random();
            int verticalRandom = rnd.Next(-500, 500);

            velocity.X += velocity.X * 0.15F;
            velocity.Y += (velocity.Y * 0.15F) + verticalRandom/100;
            velocity.X *= -1;
        }
        public void Update()
        {

            var viewport = game.GraphicsDevice.Viewport;

            if (position.Y < 0)
            {
                position.Y = 0;
                velocity.Y *= -1; //inverte velocidade
            }

            if (position.Y + texture.Height > viewport.Height)
            {
                position.Y = viewport.Height - texture.Height;
                velocity.Y *= -1; //inverte velocidade
            }

            position += velocity;

            if (position.X + texture.Width < 0 || position.X > viewport.Width)
            {
                outOfBounds = true;
                //velocity = Vector2.Zero;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }



    
}

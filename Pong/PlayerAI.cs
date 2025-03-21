using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class PlayerAI
    {
        Vector2 position;
        Texture2D texture;
        Keys keyUp;
        Keys keyDown;
        Game game;
        Ball ball;

        public Vector2 Position { get => position; }
        public Texture2D Texture { get => texture; }

        public const float START_CATCH_DISTANCE_OFFSET = 200;

        public float player_speed = 5F;

        public PlayerAI(Game game, Vector2 position, Texture2D texture, Keys keyUp, Keys keyDown, Ball ball)
        {
            this.game = game;
            this.position = position;
            this.texture = texture;
            this.keyUp = keyUp;
            this.keyDown = keyDown;
            this.ball = ball;
        }

        public void Update()
        {

            if (ball.Position.Y + (ball.Texture.Height/2) < position.Y - 5 + (texture.Height/2))
            {
                position.Y -= player_speed;
            }
            else if (ball.Position.Y + (ball.Texture.Height / 2) > position.Y + 5 + (texture.Height / 2))
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

            var viewport = game.GraphicsDevice.Viewport;

            float startCatchDistance = viewport.Width - START_CATCH_DISTANCE_OFFSET;
            int catchDistance = viewport.Width - 55;

            //if ((position.Y + (texture.Height / 2)) - (ball.Position.Y + (ball.Texture.Height / 2)) > 50 
            //    || (position.Y + (texture.Height / 2)) - (ball.Position.Y + (ball.Texture.Height / 2)) < -50)
            if (ball.Velocity.X > 0)
            {
                if (catchDistance > ball.Position.X && ball.Position.X > startCatchDistance)
                {
                    //var ballDistance = position.X - ball.Position.X;

                    var ballRelativePosition = ball.Position.X - startCatchDistance;

                    float linePosition;

                    if (ballRelativePosition < (position.X - startCatchDistance) / 2)
                    {
                        linePosition = position.X - ballRelativePosition;
                    }
                    else
                    {
                        linePosition = ball.Position.X;
                    }

                    DrawLineBetween(spriteBatch,
                        new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2)),
                        new Vector2(linePosition, ball.Position.Y + (ball.Texture.Height / 2)),
                        10,
                        Color.White);
                }

                if (ball.Position.X > catchDistance)
                {
                    DrawLineBetween(spriteBatch,
                        new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2)),
                        new Vector2(ball.Position.X + (ball.Texture.Width / 2), ball.Position.Y + (ball.Texture.Height / 2)),
                        10,
                        Color.White);
                }
            }
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

        public void DrawLineBetween(
            SpriteBatch spriteBatch,
            Vector2 startPos,
            Vector2 endPos,
            int thickness,
            Color color)
        {
            // Create a texture as wide as the distance between two points and as high as
            // the desired thickness of the line.
            var distance = (int)Vector2.Distance(startPos, endPos);
            var texture = new Texture2D(spriteBatch.GraphicsDevice, distance, thickness);

            // Fill texture with given color.
            var data = new Color[distance * thickness];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }
            texture.SetData(data);

            // Rotate about the beginning middle of the line.
            var rotation = (float)Math.Atan2(endPos.Y - startPos.Y, endPos.X - startPos.X);
            var origin = new Vector2(0, thickness / 2);

            spriteBatch.Draw(
                texture,
                startPos,
                null,
                Color.White,
                rotation,
                origin,
                1.0f,
                SpriteEffects.None,
                1.0f);
        }
    }
}

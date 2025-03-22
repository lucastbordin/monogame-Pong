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

        bool flagAnimationStarted = false;
        float linePositionY = 0;

        public Vector2 Position { get => position; }
        public Texture2D Texture { get => texture; }

        public const float START_CATCH_DISTANCE_OFFSET = 300;

        public float player_speed = 0F;

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

            var ballMiddleY = ball.Position.Y - ball.Texture.Height / 2;
            var middleY = position.Y + texture.Height / 2;

            


            if (ball.Velocity.X < 0) flagAnimationStarted = false;

            //TODO find actual line position 
            linePositionY = ball.Position.Y;


            if (flagAnimationStarted || (ball.Velocity.X > 0 && (ballMiddleY > middleY + 100 || ballMiddleY < middleY - 100)) /*&& (ball.Velocity.X>5 || ball.Velocity.Y>5)*/ )
            {
                if (catchDistance > ball.Position.X && ball.Position.X > startCatchDistance)
                {
                    //var ballDistance = position.X - ball.Position.X;
                    flagAnimationStarted = true;

                    var ballRelativePositionX = ball.Position.X - startCatchDistance;
                    //var ballRelativePositionY = ball.Position.Y - position.Y + (texture.Height/2);
                    var ballDistance = (ball.Position.X - position.X);

                    float linePositionX;
                    

                    if (ballRelativePositionX < (position.X - startCatchDistance) / 2)
                    {
                        linePositionX = position.X - ballRelativePositionX;
                        
                    }
                    else
                    {
                        linePositionX = ball.Position.X;
                        linePositionY = ball.Position.Y;
                    }

                    DrawLineBetween(spriteBatch,
                        new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2)),
                        new Vector2(linePositionX, linePositionY),
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

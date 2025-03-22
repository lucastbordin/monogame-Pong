using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player player1;
        PlayerAI player2;
        Ball ball;

        bool flagAnimationStarted = false;

        SpriteFont font1;

        HUD gameHUD;

        public const float START_CATCH_DISTANCE_OFFSET = 300;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D barTexture = Content.Load<Texture2D>("assets/bar");
            Texture2D ballTexture = Content.Load<Texture2D>("assets/ball");

            font1 = Content.Load<SpriteFont>("ScoreFont");
            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            
            gameHUD = new HUD(0,0, new Vector2(viewport.Width * 0.25f, viewport.Height / 4), new Vector2(viewport.Width * 0.75f, viewport.Height / 4), font1);

            ball = new Ball(this, ballTexture);
            player1 = new Player(this, new Vector2(10, 100), barTexture, Keys.W, Keys.S);
            player2 = new PlayerAI(this, new Vector2(760, 100), barTexture, Keys.Up, Keys.Down, ball);

            ball.SetInStartPosition(new Vector2(-2.5F));

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (ball.OutOfBounds)
            {
                if (ball.Velocity.X > 0)
                {
                    gameHUD.ScoreP1 += 1;
                }
                else
                {
                    gameHUD.ScoreP2 += 1;
                }

                    ball.SetInStartPosition(-ball.Velocity);
            }

            player1.Update();
            player2.Update();

            Viewport viewport = _graphics.GraphicsDevice.Viewport;

            var ballMiddleY = ball.Position.Y - ball.Texture.Height / 2;
            var player2MiddleY = player2.Position.Y + player2.Texture.Height / 2;

            if (ball.Velocity.X < 0) flagAnimationStarted = false;

            if (flagAnimationStarted || (ball.Position.X > viewport.Width - START_CATCH_DISTANCE_OFFSET / 2 && ball.Velocity.X > 0 && (ballMiddleY > player2MiddleY+100 || ballMiddleY < player2MiddleY-100)) /*&& (ball.Velocity.X > 5 || ball.Velocity.Y > 5)*/)
            {
                flagAnimationStarted = true;

                float direction = (player2.Position.Y + player2.Texture.Height/2 -20) - (ball.Position.Y - ball.Texture.Height/2);

                //if (ball.Position.Y > player2.Position.Y) 
                //    ball.Position += new Vector2 (ball.Velocity.X, -ball.Position.Y/player2.Position.Y);
                //else
                //    ball.Position += new Vector2(ball.Velocity.X, player2.Position.Y / ball.Position.Y);

                var ballDistance = (ball.Position.X - player2.Position.X);
                if (ballDistance != 0)
                    ball.Position += new Vector2(ball.Velocity.X, (-( player2MiddleY - ballMiddleY)/(ballDistance/ball.Velocity.X)));
                else
                    ball.Position += new Vector2(ball.Velocity.X,0);

            }
            else
            {
                ball.Position += ball.Velocity;
            }

            ball.Update();

            player1.HasCollided(ball);
            player2.HasCollided(ball);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            gameHUD.Draw(_spriteBatch);
            player1.Draw(_spriteBatch);
            player2.Draw(_spriteBatch);
            ball.Draw(_spriteBatch);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

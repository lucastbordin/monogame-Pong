using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player player1;
        PlayerAI player2;
        Ball ball;

        SpriteFont font1;

        HUD gameHUD;

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

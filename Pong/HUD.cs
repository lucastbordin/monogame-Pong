using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class HUD
    {
        int scoreP1;
        int scoreP2;
        Vector2 scoreP1Position;
        Vector2 scoreP2Position;
        SpriteFont scoreFont;

        public int ScoreP1 { get => scoreP1; set => scoreP1 = value; }
        public int ScoreP2 { get => scoreP2; set => scoreP2 = value; }

        public HUD(int scoreP1, int scoreP2, Vector2 scoreP1Position, Vector2 scoreP2Position, SpriteFont scoreFont) {
            this.scoreP1 = scoreP1;
            this.scoreP2 = scoreP2;
            this.scoreP1Position = scoreP1Position;
            this.scoreP2Position = scoreP2Position;
            this.scoreFont = scoreFont;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string output1 = "Score: "+scoreP1;
            string output2 = "Score: "+scoreP2;
            Vector2 FontOrigin1 = scoreFont.MeasureString(output1) / 2;
            Vector2 FontOrigin2 = scoreFont.MeasureString(output2) / 2;
            spriteBatch.DrawString(scoreFont, output1, scoreP1Position, Color.White,
                0, FontOrigin1, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(scoreFont, output2, scoreP2Position, Color.White,
                0, FontOrigin2, 1.0f, SpriteEffects.None, 0.5f);

        }

    }
}

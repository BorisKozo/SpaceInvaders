using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
  public class GameOverScene:Scene
  {

    private TextureImage m_Background;
    private TextureImage m_GameOverImage;
    private SpriteFont m_GameFont;

    public int Score { get; set; }

    public GameOverScene():base("Game Over Scene") { }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_GameFont = Content.Load<SpriteFont>("GameFont");
      m_Background = TextureImageManager.Instance.GetTextureImage("Images//background");
      m_GameOverImage = TextureImageManager.Instance.GetTextureImage("Images//GameOver");
    }

    protected override void BeforeDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(m_Background.Image, new Vector2(), Color.DarkGray);
      spriteBatch.Draw(m_GameOverImage.Image, new Vector2(220,160), Color.White);
      spriteBatch.DrawString(m_GameFont, "SCORE ", new Vector2(345, 250), Color.White);
      spriteBatch.DrawString(m_GameFont, Score.ToString(), new Vector2(430, 250), Color.GreenYellow);
      spriteBatch.DrawString(m_GameFont, "PRESS ENTER TO START A NEW GAME", new Vector2(240, 360), Color.White);
    }

    public override void TransitionReset(string transitionWord, object transitionData)
    {
      Score = (int)transitionData;
    }

    protected override void InnerUpdate(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();
      if (state.IsKeyDown(Keys.Enter))
        Game2D.CurrentGame.Arbiter.Transition("Game Start");
    }

    protected override void InnerDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {

    }
  }
}

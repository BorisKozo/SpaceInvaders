using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
  public class GameScene : Scene
  {
    private Ship m_Ship;
    private TextureImage m_Background;
    private InvaderGrid m_Invaders;
    private Shield[] m_Shileds;
    private Mothership m_Mothership;
    private SoundEffectInstance m_MothershipAppearSound;

    private SpriteFont m_GameFont;

    private bool m_PauseForMessage;
    private string m_Message;
    private TimeSpan m_MessageTotalTime;
    private TimeSpan m_MessageCurrentTime;

    private int m_InvaderFireSpeed;

    public int Score { get; set; }
    public int Lives { get; set; }

    private void ResetScene(bool resetShields)
    {
      m_PauseForMessage = false;
      SkipUpdateDisplayedItems = false;
      m_Mothership.Alive = false;
      m_Invaders.Reset(m_InvaderFireSpeed);
      m_Ship.Reset();
      m_BottomSprites.Clear();
      for (int i = 0; i < 3; i++)
      {
        m_Shileds[i].Alive = true;
        if (resetShields)
          m_Shileds[i].Reset();
      }
    }


    private void NewGame()
    {
      Score = 0;
      Lives = 3;
      m_InvaderFireSpeed = 50000;

    }

    public GameScene()
      : base("Game Scene")
    {
      NewGame();
    }

    public override void Initialize(GraphicsDeviceManager graphics, Game2D game)
    {
      base.Initialize(graphics, game);
      m_Ship = new Ship(new Vector2(370, 540));
      m_Ship.ShipDead += new EventHandler(Ship_ShipDead);
      m_Invaders = new InvaderGrid(this, new Vector2(40, 70), new Vector2(game.ScreenWidth, game.ScreenHeight), m_InvaderFireSpeed);
      m_Shileds = new Shield[3];
      float shieldsHeight = 450;
      m_Shileds[0] = new Shield(new Vector2(100, shieldsHeight), "One");
      m_Shileds[1] = new Shield(new Vector2(340, shieldsHeight), "Two");
      m_Shileds[2] = new Shield(new Vector2(580, shieldsHeight), "Three");
      m_Mothership = new Mothership(new Vector2(-80, 30));
      m_Mothership.LoopType = AnimatedSpriteLoopType.LoopBackAndForth;
      m_Mothership.AnimationSpeed = TimeSpan.FromMilliseconds(300);
      m_Mothership.Dead += new InvaderDeadEventHandler(Mothership_Dead);


      m_Invaders.GameLost += new EventHandler(Invaders_GameLost);
      m_Invaders.GameWon += new EventHandler(Invaders_GameWon);
      m_Invaders.ReachedShieldLevel += new EventHandler(Invaders_ReachedShieldLevel);

    }

    void Ship_ShipDead(object sender, EventArgs e)
    {
      Invaders_GameLost(sender, e);
    }

    void Mothership_Dead(object sender, InvaderDeathEventArgs args)
    {
      Vector2 position = (sender as Sprite).Position;
      position.X = position.X + 40;
      position.Y = position.Y + 12;
      FloatingNumber number = new FloatingNumber(position, args.Score, m_GameFont,Color.GreenYellow);
      AddBottomSprite(number);
      Score += args.Score;
    }

    void Invaders_ReachedShieldLevel(object sender, EventArgs e)
    {
      for (int i = 0; i < 3; i++)
      {
        m_Shileds[i].Alive = false;
      }
    }

    void Invaders_GameWon(object sender, EventArgs e)
    {
      m_InvaderFireSpeed = Math.Max(3000, m_InvaderFireSpeed - MathCommon.FloatToIntRound((float)m_InvaderFireSpeed * 0.05f));
      ResetScene(false);
    }

    void Invaders_GameLost(object sender, EventArgs e)
    {
      if (Lives > 0)
      {
        Lives--;
        m_Message = "Lost a life!";
        m_MessageTotalTime = TimeSpan.FromSeconds(2);
        m_MessageCurrentTime = TimeSpan.FromSeconds(0);
        m_PauseForMessage = true;
        SkipUpdateDisplayedItems = true;
      }
      else
      {
        Game2D.CurrentGame.Arbiter.Transition("Game Over", Score);
      }
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_GameFont = Content.Load<SpriteFont>("GameFont");
      m_Background = TextureImageManager.Instance.GetTextureImage("Images//background");
      m_Ship.LoadContent(content);
      m_Invaders.LoadContent(content);
      m_Mothership.LoadContent(content);

      for (int i = 0; i < 3; i++)
      {
        m_Shileds[i].LoadContent(content);
        AddItem("Shild" + i.ToString(), m_Shileds[i]);
        m_Shileds[i].RegisterCollision(CollisionCoordinator);
      }
      AddItem("The Ship", m_Ship);
      AddItem("The Invaders", m_Invaders);
      AddItem("Mothership", m_Mothership);

      m_Ship.RegisterCollision(CollisionCoordinator);
      m_Invaders.RegisterCollision(CollisionCoordinator);
      m_Mothership.RegisterCollision(CollisionCoordinator);
      m_Mothership.Alive = false;
      m_MothershipAppearSound = SoundEffectManager.Instance.GetSoundEffect("Sound//MotherShipAppears");

    }

    private void HandleMessageDrawing(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (!m_PauseForMessage) return;
      m_MessageCurrentTime = m_MessageCurrentTime.Add(gameTime.ElapsedGameTime);
      if (m_MessageCurrentTime >= m_MessageTotalTime)
      {
        m_PauseForMessage = false;
        SkipUpdateDisplayedItems = false;
        ResetScene(true);
      }

      spriteBatch.DrawString(m_GameFont, m_Message, new Vector2(200, 200), Color.Red, 0, new Vector2(), Convert.ToSingle(m_MessageCurrentTime.TotalMilliseconds / 1000.0) + 2f, SpriteEffects.None, 0);

    }

    public override void TransitionReset(string transitionWord, object transitionData)
    {
      NewGame();
      ResetScene(true);
    }

    protected override void InnerUpdate(GameTime gameTime)
    {
       
      if (!m_Mothership.Alive && Game2D.GameRandom.Next(800) == 0)
      {
         m_Mothership.Alive = true;
        m_Mothership.SetLeft(-80);
        m_Mothership.ResetLoop();
        m_MothershipAppearSound.Play();
      }
    }

    protected override void BeforeDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(m_Background.Image, new Vector2(), Color.DarkGray);
      spriteBatch.DrawString(m_GameFont, "SCORE ", new Vector2(10, 5), Color.White);
      spriteBatch.DrawString(m_GameFont, Score.ToString(), new Vector2(80, 5), Color.GreenYellow);
      spriteBatch.DrawString(m_GameFont, "LIVES ", new Vector2(700, 5), Color.White);
      spriteBatch.DrawString(m_GameFont, Lives.ToString(), new Vector2(760, 5), Color.GreenYellow);

    }

    protected override void InnerDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {


      


    }

    protected override void AfterDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      HandleMessageDrawing(gameTime, spriteBatch);
    }

  }
}

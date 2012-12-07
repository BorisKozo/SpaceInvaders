using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
  public class Shield : Sprite
  {

    private Texture2D m_TextureToDraw = null;
    private Color[,] m_UpdatedImage;
    private string m_name;
    private SoundEffectInstance m_HitSound;

    public Shield(Vector2 position,string name)
      :base("Images//Shield",position)
    {
      m_name = name;
    }

    private void ReloadTexture()
    {
      Color[] tempColors = new Color[this.Width * this.Height];
      int k=0;
      for (int i = 0; i < Height; i++)
        for (int j = 0; j < Width; j++)
          tempColors[k++] = m_UpdatedImage[j, i];

      m_TextureToDraw = new Texture2D(Game2D.CurrentGame.GraphicsDevice, this.Width, this.Height);
      m_TextureToDraw.SetData<Color>(tempColors);
      m_TextureToDraw.Name = m_name;
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_UpdatedImage =  (Color[,])base.GetImageData().Clone();
      ReloadTexture();
      m_HitSound = SoundEffectManager.Instance.GetSoundEffect("Sound\\ShieldHit");
    }


    protected override Color[,] GetImageData()
    {
      return m_UpdatedImage;
    }

    public override void Update(GameTime gameTime)
    {
      //if (m_TextureToDraw == null) ReloadTexture();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (Alive)
        spriteBatch.Draw(m_TextureToDraw, Position, Color.White);

    }

    public override List<string> MyCollisionTags
    {
      get
      {
        List<string> result = new List<string>();
        result.Add("SHIELD");
        return result;
      }
    }

    private void Explode(int x, int y)
    {
      int size = 6;
      for (int i = x - size; i <= x + size; i++)
      {
        for (int j = y - size; j<= y+size; j++)
          if (i >= 0 && i < Width && j > 0 && j < Height)
          {
            if (MainGame.GameRandom.Next(2) == 0)
              m_UpdatedImage[i, j] = Color.Transparent;
          }

      }
    }

    public override void Collide(Sprite other, Vector2 screenXY)
    {
      Vector2 localXY = SelfXYFromScreenXY(screenXY);
      float temp = MathHelper.Clamp(localXY.X, 0, Width - 1);
      int x = MathCommon.FloatToIntFloor(temp);
      temp = MathHelper.Clamp(localXY.Y, 0, Height - 1);
      int y = MathCommon.FloatToIntFloor(temp);

      if (other is ShipMissile)
      {
        while ((y+1) < Height - 1 && m_UpdatedImage[x, y+1].A != 0)
          y++;

        Explode(x, y);
        ReloadTexture();
        other.Alive = false;
        m_HitSound.Play();
      }

      if (other is InvaderMissile1 || other is InvaderMissile2)
      {
        while ((y - 1) >= 0 && m_UpdatedImage[x, y - 1].A != 0)
          y--;

        Explode(x, y);
        ReloadTexture();
        other.Alive = false;
        m_HitSound.Play();
      }

      

    }

    public void RegisterCollision(SpriteCollisionCoordinator CollisionCoordinator)
    {
      CollisionCoordinator.AddItem(this,false);
    }

    internal void Reset()
    {
      m_UpdatedImage = (Color[,])base.GetImageData().Clone();
      ReloadTexture();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNA.Common
{
  public class Sprite : IDisplayedItem
  {
    protected TextureImage m_Image;
    private Vector2 m_Position;
    protected string m_AssetName;
    private Vector2 m_Origin;
    private float m_Angle;
    private float m_Scale;
    private bool m_Alive;
    protected TimeCount m_DeathTimer;

    public Rectangle? m_BoundingRect;

    public Sprite(string assetName, Vector2 position, Vector2 rotationCenter, float angle, float scale)
    {

      m_AssetName = assetName;
      Position = position;
      Origin = rotationCenter;
      Angle = angle;
      Scale = scale;
      Alive = true;
      m_DeathTimer = new TimeCount();
      m_BoundingRect = null;
    }

    public Sprite(string assetName, Vector2 position, Vector2 rotationCenter, float angle)
      : this(assetName, position, rotationCenter, angle, 1)
    { }


    public Sprite(string assetName, Vector2 position)
      : this(assetName, position, new Vector2(), 0, 1)
    { }

    protected Sprite(Sprite other)
    {
      m_AssetName = other.m_AssetName;
      m_Alive = other.m_Alive;
      m_Angle = other.m_Angle;
      m_BoundingRect = null;
      m_Image = other.m_Image;
      m_Origin = other.m_Origin;
      m_Position = other.m_Position;
      m_Scale = other.m_Scale;
    }

    #region IDisplayedItem Members

    public virtual void Update(GameTime gameTime)
    {
      if (m_DeathTimer.Active)
      {
        m_DeathTimer.CurrentTimeSpan = m_DeathTimer.CurrentTimeSpan.Add(gameTime.ElapsedGameTime);
        if (m_DeathTimer.CurrentTimeSpan >= m_DeathTimer.TotalTimeSpan)
        {
          Alive = false;
          m_DeathTimer.Active = false;
        }
      }
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (Alive)
        spriteBatch.Draw(m_Image.Image, Position, null, Color.White, Angle, Origin, Scale, SpriteEffects.None, 0);
    }

    #endregion

    public virtual void LoadContent(ContentManager content)
    {
      if (m_AssetName != "")
        m_Image = TextureImageManager.Instance.GetTextureImage(m_AssetName);
      else
        throw new ArgumentException("Empty asset name when initializing sprite");

      if (m_Image == null)
        throw new ArgumentException(String.Format("Unable to load asset {0} for sprite", m_AssetName));
    }

    protected virtual int ImageWidth()
    {
      return m_Image.Image.Width;
    }

    protected virtual int ImageHeight()
    {
      return m_Image.Image.Height;
    }

    protected virtual Color[,] GetImageData()
    {
      return Image.ImageData;
    }

    public void SetTop(float top)
    {
      float delta = m_Position.Y - BoundingRect().Top;
      m_Position.Y = top + delta;
      m_BoundingRect = null;
    }

    public void SetLeft(float left)
    {
      float delta = m_Position.X - BoundingRect().Left;
      m_Position.X = left+delta;
      m_BoundingRect = null;
    }

    public void SetPosition(Vector2 position)
    {
      SetLeft(position.X);
      SetTop(position.Y);
    }

    public void MoveVertical(float value)
    {
      Move(new Vector2(0, value));
    }

    public void MoveHorizontal(float value)
    {
      Move(new Vector2(value, 0));
    }

    public void Move(Vector2 value)
    {
      Rectangle boundingRect = BoundingRect();
      SetTop(boundingRect.Top + value.Y);
      SetLeft(boundingRect.Left + value.X);
    }

    public Rectangle BoundingRect()
    {
      if (m_BoundingRect.HasValue) return m_BoundingRect.Value;
      Matrix matrix = GetMatrix();
      Vector2 corner1 = Vector2.Transform(new Vector2(), matrix);
      Vector2 corner2 = Vector2.Transform(new Vector2(ImageWidth(),ImageHeight()), matrix);
      Vector2 corner3 = Vector2.Transform(new Vector2(0, ImageHeight()), matrix);
      Vector2 corner4 = Vector2.Transform(new Vector2(ImageWidth(), 0), matrix);

      int left = MathCommon.FloatToIntFloor(MathCommon.Min4(corner1.X, corner2.X, corner3.X, corner4.X));
      int bottom = MathCommon.FloatToIntFloor(MathCommon.Min4(corner1.Y, corner2.Y, corner3.Y, corner4.Y));
      int right = MathCommon.FloatToIntFloor(MathCommon.Max4(corner1.X, corner2.X, corner3.X, corner4.X));
      int top = MathCommon.FloatToIntFloor(MathCommon.Max4(corner1.Y, corner2.Y, corner3.Y, corner4.Y));

      m_BoundingRect = new Rectangle(left, bottom, right - left, top - bottom);
      return m_BoundingRect.Value;
    }

    public Matrix GetMatrix()
    {
      return Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0) * Matrix.CreateRotationZ(Angle) * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(m_Position.X, m_Position.Y, 0);
    }

    public Vector2 SelfXYFromScreenXY(Vector2 screenXY)
    {
      Matrix invMat = Matrix.Invert(GetMatrix());
      Vector2 result = Vector2.Transform(screenXY, invMat);
      return result;
    }


    #region Public Properties
    public int Width
    {
      get
      {
        return BoundingRect().Width;
      }
    }

    public int Height
    {
      get
      {
        return BoundingRect().Height;

      }
    }

    public Vector2 Position
    {
      get { return m_Position; }
      set { 
        m_Position = value;
        m_BoundingRect = null;
      }
    }

    public virtual TextureImage Image
    {
      get { return m_Image; }
    }

    public Vector2 Origin
    {
      get { return m_Origin; }
      set 
      { 
        m_Origin = value;
        m_BoundingRect = null; 
      }
    }

    public float Angle
    {
      get { return m_Angle; }
      set 
      { 
        m_Angle = value;
        m_BoundingRect = null; 
      }
    }

    public float Scale
    {
      get { return m_Scale; }
      set 
      { 
        m_Scale = value; 
        m_BoundingRect = null; 
      }
    }

    public bool Alive
    {
      get { return m_Alive; }
      set { m_Alive = value; }
    }
    #endregion

    #region Collision
    /// <summary>
    /// The tags of the sprites I will collide with
    /// </summary>
    public virtual List<string> CollideWithTags 
    {
      get 
      {
        return new List<string>();
      } 
    }

    /// <summary>
    /// My tags for other sprites to check
    /// </summary>
    public virtual List<string> MyCollisionTags
    {
      get
      {
        return new List<string>();
      }
    }

    public virtual void BeforeCollide(Sprite other, Vector2 screenXY){}
    public virtual void Collide(Sprite other, Vector2 screenXY){}
    public virtual void AfterCollide(Sprite other, Vector2 screenXY){}
    
    #endregion

    #region Static functions
    private static bool XYInRange(Color[,] imageData, int x, int y)
    {
      if (x < 0 || y < 0 || x >= imageData.GetLength(0) || y >= imageData.GetLength(1))
        return false;
      return true;
    }

    public static Vector2 SpritesCollide(Sprite sprite1, Sprite sprite2)
    {
      Rectangle rectangleA = sprite1.BoundingRect();
      Rectangle rectangleB = sprite2.BoundingRect();
      // Find the bounds of the rectangle intersection
      int top = Math.Max(rectangleA.Top, rectangleB.Top);
      int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
      int left = Math.Max(rectangleA.Left, rectangleB.Left);
      int right = Math.Min(rectangleA.Right, rectangleB.Right);
                   
      Matrix invMat1 = Matrix.Invert(sprite1.GetMatrix());
      Matrix invMat2 = Matrix.Invert(sprite2.GetMatrix());

      Color[,] tex1 = sprite1.GetImageData();
      Color[,] tex2 = sprite2.GetImageData();

      //Matrix mat1to2 = mat1 * Matrix.Invert(mat2);

      //int width1 = tex1.GetLength(0);
      //int height1 = tex1.GetLength(1);
      //int width2 = tex2.GetLength(0);
      //int height2 = tex2.GetLength(1);

      for (int x = left; x < right; x++)
      {
        for (int y = top; y < bottom; y++)
        {
          Vector2 screenXY = new Vector2(x, y);
          Vector2 pos1 = Vector2.Transform(screenXY, invMat1);
          Vector2 pos2 = Vector2.Transform(screenXY, invMat2);
          int x1 = MathCommon.FloatToIntRound(pos1.X);
          int y1 = MathCommon.FloatToIntRound(pos1.Y);


          if (XYInRange(tex1,x1,y1) && tex1[x1,y1].A > 0)
          {
            int x2 = MathCommon.FloatToIntRound(pos2.X);
            int y2 = MathCommon.FloatToIntRound(pos2.Y);

            if (XYInRange(tex2, x2, y2) && tex2[x2, y2].A > 0)
            {
              return screenXY;
            }
          }
        }
      }

      return new Vector2(float.NaN, float.NaN);
    }

    #endregion

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNA.Common
{
  public abstract class Scene : IDisplayedItem
  {

    private readonly string m_Name;
    private List<CompareablePair<string, IDisplayedItem>> m_Items;
    protected LinkedList<Sprite> m_BottomSprites;
    protected LinkedList<Sprite> m_TopSprites;
    private GraphicsDeviceManager m_Graphics = null;
    private ContentManager m_Content = null;
    private MouseCursor m_Cursor = null;
    private Game2D m_Game = null;
    private SpriteCollisionCoordinator m_CollisionCoordinator = null;
    public bool SkipUpdateDisplayedItems { get; set; }
    public bool SkipDrawDisplayedItems { get; set; }

    public MouseCursor Cursor
    {
      get { return m_Cursor; }
      set { m_Cursor = value; }
    }

    public ContentManager Content
    {
      get { return m_Content; }
    }

    public GraphicsDeviceManager Graphics
    {
      get { return m_Graphics; }
    }

    public Game2D Game
    {
      get { return m_Game; }
    }

    public SpriteCollisionCoordinator CollisionCoordinator
    {
      get
      {
        return m_CollisionCoordinator;
      }
    }

    public Scene(string name)
    {
      m_Items = new List<CompareablePair<string, IDisplayedItem>>();
      m_BottomSprites = new LinkedList<Sprite>();
      m_TopSprites = new LinkedList<Sprite>();
      m_CollisionCoordinator = new SpriteCollisionCoordinator();
      SkipDrawDisplayedItems = false;
      SkipUpdateDisplayedItems = false;
      m_Name = name;
    }

    public virtual void Initialize(GraphicsDeviceManager graphics, Game2D game)
    {
      m_Game = game;
      m_Graphics = graphics;
    }

    public virtual void LoadContent(ContentManager content)
    {
      m_Content = content;
      if (Cursor != null)
        Cursor.LoadContent(content);
    }

    #region IDisplayedItem Members

    protected abstract void InnerUpdate(GameTime gameTime);

    public void Update(GameTime gameTime)
    {
      LinkedListNode<Sprite> iterator = m_BottomSprites.First;
      while (iterator != null)
      {
        iterator.Value.Update(gameTime);
        if (!iterator.Value.Alive)
        {
          LinkedListNode<Sprite> tempNode = iterator;
          iterator = iterator.Next;
          m_BottomSprites.Remove(tempNode);
          continue;
        }
        iterator = iterator.Next;
      }

      if (m_Cursor != null)
        m_Cursor.Update(gameTime);
      if (!SkipUpdateDisplayedItems)
      {
        for (int i = 0; i < m_Items.Count; i++)
          m_Items[i].Second.Update(gameTime);
      }
      InnerUpdate(gameTime);
      iterator = m_TopSprites.First;
      while (iterator != null)
      {
        iterator.Value.Update(gameTime);
        if (!iterator.Value.Alive)
        {
          LinkedListNode<Sprite> tempNode = iterator;
          iterator = iterator.Next;
          m_TopSprites.Remove(tempNode);
          continue;
        }
        iterator = iterator.Next;
      }

      m_CollisionCoordinator.Run();
    }

    protected abstract void InnerDraw(GameTime gameTime, SpriteBatch spriteBatch);
    protected virtual void BeforeDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
    protected virtual void AfterDraw(GameTime gameTime, SpriteBatch spriteBatch) { }


    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      BeforeDraw(gameTime, spriteBatch);
      LinkedListNode<Sprite> iterator = m_BottomSprites.First;
      while (iterator != null)
      {
        iterator.Value.Draw(gameTime, spriteBatch);
        iterator = iterator.Next;
      }
      InnerDraw(gameTime, spriteBatch);
      if (!SkipDrawDisplayedItems)
      {
        for (int i = 0; i < m_Items.Count; i++)
          m_Items[i].Second.Draw(gameTime, spriteBatch);
      }
      
      iterator = m_TopSprites.First;
      while (iterator != null)
      {
        iterator.Value.Draw(gameTime, spriteBatch);
        iterator = iterator.Next;
      }
      if (m_Cursor != null)
        m_Cursor.Draw(gameTime, spriteBatch);
      AfterDraw(gameTime, spriteBatch);
    }

    #endregion

    public virtual void TransitionReset(string transitionWord,object transitionData)
    {
    }

    public void AddItem(string name, IDisplayedItem item)
    {
      m_Items.Add(new CompareablePair<string, IDisplayedItem>(name, item));
    }

    public void RemoveItem(string name)
    {
      for (int i = 0; i < m_Items.Count; i++)
      {
        if (m_Items[i].First == name)
        {
          m_Items.RemoveAt(i);
          break;
        }
      }
    }

    public void AddBottomSprite(Sprite sprite)
    {
      m_BottomSprites.AddLast(sprite);
    }

    public void AddTopSprite(Sprite sprite)
    {
      m_TopSprites.AddLast(sprite);
    }

    public string Name
    {
      get { return m_Name; }
    }
  }
}


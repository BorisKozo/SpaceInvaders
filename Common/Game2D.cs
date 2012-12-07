using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA.Common
{
  public class Game2D : Game
  {

    #region Members that allow 2D primitives drawing
    private Matrix worldMatrix;
    private Matrix viewMatrix;
    private Matrix projectionMatrix;

    private BasicEffect basicEffect;
    //private VertexDeclaration vertexDeclaration;
    VertexPositionColor[] pointList;
    

    /// <summary>
    /// Initializes the transforms used by the game.
    /// </summary>
    private void InitializeTransform()
    {

      viewMatrix = Matrix.CreateLookAt(
          new Vector3(0.0f, 0.0f, 1.0f),
          Vector3.Zero,
          Vector3.Up
          );

      projectionMatrix = Matrix.CreateOrthographicOffCenter(
          0,
          (float)m_Graphics.GraphicsDevice.Viewport.Width,
          (float)m_Graphics.GraphicsDevice.Viewport.Height,
          0,
          1.0f, 1000.0f);
    }

    /// <summary>
    /// Initializes the effect (loading, parameter setting, and technique selection)
    /// used by the game.
    /// </summary>
    private void InitializeEffect()
    {

      //VertexBuffer vb = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
      //vertexDeclaration = new VertexDeclaration(
      //    //GraphicsDevice,
      //    VertexPositionColor.VertexElements
      //    );

      basicEffect = new BasicEffect(GraphicsDevice);
      basicEffect.VertexColorEnabled = true;

      //worldMatrix = Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2f - 150,
      //    GraphicsDevice.Viewport.Height / 2f - 50, 0);
      worldMatrix = Matrix.Identity;
      basicEffect.World = worldMatrix;
      basicEffect.View = viewMatrix;
      basicEffect.Projection = projectionMatrix;
    }

    protected override void Initialize()
    {
      InitializeTransform();
      InitializeEffect();
      Arbiter.Initialize(m_Graphics, this);
//      if (CurrentScene != null)
        //CurrentScene.Initialize(m_Graphics, this);
      base.Initialize();
    }

    public void DrawRect(Rectangle rectangle, Color color)
    {

      Vector2 topLeft = new Vector2(rectangle.Left, rectangle.Top);
      Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);
      DrawRect(topLeft, bottomRight, color);
    }

    public void DrawRect(Vector2 topLeft, Vector2 bottomRight, Color color)
    {
      pointList = new VertexPositionColor[4];
      pointList[0].Position = new Vector3(topLeft, 0);
      pointList[0].Color = color;
      pointList[1].Position = new Vector3(bottomRight.X, topLeft.Y, 0);
      pointList[1].Color = color;
      pointList[2].Position = new Vector3(bottomRight, 0);
      pointList[2].Color = color;
      pointList[3].Position = new Vector3(topLeft.X, bottomRight.Y, 0);
      pointList[3].Color = color;
      //GraphicsDevice.VertexDeclaration = vertexDeclaration;

      //VertexBuffer vertexBuffer = new VertexBuffer(GraphicsDevice,
      //          VertexPositionColor.SizeInBytes * (pointList.Length),
      //          BufferUsage.None);

      //      // Set the vertex buffer data to the array of vertices.
      //      vertexBuffer.SetData<VertexPositionColor>(pointList);

      int[] indices = new int[] { 0, 1, 2, 3, 0 };
      // The effect is a compiled effect created and compiled elsewhere
      // in the application.
      //basicEffect.Begin();
      
      foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                  PrimitiveType.LineStrip,
                  pointList,
                  0,   // vertex buffer offset to add to each element of the index buffer
                  4,   // number of vertices to draw
                  indices,
                  0,   // first index element to read
                  4    // number of primitives to draw
              );
        //GraphicsDevice.RenderState.FillMode = FillMode.Solid;
       
      }
    }
    #endregion


    protected GraphicsDeviceManager m_Graphics;
    protected SpriteBatch m_SpriteBatch;
    protected GameArbiter m_Arbiter;

    private static Game2D m_Game = null;
    public static Game2D CurrentGame
    {
      get { return Game2D.m_Game; }
      set { Game2D.m_Game = value; }
    }

    private static Random m_Random = new Random();
    public static Random GameRandom
    {
      get
      {
        return m_Random;
      }
    }


    public Game2D()
    {
      m_Graphics = new GraphicsDeviceManager(this);
      m_Game = this;
      m_Arbiter = new GameArbiter();
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      m_SpriteBatch = new SpriteBatch(GraphicsDevice);
      TextureImageManager.Instance.LoadContent(Content);
      SoundEffectManager.Instance.LoadContent(Content);
      foreach (Scene scene in m_Arbiter.GetAllScenes())
        scene.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (m_Arbiter.CurrentScene != null)
        m_Arbiter.CurrentScene.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      m_SpriteBatch.Begin();
      if (m_Arbiter.CurrentScene != null)
        m_Arbiter.CurrentScene.Draw(gameTime, m_SpriteBatch);
      m_SpriteBatch.End();
    }

    public Scene CurrentScene
    {
      get { return m_Arbiter.CurrentScene; }
      protected set
      {
        m_Arbiter.CurrentScene = value;
      }
    }

    public GameArbiter Arbiter
    {
      get
      {
        return m_Arbiter;
      }
    }

    public int ScreenWidth
    {
      get
      {
        return m_Graphics.PreferredBackBufferWidth;
      }
    }

    public int ScreenHeight
    {
      get
      {
        return m_Graphics.PreferredBackBufferHeight;
      }
    }
  }
}

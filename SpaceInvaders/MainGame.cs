using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XNA.Common;

namespace SpaceInvaders
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class MainGame : Game2D
  {

    public MainGame():base()
    {
      Content.RootDirectory = "Content";
      m_Graphics.PreferredBackBufferWidth = 800;
      m_Graphics.PreferredBackBufferHeight = 600;
      m_Graphics.IsFullScreen = false;
      m_Graphics.ApplyChanges();
      GameScene gameScene = new GameScene();
      Arbiter.AddScene(gameScene);
      GameOverScene gameOverScene = new GameOverScene();
      Arbiter.AddScene(gameOverScene);
      Arbiter.AddTransition(gameScene, gameOverScene, "Game Over");
      Arbiter.AddTransition(gameOverScene, gameScene, "Game Start");
      CurrentScene = gameScene;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      base.LoadContent();
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      base.Draw(gameTime);
    }
  }
}

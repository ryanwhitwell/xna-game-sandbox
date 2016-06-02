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

namespace BadGuySmasher
{
  public class BadGuySmasherGame : Microsoft.Xna.Framework.Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    GameState gameState = GameState.Menu;
    SplashScreen splashScreen;

    BasicBadGuy _badGuy;

    public BadGuySmasherGame()
    {
      graphics = new GraphicsDeviceManager(this);

      graphics.IsFullScreen = false;
      graphics.PreferredBackBufferHeight = 900;
      graphics.PreferredBackBufferWidth = 1200;

      Content.RootDirectory = "Content";

      splashScreen = new SplashScreen(Content, graphics);
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      Vector2 badGuyspeed     = new Vector2(50.0f, 50.0f);
      Vector2 badGuyposition  = new Vector2(200.0f, 200.0f);

      _badGuy = new BasicBadGuy(this.Content, GraphicsDevice, badGuyspeed, badGuyposition);

      splashScreen.LoadContent();
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
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      UpdateInput();

      _badGuy.UpdateSpriteVectors(gameTime);

      base.Update(gameTime);
    }

    private void UpdateInput()
    {
      if (splashScreen.UpdateInput() == SplashScreen.State.Exit)
      {
        gameState = GameState.Game;
      }
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      var mouseState = Mouse.GetState();

      spriteBatch.Begin();

      if (gameState == GameState.Menu)
      {
        splashScreen.Draw(spriteBatch);
      }
      else
      {
        _badGuy.Draw(gameTime);
      }

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}

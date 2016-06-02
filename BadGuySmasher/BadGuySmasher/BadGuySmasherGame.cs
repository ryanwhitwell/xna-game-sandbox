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
    WorldMap _worldMap;

    BadGuyGenerator _badGuyGenerator;

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
      _worldMap = new WorldMap(this.GraphicsDevice);

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

      Vector2 badGuyspeed1     = new Vector2(150.0f, 150.0f);
      Vector2 badGuyposition1  = new Vector2(200.0f, 200.0f);

      Vector2 badGuyGeneratorPosition = new Vector2(100.0f, 100.0f);
      _badGuyGenerator = new BadGuyGenerator(Content, GraphicsDevice, badGuyGeneratorPosition, 10);
      Vector2 badGuyspeed2     = new Vector2(150.0f, -150.0f);
      Vector2 badGuyposition2  = new Vector2(300.0f, 200.0f);

      Sprite advancedBadGuy1 = new Sprite(this.Content, GraphicsDevice, _worldMap, badGuyspeed1, badGuyposition1);
      Sprite advancedBadGuy2 = new Sprite(this.Content, GraphicsDevice, _worldMap, badGuyspeed2, badGuyposition2);

      _worldMap.Sprites.Add(advancedBadGuy1);
      _worldMap.Sprites.Add(advancedBadGuy2);

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

      if (gameState == GameState.Game)
      {
        _badGuyGenerator.Update(gameTime);
      }
      _worldMap.UpdateSpriteVectors(gameTime);

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
        _badGuyGenerator.Draw(gameTime);
        _worldMap.DrawSprites(gameTime);
      }

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}

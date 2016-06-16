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
    GameState gameState = GameState.Menu;
    SplashScreen _splashScreen;
    WorldMap _worldMap;

    SpriteGenerator _badGuyGenerator;

    public BadGuySmasherGame()
    {
      graphics = new GraphicsDeviceManager(this);

      graphics.IsFullScreen               = false;
      graphics.PreferredBackBufferHeight  = 900;
      graphics.PreferredBackBufferWidth   = 1200;
      Content.RootDirectory               = "Content";
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
      _worldMap     = new WorldMap(this.GraphicsDevice);
      _splashScreen = new SplashScreen(Content, graphics, "TitleFont");

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      Vector2 badGuyGeneratorPosition = new Vector2(300.0f, 300.0f);
      _badGuyGenerator = new SpriteGenerator(Content, GraphicsDevice, _worldMap, badGuyGeneratorPosition, 5, 1, "BadGuyGenerator", "badguy");
      _badGuyGenerator.DrawBounds = true;

      Vector2 wallPosition = new Vector2(900.0f, 200.0f);
      Sprite wall = new Sprite(this.Content, GraphicsDevice, _worldMap, wallPosition, "wall", new SpriteProperties(-50, new Vector2(), new Vector2()));
      wall.DrawBounds = true;

      _worldMap.Sprites.Add(_badGuyGenerator);
      _worldMap.Sprites.Add(wall);
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
        _worldMap.UpdateSpriteVectors(gameTime);
      }

      base.Update(gameTime);
    }

    private void UpdateInput()
    {
      if (_splashScreen.UpdateInput() == SplashScreen.State.Exit)
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

      if (gameState == GameState.Menu)
      {
        _splashScreen.Draw();
      }
      else
      {
        _worldMap.DrawSprites(gameTime);
      }

      base.Draw(gameTime);
    }
  }
}

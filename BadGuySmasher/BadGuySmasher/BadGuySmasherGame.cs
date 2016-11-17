using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BadGuySmasher.GameManagement;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.Sprites;
using BadGuySmasher.Sprites.BadGuys;
using BadGuySmasher.Sprites.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BadGuySmasher
{
  public class BadGuySmasherGame : Game
  {
    GraphicsDeviceManager _graphics;
    MainMenu              _mainMenu;
    IWorldMapManager      _worldMapManager;
    GameState             _gameState;

    public BadGuySmasherGame()
    {
      _gameState = GameState.Menu;

      _graphics = new GraphicsDeviceManager(this);
      _graphics.IsFullScreen               = false;
      _graphics.PreferredBackBufferHeight  = 900;
      _graphics.PreferredBackBufferWidth   = 1200;

      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      _worldMapManager  = new WorldMapManager(Content, this.GraphicsDevice);
      _mainMenu         = new MainMenu(Content, _graphics, "TitleFont");

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() 
    {
      _worldMapManager.GameBegin();
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent() { }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      UpdateInput();

      _worldMapManager.SetNumberOfPlayers(_mainMenu.NumberOfPlayers);

      if (_gameState == GameState.Game)
      {
        _worldMapManager.UpdateWorldMapSpriteVectors(gameTime);
      }

      base.Update(gameTime);
    }

    private void UpdateInput()
    {
      if (_mainMenu.UpdateInput() == MainMenu.State.Exit)
      {
        _gameState = GameState.Game;
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

      if (_gameState == GameState.Menu)
      {
        _mainMenu.Draw();
      }
      else
      {
        _worldMapManager.DrawWorldMapSprites(gameTime);
      }

      base.Draw(gameTime);
    }
  }
}

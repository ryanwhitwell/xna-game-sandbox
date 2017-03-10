using System.Diagnostics;
using BadGuySmasher.GameManagement;
using BadGuySmasher.GameManagement.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  public class BadGuySmasherGame : Game
  {
    GraphicsDeviceManager _graphics;
    IGameManager          _gameManager;

    public BadGuySmasherGame()
    {
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
      _gameManager = new GameManager(Content, _graphics, GraphicsDevice);

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() 
    {
      
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
      Debug.WriteLine("GameManager GameState: '{0}', MenuManager CurrentMenu GameState: '{1}', CurrentMenu: '{2}'.", _gameManager.GameState, _gameManager.MenuManager.CurrentMenu.UpdateInput(), _gameManager.MenuManager.CurrentMenu.Name);

      _gameManager.GameState = _gameManager.MenuManager.CurrentMenu.UpdateInput();

      if (_gameManager.GameState == GameState.Play)
      {
        _gameManager.LevelManager.UpdateWorldMapSpriteVectors(gameTime);
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      if (_gameManager.GameState == GameState.Menu || _gameManager.GameState == GameState.End)
      {
        _gameManager.MenuManager.CurrentMenu.Draw();
      }

      _gameManager.LevelManager.DrawWorldMapSprites(gameTime);

      base.Draw(gameTime);
    }
  }
}

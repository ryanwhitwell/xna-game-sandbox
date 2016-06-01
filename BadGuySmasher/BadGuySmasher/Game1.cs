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
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    GameState gameState = GameState.Menu;
    SpriteFont titleFont;
    MouseState lastMouseState;
    KeyboardState lastKeyboardState;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);

      graphics.IsFullScreen = false;
      graphics.PreferredBackBufferHeight = 900;
      graphics.PreferredBackBufferWidth = 1200;

      Content.RootDirectory = "Content";

      lastKeyboardState = Keyboard.GetState();
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

      titleFont = Content.Load<SpriteFont>("TitleFont");
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

      base.Update(gameTime);
    }

    private void UpdateInput()
    {
      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.Space))
      {
        // If not down last update, key has just been pressed.
        if (lastKeyboardState.IsKeyDown(Keys.Space))
        {
          gameState = GameState.Game;
        }
      }

      // Update saved state.
      lastKeyboardState = newState;
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
        string text = "Bad Guy Smasher";
        spriteBatch.DrawString(titleFont, text, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - titleFont.MeasureString(text).Length() / 2, graphics.GraphicsDevice.Viewport.Height / 2), Color.Black);

        text = "Press enter to begin...";
        spriteBatch.DrawString(titleFont, text, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - titleFont.MeasureString(text).Length() / 2, graphics.GraphicsDevice.Viewport.Height / 2 + titleFont.LineSpacing), Color.Black);
      }
      else
      {
      }

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}

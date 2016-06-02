using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher
{
  class SplashScreen
  {
    SpriteFont            _titleFont;
    KeyboardState         _lastKeyboardState;
    ContentManager        _contentManager;
    GraphicsDeviceManager _graphics;
    State                 _state = State.Showing;

    public enum State
    {
      Showing,
      Exit,
      Done
    }

    public SplashScreen(ContentManager contentManager, GraphicsDeviceManager graphics)
    {
      _contentManager    = contentManager;
      _graphics          = graphics;
      _lastKeyboardState = Keyboard.GetState();
    }

    public void LoadContent()
    {
      _titleFont = _contentManager.Load<SpriteFont>("TitleFont");
    }

    public State UpdateInput()
    {
      if (_state == State.Done || _state == State.Exit)
      {
        return State.Done;
      }

      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.Space))
      {
        // If not down last update, key has just been pressed.
        if (_lastKeyboardState.IsKeyDown(Keys.Space))
        {
          _state = State.Exit;
        }
      }

      // Update saved state.
      _lastKeyboardState = newState;

      return _state;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      string text = "Bad Guy Smasher";
      spriteBatch.DrawString(_titleFont, text, new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - _titleFont.MeasureString(text).Length() / 2, _graphics.GraphicsDevice.Viewport.Height / 2), Color.Black);

      text = "Press the space bar to begin.";
      spriteBatch.DrawString(_titleFont, text, new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - _titleFont.MeasureString(text).Length() / 2, _graphics.GraphicsDevice.Viewport.Height / 2 + _titleFont.LineSpacing), Color.Black);
    }
  }
}

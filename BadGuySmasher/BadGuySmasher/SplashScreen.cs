using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher
{
  public class SplashScreen
  {
    SpriteFont            _titleFont;
    KeyboardState         _lastKeyboardState;
    ContentManager        _contentManager;
    GraphicsDeviceManager _graphics;
    State                 _state = State.Showing;
    SpriteBatch           _spriteBatch;

    public enum State
    {
      Showing,
      Exit,
      Done
    }

    public SplashScreen(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphics == null)
      {
        throw new ArgumentNullException("graphics");
      }

      if (string.IsNullOrWhiteSpace(spriteFontAssetName))
      {
        throw new ArgumentNullException("spriteFontAssetName");
      }
      
      _contentManager     = contentManager;
      _graphics           = graphics;
      _lastKeyboardState  = Keyboard.GetState();
      _spriteBatch        = new SpriteBatch(graphics.GraphicsDevice);
      _titleFont          = _contentManager.Load<SpriteFont>(spriteFontAssetName);
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

    public void Draw()
    {
      _spriteBatch.Begin();

      string text = "Bad Guy Smasher";
      _spriteBatch.DrawString(_titleFont, text, new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - _titleFont.MeasureString(text).Length() / 2, _graphics.GraphicsDevice.Viewport.Height / 2), Color.Black);

      text = "Press the space bar to begin.";
      _spriteBatch.DrawString(_titleFont, text, new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - _titleFont.MeasureString(text).Length() / 2, _graphics.GraphicsDevice.Viewport.Height / 2 + _titleFont.LineSpacing), Color.Black);

      _spriteBatch.End();
    }
  }
}

using System;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher
{
  public class MainMenu : IMainMenu
  {
    SpriteFont            _titleFont;
    KeyboardState         _lastKeyboardState;
    ContentManager        _contentManager;
    GraphicsDeviceManager _graphics;
    State                 _state;
    SpriteBatch           _spriteBatch;
    int                   _numberOfPlayers;

    public enum State
    {
      Showing,
      Exit,
      Done
    }

    public int NumberOfPlayers { get { return _numberOfPlayers; } }

    public MainMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName)
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

      if (newState.IsKeyUp(Keys.D1))
      {
        // If not down last update, key has just been pressed.
        if (_lastKeyboardState.IsKeyDown(Keys.D1))
        {
          _numberOfPlayers = 1;
          _state = State.Exit;
        }
      }
      
      if (newState.IsKeyUp(Keys.D2))
      {
        // If not down last update, key has just been pressed.
        if (_lastKeyboardState.IsKeyDown(Keys.D2))
        {
          // TODO: 2-Players - We currently only support 1 player, so we card-code the main menu to force the selection of 1 player
          _numberOfPlayers = 1; 
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

      text = "Please select a number of Players to continue (1 or 2)";
      _spriteBatch.DrawString(_titleFont, text, new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - _titleFont.MeasureString(text).Length() / 2, _graphics.GraphicsDevice.Viewport.Height / 2 + _titleFont.LineSpacing), Color.Black);

      _spriteBatch.End();
    }
  }
}

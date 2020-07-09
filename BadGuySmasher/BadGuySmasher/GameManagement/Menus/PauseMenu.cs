using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher.GameManagement.Menus
{
  public class PauseMenu : BaseMenu, ICurrentMenu
  {
    private const string PausedText = "- PAUSED -";

    private bool    _paused;
    private string  _name;
    
    public PauseMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName, IMenuManager menuManager, string name) : base(contentManager, graphics, spriteFontAssetName, menuManager)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException("name");
      }

      _name = name;
    }

    public string Name { get { return _name; } }

    public void Draw()
    {
      if (MenuManager.GameManager.GameState != GameState.Menu)
      {
        return;
      }

      SpriteBatch.Begin();

      SpriteBatch.DrawString(MenuFont, PausedText, new Vector2(GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - MenuFont.MeasureString(PausedText).Length() / 2, GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2), Color.Black);

      SpriteBatch.End();
    }

    public GameState UpdateInput()
    {
      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.P))
      {
        // If not down last update, key has just been pressed.
        if (LastKeyboardState.IsKeyDown(Keys.P))
        {
          _paused = !_paused;
        }
      }

      // Update saved state.
      LastKeyboardState = newState;

      return GetGameState(_paused);
    }

    private GameState GetGameState(bool paused)
    {
      if (paused)
      {
        return GameState.Menu;
      }

      return GameState.Play;
    }

    public void SetGameState(GameState gameState)
    {
      MenuManager.GameManager.GameState = gameState;
    }
  }
}

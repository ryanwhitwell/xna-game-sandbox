using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher.GameManagement.Menus
{
  public class PauseMenu : BaseMenu, ICurrentMenu
  {
    private bool _paused;
    
    public PauseMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName, IGameStateManager gameStateManager) : base(contentManager, graphics, spriteFontAssetName, gameStateManager) { }

    public void Draw()
    {
      base.SpriteBatch.Begin();

      string text = "!!! PAUSED !!!";
      base.SpriteBatch.DrawString(base.MenuFont, text, new Vector2(base.GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - base.MenuFont.MeasureString(text).Length() / 2, base.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2), Color.Black);

      base.SpriteBatch.End();
    }

    public MenuState UpdateInput()
    {
      if (base.MenuState == MenuState.Exit)
      {
        return MenuState.Exit;
      }

      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.P))
      {
        // If not down last update, key has just been pressed.
        if (base.LastKeyboardState.IsKeyDown(Keys.P))
        {
          _paused = !_paused;
        }
      }

      // Update saved state.
      base.LastKeyboardState = newState;

      return GetMenuState(_paused);
    }

    private MenuState GetMenuState(bool paused)
    {
      if (paused)
      {
        return MenuState.Show;
      }

      return MenuState.Exit;
    }
  }
}

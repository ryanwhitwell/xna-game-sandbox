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
  public class GameOverMenu : BaseMenu, ICurrentMenu
  {
    public GameOverMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName, IGameStateManager gameStateManager) : base(contentManager, graphics, spriteFontAssetName, gameStateManager) { }
  
    public MenuState UpdateInput()
    {
      if (base.MenuState == MenuState.Exit)
      {
        return MenuState.Exit;
      }

      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.Escape))
      {
        // If not down last update, key has just been pressed.
        if (base.LastKeyboardState.IsKeyDown(Keys.Escape))
        {
          base.MenuState = MenuState.Exit;

          // Update the game state with the number of players to 0
          base.GameStateManager.WorldMapManager.SetNumberOfPlayers(0);

          base.GameStateManager.MenuManager.SetCurrentMenu(MenuManager.StartMenu);
        }
      }

      // Update saved state.
      base.LastKeyboardState = newState;

      return base.MenuState;
    }

    public void Draw()
    {
      base.SpriteBatch.Begin();

      string text = "!!! Game Over !!!";
      base.SpriteBatch.DrawString(base.MenuFont, text, new Vector2(base.GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - base.MenuFont.MeasureString(text).Length() / 2, base.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2), Color.Black);

      base.SpriteBatch.End();
    }

    public void SetMenuState(MenuState menuState)
    {
      base.MenuState = menuState;
    }
  }
}

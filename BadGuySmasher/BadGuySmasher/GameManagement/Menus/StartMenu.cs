using System;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher
{
  public class StartMenu : BaseMenu, IStartMenu
  {
    private int _numberOfPlayers;

    public int NumberOfPlayers { get { return _numberOfPlayers; } }

    public StartMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName, IGameStateManager gameStateManager) : base(contentManager, graphics, spriteFontAssetName, gameStateManager) { }

    public MenuState UpdateInput()
    {
      if (base.MenuState == MenuState.Exit)
      {
        return MenuState.Exit;
      }

      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.D1))
      {
        // If not down last update, key has just been pressed.
        if (base.LastKeyboardState.IsKeyDown(Keys.D1))
        {
          _numberOfPlayers = 1;
          base.MenuState = MenuState.Exit;
        }
      }
      
      if (newState.IsKeyUp(Keys.D2))
      {
        // If not down last update, key has just been pressed.
        if (base.LastKeyboardState.IsKeyDown(Keys.D2))
        {
          // TODO: 2-Players - We currently only support 1 player, so we card-code the main menu to force the selection of 1 player
          _numberOfPlayers = 1; 
          base.MenuState = MenuState.Exit;
        }
      }

      // Update saved state.
      base.LastKeyboardState = newState;

      // Update the game state with the number of players selected on this Menu
      base.GameStateManager.WorldMapManager.SetNumberOfPlayers(_numberOfPlayers);

      if (_numberOfPlayers != 0)
      {
        // Update the current menu after the player selection is made
        base.GameStateManager.MenuManager.SetCurrentMenu("PauseMenu");
      }

      return base.MenuState;
    }

    public void Draw()
    {
      base.SpriteBatch.Begin();

      string text = "Bad Guy Smasher";
      base.SpriteBatch.DrawString(base.MenuFont, text, new Vector2(base.GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - base.MenuFont.MeasureString(text).Length() / 2, base.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2), Color.Black);

      text = "Please select a number of Players to continue (1 or 2)";
      base.SpriteBatch.DrawString(base.MenuFont, text, new Vector2(base.GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - base.MenuFont.MeasureString(text).Length() / 2, base.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2 + base.MenuFont.LineSpacing), Color.Black);

      base.SpriteBatch.End();
    }
  }
}

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
    private string  _name;
    private int     _numberOfPlayers;

    public string Name { get { return _name; } }

    public int NumberOfPlayers { get { return _numberOfPlayers; } }

    public StartMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName, IMenuManager menuManager, string name) : base(contentManager, graphics, spriteFontAssetName, menuManager)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException("name");
      }

      _name = name;
    }

    public GameState UpdateInput()
    {
      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.D1))
      {
        // If not down last update, key has just been pressed.
        if (LastKeyboardState.IsKeyDown(Keys.D1))
        {
          _numberOfPlayers = 1;
          MenuManager.GameManager.GameState = GameState.Ready;
        }
      }
      
      if (newState.IsKeyUp(Keys.D2))
      {
        // If not down last update, key has just been pressed.
        if (LastKeyboardState.IsKeyDown(Keys.D2))
        {
          // TODO: 2-Players - We currently only support 1 player, so we card-code the main menu to force the selection of 1 player
          _numberOfPlayers = 1; 
          MenuManager.GameManager.GameState = GameState.Ready;
        }
      }

      // Update saved state.
      LastKeyboardState = newState;

      // Update the game state with the number of players selected on this Menu
      MenuManager.GameManager.LevelManager.SetNumberOfPlayers(_numberOfPlayers);

      if (_numberOfPlayers != 0)
      {
        // Update the current menu after the player selection is made
        MenuManager.SetCurrentMenu(GameManagement.Menus.MenuManager.PauseMenu);
      }

      return MenuManager.GameManager.GameState;
    }

    public void Draw()
    {
      SpriteBatch.Begin();

      string text = "Bad Guy Smasher";
      SpriteBatch.DrawString(MenuFont, text, new Vector2(GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - MenuFont.MeasureString(text).Length() / 2, GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2), Color.Black);

      text = "Please select a number of Players to continue (1 or 2)";
      SpriteBatch.DrawString(MenuFont, text, new Vector2(GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - MenuFont.MeasureString(text).Length() / 2, GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2 + MenuFont.LineSpacing), Color.Black);

      SpriteBatch.End();
    }

    public void SetGameState(GameState gameState)
    {
      MenuManager.GameManager.GameState = gameState;
    }
  }
}

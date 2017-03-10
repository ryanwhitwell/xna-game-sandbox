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
    private string _name;

    public GameOverMenu(ContentManager contentManager, GraphicsDeviceManager graphics, string spriteFontAssetName, IMenuManager menuManager, string name) : base(contentManager, graphics, spriteFontAssetName, menuManager)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException("name");
      }

      _name = name;
    }

    public string Name { get { return _name; } }
  
    public GameState UpdateInput()
    {
      KeyboardState newState = Keyboard.GetState();

      if (newState.IsKeyUp(Keys.Escape))
      {
        // If not down last update, key has just been pressed.
        if (LastKeyboardState.IsKeyDown(Keys.Escape))
        {
          MenuManager.GameManager.GameState = GameState.Menu;

          // Update the game state with the number of players to 0
          MenuManager.GameManager.LevelManager.SetNumberOfPlayers(0);

          MenuManager.SetCurrentMenu(Menus.MenuManager.StartMenu);

          MenuManager.GameManager.GameState = GameState.Ready;
        }
      }

      // Update saved state.
      LastKeyboardState = newState;

      return MenuManager.GameManager.GameState;
    }

    public void Draw()
    {
      SpriteBatch.Begin();

      string text = "!!! Game Over !!!";
      SpriteBatch.DrawString(base.MenuFont, text, new Vector2(base.GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2 - base.MenuFont.MeasureString(text).Length() / 2, base.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2), Color.Black);

      SpriteBatch.End();
    }

    public void SetGameState(GameState gameState)
    {
       MenuManager.GameManager.GameState = gameState;
    }
  }
}

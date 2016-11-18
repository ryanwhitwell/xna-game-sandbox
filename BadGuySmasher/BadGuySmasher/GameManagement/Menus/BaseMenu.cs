using System;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher.GameManagement.Menus
{
  public abstract class BaseMenu
  {
    private SpriteFont            _menuFont;
    private KeyboardState         _lastKeyboardState;
    private ContentManager        _contentManager;
    private GraphicsDeviceManager _graphicsDeviceManager;
    private MenuState             _menuState;
    private SpriteBatch           _spriteBatch;
    private IGameStateManager     _gameStateManager;

    public BaseMenu(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, string spriteFontAssetName, IGameStateManager gameStateManager)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDeviceManager == null)
      {
        throw new ArgumentNullException("graphicsDeviceManager");
      }

      if (string.IsNullOrWhiteSpace(spriteFontAssetName))
      {
        throw new ArgumentNullException("spriteFontAssetName");
      }

      if (gameStateManager == null)
      {
        throw new ArgumentNullException("gameStateManager");
      }
      
      _contentManager         = contentManager;
      _graphicsDeviceManager  = graphicsDeviceManager;
      _lastKeyboardState      = Keyboard.GetState();
      _spriteBatch            = new SpriteBatch(graphicsDeviceManager.GraphicsDevice);
      _menuFont               = _contentManager.Load<SpriteFont>(spriteFontAssetName);
      _gameStateManager       = gameStateManager;
    }

    protected SpriteBatch SpriteBatch
    {
      get
      {
        return _spriteBatch;
      }
    }

    protected MenuState MenuState
    {
      get
      {
        return _menuState;
      }

      set
      {
        _menuState = value;
      }
    }

    protected KeyboardState LastKeyboardState
    {
      get
      {
        return _lastKeyboardState;
      }

      set
      {
        _lastKeyboardState = value;
      }
    }

    protected SpriteFont MenuFont
    {
      get
      {
        return _menuFont;
      }
    }

    protected GraphicsDeviceManager GraphicsDeviceManager
    {
      get
      {
        return _graphicsDeviceManager;
      }
    }

    protected IGameStateManager GameStateManager
    {
      get
      {
        return _gameStateManager;
      }
    }
  }
}

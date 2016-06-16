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
  public class Player : Sprite
  {
    private const int MoveSpeed = 5;
    
    private int _number = 0;
    
    public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, string textureAssetName, int playerNumber) : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null)
    {
      _number = playerNumber;
    }

    public int Number { get { return _number; } }

    public override void Update(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();

      float currentX = Position.X;
      float currentY = Position.Y;

      bool keyComboPressed = false;
      
      if (_number == 1)
      {
        if (state.IsKeyDown(Keys.D) && state.IsKeyDown(Keys.W))
        {
          SetXPosition(Position.X + MoveSpeed);
          SetYPosition(Position.Y - MoveSpeed);
          keyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.W))
        {
          SetXPosition(Position.X - MoveSpeed);
          SetYPosition(Position.Y - MoveSpeed);
          keyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.S))
        {
          SetXPosition(Position.X - MoveSpeed);
          SetYPosition(Position.Y + MoveSpeed);
          keyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.D) && state.IsKeyDown(Keys.S))
        {
          SetXPosition(Position.X + MoveSpeed);
          SetYPosition(Position.Y + MoveSpeed);
          keyComboPressed = true;
        }

        if (!keyComboPressed)
        {
          if (state.IsKeyDown(Keys.D))
            SetXPosition(Position.X + MoveSpeed);
          if (state.IsKeyDown(Keys.A))
            SetXPosition(Position.X - MoveSpeed);
          if (state.IsKeyDown(Keys.W))
            SetYPosition(Position.Y - MoveSpeed);
          if (state.IsKeyDown(Keys.S))
            SetYPosition(Position.Y + MoveSpeed);
          }
      }
      else if (_number == 2)
      {
        if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Up))
        {
          SetXPosition(Position.X + MoveSpeed);
          SetYPosition(Position.Y - MoveSpeed);
          keyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Up))
        {
          SetXPosition(Position.X - MoveSpeed);
          SetYPosition(Position.Y - MoveSpeed);
          keyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Down))
        {
          SetXPosition(Position.X - MoveSpeed);
          SetYPosition(Position.Y + MoveSpeed);
          keyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Down))
        {
          SetXPosition(Position.X + MoveSpeed);
          SetYPosition(Position.Y + MoveSpeed);
          keyComboPressed = true;
        }
        
        if (!keyComboPressed)
        {
          if (state.IsKeyDown(Keys.Right))
            SetXPosition(Position.X + MoveSpeed);
          if (state.IsKeyDown(Keys.Left))
            SetXPosition(Position.X - MoveSpeed);
          if (state.IsKeyDown(Keys.Up))
            SetYPosition(Position.Y - MoveSpeed);
          if (state.IsKeyDown(Keys.Down))
            SetYPosition(Position.Y + MoveSpeed);
        }
      }

      UpdateSpriteBounds(Position);

      CollisionResults results = WorldMap.GetCollisionResults(this);

      if (results.XMove != 0)
      {
        SetXPosition(currentX);
      }
      
      if (results.YMove != 0)
      {
        SetYPosition(currentY);
      }

      SetXVelocity(0.0f);
      SetYVelocity(0.0f);
      
      base.UpdateSpriteBounds(Position);
    }
  }
}

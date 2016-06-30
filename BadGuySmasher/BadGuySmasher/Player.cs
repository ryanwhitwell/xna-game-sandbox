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

    private void MoveForward(GameTime gameTime)
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    private void MoveBackward(GameTime gameTime)
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

      direction.Normalize();

      Position -= direction * MoveSpeed;
    }

    private void MoveLeft(GameTime gameTime)
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation - 90) * (float)(Math.PI / -180.0), (float)Math.Sin(Rotation - 90) * (float)(Math.PI / -180.0));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    private void MoveRight(GameTime gameTime)
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation + 90) * (float)(Math.PI / -180.0), (float)Math.Sin(Rotation + 90)* (float)(Math.PI / -180.0));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    public override void Update(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();

      float currentX = Position.X;
      float currentY = Position.Y;

      bool movementKeyComboPressed = false;
      
      if (_number == 1)
      {
        if (state.IsKeyDown(Keys.D) && state.IsKeyDown(Keys.W))
        {
          //SetXPosition(Position.X + MoveSpeed);
          //SetYPosition(Position.Y - MoveSpeed);
          movementKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.W))
        {
          //SetXPosition(Position.X - MoveSpeed);
          //SetYPosition(Position.Y - MoveSpeed);
          movementKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.S))
        {
          //SetXPosition(Position.X - MoveSpeed);
          //SetYPosition(Position.Y + MoveSpeed);
          movementKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.D) && state.IsKeyDown(Keys.S))
        {
          //SetXPosition(Position.X + MoveSpeed);
          //SetYPosition(Position.Y + MoveSpeed);
          movementKeyComboPressed = true;
        }

        if (!movementKeyComboPressed)
        {
          if (state.IsKeyDown(Keys.D))
          {
            MoveRight(gameTime);
            //SetXPosition(Position.X + MoveSpeed);
          }

          if (state.IsKeyDown(Keys.A))
          {
            MoveLeft(gameTime);
            //SetXPosition(Position.X - MoveSpeed);
          }

          if (state.IsKeyDown(Keys.W))
          {
            //SetYPosition(Position.Y - MoveSpeed);
            MoveForward(gameTime);
          }

          if (state.IsKeyDown(Keys.S))
          {
            //SetYPosition(Position.Y + MoveSpeed);
            MoveBackward(gameTime);
          }
        }

        bool rotationKeyComboPressed = false;

        if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Up))
        {
          Rotation += 0.1f;
          rotationKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Up))
        {
          Rotation -= 0.1f;
          rotationKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Down))
        {
          Rotation -= 0.1f;
          rotationKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Down))
        {
          Rotation += 0.1f;
          rotationKeyComboPressed = true;
        }
        
        if (!rotationKeyComboPressed)
        {
          if (state.IsKeyDown(Keys.Right))
            Rotation += 0.1f;
          if (state.IsKeyDown(Keys.Left))
            Rotation -= 0.1f;
          if (state.IsKeyDown(Keys.Up))
            Rotation += 0.1f;
          if (state.IsKeyDown(Keys.Down))
            Rotation -= 0.1f;
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

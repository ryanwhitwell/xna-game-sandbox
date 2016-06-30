using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher
{
  public class Player : Sprite
  {
    private const int   MoveSpeed     = 5;
    private const float RotationSpeed = 0.1f;
    
    private int _number = 0;
    
    public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, string textureAssetName, int playerNumber) : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null)
    {
      _number = playerNumber;
    }

    public int Number { get { return _number; } }

    private void MoveForward()
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    private void MoveBackward()
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

      direction.Normalize();

      Position -= direction * MoveSpeed;
    }

    private void MoveLeft()
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation - 90), (float)Math.Sin(Rotation - 90));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    private void MoveRight()
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation + 90), (float)Math.Sin(Rotation + 90));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    public override void Update(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();

      float currentX = Position.X;
      float currentY = Position.Y;
      
      if (_number == 1)
      {
        if (state.IsKeyDown(Keys.D))
        {
          MoveRight();
        }

        if (state.IsKeyDown(Keys.A))
        {
          MoveLeft();
        }

        if (state.IsKeyDown(Keys.W))
        {
          MoveForward();
        }

        if (state.IsKeyDown(Keys.S))
        {
          MoveBackward();
        }

        bool rotationKeyComboPressed = false;

        if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Up))
        {
          Rotation += RotationSpeed;
          rotationKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Up))
        {
          Rotation -= RotationSpeed;
          rotationKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Down))
        {
          Rotation -= RotationSpeed;
          rotationKeyComboPressed = true;
        }
        if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Down))
        {
          Rotation += RotationSpeed;
          rotationKeyComboPressed = true;
        }
        
        if (!rotationKeyComboPressed)
        {
          if (state.IsKeyDown(Keys.Right))
            Rotation += RotationSpeed;
          if (state.IsKeyDown(Keys.Left))
            Rotation -= RotationSpeed;
          if (state.IsKeyDown(Keys.Up))
            Rotation += RotationSpeed;
          if (state.IsKeyDown(Keys.Down))
            Rotation -= RotationSpeed;
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

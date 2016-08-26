using System;
using System.Collections.Generic;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher.Sprites.Players
{
  public class Player : Sprite
  {
    private const int   MoveSpeed     = 5;
    private const float RotationSpeed = 0.1f;
    
    private int _number = 0;

    private List<PlayerProjectile> _projectiles = new List<PlayerProjectile>();

    private KeyboardState _previousKeyboardState;
    
    public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, string textureAssetName, int playerNumber) : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null)
    {
      _number = playerNumber;
    }

    private static float GetStrafeDirection() 
    { 
      return (90.05f - ((90.0f / 2) / 2));
    }

    public int Number { get { return _number; } }

    public override void Update(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();

      float currentX = Position.X;
      float currentY = Position.Y;

      HandleMovement(state);

      HandleCollision(currentX, currentY);

      UpdateProjectiles(gameTime, state, _previousKeyboardState);

      _previousKeyboardState = state;
    }

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
      Vector2 direction = new Vector2((float)Math.Cos(Rotation + GetStrafeDirection()), (float)Math.Sin(Rotation + GetStrafeDirection()));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    private void MoveRight()
    {
      Vector2 direction = new Vector2((float)Math.Cos(Rotation - GetStrafeDirection()), (float)Math.Sin(Rotation - GetStrafeDirection()));

      direction.Normalize();

      Position += direction * MoveSpeed;
    }

    private void HandleCollision(float currentX, float currentY)
    {
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
      
      UpdateSpriteBounds(Position);
    }

    private void HandleMovement(KeyboardState state)
    {
      if (_number == 1)
      {
        // Process movement keys
        if (state.IsKeyDown(Keys.D))
          MoveRight();
        if (state.IsKeyDown(Keys.A))
          MoveLeft();
        if (state.IsKeyDown(Keys.W))
          MoveForward();
        if (state.IsKeyDown(Keys.S))
          MoveBackward();

        // Process rotation keys
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
    }

    private void UpdateProjectiles(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
      if (_number == 1 && keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
      {
        Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

        Vector2 speed     = new Vector2(PlayerProjectile.ProjectileMoveSpeed, PlayerProjectile.ProjectileMoveSpeed);
        Vector2 position  = Position + new Vector2(Texture.Width / 2, Texture.Height / 2);

        PlayerProjectile newProjectile = new PlayerProjectile(ContentManager, GraphicsDevice, WorldMap, position, speed, direction, "projectile");
        newProjectile.Rotation = Rotation;
        newProjectile.DrawBounds = true;

        _projectiles.Add(newProjectile);

        WorldMap.Sprites.Add(newProjectile);
      }

      // Update collection of player projectiles
      for (int i = _projectiles.Count - 1; i >= 0; --i)
      {
        if (!_projectiles[i].Visible)
        {
          // Remove from WorldMap
          _projectiles[i].Delete();

          // Remove from Player Collection
          _projectiles.RemoveAt(i);
        }
      }
    }
  }
}

using System;
using System.Collections.Generic;
using BadGuySmasher.GameManagement;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BadGuySmasher.Sprites.Players
{
  public class Player : Sprite, IPlayer
  {
    private const int   MoveSpeed                   = 5;
    private const float RotationSpeed               = 0.1f;
    private const int   DefaultGunPower             = 5;
    private const int   BadGuyHitHitPointDeduction  = 5;
    
    private int _number = 0;
    private int _hitPoints;

    private List<PlayerProjectile> _projectiles = new List<PlayerProjectile>();

    private KeyboardState _previousKeyboardState;
    
    public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, int hitPoints, string textureAssetName, int playerNumber) : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null)
    {
      _hitPoints  = hitPoints;
      _number     = playerNumber;
    }

    private static float GetStrafeDirection() 
    { 
      return (90.05f - ((90.0f / 2) / 2));
    }

    public int Number { get { return _number; } }

    public int HitPoints
    { 
      get { return _hitPoints; } 
      set { _hitPoints = value; } 
    }

    protected override void Move(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();

      float currentX = Position.X;
      float currentY = Position.Y;

      HandleMovement(state);

      UpdateProjectiles(gameTime, state, _previousKeyboardState);

      _previousKeyboardState = state;
    }

    protected override void HandleCollesionResults(Vector2 originalPosition, CollisionResults collisionResults)
    {
      HandleCollision(originalPosition.X, originalPosition.Y);
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
    }

    private void HandleMovement(KeyboardState state)
    {
      // TODO: 2-Players - We are currently only handling movement for 1 Player. When we support more than 1 player, we'll need to handle the 
      // cases for _number == 2.
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
    }

    private void UpdateProjectiles(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
      if (_number == 1 && keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
      {
        Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

        Vector2 speed     = new Vector2(PlayerProjectile.ProjectileMoveSpeed, PlayerProjectile.ProjectileMoveSpeed);
        Vector2 position  = Position + new Vector2(Texture.Width / 2, Texture.Height / 2);

        PlayerProjectile newProjectile = new PlayerProjectile(ContentManager, GraphicsDevice, WorldMap, DefaultGunPower, position, speed, direction, "projectile");
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

    public override void Draw(GameTime gameTime)
    {
      this.Begin();

      string xText = "HP:" + _hitPoints.ToString();
        
      this.DrawString(SpriteFont, xText, new Vector2(Bounds.Right + 5, Bounds.Top + 30), Color.WhiteSmoke);

      this.End();
      
      base.Draw(gameTime);
    }

    public void Die()
    {
      Delete();
    }

    public void GetHit(IBadGuy badGuy)
    {
      // Currently every time the Player gets hit by a BadGuy, the payer has a constant number of HP deducted from
      // their total HP. It could be interesting if this varied based on the BadGuy that hit the Player.
      _hitPoints -= BadGuyHitHitPointDeduction;

      if (_hitPoints <= 0)
      {
        this.Die();
      }
    }
 }
}

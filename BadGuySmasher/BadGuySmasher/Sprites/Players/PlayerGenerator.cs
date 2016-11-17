using System;
using System.Collections.Generic;
using System.Linq;
using BadGuySmasher.GameManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Players
{
  public class PlayerGenerator : Sprite
  {
    private const int PlayerHitPoints = 100;
    
    private string              _playerTextureAssetName;
    private ICollection<Player> _players;
    private int                 _numberOfPlayers;
  
    public PlayerGenerator(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 spritePosition, string generatorTextureAssetName, string playerTextureAssetName) : base(contentManager, graphicsDevice, worldMap, spritePosition, generatorTextureAssetName, null)
    {
      if (string.IsNullOrWhiteSpace(playerTextureAssetName))
      {
        throw new ArgumentNullException("playerTextureAssetName");
      }

      _playerTextureAssetName = playerTextureAssetName;
    }

    public void SetNumberOfPlayers(int value)
    {
      _numberOfPlayers = value;
    }

    public ICollection<Player> Players { get { return _players; } }

    protected override void Move(GameTime gameTime)
    {
      // Only create Players once
      if (_players == null || _players.Count <= 0)
      {
        SpawnPlayers();
      }
    }

    private void SpawnPlayers()
    {
      _players = new List<Player>();

      for (int i = 1; i <= _numberOfPlayers; i++)
      {
        Vector2 playerPosition = Position;
        playerPosition.X = Position.X * (i / 2.7f);
        Player newPlayer = new Player(ContentManager, GraphicsDevice, WorldMap, playerPosition, PlayerHitPoints, _playerTextureAssetName, i);
        newPlayer.DrawBounds = DrawBounds;
        _players.Add(newPlayer);
        WorldMap.Sprites.Add(newPlayer);
      }
    }
  }
}

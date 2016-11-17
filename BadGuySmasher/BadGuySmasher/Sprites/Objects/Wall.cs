using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Objects
{
  public class Wall : Sprite
  {
    public Wall(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, string textureAssetName, SpriteProperties spriteProperties)
      : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, spriteProperties)
    {
    }
  }
}

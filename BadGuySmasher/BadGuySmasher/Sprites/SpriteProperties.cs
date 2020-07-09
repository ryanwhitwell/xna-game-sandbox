using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BadGuySmasher.Sprites
{
  public class SpriteProperties
  {
    public SpriteProperties()
    {
    }

    public SpriteProperties(int squishiness, Vector2 maxVelocity, Vector2 minVelocity)
    {
      Squishiness = squishiness;
      MaxVelocity = maxVelocity;
      MinVelocity = minVelocity;
    }

    public int     Squishiness { get; set; }
    public Vector2 MaxVelocity { get; set; }
    public Vector2 MinVelocity { get; set; }
  }
}

using System;
using UnityEngine;

namespace Redpenguin.Core.Extensions
{
  
  public static class CollisionExtensions
  {
    public static bool Matches(this Collider2D collider, LayerMask layerMask) =>
      ((1 << collider.gameObject.layer) & layerMask) != 0;
  }
}
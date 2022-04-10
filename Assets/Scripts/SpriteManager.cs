using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
    public static Vector2 ResizeSprite(Vector2 originalSize, Vector2 maxSize)
    {
        return originalSize * Mathf.Min(maxSize.x / originalSize.x, maxSize.y / originalSize.y);
    }
}

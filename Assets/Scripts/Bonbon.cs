using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonbon : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;

    [HideInInspector] public int score;
    [HideInInspector] public BonbonType bonbonType;
    [HideInInspector] public BonbonScriptableObject sprites;
    [HideInInspector] public bool isPiment;

    public void Init(BonbonScriptableObject _sprites, int _score, BonbonType type, bool _isPiment = false)
    {
        sprites = _sprites;
        score = _score;
        bonbonType = type;
        isPiment = _isPiment;

        renderer.sprite = sprites.start;
    }

    public void NextSprite()
    {
        if (renderer.sprite == sprites.start)
        {
            renderer.sprite = sprites.mid;
        }
        else renderer.sprite = sprites.end;
    }
}

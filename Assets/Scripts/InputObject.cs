using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputObject : MonoBehaviour
{
    private int keycode;
    private Sprite sprite;
    [SerializeField] SpriteRenderer renderer;

    public void Init(int key, Sprite _sprite)
    {
        keycode = key;
        sprite = _sprite;

        renderer.sprite = sprite;
    }

    public void StartFall(float height, float fallDuration)
    {
        StartCoroutine(InputFall(height, fallDuration));
    }

    private IEnumerator InputFall(float height, float fallDuration)
    {
        float posDepart = transform.position.y;
        float posFinal = posDepart - height;
        float posIntermediaire = 0;

        float timer = 0f;

        while (timer <= fallDuration)
        {
            timer += Time.deltaTime;

            posIntermediaire = Mathf.Lerp(posDepart, posFinal, timer / fallDuration);

            transform.position = new Vector3(transform.position.x, posIntermediaire, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, posFinal, transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClignotement : MonoBehaviour
{
    [SerializeField, Min(1)] float maxScale = 2f;
    [SerializeField] float duration = 1;

    float currentScale = 1f;

    // Update is called once per frame
    void Update()
    {
        currentScale = Mathf.Lerp(1, maxScale, (1 + Mathf.Cos(2 * Mathf.PI * Time.time / duration))/2);
        transform.localScale = currentScale * Vector3.one;
    }
}

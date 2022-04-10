using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    public GameObject piment1;
    public GameObject piment2;

    public static CanvasManager Get;
    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        piment1.SetActive(false);
        piment2.SetActive(false);
    }
}

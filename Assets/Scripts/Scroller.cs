using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private float _offset;
    private Material _mat;
  
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        _offset += (Time.deltaTime * scrollSpeed);
        _mat.SetTextureOffset("_MainTex", new Vector2(0, _offset));
    }
}

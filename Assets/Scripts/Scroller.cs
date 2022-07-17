using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float scrollSpeed = 0.5f;
    private float _offset;
    private Material _mat;
  
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _offset += (Time.deltaTime * scrollSpeed);
        _mat.SetTextureOffset("_MainTex", new Vector2(0, _offset));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2ReverseLaser : MonoBehaviour
{
    private float _laserSpeed = 4f;

    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y > 7)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}

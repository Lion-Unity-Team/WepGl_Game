using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Vector2 _resetPos = new Vector2(0, 11);

    private void Update()
    {
        transform.position += Time.deltaTime * Enemy._speed * Vector3.down;

        if (transform.position.y <= -_resetPos.y)
            transform.position = _resetPos;
    }
}

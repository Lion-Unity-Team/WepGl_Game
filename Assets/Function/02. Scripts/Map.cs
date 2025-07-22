using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Vector2 _resetPos = new Vector2(0, 11);
    public static float _speed = 5f;
    public Slime Slime;
    private void Update()
    {
        transform.position += Time.deltaTime * _speed * Vector3.down;

        if (transform.position.y <= -_resetPos.y)
            transform.position = _resetPos;

        if (Slime != null && Slime.isDead)
        {
            _speed = 0f;
        }
    }
}

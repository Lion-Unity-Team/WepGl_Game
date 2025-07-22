using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public static float _speed = 5f;
    public TextMeshPro hpText;

    private void Start()
    {
        hpText.text = hp.ToString();
    }

    private void Update()
    {
        transform.position += Time.deltaTime * _speed * Vector3.down;
        if(transform.position.y <= -6)
            Destroy(gameObject);
    }
}

using System;
using TMPro;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Animator _animator;
    public TextMeshPro hpText;
    public TextMeshPro hpBigText;
    private Vector2 _mousePos;
    public static int hp = 5;
    public bool isDead = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hpText.text = hp.ToString();
        hpBigText.text = hp.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_mousePos.x > 1)
                transform.position = new Vector2(2, -4);
            else if (_mousePos.x < -1)
                transform.position = new Vector2(-2, -4);
            else if(_mousePos.x < 1 && _mousePos.x > -1)
                transform.position = new Vector2(0, -4);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hp > other.GetComponent<Enemy>().hp)
            {
                hp += other.GetComponent<Enemy>().hp;
                hpText.text = hp.ToString();
                hpBigText.text = hp.ToString();
                Destroy(other.gameObject);
            }
            else
            {
                _animator.SetTrigger("Death");
                hpText.gameObject.SetActive(false);
                isDead = true;
                
            }
        }
    }
}

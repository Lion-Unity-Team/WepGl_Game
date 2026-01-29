using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private Enemy_Spawner _enemySpawner;
    private Vector3 _resetPos;
    private Vector3 _originPos;
    
    private static float _speed = 0;
    public int n;

    public static bool canMoving = false;
    private void Awake()
    {
        _resetPos = new Vector3(0, 18, 0);
        _originPos = transform.position;
    }

    private void OnEnable()
    {
        transform.position = _originPos;
    }

    private void Start()
    {
        _enemySpawner = FindFirstObjectByType<Enemy_Spawner>();
    }

    private void Update()
    {
        if (!canMoving)
            return;
        
        _speed = _enemySpawner.enemySpeed;
        transform.position += Time.deltaTime * _speed * Vector3.down;

        if (transform.position.y <= -_resetPos.y)
        {
            if (n != Map.instance._mapIndex % Map.instance.background.Length)
            {
                gameObject.SetActive(false);
                Map.instance.background[(Map.instance._mapIndex - 1) % Map.instance.background.Length].SetActive(false);
            }    
            
            // 임계값 넘을때 생기는 간극 방지
            var len = transform.position + _resetPos;
            transform.position = _resetPos + len;
            
            if (Map.instance.time >= 30)
            {
                Map.instance.time = 0;
                Map.instance.OnBackground(len.y);
                PlayerManager.instance.PlayerData.turnStage++;
                gameObject.SetActive(false);
            }
        }
    }
}

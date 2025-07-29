using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy_Prefab;
    private bool[] spawn = new bool[3];
    private List<GameObject> enemyList = new List<GameObject>();
    public float enemySpeed = 5.0f;
    public float speedTimer = 0f;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => AllEnemiesDead());

            int spawnCount = Random.Range(1, 4);

            for (int i = 0; i < 3; i++) spawn[i] = false;

            int enemy_count = 0;
            while (enemy_count < spawnCount)
            {
                int enemy_Pos = Random.Range(0, 3);
                if (!spawn[enemy_Pos])
                {
                    spawn[enemy_Pos] = true;
                    GameObject enemy = Instantiate(enemy_Prefab, new Vector2(-2 + 2 * enemy_Pos, 7), Quaternion.identity);
                    Enemy_Movement enemyscript = enemy.GetComponent<Enemy_Movement>();
                    enemyscript.speed = enemySpeed;
                    enemyList.Add(enemy);
                    enemy_count++;
                }
            }

            yield return null;
        }
    }

    private void Update()
    {
        speedTimer += Time.deltaTime;
        if(speedTimer >= 1f) { enemySpeed += 0.1f;
            speedTimer = 0f;
        }
    }

    private bool AllEnemiesDead()
    {
        enemyList.RemoveAll(enemy => enemy == null); // 파괴된 오브젝트 제거
        return enemyList.Count == 0;
    }
}

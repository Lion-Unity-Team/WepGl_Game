using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy_Prefab;
    [SerializeField] private GameObject playerSlime;
    [SerializeField] private GameObject springFruitPrefab;
    [SerializeField] private GameObject summerFruitPrefab;
    [SerializeField] private GameObject fallFruitPrefab;
    [SerializeField] private GameObject winterFruitPrefab;
    private bool[] spawn = new bool[3];
    private List<GameObject> enemyList = new List<GameObject>();

    public Timer timer;
    public float enemySpeed = 5.0f;
    public float speedTimer = 0f;
    public Animator _anime;
   

    public Coroutine spawnCoroutine;
    public Coroutine spawnFruitCoroutine;

    public void StartSpawning()
    {

        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        if (spawnFruitCoroutine == null)
            spawnFruitCoroutine = StartCoroutine(SpawnFruits());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        if (spawnFruitCoroutine != null)
        {
            StopCoroutine(spawnFruitCoroutine);
            spawnFruitCoroutine = null;
        }
    }

    public IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitUntil(() => AllEnemiesDead());
            yield return new WaitForSeconds(2.5f);
            TMP_Text playerText = playerSlime.GetComponentInChildren<TMP_Text>();
            BigInteger playerHp = BigInteger.Parse(playerText.text);

            BigInteger minHp = playerHp * 9 / 10;
            BigInteger maxHp = playerHp * 11 / 10;

            int spawnCount = Random.Range(1, 4);

            for (int i = 0; i < 3; i++) spawn[i] = false;

            List<BigInteger> enemyHpList = new List<BigInteger>();
            int guaranteedIndex = Random.Range(0, spawnCount);

            for (int i = 0; i < spawnCount; i++)
            {
                BigInteger hp;
                if (i == guaranteedIndex)
                {
                    hp = RandomBigInteger(minHp, playerHp - 1);
                }
                else
                {
                    hp = RandomBigInteger(minHp, maxHp); 
                }
                enemyHpList.Add(hp);
            }

           
            int enemy_count = 0;
            while (enemy_count < spawnCount)
            {
                int enemy_Pos = Random.Range(0, 3);
                if (!spawn[enemy_Pos])
                {
                    spawn[enemy_Pos] = true;
                    GameObject enemy = Instantiate(enemy_Prefab, new UnityEngine.Vector3(-2 + 2 * enemy_Pos, 10, 0), UnityEngine.Quaternion.identity);

                    
                    Enemy_Movement enemyscript = enemy.GetComponent<Enemy_Movement>();
                    enemyscript.speed = enemySpeed;

                    
                    TMP_Text enemyText = enemy.GetComponentInChildren<TMP_Text>();
                    BigInteger enemyHp = enemyHpList[enemy_count];
                    enemyText.text = enemyHp.ToString();

                    int digitCount = enemyHp.ToString().Length;
                    float fontSize = 1f;

                    if (digitCount >= 15)
                        fontSize = 0.35f;
                    else if (digitCount >= 14)
                        fontSize = 0.37f;
                    else if (digitCount >= 13)
                        fontSize = 0.4f;
                    else if (digitCount >= 12)
                        fontSize = 0.45f;
                    else if (digitCount >= 11)
                        fontSize = 0.5f;
                    else if (digitCount >= 10)
                        fontSize = 0.55f;
                    else if (digitCount >= 9)
                        fontSize = 0.6f;
                    else if (digitCount >= 8)
                        fontSize = 0.7f;
                    else if (digitCount >= 7)
                        fontSize = 0.8f;
                    else if (digitCount >= 6)
                        fontSize = 0.9f;

                    enemyText.fontSize = fontSize;

                    enemyList.Add(enemy);
                    enemy_count++;
                }
            }
        }
    }

private void Update()
    {
        bool IsRun = _anime.GetBool("IsRun");
        if (IsRun)
        {
            speedTimer += Time.deltaTime;
            if (speedTimer >= 1f)
            {
                Map.instance.time++;
                enemySpeed += 0.05f;
                speedTimer = 0f;
            }
        }
    }

    private bool AllEnemiesDead()
    {
        enemyList.RemoveAll(enemy => enemy == null);
        return enemyList.Count == 0;
    }

    
    private BigInteger RandomBigInteger(BigInteger min, BigInteger max)
    {
        
        BigInteger range = max - min + 1;

        byte[] bytes = range.ToByteArray();
        BigInteger result;
        do
        {
            System.Random rng = new System.Random();
            rng.NextBytes(bytes);
            bytes[bytes.Length - 1] &= 0x7F; 
            result = new BigInteger(bytes);
        } while (result >= range);

        return min + result;
    }

    

    public float GetCurrentSpeed()
    {
        return enemySpeed;
    }
    // 과일 스폰을 언제마다 할것인가에 대한 결론은 애너미스폰과 관계없이 스폰하는것으로함. 
    // 애너미스폰과 같은 주기로 적 생성후 몇초후에 고정된다면플레이어는 단순 패턴을 학습하고 예상하고 같은 플레이의 반복일것.
    // 하지만 따로한다면 적과 과일이 겹치는일이 있더라도 속도가 다르기에 어려운 플레이를 도전해서 먹어낼수도있을것, 이와 같은 난이도 상승과 플레이의 다양성과 무작위성이 생김.
    // 애너미와 함께 같은 라인에 스폰한다면 플레이어는 현재 스태미너(OR배고픔)에 따라 슬라임을 처치하는것을 포기하고 과일을 먹는다는 선택지가 주어지는 장점은 있음.
    // 하지만 결국 적 생성속도와 같다는 이야기이기 때문에 매번 적이 나올때 마다 반드시 과일이 함께 스폰할지 OR 만약 매번 스폰하지 않는다면 적 두번 스폰에 한번은 반드시 과일이 스폰한다던지 복잡성이 생김.

    // 배고픔에 대한 내 생각은 후반으로 갈수록 더 빠르게 배고픔이 닳고, 그만큼 자주 과일을 먹어줘야 한다는것 = 난이도가 적당히 올라갈수있음 
    // 또한 후반에 과일 스폰 속도를 늘린다면 난이도가 조금은 내려갈수있음(EX5초마다 스폰하던 과일을 초반에는 15~10초에 한개씩 먹으면 되던것을 후반에는 5초마다 나오는 과일을 90퍼센트는 먹어야한다거나...)

    public IEnumerator SpawnFruits()
    {
        int spawnInterval = Random.Range(3, 6);
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int lane = Random.Range(0, 3);

            GameObject fruitPrefab;

            if (timer.playTime > 90f)
            {
                fruitPrefab = springFruitPrefab;
            }
            else if (timer.playTime > 60f)
            {
                fruitPrefab = summerFruitPrefab;
            }
            else if (timer.playTime > 30f)
            {
                fruitPrefab = fallFruitPrefab;
            }
            else
            {
                fruitPrefab = winterFruitPrefab;
            }

            // 프리펩 생성
            GameObject fruit = Instantiate(
                fruitPrefab,
                new UnityEngine.Vector3(-2 + 2 * lane, 10, 0),
                UnityEngine.Quaternion.identity
            );

            // 50% 확률로 스케일을 0.7로 변경
            if (Random.value < 0.5f)
            {
                fruit.transform.localScale *= 0.7f;
            }
        }
    }


}

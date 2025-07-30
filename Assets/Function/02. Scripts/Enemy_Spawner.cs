using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics; // BigInteger ���

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy_Prefab;
    [SerializeField] private GameObject playerSlime; // �÷��̾� ������Ʈ ����
    private bool[] spawn = new bool[3];
    private List<GameObject> enemyList = new List<GameObject>();
    public float enemySpeed = 5.0f;
    public float speedTimer = 0f;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => AllEnemiesDead());

            // �÷��̾� ü�� TMP���� ���� ���
            TMP_Text playerText = playerSlime.GetComponentInChildren<TMP_Text>();
            BigInteger playerHp = BigInteger.Parse(playerText.text);

            BigInteger minHp = playerHp * 8 / 10;
            BigInteger maxHp = playerHp * 12 / 10;

            int spawnCount = Random.Range(1, 4);

            for (int i = 0; i < 3; i++) spawn[i] = false;

            // �� ü�� ����Ʈ �غ� (������ �� ������ player���� ���� ����)
            List<BigInteger> enemyHpList = new List<BigInteger>();
            int guaranteedIndex = Random.Range(0, spawnCount); // �� �ε����� ���� ������ player���� ����

            for (int i = 0; i < spawnCount; i++)
            {
                BigInteger hp;
                if (i == guaranteedIndex)
                {
                    hp = RandomBigInteger(minHp, playerHp - 1); // ������ ���� ��
                }
                else
                {
                    hp = RandomBigInteger(minHp, maxHp); // ��20% ����
                }
                enemyHpList.Add(hp);
            }

            // ���� �� ����
            int enemy_count = 0;
            while (enemy_count < spawnCount)
            {
                int enemy_Pos = Random.Range(0, 3);
                if (!spawn[enemy_Pos])
                {
                    spawn[enemy_Pos] = true;
                    GameObject enemy = Instantiate(enemy_Prefab, new UnityEngine.Vector2(-2 + 2 * enemy_Pos, 7), UnityEngine.Quaternion.identity);

                    // �ӵ� ����
                    Enemy_Movement enemyscript = enemy.GetComponent<Enemy_Movement>();
                    enemyscript.speed = enemySpeed;

                    // ü�� TMP ����
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
                        fontSize = 0.43f;
                    else if (digitCount >= 11)
                        fontSize = 0.48f;
                    else if (digitCount >= 10)
                        fontSize = 0.55f;
                    else if (digitCount >= 9)
                        fontSize = 0.7f;
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

            yield return null;
        }
    }

    private void Update()
    {
        speedTimer += Time.deltaTime;
        if (speedTimer >= 1f)
        {
            enemySpeed += 0.1f;
            speedTimer = 0f;
        }
    }

    private bool AllEnemiesDead()
    {
        enemyList.RemoveAll(enemy => enemy == null);
        return enemyList.Count == 0;
    }

    // BigInteger ���� ������ ���� �� ����
    private BigInteger RandomBigInteger(BigInteger min, BigInteger max)
    {
        // Random.Range�� int������ �����ϹǷ� BigInteger ������ ���� ���� ó��
        BigInteger range = max - min + 1;

        byte[] bytes = range.ToByteArray();
        BigInteger result;
        do
        {
            System.Random rng = new System.Random();
            rng.NextBytes(bytes);
            bytes[bytes.Length - 1] &= 0x7F; // ����� ����
            result = new BigInteger(bytes);
        } while (result >= range);

        return min + result;
    }
}

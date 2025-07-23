using System.Collections;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy_Slime;

    private int enemy_Max = 3;
    private int enemy_Min = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        while (true)
        {
            int enemy_Pos = Random.Range(0, 3);
            GameObject Enemy = Instantiate(enemy_Slime, new Vector2(-2 + 2 * enemy_Pos, 7), Quaternion.identity);
            
            yield return new WaitForSeconds(3f);
        }
        //for (int i = 0; i < 10; i++) { int enemy_Pos = Random.Range(0, 3); GameObject Enemy = Instantiate(enemy_Slime, new Vector2(-2 + 2 * enemy_Pos, 7), Quaternion.identity); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject _slime;
    private List<int> _enemyHpList = new List<int>();
    
    private int _enemyNum;
    private int _enemyHp;
    private int _randomHp;
    private int _enemyHpMin;
    private int _enemyHpMax;
    private int _enemyPos;

    private bool _isExist;

    private IEnumerator Start()
    {
        while (true)
        {                
            _enemyNum = Random.Range(1, 4);
            _enemyHpMin = Slim.hp - (int)(Slim.hp * 0.2f);
            _enemyHpMax = Slim.hp + (int)(Slim.hp * 0.2f);
            
            _isExist = false;
            _enemyHpList.Clear();
            for (int i = 0; i < _enemyNum; i++)
            {
                _randomHp = Random.Range(_enemyHpMin, _enemyHpMax);
                _enemyHpList.Add(_randomHp);
                if (_randomHp < Slim.hp)
                    _isExist = true;
            }

            if (_enemyNum == 3 && !_isExist)
            {
                int index = Random.Range(0, _enemyNum);
                _enemyHpList[index] = Random.Range(_enemyHpMin, Slim.hp);
            }

            int[] pos = new int[3]{0, 0, 0};
            for (int i = 0; i < _enemyNum; i++)
            {
                while (true)
                {
                    int index = Random.Range(0, 3);
                    if (pos[index] == 0)
                    {
                        pos[index] = 1;
                        GameObject enemy = Instantiate(_slime, new Vector2(-2 + 2 * index, 6), Quaternion.identity);
                        enemy.GetComponent<Enemy>().hp = _enemyHpList[i];
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
}

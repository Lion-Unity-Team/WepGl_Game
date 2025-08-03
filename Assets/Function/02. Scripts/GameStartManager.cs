using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject player;
    public Enemy_Spawner enemyspawner;
    public GameObject GameOver;
    public GameObject StartWindow;

    private void Start()        //게임이시작하면
    {
        player.SetActive(false);        //일단플레이어숨김
        enemyspawner.StopSpawning();    //일단적생성정지
        GameOver.SetActive(false);      //게임오버UI숨김
        StartWindow.SetActive(true);    //게임시작UI켜기
    }

    public void StartGame()     //게임시작
    {
        player.SetActive(true);     // 플레이어등장  
        enemyspawner.StartSpawning(); // 적생성시작
    }

    public void EndGame()
    {
        GameOver.SetActive(true);   // 게임오버UI켜짐
        enemyspawner.StopSpawning();    // 적생성정지
    }
}

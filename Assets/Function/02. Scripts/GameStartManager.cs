using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject player;
    public Enemy_Spawner enemyspawner;
    public GameObject GameOver;
    public GameObject StartWindow;

    private Animator _playerAnime;
    private string _playerIdleKey;

    private void Start()        //게임이시작하면
    {
        player.SetActive(false);        //일단플레이어숨김
        enemyspawner.StopSpawning();    //일단적생성정지
        GameOver.SetActive(false);      //게임오버UI숨김
        StartWindow.SetActive(true);    //게임시작UI켜기
        //이미 배경은 멈춰있음

        _playerAnime = player.GetComponentInChildren<Animator>();  // 플레이어 애니메이션
        _playerIdleKey = "Idle";
    }

    public void StartGame()     //게임시작버튼누르면
    {
        player.SetActive(true);     // 플레이어등장
        _playerAnime.SetBool(_playerIdleKey, true);
        enemyspawner.StartSpawning(); // 적생성시작
        FindObjectOfType<PropsMovement>().StartMoving(); // 배경 움직임 시작
    }

    public void EndGame()
    {
        GameOver.SetActive(true);   // 게임오버UI켜짐
        enemyspawner.StopSpawning();    // 적생성정지
        FindObjectOfType<PropsMovement>().StopMoving(); // 배경 움직임 정지
    }
}

using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject player;
    public Enemy_Spawner enemyspawner;
    public GameObject GameOver;
    public GameObject StartWindow;

    private Animator _playerAnime;
    private string _playerIdleKey;

    private void Start()        //�����̽����ϸ�
    {
        player.SetActive(false);        //�ϴ��÷��̾����
        enemyspawner.StopSpawning();    //�ϴ�����������
        GameOver.SetActive(false);      //���ӿ���UI����
        StartWindow.SetActive(true);    //���ӽ���UI�ѱ�
        //�̹� ����� ��������

        _playerAnime = player.GetComponentInChildren<Animator>();  // �÷��̾� �ִϸ��̼�
        _playerIdleKey = "Idle";
    }

    public void StartGame()     //���ӽ��۹�ư������
    {
        player.SetActive(true);     // �÷��̾����
        _playerAnime.SetBool(_playerIdleKey, true);
        enemyspawner.StartSpawning(); // ����������
        FindObjectOfType<PropsMovement>().StartMoving(); // ��� ������ ����
    }

    public void EndGame()
    {
        GameOver.SetActive(true);   // ���ӿ���UI����
        enemyspawner.StopSpawning();    // ����������
        FindObjectOfType<PropsMovement>().StopMoving(); // ��� ������ ����
    }
}

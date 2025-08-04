using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject player;
    public Enemy_Spawner enemyspawner;
    public GameObject GameOver;
    public GameObject StartWindow;

    private void Start()        //�����̽����ϸ�
    {
        player.SetActive(false);        //�ϴ��÷��̾����
        enemyspawner.StopSpawning();    //�ϴ�����������
        GameOver.SetActive(false);      //���ӿ���UI����
        StartWindow.SetActive(true);    //���ӽ���UI�ѱ�
        //�̹� ����� ��������
    }

    public void StartGame()     //���ӽ��۹�ư������
    {
        player.SetActive(true);     // �÷��̾����  
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

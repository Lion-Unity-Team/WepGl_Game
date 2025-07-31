using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject player;
    public Enemy_Spawner enemyspawner;
    public GameObject GameOver;
    public GameObject StartWindow;
    public GameObject EndWindow;

    private void Start()
    {
        player.SetActive(false);
        enemyspawner.StopSpawning();
        GameOver.SetActive(false);
        StartWindow.SetActive(true);
    }

    public void StartGame()
    {
        player.SetActive(true);

        enemyspawner.StartSpawning();
    }

    public void KeepGame()
    {
        
        GameOver.SetActive(true);
        EndWindow.SetActive(true);
        enemyspawner.StopSpawning();
    }
}

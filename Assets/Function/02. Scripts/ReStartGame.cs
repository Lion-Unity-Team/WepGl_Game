using UnityEngine;
using UnityEngine.SceneManagement;

public class ReStartGame : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

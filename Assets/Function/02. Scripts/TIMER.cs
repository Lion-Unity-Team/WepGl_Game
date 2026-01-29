using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject player;
    public PlayerSlime playerSlime;
    public TMP_Text playTimeText;  
    public GameObject UI1;   
    public GameObject UI2;   
    public GameObject UI3;   
    public Button UI4;
    public TMP_Text OVER;
    public TMP_Text CLAER;

    public float playTime; 
    private float stamina; 
    private Animator _anime;
    public int playTime2 = 0;
    public float elapsedTime = 0f;

    public int playTime1 = 0;
    private int eatFruit = 0;
    private int killSlime = 0;

    private bool flag;
    private void Start()
    {
        _anime = player.GetComponentInChildren<Animator>();
        playTime = 120;
    }

    private void Awake()
    {
        playerSlime = player.GetComponent<PlayerSlime>();
    }

    void Update()
    {
        int killSlime = playerSlime.killSlime;
        int eatFruit = playerSlime.eatFruit;
        int playTime1 = playerSlime.playTime1;

        if (!UI1.activeSelf && !UI2.activeSelf && !UI3.activeSelf)
        {
            playTime -= Time.deltaTime;
            stamina += Time.deltaTime;
            elapsedTime += Time.deltaTime;
            playTime2 = (int)elapsedTime;
        }

        
        int minutes = (int)((playTime % 3600) / 60);
        int seconds = (int)(playTime % 60);

        
        playTimeText.text = $"{minutes:00}:{seconds:00}";
        if (minutes == 0 && seconds == 0 && !flag)
        {
            flag = true;
            UI4.interactable = false;
            OVER.gameObject.SetActive(false);
            CLAER.gameObject.SetActive(true);
            FindObjectOfType<GameStartManager>().EndGame();
            FindObjectOfType<GameOverManager>().Score();
            _anime.speed = 0f;
            PlayerManager.instance.PlayerData.killSlime += killSlime;
            PlayerManager.instance.PlayerData.eatFruit += eatFruit;
            PlayerManager.instance.PlayerData.playTime1 += playTime1;
            PlayerManager.instance.PlayerData.playTime2 += playTime2;
            Debug.Log("When Clear Game");
            Debug.Log("killSlime : ");
            Debug.Log(PlayerManager.instance.PlayerData.killSlime);
            Debug.Log("eatFruit : ");
            Debug.Log(PlayerManager.instance.PlayerData.eatFruit);
            Debug.Log("playTime1 : ");
            Debug.Log(PlayerManager.instance.PlayerData.playTime1);
            Debug.Log("playTime2 : ");
            Debug.Log(PlayerManager.instance.PlayerData.playTime2);
        }

        if (stamina >= 1)
        {
            StaminaManager.instance.StaminaPlus(-3);
            stamina = 0;
        }

    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSlime : MonoBehaviour
{
    private Animator _anime;
    public TMP_Text playerHpText;
    public static double playerHp;

    public Button UI1;
    public TMP_Text OVER;
    public TMP_Text CLAER;

    public int playTime1 = 0;
    private float elapsedTime = 0f;

    private bool isCounting = true;

    private string _deathAnimeKey;
    private string _runAnimeKey;

    public int eatFruit;
    public int killSlime;

    private void Start()
    {
        playerHp = double.Parse(playerHpText.text);
        _anime = GetComponentInChildren<Animator>();

        _deathAnimeKey = "Death";
        _runAnimeKey = "IsRun";

        
        Debug.Log("When Start Game");
        Debug.Log("killSlime : ");
        Debug.Log(PlayerManager.instance.PlayerData.killSlime);
        Debug.Log("eatFruit : ");
        Debug.Log(PlayerManager.instance.PlayerData.eatFruit);
        Debug.Log("playTime1 : ");
        Debug.Log(PlayerManager.instance.PlayerData.playTime1);
        Debug.Log("playTime2 : ");
        Debug.Log(PlayerManager.instance.PlayerData.playTime2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            isCounting = false;
            TMP_Text enemyHpText = collision.GetComponentInChildren<TMP_Text>();
            if (enemyHpText == null) return;

            double enemyHp = double.Parse(enemyHpText.text);

            if (playerHp <= enemyHp)
            {
                PlayerManager.instance.PlayerData.killSlime += killSlime;
                PlayerManager.instance.PlayerData.eatFruit += eatFruit;
                PlayerManager.instance.PlayerData.playTime1 += playTime1;
                Timer timer = GameObject.Find("TimerManager").GetComponent<Timer>();
                int playTime2 = timer.playTime2;
                PlayerManager.instance.PlayerData.playTime2 += playTime2;
                
                killSlime = 0;
                eatFruit = 0;
                
                FindObjectOfType<GameStartManager>().EndGame();
                FindObjectOfType<GameOverManager>().Score();
                _anime.SetTrigger(_deathAnimeKey);
                _anime.SetBool(_runAnimeKey, false);
                Debug.Log("When Over Game");
                Debug.Log("killSlime : ");
                Debug.Log(PlayerManager.instance.PlayerData.killSlime);
                Debug.Log("eatFruit : ");
                Debug.Log(PlayerManager.instance.PlayerData.eatFruit);
                Debug.Log("playTime1 : ");
                Debug.Log(PlayerManager.instance.PlayerData.playTime1);
                Debug.Log("playTime2 : ");
                Debug.Log(PlayerManager.instance.PlayerData.playTime2);
            }
            else
            {
                playerHp += enemyHp;
                if (playerHp >= 100000000)
                {
                    OVER.gameObject.SetActive(false);
                    CLAER.gameObject.SetActive(true);
                    playerHp = 100000000;
                    playerHpText.text = playerHp.ToString();
                    UI1.interactable = false;
                    FindObjectOfType<GameStartManager>().EndGame();
                    FindObjectOfType<GameOverManager>().Score();
                    _anime.speed = 0f;
                }

                playerHpText.text = playerHp.ToString();
                killSlime++;
            }

            StaminaManager.instance.StaminaPlus(-15);
            SoundManager.instance.SfxPlay("Attack");
        }

        if (collision.CompareTag("Fruit"))
        {
            if (collision.transform.localScale.x >= 0.9)
            {
                StaminaManager.instance.StaminaPlus(40);
            }
            else
            {
                StaminaManager.instance.StaminaPlus(15);
            }

            SoundManager.instance.SfxPlay("Fruit");
            eatFruit++;
        }

    }

    void Update()
    {
        if (isCounting)
        {
            elapsedTime += Time.deltaTime;
            playTime1 = (int)elapsedTime;
        }

        string value = playerHpText.text;
        int digitCount = value.Length;

        float fontSize = 1f;

        if (digitCount >= 15)
            fontSize = 0.35f;
        else if (digitCount >= 14)
            fontSize = 0.37f;
        else if (digitCount >= 13)
            fontSize = 0.4f;
        else if (digitCount >= 12)
            fontSize = 0.45f;
        else if (digitCount >= 11)
            fontSize = 0.5f;
        else if (digitCount >= 10)
            fontSize = 0.55f;
        else if (digitCount >= 9)
            fontSize = 0.6f;
        else if (digitCount >= 8)
            fontSize = 0.7f;
        else if (digitCount >= 7)
            fontSize = 0.8f;
        else if (digitCount >= 6)
            fontSize = 0.9f;

        playerHpText.fontSize = fontSize;

        float Stamina = StaminaManager.instance.GetCurrentStamina();

        if (Stamina <= 0 && _anime.GetBool(_runAnimeKey))
        {
            PlayerManager.instance.PlayerData.killSlime += killSlime;
            PlayerManager.instance.PlayerData.eatFruit += eatFruit;
            PlayerManager.instance.PlayerData.playTime1 += playTime1;
            Timer timer = GameObject.Find("TimerManager").GetComponent<Timer>();
            int playTime2 = timer.playTime2;
            PlayerManager.instance.PlayerData.playTime2 += playTime2;
            
            killSlime = 0;
            eatFruit = 0;
            
            FindObjectOfType<GameStartManager>().EndGame();
            FindObjectOfType<GameOverManager>().Score();
            _anime.SetTrigger(_deathAnimeKey);
            _anime.SetBool(_runAnimeKey, false);
        }
    }
}

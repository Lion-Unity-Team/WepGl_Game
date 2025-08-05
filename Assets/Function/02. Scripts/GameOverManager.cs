using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;
    public PlayerSlime playerSlime;

    private const string BestScoreKey = "BestPlayerHP";
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("BestPlayerHP"))
        {
            Debug.Log("로드된 최고 점수: " + PlayerPrefs.GetString("BestPlayerHP"));
        }
        else
        {
            Debug.Log("최고 점수 없음");
        }
    }

    public void Score()
    {
        double currentHP = double.Parse(playerSlime.playerHpText.text);
        currentScoreText.text = "슬라임 크기 : " + currentHP.ToString();

        double bestHp = PlayerPrefs.HasKey(BestScoreKey)
            ? double.Parse(PlayerPrefs.GetString(BestScoreKey)) : currentHP;

        if(currentHP >= bestHp)
        {
            bestHp = currentHP;
            PlayerPrefs.SetString(BestScoreKey, bestHp.ToString());
            PlayerPrefs.Save();
        }

        bestScoreText.text = "역대 최고 슬라임 크기 : " + bestHp.ToString();
    }
}

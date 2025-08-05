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
            Debug.Log("�ε�� �ְ� ����: " + PlayerPrefs.GetString("BestPlayerHP"));
        }
        else
        {
            Debug.Log("�ְ� ���� ����");
        }
    }

    public void Score()
    {
        double currentHP = double.Parse(playerSlime.playerHpText.text);
        currentScoreText.text = "������ ũ�� : " + currentHP.ToString();

        double bestHp = PlayerPrefs.HasKey(BestScoreKey)
            ? double.Parse(PlayerPrefs.GetString(BestScoreKey)) : currentHP;

        if(currentHP >= bestHp)
        {
            bestHp = currentHP;
            PlayerPrefs.SetString(BestScoreKey, bestHp.ToString());
            PlayerPrefs.Save();
        }

        bestScoreText.text = "���� �ְ� ������ ũ�� : " + bestHp.ToString();
    }
}

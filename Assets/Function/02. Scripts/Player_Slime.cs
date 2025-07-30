using UnityEngine;
using TMPro;

public class PlayerSlime : MonoBehaviour
{
    public TMP_Text playerHpText;
    private double playerHp;

    private void Start()
    {
        playerHp = double.Parse(playerHpText.text);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TMP_Text enemyHpText = collision.GetComponentInChildren<TMP_Text>();
        if (enemyHpText == null) return;

        double enemyHp = double.Parse(enemyHpText.text);

        if (playerHp < enemyHp)
        {
            // 플레이어 사망
            Destroy(gameObject);
        }
        else
        {
            // 플레이어가 적을 먹고 체력 증가
            playerHp += enemyHp;
            playerHpText.text = playerHp.ToString();
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
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
            fontSize = 0.43f;
        else if (digitCount >= 11)
            fontSize = 0.48f;
        else if (digitCount >= 10)
            fontSize = 0.55f;
        else if (digitCount >= 9)
            fontSize = 0.7f;
        else if (digitCount >= 8)
            fontSize = 0.7f;
        else if (digitCount >= 7)
            fontSize = 0.8f;
        else if (digitCount >= 6)
            fontSize = 0.9f;

        playerHpText.fontSize = fontSize;
    }
}

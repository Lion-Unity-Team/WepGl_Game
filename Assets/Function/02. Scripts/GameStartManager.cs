using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    public GameObject player;
    public Enemy_Spawner enemyspawner;
    public GameObject GameOver;
    public GameObject StartWindow;
    public Button KeepPlay;
    
    [SerializeField] public ParticleSystem[] particles;

    private int click = 0;
    private Animator _playerAnime;
    private string _playerRunKey;
    private string _PlayerWakeUpKey;
    
    [SerializeField] private CanvasGroup _gameEndCanvasGroup;
    [SerializeField] private RectTransform _gameEndRectTransform;

    [SerializeField] private CanvasGroup _gameStartCanvasGroup;
    [SerializeField] private RectTransform _gameStartTransform;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Slime_Movement _movement;
    
    private void Start()        //�����̽����ϸ�
    {
        _gameEndCanvasGroup.alpha = 1;
        _gameEndRectTransform.localScale = Vector3.one;
        
        _gameEndCanvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        _gameEndRectTransform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetUpdate(UpdateType.Normal,
            true).OnComplete(() =>
        {
            _gameEndCanvasGroup.gameObject.SetActive(false);
        });
        
        KeepPlay.onClick.AddListener(CountClick);
        enemyspawner.StopSpawning();    //�ϴ�����������
        GameOver.SetActive(false);      //���ӿ���UI����
        StartWindow.SetActive(true);    //���ӽ���UI�ѱ�
        //�̹� ����� ��������

        _playerAnime = player.GetComponentInChildren<Animator>(); // �÷��̾� �ִϸ��̼�
        _playerAnime.speed = 0;         // �÷��̾� �̵� ����
        _playerRunKey = "IsRun";
        _PlayerWakeUpKey = "WakeUp";
        CloudSpawner.isPlay = false;

        AnimatorManager.Instance.ChangeAnimator(PlayerPrefs.GetInt("CurrentSkin" , 0));
        
        
        if (PlayerPrefs.HasKey("BestPlayerHP"))
        {
            //Debug.Log("�ε�� �ְ� ����: " + PlayerPrefs.GetString("BestPlayerHP"));
            SkinManager.instance.LoadData();
            PlayerManager.instance.LoadData();
        }
        else
        {
            //Debug.Log("�ְ� ���� ���� player, skin �ʱ�ȭ");
            InitData();
        }
    }
    
    public void InitData()
    {
        PlayerManager.instance.InitData();
        SkinManager.instance.InitData();
            
        PlayerManager.instance.LoadData();
        SkinManager.instance.LoadData();
    }


    public void CountClick()
    {
        click++;
        if(click > 0)
        {
            KeepPlay.interactable = false;
        }
    }

    public void StartGame()     //���ӽ��۹�ư������
    {
        _playerAnime.speed = 1;     // �÷��̾� �̵� ����
        _playerAnime.SetBool(_playerRunKey, true);
        enemyspawner.StartSpawning(); // ����������
        Ground.canMoving = true;
        
        _gameStartCanvasGroup.alpha = 1;
        _gameStartTransform.localScale = Vector3.one;
        
        _gameStartCanvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        _gameStartTransform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetUpdate(UpdateType.Normal,
            true).OnComplete(() =>
        {
            _gameStartCanvasGroup.gameObject.SetActive(false);
        });
        CloudSpawner.isPlay = true;
    }

    public void OnClickedShowAd()
    {
        SoundManager.instance.BgmMute(true);
        SoundManager.instance.SfxMute(true);
        AdManager.Instance.ShowAd(() => StartCoroutine(StartCountdown()));
    }

    IEnumerator StartCountdown()
    {
        _movement.isCountdown = true;
        SoundManager.instance.BgmMute(false);
        SoundManager.instance.SfxMute(false);
        Time.timeScale = 0;
        
        _gameEndCanvasGroup.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            
            Sequence seq = DOTween.Sequence();
            
            seq.Append(countdownText.transform.DOScale(1.8f, 0.15f).SetEase(Ease.OutExpo));
            
            seq.Append(countdownText.transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
            
            seq.SetUpdate(true);

            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.text = "GO!";
        countdownText.transform.DOScale(2f, 0.2f).SetUpdate(true);
        countdownText.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => {
            countdownText.gameObject.SetActive(false);
            Time.timeScale = 1;
            _movement.isCountdown = false;
            KeepGame();
        });
    }

    public void KeepGame()
    {
        CloudSpawner.isPlay = true;
        
        StaminaManager.instance.StaminaChange(70);
        StaminaManager.instance.StaminaPlus(0); // ���׹̳� �ٸ� ���� �ϱ� ����
        
        _playerAnime.SetTrigger(_PlayerWakeUpKey);  //�Ͼ�µ���
        _playerAnime.SetBool(_playerRunKey, true);  //�޸��µ���
        enemyspawner.StartSpawning(); // ����������
        Ground.canMoving = true;
        
        foreach (var particle in particles) // ��ƼŬ ��� ����
        {
            particle.Play();
        }
    }

    public void EndGame()
    {
        SkinManager.instance.AchievementCheak();
        CloudSpawner.isPlay = false;
        _gameEndCanvasGroup.alpha = 0;
        _gameEndRectTransform.localScale = Vector3.zero;

        _gameEndCanvasGroup.DOFade(1, 0.3f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        _gameEndRectTransform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(UpdateType.Normal, true);
        
        GameOver.SetActive(true);   // ���ӿ���UI����
        enemyspawner.StopSpawning();    // ����������
        Ground.canMoving = false;
        
        foreach (var particle in particles) // ��ƼŬ ��� �Ͻ� ����
        {
            particle.Pause();
        }
    }
}

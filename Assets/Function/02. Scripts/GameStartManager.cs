using DG.Tweening;
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
    

    private void Start()        //게임이 시작하면
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
        enemyspawner.StopSpawning();    //일단적생성정지
        GameOver.SetActive(false);      //게임오버UI숨김
        StartWindow.SetActive(true);    //게임시작UI켜기
        //이미 배경은 멈춰있음

        _playerAnime = player.GetComponentInChildren<Animator>(); // 플레이어 애니메이션
        _playerAnime.speed = 0;         // 플레이어 이동 멈춤
        _playerRunKey = "IsRun";
        _PlayerWakeUpKey = "WakeUp";
        CloudSpawner.isPlay = false;

        AnimatorManager.Instance.ChangeAnimator(PlayerPrefs.GetInt("CurrentSkin" , 0));
        
        
        if (PlayerPrefs.HasKey("BestPlayerHP"))
        {
            //Debug.Log("로드된 최고 점수: " + PlayerPrefs.GetString("BestPlayerHP"));
            SkinManager.instance.LoadData();
            PlayerManager.instance.LoadData();
        }
        else
        {
            //Debug.Log("최고 점수 없음 player, skin 초기화");
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

    public void StartGame()     //게임시작버튼누르면
    {
        _playerAnime.speed = 1;     // 플레이어 이동 시작
        _playerAnime.SetBool(_playerRunKey, true);
        enemyspawner.StartSpawning(); // 적생성시작
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

    public void KeepGame()
    {
        CloudSpawner.isPlay = true;
        _gameEndCanvasGroup.alpha = 1;
        _gameEndRectTransform.localScale = Vector3.one;
        
        _gameEndCanvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        _gameEndRectTransform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetUpdate(UpdateType.Normal,
            true).OnComplete(() =>
        {
            _gameEndCanvasGroup.gameObject.SetActive(false);
        });
        
        StaminaManager.instance.StaminaChange(70);
        StaminaManager.instance.StaminaPlus(0); // 스테미너 바를 갱신 하기 위함
        
        _playerAnime.SetTrigger(_PlayerWakeUpKey);  //일어나는동작
        _playerAnime.SetBool(_playerRunKey, true);  //달리는동작
        enemyspawner.StartSpawning(); // 적생성시작
        Ground.canMoving = true;
        
        foreach (var particle in particles) // 파티클 모두 시작
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
        
        GameOver.SetActive(true);   // 게임오버UI켜짐
        enemyspawner.StopSpawning();    // 적생성정지
        Ground.canMoving = false;
        
        foreach (var particle in particles) // 파티클 모두 일시 정지
        {
            particle.Pause();
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 게임의 사운드를 관리하는 스크립트
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    [SerializeField] private AudioSource bgm_Player;
    [SerializeField] private AudioSource sfx_Player;

    [SerializeField] private Button[] bgm_Button;
    [SerializeField] private Button[] sfx_Button;
    
    [SerializeField] private AudioClip[] bgm_Clips;
    [SerializeField] private AudioClip[] sfx_Clips;
    
    [SerializeField] private Slider bgm_Slider;
    [SerializeField] private Slider sfx_Slider;

    [SerializeField] private GameObject bgm_FillArea;
    [SerializeField] private GameObject sfx_FillArea;
    
    private bool isBgmMute = false;
    private bool isSfxMute = false;
    
    // 싱글톤 기법을 사용
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    #region Background event
    private void OnEnable()
    {
        try
        {
            bool current = AppsInToss.AITVisibilityHelper.IsVisible;
            ApplyVisibility(current);
        }
        catch (Exception)
        {
            // SDK 호출 실패 시 기본 동작: 재생 유지
        }

        // 이벤트 구독
        AppsInToss.AITVisibilityHelper.OnVisibilityChanged += OnVisibilityChanged;
    }

    private void OnDisable()
    {
        AppsInToss.AITVisibilityHelper.OnVisibilityChanged -= OnVisibilityChanged;
    }

    private void OnVisibilityChanged(bool isVisible)
    {
        Debug.Log($"[BgmMinimal] Visibility changed -> {isVisible}");
        ApplyVisibility(isVisible);
    }
    
    public void SetVisibility(bool isVisible) => ApplyVisibility(isVisible);

    private void ApplyVisibility(bool isVisible)
    {
        if (bgm_Player == null) return;

        if (isVisible)
        {
            if (bgm_Player.time > 0f && !bgm_Player.isPlaying) bgm_Player.UnPause();
            else if (!bgm_Player.isPlaying) bgm_Player.Play();
        }
        else
        {
            bgm_Player.Pause();
        }
    }
    #endregion

    private void Start()
    {
        bgm_Slider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfx_Slider.onValueChanged.AddListener(OnSFXVolumeChanged);
        
        for (int i = 0; i < bgm_Button.Length; i++)
        {
            bgm_Button[i].onClick.AddListener(OnBGMMute);
            sfx_Button[i].onClick.AddListener(OnSFXMute);
        }

        
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            bgm_Player.volume = PlayerPrefs.GetFloat("BGMVolume");
            sfx_Player.volume = PlayerPrefs.GetFloat("SFXVolume");
            
            sfx_Slider.value = sfx_Player.volume * sfx_Slider.maxValue;
            bgm_Slider.value = bgm_Player.volume * bgm_Slider.maxValue; 
            
            isBgmMute = PlayerPrefs.GetInt("BGMMute") == 1;
            isSfxMute = PlayerPrefs.GetInt("SFXMute") == 1;
            
            BgmMute(isBgmMute);
            SfxMute(isSfxMute);
        }
        
        bgm_Slider.onValueChanged.AddListener((temp) => SfxPlay("UI_Button"));
        sfx_Slider.onValueChanged.AddListener((temp) => SfxPlay("UI_Button"));
    }

    private void OnBGMMute()
    {
        bgm_Player.mute = !bgm_Player.mute;
        
        if (bgm_Player.mute)
        {
            PlayerPrefs.SetInt("BGMMute", 1);
        }
        else
        {
            PlayerPrefs.SetInt("BGMMute", 0);
        }
    }
    
    private void OnSFXMute()
    {
        sfx_Player.mute = !sfx_Player.mute;
        
        if (sfx_Player.mute)
        {
            PlayerPrefs.SetInt("SFXMute", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SFXMute", 0);
        }
    }

    private void OnBGMVolumeChanged(float volume)
    {
        bgm_Player.volume = volume / bgm_Slider.maxValue;
    }
    
    private void OnSFXVolumeChanged(float volume)
    {
        sfx_Player.volume = volume / sfx_Slider.maxValue;
    }
    
    // 효과음을 출력하는 함수
    public void SfxPlay(string clipName)
    {
        foreach (var clip in sfx_Clips)
        {
            if (clip.name == clipName)
            {
                sfx_Player.PlayOneShot(clip);
                return;
            }
        }

        Debug.Log($"{clipName} not found");
    }

    public void BgmPlay(string clipName)
    {
        StartCoroutine(FadeBgmPlay(clipName));
    }

    public void BgmMute(bool isMute) // 개선 가능할지도
    {
        if (isMute)
        {
            bgm_Player.mute = true;
            bgm_Slider.interactable = false;
            bgm_FillArea.SetActive(false);
            
            bgm_Button[0].gameObject.SetActive(true); // OFF
            bgm_Button[1].gameObject.SetActive(false); // ON
        }
        else
        {
            bgm_Player.mute = false;
            bgm_Slider.interactable = true;
            bgm_FillArea.SetActive(true);
            
            bgm_Button[0].gameObject.SetActive(false); // OFF
            bgm_Button[1].gameObject.SetActive(true); // ON
        }
    }

    public void SfxMute(bool isMute)
    {
        if (isMute)
        {
            sfx_Player.mute = true;
            sfx_Slider.interactable = false;
            sfx_FillArea.SetActive(false);
            
            sfx_Button[0].gameObject.SetActive(true); // OFF
            sfx_Button[1].gameObject.SetActive(false); // ON
        }
        else
        {
            sfx_Player.mute = false;
            sfx_Slider.interactable = true;
            sfx_FillArea.SetActive(true);
            
            sfx_Button[0].gameObject.SetActive(false); // OFF
            sfx_Button[1].gameObject.SetActive(true); // ON
        }
    }

    IEnumerator FadeBgmPlay(string clipName)
    {
        float currentVolume = bgm_Player.volume;
        float timer = 0f;
        float fadeDuration = 0.2f;

        while (timer < fadeDuration)
        {
            bgm_Player.volume = Mathf.Lerp(bgm_Player.volume, 0f, timer/fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        bgm_Player.volume = 0f;
        bgm_Player.Stop();

        foreach (var clip in bgm_Clips)
        {
            if (clip.name == clipName)
            {
               bgm_Player.clip = clip;
               break;
            }
        }
        bgm_Player.Play();
        timer = 0;
        
        while (timer < fadeDuration)
        {
            bgm_Player.volume = Mathf.Lerp(0, currentVolume, timer/fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        bgm_Player.volume = currentVolume;
        
        Debug.Log($"배경음 변경 : {clipName}");
    }

    public void SoundSave()
    {
        PlayerPrefs.SetFloat("BGMVolume",bgm_Player.volume);
        PlayerPrefs.SetFloat("SFXVolume",sfx_Player.volume);
        PlayerPrefs.Save();
        Debug.Log("저장됨");
    }
}

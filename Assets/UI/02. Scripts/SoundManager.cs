using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    
    [SerializeField] private AudioSource bgm_Player;
    [SerializeField] private AudioSource sfx_Player;

    [SerializeField] private Button[] bgm_Button;
    [SerializeField] private Button[] sfx_Button;
    
    [SerializeField] private AudioClip[] sfx_Clips;
    
    [SerializeField] private Slider bgm_Slider;
    [SerializeField] private Slider sfx_Slider;
    
    // 싱글톤 기법
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        bgm_Player.volume = bgm_Slider.value / bgm_Slider.maxValue;
        sfx_Player.volume = sfx_Slider.value / sfx_Slider.maxValue;
    }

    private void Start()
    {
        bgm_Slider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfx_Slider.onValueChanged.AddListener(OnSFXVolumeChanged);

        for (int i = 0; i < bgm_Button.Length; i++)
        {
            bgm_Button[i].onClick.AddListener(OnBGMMute);
            sfx_Button[i].onClick.AddListener(OnSFXMute);
        }
    }

    private void OnBGMMute()
    {
        bgm_Player.mute = !bgm_Player.mute;
    }
    
    private void OnSFXMute()
    {
        sfx_Player.mute = !sfx_Player.mute;
    }

    private void OnBGMVolumeChanged(float volume)
    {
        bgm_Player.volume = volume / bgm_Slider.maxValue;
    }
    
    private void OnSFXVolumeChanged(float volume)
    {
        sfx_Player.volume = volume / sfx_Slider.maxValue;
    }
    
    public void SFXPlay(string clipName)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSound : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] Slider _bgmSoundControllSlider;
    [SerializeField] Toggle _bgmMuteToggle;
    [SerializeField] Text _bgmSoundValueText;

    [Header("SFX")]
    [SerializeField] Slider _sfxSoundControllSlider;
    [SerializeField] Toggle _sfxMuteToggle;
    [SerializeField] Text _sfxSoundValueText;

    AudioSource _bgmSource;
    AudioSource _sfxSource;

    bool _previousBGMMute;
    bool _previousSFXMute;

    float _previousBGMVolum;
    float _previousSFXVolum;

    string _previousBGMVolumText;
    string _previousSFXVolumText;
    void OnEnable()
    {
        PreviousOption();
    }
    void Start()
    {
        InitData();
    }
    void InitData()
    {
        _bgmSource = AudioManager._instance.BGMController;
        _sfxSource = AudioManager._instance.SFXController;
        _bgmMuteToggle.isOn = _sfxMuteToggle.isOn = true;
        _bgmSoundControllSlider.maxValue = _sfxSoundControllSlider.maxValue = 100;
        _bgmSoundControllSlider.value = _sfxSoundControllSlider.value = _sfxSoundControllSlider.maxValue;
    }
    public void BGMController()
    {
        _bgmSource.mute = !_bgmMuteToggle.isOn;
        _bgmSource.volume = _bgmSoundControllSlider.value * 0.01f;
        _bgmSoundValueText.text = ((int)_bgmSoundControllSlider.value).ToString();
    }
    public void SFXController()
    {
        _sfxSource.mute = !_sfxMuteToggle.isOn;
        _sfxSource.volume = _sfxSoundControllSlider.value * 0.01f;
        _sfxSoundValueText.text = ((int)_sfxSoundControllSlider.value).ToString();
    }
    void PreviousOption()
    {
        _previousBGMMute = _bgmMuteToggle.isOn;
        _previousSFXMute = _sfxMuteToggle.isOn;
        _previousBGMVolum = _bgmSoundControllSlider.value;
        _previousSFXVolum = _sfxSoundControllSlider.value;

        _previousBGMVolumText = ((int)_bgmSoundControllSlider.value).ToString(); 
        _previousSFXVolumText = ((int)_sfxSoundControllSlider.value).ToString();

    }
    public void ClickCancelButton()
    {
        _bgmMuteToggle.isOn = _previousBGMMute; 
        _sfxMuteToggle.isOn = _previousSFXMute;
        _bgmSoundControllSlider.value = _previousBGMVolum;
        _sfxSoundControllSlider.value = _previousSFXVolum;
        _previousBGMVolumText = ((int)_bgmSoundControllSlider.value).ToString();
        _previousSFXVolumText = ((int)_sfxSoundControllSlider.value).ToString();
    }
}

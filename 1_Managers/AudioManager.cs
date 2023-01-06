using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager _uniqueInstance;
    public static AudioManager _instance => _uniqueInstance;

    [SerializeField] AudioSource _bgmController;
    [SerializeField] AudioSource _sfxController;

    [SerializeField] AudioClip[] _attackSound;
    [SerializeField] AudioClip[] _skillSound;
    [SerializeField] AudioClip _footStepSound;
    [SerializeField] AudioClip[] _bgms;
    [SerializeField] AudioClip[] _effectSound;


    public AudioSource BGMController => _bgmController;
    public AudioSource SFXController => _sfxController;
    private void Awake()
    {
        if(_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SFXSoundController(AudioClip clip)
    {
        _sfxController.PlayOneShot(clip);
    }
    public void BGMSoundController(DefineEnumHelper.BGMKind clip, bool play = true)
    {
        if(!play)
        {
            _bgmController.Stop();
        }
        _bgmController.loop = true;
        _bgmController.clip = _bgms[(int)clip];
        _bgmController.Play();
    }
    public void  AttackSound(int index)
    {
        _sfxController.PlayOneShot(_attackSound[index]);
    }
    public void FootStepSound()
    {
        _sfxController.PlayOneShot(_footStepSound);
    }
    public void SkillSound(DefineEnumHelper.SkillKind index)
    {
        _sfxController.PlayOneShot(_skillSound[(int)index]);
    }
    public void EffectSound(DefineEnumHelper.SFXSound clip)
    {
        _sfxController.PlayOneShot(_effectSound[(int)clip]);
    }

}

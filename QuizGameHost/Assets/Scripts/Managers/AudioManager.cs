using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
class AudioClipInfo
{
    public AudioClip clip;
    public float volume;
}

class AudioSourceInfo
{
    GameObject go;
    public bool isLooping;
    public AudioSource audioSource;
}

public class AudioManager : SingletonBase<AudioManager>
{
    private AudioSource _audioSource;

    [SerializeField] private GameObject _audioSourceParent;

    [Header("Battle Sounds")]
    [SerializeField] private AudioClip[] _deathSounds;
    [SerializeField] private AudioClip[] _attackSounds;
    [SerializeField] private AudioClip _victorySound;
    [SerializeField] private AudioClip _defeatSound;
    [SerializeField] private AudioClip _fightStartSound;

    [Header("Background Sounds")]
    private AudioSource _backgroundSoundSource;
    [SerializeField] private AudioClipInfo _questionStateSound;
    [SerializeField] private AudioClipInfo _gameBackgroundSound;
    [SerializeField] private AudioClipInfo _battleBackgroundSound;

    [Header("Other Sounds")]
    [SerializeField] private AudioClip _cameraFlySound;

    private void Awake()      
    {
        base.Awake();

        _backgroundSoundSource = gameObject.AddComponent<AudioSource>();
        _backgroundSoundSource.loop = true;
        _backgroundSoundSource.spatialBlend = 0f;
    }

    private AudioSource CreateAudioSourceOnTheFly(Vector3 position, bool twoDimensional)
    {
        GameObject go = new GameObject();
        go.transform.parent = _audioSourceParent.transform;
        go.transform.position = position;
        var audioSource = go.AddComponent<AudioSource>();
        audioSource.spatialBlend = twoDimensional ? 0 : 1f;

        return audioSource;
    }

    private void PlaySoundAtPosition(AudioClip clip, Vector3 position, bool twoDimensional)
    {
        var audioSource = CreateAudioSourceOnTheFly(position, twoDimensional);
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource.gameObject, clip.length);
    }

    private void PlaySound2D(AudioClip clip) 
    {
        PlaySoundAtPosition(clip, Vector3.zero, true);
    }

    private void PlaySound3D(AudioClip clip, Vector3 position)
    {
        PlaySoundAtPosition(clip, position, false);
    }

    private void PlayRandomSound3D(AudioClip[] clips, Vector3 position)
    {
        if (clips.Length == 0)
            return;

        var randomIndex = Random.Range(0, clips.Length);
        PlaySound3D(clips[randomIndex], position);
    }

    private void PlayBackgroundSound(AudioClipInfo clipInfo)
    {
        if (clipInfo.clip == null)
            return;

        _backgroundSoundSource.clip = clipInfo.clip;
        _backgroundSoundSource.volume = clipInfo.volume;
        _backgroundSoundSource.Play();
    }

    private void StopBackgroundSound()
    {
        if (_backgroundSoundSource.clip == null)
            return;

        _backgroundSoundSource.Stop();
        _backgroundSoundSource.clip = null;
    }

    public void PlayRandomDeathSound(Vector3 position)
    {
        PlayRandomSound3D(_deathSounds, position);
    }

    public void PlayRandomAttackSound(Vector3 position)
    {
        PlayRandomSound3D(_attackSounds, position);
    }

    public void PlayQuestionStateSound()
    {
        PlayBackgroundSound(_questionStateSound);
    }

    public void StopQuestionStateSound()
    {
        StopBackgroundSound();
    }

    public void PlayGameBackgroundSound()
    {
        _backgroundSoundSource.volume = 0.5f;
        PlayBackgroundSound(_gameBackgroundSound);
    }

    public void StopGameBackgroundSound()
    {
        StopBackgroundSound();
    }

    public void PlayVictorySound()
    {
        PlaySound2D(_victorySound);
    }

    public void PlayDefeatSound()
    {
        PlaySound2D(_defeatSound);
    }

    public void PlayFightStartSound()
    {
        PlaySound2D(_fightStartSound);
    }

    public void PlayCameraFlySound()
    {
        PlaySound2D(_cameraFlySound);
    }
}

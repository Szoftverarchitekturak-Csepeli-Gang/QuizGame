using Unity.AppUI.UI;
using UnityEngine;

public class AudioManager : SingletonBase<AudioManager>
{
    private AudioSource _audioSource;
    [SerializeField] private GameObject _audioSourceParent;
    [SerializeField] private AudioClip[] _deathSounds;
    [SerializeField] private AudioClip[] _attackSounds;

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

    public void PlayRandomDeathSound(Vector3 position)
    {
        if (_deathSounds.Length == 0)
            return;

        var randomIndex = Random.Range(0, _deathSounds.Length);
        PlaySound3D(_deathSounds[randomIndex], position);
    }

    public void PlayRandomAttackSound(Vector3 position)
    {
        if (_attackSounds.Length == 0)
            return;

        var randomIndex = Random.Range(0, _attackSounds.Length);
        PlaySound3D(_attackSounds[randomIndex], position);
    }
}

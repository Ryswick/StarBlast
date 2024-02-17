using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource _audioSource;

    Transform _ownTransform;
    Transform _playTransform;

    private void Update()
    {
        if(_playTransform != null)
            transform.position = _playTransform.position;
    }

    public void PlayAudio(ClipInformation clipInformation, Transform playTransform, bool isPlayer)
    {
        if (_audioSource == null)
        {
            _ownTransform = transform;
            _audioSource = GetComponent<AudioSource>();
        }

        _playTransform = playTransform;

        AudioClip selectAudioClip = clipInformation.AudioClips[Random.Range(0, clipInformation.AudioClips.Count)];

        _audioSource.clip = selectAudioClip;

        _audioSource.loop = clipInformation.IsLooping;

        _audioSource.priority = clipInformation.Priority;

        _audioSource.volume = clipInformation.Volume;

        if (clipInformation.IsPositioned)
        {
            _audioSource.spatialBlend = 1;
            transform.position = _playTransform.position;

            // If it's not the player, the sound is less important, so we reduce the volume and priority
            if (!isPlayer)
            {
                _audioSource.volume /= 4.0f;
                _audioSource.priority *= 2;
            }
        }
        else
        {
            _audioSource.spatialBlend = 0;
        }

        _audioSource.Play();

        if (!clipInformation.IsLooping)
        {
            Invoke("OnAudioFinished", selectAudioClip.length);
        }
    }

    void OnAudioFinished()
    {
        Stop();
    }

    public void Stop()
    {
        _audioSource.Stop();

        LeanPool.Despawn(gameObject);
    }
}

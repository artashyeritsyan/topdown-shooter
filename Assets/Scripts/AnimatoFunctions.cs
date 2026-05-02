using UnityEngine;

public class AnimatoFunctions : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float audioClipsVolume = 1f;

    public void PlayRandomSounds()
    {
        float randomPitch = UnityEngine.Random.Range(0.8f, 1.2f);

        AudioManager.Play(audioClips[Random.Range(0, audioClips.Length)], audioClipsVolume, randomPitch);
    }
}

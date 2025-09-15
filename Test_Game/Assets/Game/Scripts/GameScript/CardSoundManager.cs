using UnityEngine;

public class CardSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;

    [SerializeField] private AudioSource audioSource;

    public void PlayFlip()
    {
        PlaySound(flipSound);
        Debug.Log("PlayFlip Sound");
    }

    public void PlayMatch()
    {
        PlaySound(matchSound);
        Debug.Log("PlayMatch Sound");
    }

    public void PlayMismatch()
    {
        PlaySound(mismatchSound);
        Debug.Log("PlayMismatch Sound");
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}

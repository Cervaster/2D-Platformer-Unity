using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip move2;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip danho;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attack);
    }

    public void PlayMoveSound()
    {
        audioSource.PlayOneShot(move);
    }

    public void PlayMove2Sound()
    {
        audioSource.PlayOneShot(move2);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jump);
    }

    public void PlayDanhoSound()
    {
        audioSource.PlayOneShot(danho);
    }
}

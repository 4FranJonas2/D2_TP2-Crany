using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip hoverButtonClip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlayHoverButton()
    {
        audioSource.PlayOneShot(hoverButtonClip);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
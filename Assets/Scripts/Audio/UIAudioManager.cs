using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip hoverButtonClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }

    public void PlayHoverButton()
    {
        audioSource.PlayOneShot(hoverButtonClip);
    }
}

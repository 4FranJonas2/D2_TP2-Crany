using UnityEngine;

public class CreditsLinks : MonoBehaviour
{
    public void OpenGitHub()
    {
        URL("https://github.com/4FranJonas2/D2_TP2-Crany");
    }

    public void OpenClawURL()
    {
        URL("https://assetstore.unity.com/packages/3d/vehicles/land/loader-heavy-machinery-31696");
    }

    private void URL(string url)
    {
        Application.OpenURL(url);
    }
}

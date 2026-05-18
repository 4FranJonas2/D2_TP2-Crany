using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text cubesPlacedText;
    [SerializeField] private TMP_Text cubesLostText;
    [SerializeField] private TMP_Text cubesToWinText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timerText;

    private void OnEnable()
    {
        GameManager.OnCubesPlacedChanged += UpdateCubesPlacedText;
        GameManager.OnCubesLostChanged += UpdateCubesLostText;
        GameManager.OnCubesToWinChanged += UpdateCubesToWinText;
        GameManager.OnLevelChanged += UpdateLevelText;
        GameManager.OnTimeChanged += UpdateTimerText;
    }

    private void OnDisable()
    {
        GameManager.OnCubesPlacedChanged -= UpdateCubesPlacedText;
        GameManager.OnCubesLostChanged -= UpdateCubesLostText;
        GameManager.OnCubesToWinChanged -= UpdateCubesToWinText;
        GameManager.OnLevelChanged -= UpdateLevelText;
        GameManager.OnTimeChanged -= UpdateTimerText;
    }

    private void UpdateCubesPlacedText(int cubesPlaced)
    {
        cubesPlacedText.text = $"Cubes Placed: {cubesPlaced}";
    }

    private void UpdateCubesLostText(int cubesLost)
    {
        cubesLostText.text = $"Cubes Lost: {cubesLost}";
    }

    private void UpdateCubesToWinText(int cubesToWin)
    {
        cubesToWinText.text = $"Cubes to Win: {cubesToWin}";
    }

    private void UpdateLevelText(int level)
    {
        levelText.text = $"Level: {level}";
    }

    private void UpdateTimerText(float timer)
    {
        timerText.text = $"Time: {timer:F2}s";
    }
}
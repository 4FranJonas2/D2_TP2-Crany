using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private LevelManager[] levels;
    [SerializeField] private PlayerMove playerSpeed;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalLevelAchieved;

    [Header("Win Menu")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winTimeText;
    [SerializeField] private TMP_Text winScoreText;
    [SerializeField] private TMP_Text winLevelAchieved;

    [Header("UI")]
    [SerializeField] private TMP_Text cubesPlacedText;
    [SerializeField] private TMP_Text cubesToWinText;
    [SerializeField] private TMP_Text cubesLostText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timeText;

    [Header("SFX")]
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] public AudioClip dropSFX;
    [SerializeField] public AudioClip hitCubeSFX;


    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentCube;

    private int currentLevel = 0;
    private int maxLives;

    private float gameTime = 0f;
    public int cubesPlaced = 0;
    private int cubesLost = 0;

    private bool cubeDropped = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadLevel();

        SpawnNextCube();
    }

    private void Update()
    {
        UpdateUI();

        if (currentCube == null) return;

        if (!cubeDropped)
        {
            currentCube.transform.position = spawnPoint.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                sfxSource.PlayOneShot(dropSFX);
                DropCube();
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateUI()
    {
        cubesPlacedText.text = "Cubes placed: " + cubesPlaced;

        cubesToWinText.text = "Cubes to win: " + levels[currentLevel].cubesToWin;

        cubesLostText.text = "Cubes lost: " + cubesLost;

        levelText.text = "Level: " + (currentLevel + 1);

        timeText.text = "Time: " + FormatTime(gameTime) + "s";
    }

    
    //Cube logic
    public void SpawnNextCube()
    {
        cubeDropped = false;

        currentCube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);

        Rigidbody rb = currentCube.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void DropCube()
    {
        cubeDropped = true;

        Rigidbody rb = currentCube.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void CubeLost()
    {
        cubesLost++;

        Debug.Log("Cubos perdidos: " + cubesLost);

        if (cubesLost >= maxLives)
        {
            gameUI.SetActive(false);
            GameOver();
        }
        else
        {
            SpawnNextCube();
            CheckLevelComplete();
        }
    }


    //Win/Lose panels
    private void GameOver()
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Final score: " + cubesPlaced;

        finalTimeText.text = "Time played: " + FormatTime(gameTime) + "s";

        finalLevelAchieved.text = "Level achieved: " + (currentLevel + 1);

        Time.timeScale = 0f;
    }
    
    private void WinGame()
    {
        winPanel.SetActive(true);

        winScoreText.text = "Final score: " + cubesPlaced;

        winTimeText.text = "Time played: " + FormatTime(gameTime) + "s";

        winLevelAchieved.text = "Level achieved: " + (currentLevel + 1);

        Time.timeScale = 0f;
    }

    //Buttons logic for Pause/Win/Lose panels
    public void RestartGame()
    {
        cubesPlaced = 0;
        cubesLost = 0;
        gameTime = 0f;

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameUI.SetActive(true);

        ClearLevel();

        LoadLevel();

        SpawnNextCube();
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;

        currentLevel = 0;

        RestartGame();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Salir del juego");
    }

    
    //levelsSTuff
    private void LoadLevel()
    {
        cubesLost = 0;
        cubesPlaced = 0;

        LevelManager level = levels[currentLevel];

        maxLives = level.maxLives;

        playerSpeed.SetLevelSpeed(level.playerSpeed);

        LevelManager nextLevel = levels[currentLevel];
    }

    public void CheckLevelComplete()
    {
        LevelManager level = levels[currentLevel];

        if ((cubesPlaced == level.cubesToWin))
        {
            WinGame();
        }
    }

    private void ClearLevel()
    {
        Cube[] cubes = FindObjectsByType<Cube>(FindObjectsSortMode.None);

        foreach (Cube cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        currentLevel++;

        if (currentLevel >= levels.Length)
        {
           currentLevel = levels.Length - 1;

            Debug.Log("Game Complete!!");

            return;
        }

        RestartGame();
    }
}
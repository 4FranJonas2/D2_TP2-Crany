using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static event Action<int> OnCubesPlacedChanged;
    public static event Action<int> OnCubesLostChanged;
    public static event Action<int> OnCubesToWinChanged;
    public static event Action<float> OnTimeChanged;
    public static event Action<int> OnLevelChanged;

    [SerializeField] private LevelManager[] levels;
    [SerializeField] private PlayerMove playerSpeed;

    [Header("Tower Progression")]
    [SerializeField] private Transform gamePlayBase;
    [SerializeField] private float cubeHeight;
    [SerializeField] private int cubesBeforeBaseMove;

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
    
    private Vector3 initialBasePosition;

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
        initialBasePosition = gamePlayBase.position;

        LoadLevel();

        SpawnNextCube();
    }

    private void Update()
    {
        AddTime(Time.deltaTime);
        SetLevel(currentLevel);
        SetCubesToWin(levels[currentLevel].cubesToWin);

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

    //Tower progression logic
    private void CheckTowerHeight()
    {
        if(cubesPlaced <= cubesBeforeBaseMove) return;

        float targetHeight = (cubesPlaced - cubesBeforeBaseMove) * cubeHeight;

        gamePlayBase.position = initialBasePosition  + Vector3.up * targetHeight;
    }

    private void ResetTowerHeight()
    {
        gamePlayBase.position = initialBasePosition;
    }   

    //HUD Management
    public void AddPlacedCube()
    {
        cubesPlaced++;

        CheckTowerHeight();

        OnCubesPlacedChanged?.Invoke(cubesPlaced);

        CheckLevelComplete();
    }

    public void AddLostCube()
    {
        cubesLost++;
        OnCubesLostChanged?.Invoke(cubesLost);
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

    public void AddTime(float time)
    {
        gameTime += time;
        OnTimeChanged?.Invoke(gameTime);

        CheckLevelComplete();
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        OnLevelChanged?.Invoke(currentLevel);
    }

    public void SetCubesToWin(int cubesToWin)
    {
        OnCubesToWinChanged?.Invoke(cubesToWin);
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

    //Win/Lose panels
    private void GameOver()
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Final score: " + cubesPlaced;

        finalTimeText.text = "Time played: " + gameTime + "s";

        finalLevelAchieved.text = "Level achieved: " + (currentLevel + 1);

        Time.timeScale = 0f;
    }

    private void WinGame()
    {
        winPanel.SetActive(true);

        winScoreText.text = "Final score: " + cubesPlaced;

        winTimeText.text = "Time played: " + gameTime + "s";

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

        ResetTowerHeight();

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
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

    [Header("UI")]
    [SerializeField] private TMP_Text cubesPlacedText;
    [SerializeField] private TMP_Text cubesToWinText;
    [SerializeField] private TMP_Text cubesLostText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timeText;


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
        UiTimeLogic();

        if (currentCube == null) return;

        if (!cubeDropped)
        {
            currentCube.transform.position = spawnPoint.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropCube();
            }
        }
    }

   
    //UI logic
    private void UiTimeLogic()
    {
        gameTime += Time.deltaTime;

        UpdateUI();
    }

    private void UpdateUI()
    {
        cubesPlacedText.text = "Cubes placed: " + cubesPlaced;

        cubesToWinText.text = "Cubes to win: " + levels[currentLevel].cubesToWin;

        cubesLostText.text = "Cubes lost: " + cubesLost;

        levelText.text = "Level: " + (currentLevel + 1);

        timeText.text = "Time: " + Mathf.FloorToInt(gameTime) + "s";
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

    
    //Buttons logic
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Final score: " + cubesPlaced;

        finalTimeText.text = "Time played: " + Mathf.FloorToInt(gameTime) + "s";

        finalLevelAchieved.text = "Level achieved: " + (currentLevel + 1);

        Time.timeScale = 0f;
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;

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

        if ((cubesPlaced >= level.cubesToWin))
        {
            NextLevel();
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

    private void NextLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Length)
        {
           currentLevel = levels.Length - 1;

            Debug.Log("Game Complete!!");

            return;
        }

        LoadLevel();

        SpawnNextCube();
    }
}
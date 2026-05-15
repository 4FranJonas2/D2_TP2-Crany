using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentCube;

    public int cubesPlaced = 0;
    private int cubesLost = 0;  

    private bool cubeDropped = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnNextCube();
    }

    private void Update()
    {
        if (currentCube == null) return;

        if(!cubeDropped)
        {
            currentCube.transform.position = spawnPoint.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropCube();
            }
        }
    }

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

        if (cubesLost > 3)
        {
            RestartGame();
        }
        else
        {
            SpawnNextCube();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
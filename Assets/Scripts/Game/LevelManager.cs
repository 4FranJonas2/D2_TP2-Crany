using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LevelManager")]
public class LevelManager : ScriptableObject
{
    [Header("Level info")]
    public int level;

    [Header("Level settings")]
    public int maxLives = 3;
    public int cubesToWin = 10;
    public float playerSpeed = 5f;
}

using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movimiento")]
    public float levelSpeed;

    [Header("Limites")]
    public float leftLimit = -5.0f;
    public float rightLimit = 6.0f;

    private int direction = 1;

    // Update is called once per frame
    void Update()
    {
        PlayerMovemnt();
    }

    private void PlayerMovemnt()
    {
        transform.position += Vector3.right * direction * levelSpeed * Time.deltaTime;

        if (transform.position.x >= rightLimit)
        {
            direction = -1;
        }
        else if (transform.position.x <= leftLimit)
        {
            direction = 1;
        }
    }

    public void SetLevelSpeed(float newSpeed)
    {
        levelSpeed = newSpeed;
    }
}

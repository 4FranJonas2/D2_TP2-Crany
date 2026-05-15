using System.Runtime.CompilerServices;
using UnityEditor.Build.Content;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody rb;

    private bool score = false;
    private bool lost = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // PERDER
        if (!lost && collision.gameObject.CompareTag("Ground"))
        {
            lost = true;

            GameManager.instance.CubeLost();

            GameManager.instance.cubesLost++;

            return;
        }

        // PRIMER CUBO
        if (!score && GameManager.instance.cubesPlaced == 0 && collision.gameObject.CompareTag("Ground"))
        {
            score = true;

            GameManager.instance.cubesPlaced++;

            StartCoroutine(WaitUntilStable());
        }

        // RESTO DE CUBOS
        else if (!score && GameManager.instance.cubesPlaced > 0 && collision.gameObject.CompareTag("Cube"))
        {
            score = true;

            GameManager.instance.cubesPlaced++;

            StartCoroutine(WaitUntilStable());
        }
    }

    private System.Collections.IEnumerator WaitUntilStable()
    {
        yield return new WaitForSeconds(1.5f);

        GameManager.instance.SpawnNextCube();
    }

    //private void CubeSecuence()
    //{
    //    landed = true;

    //    GameManager.instance.cubesPlaced++;

    //    StartCoroutine(WaitUntilStable());
    //}
}
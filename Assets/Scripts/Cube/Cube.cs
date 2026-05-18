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
        // PRIMER CUBO
        if (!score && GameManager.instance.cubesPlaced == 0 && collision.gameObject.CompareTag("Ground"))
        {
            score = true;

            PlaySFXSound();

            GameManager.instance.AddPlacedCube();

            StartCoroutine(WaitUntilStable());
        }

        // PERDER
        else if (!lost && GameManager.instance.cubesPlaced > 0 && collision.gameObject.CompareTag("Ground"))
        {
            lost = true;

            GameManager.instance.AddLostCube();

            PlaySFXSound();

            return;
        }


        // RESTO DE CUBOS
        else if (!score && GameManager.instance.cubesPlaced > 0 && collision.gameObject.CompareTag("Cube"))
        {
            score = true;

            PlaySFXSound();

            GameManager.instance.AddPlacedCube();

            StartCoroutine(WaitUntilStable());
        }
    }

    private System.Collections.IEnumerator WaitUntilStable()
    {
        yield return new WaitForSeconds(1.5f);

        GameManager.instance.SpawnNextCube();
    }

    private void PlaySFXSound()
    {
        if (GameManager.instance.sfxSource != null && GameManager.instance.dropSFX != null)
        {
            GameManager.instance.sfxSource.PlayOneShot(GameManager.instance.hitCubeSFX);
        }
    }
}
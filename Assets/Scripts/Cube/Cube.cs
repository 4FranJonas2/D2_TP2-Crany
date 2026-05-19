using System.Runtime.CompilerServices;
using UnityEditor.Build.Content;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody rb;

    private bool score = false;
    private bool lost = false;
    private bool finalized = false;

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

            StartCoroutine(WaitUntilStable());
        }

        // PERDER
        else if (!lost && !finalized && GameManager.instance.cubesPlaced > 0 && collision.gameObject.CompareTag("Ground"))
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

            StartCoroutine(WaitUntilStable());
        }
    }

    private System.Collections.IEnumerator WaitUntilStable()
    {
        yield return new WaitForSeconds(1.5f);

        if (lost || finalized)
            yield break;

        finalized = true;

        GameManager.instance.AddPlacedCube();
    }

    private void PlaySFXSound()
    {
        if (GameManager.instance.sfxSource != null && GameManager.instance.dropSFX != null)
        {
            GameManager.instance.sfxSource.PlayOneShot(GameManager.instance.hitCubeSFX);
        }
    }
}
using UnityEngine;

public class Hittable : MonoBehaviour
{

    private AudioSource audioSource;

    void Start()
    {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Hit()
    {
        audioSource.Play(); // Play sound without interrupting others
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private AudioSource raccoonAudio;
    public AudioClip stepSFX;

    // Start is called before the first frame update
    void Start()
    {
        raccoonAudio = GetComponent<AudioSource>();
    }

    void step()
    {
        raccoonAudio.pitch = (Random.Range(0.7f, 1.15f));
        raccoonAudio.PlayOneShot(stepSFX, 0.1F);
    }
}

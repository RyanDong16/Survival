using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SinkingPlatform : MonoBehaviour
{
    public float speed = 2f;

    private Vector3 originPos;
    public bool triggered = false;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            // sinking downwards
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else
        {
            // rising upwards to original position
            transform.position = Vector3.MoveTowards(
                transform.position,
                originPos,
                speed * Time.deltaTime
            );
        }
    }

    // Player steps on platform
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Play sink sound
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    // Player steps off platform
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SinkingPlatform : MonoBehaviour
{
    public float sinkDistance = 1.5f;
    public float speed = 2f;

    private Vector3 originPos;
    private Vector3 sinkPos;
    private bool triggered = false;

    private AudioSource audioSource;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        originPos = transform.position;
        sinkPos = originPos + Vector3.down * sinkDistance;
    }

    // Update is called once per frame; wheen the player is colliding with platform
    void Update()
    {
        if (triggered)
        {
            // Smoothly sink to target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                sinkPos,
                speed * Time.deltaTime
            );
        }
        else
        {
            // Smoothly return to original position
            transform.position = Vector3.MoveTowards(
                transform.position,
                originPos,
                speed * Time.deltaTime
            );
        }
    }

    // Collision activation
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    // Collision deactivation
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
            audioSource.Stop();
        }
    }
}

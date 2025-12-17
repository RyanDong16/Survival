using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    public float speed;
    private Vector3 originPos;

    public bool triggered = false;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame; wheen the player is colliding with platform
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
            transform.position = Vector3.MoveTowards(transform.position, originPos, speed * Time.deltaTime);
        }
    }

    // Collision activation
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Play sink sound
            if (!audioSource.isPlaying)
                audioSource.Play();

            other.transform.SetParent(transform);
        }
    }

    // Collision deactivation
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
            other.transform.SetParent(null);
        }
    }
}

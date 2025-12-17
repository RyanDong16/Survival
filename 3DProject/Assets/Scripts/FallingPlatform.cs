using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    bool isFalling = false;
    float downSpeed = 0;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Trigger with player steps on platform
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
            isFalling = true;

            // Play falling sound
            audioSource.Play();
            
            Destroy(gameObject, 1);
    }

    // Changes y axis when colliding with
    void Update()
    {
        if (isFalling)
        {
            downSpeed += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, transform.position.z);
        }
    }
}

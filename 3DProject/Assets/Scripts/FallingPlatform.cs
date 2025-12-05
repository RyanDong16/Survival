using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    bool isFalling = false;
    float downSpeed = 0;

    // Trigger with player steps on platform
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
            isFalling = true;
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

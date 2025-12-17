using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] string playerTag;
    [SerializeField] float bounceForce;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // player collides with mushroom object
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == playerTag)
        {
            Rigidbody otherRB = collision.rigidbody;

            otherRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
        }
        // Play bounce sound
        audioSource.Play();
    }
    

}

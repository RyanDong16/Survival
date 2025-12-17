using UnityEngine;

[RequireComponent(typeof(Animation))]
public class TreasureVictory : MonoBehaviour
{
    private Animation anim;
    private bool hasPlayed = false;

    void Awake()
    {
        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasPlayed)
        {
            anim.Play();
            hasPlayed = true;
        }
    }
}

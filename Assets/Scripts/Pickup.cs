using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AffinityType affinity;
    public float rotationSpeed = 100;
    public Vector3 rotationDirection = Vector3.forward;
    public AudioClip pickupSound;

    Transform rotatingGameObject;

    private void Awake()
    {
        rotatingGameObject = transform.Find(affinity.ToString());
    }

    void FixedUpdate()
    {
        rotatingGameObject.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerBehavior player = other.GetComponent<PlayerBehavior>();
            player.SetupAffinity(affinity);
            player.PlayAudio(pickupSound, 3);
            Destroy(gameObject);
        }
    }
}

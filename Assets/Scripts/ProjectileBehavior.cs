using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float moveSpeed = 14;
    public float rotateSpeed = 8;
    public AudioClip bloodSound;
    public AudioClip impactSound;

    // Who shot the projectile
    GameObject owner;
    AffinityType affinity;
    Vector3 rotateDirection;
    Vector3 moveDirection;
    GameObject bloodEffect;
    GameObject deathEffect;
    AudioSource audioSource;

    string particlePrefabPath = "Particles/";

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bloodEffect = Resources.Load(particlePrefabPath + "Blood Hit") as GameObject;
    }

    public void Setup(GameObject parent, AffinityType _affinity, Vector3 _moveDirection)
    {
        owner = parent;
        affinity = _affinity;
        moveDirection = _moveDirection;
        deathEffect = Resources.Load(particlePrefabPath + "Projectile Death") as GameObject;
        // Paper model rotates weird;
        if (_affinity == AffinityType.Paper1)
        {
            rotateDirection = Vector3.back;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            rotateDirection = Vector3.right;
        }
    }

    void Update()
    {
        transform.Rotate(rotateDirection * rotateSpeed);
        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other || !owner) return;
        if (other.tag == "Player" && other.name != owner.name)
        {
            ExecuteDamage(other);
            Destroy(gameObject);
        }
    }

    void ExecuteDamage(Collider other)
    {
        PlayerBehavior player = other.GetComponent<PlayerBehavior>();
        int dmg = player.TakeDamage(affinity);
        if (dmg > 0)
        {
            // Blood
            GameObject _bloodEffect = Instantiate(bloodEffect, transform.position, transform.rotation);
            _bloodEffect.transform.LookAt(Camera.main.transform, transform.up);
            Destroy(_bloodEffect, 2);

            player.PlayAudio(bloodSound, 1f);
        }

        // Impact
        GameObject impactEffect = Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(impactEffect, 2);

        player.PlayAudio(impactSound, 1.4f);
    }
}

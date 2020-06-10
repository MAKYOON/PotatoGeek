using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic_Projectile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] ParticleSystem explosion;
    Rigidbody projectileRb;


    float speed = 350.0f;
    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        projectileRb.velocity = transform.forward * speed;

    }

    private void OnTriggerEnter(Collider other)
    {
        explosion.Play();
        Destroy(gameObject,0.5f);
    }
}

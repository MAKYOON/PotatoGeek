using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic_Projectile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] ParticleSystem explosion;
    Rigidbody projectileRb;


    float speed = 100.0f;
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

    IEnumerator Hit()
    {
        Debug.Log("in hit");
        yield return new WaitForSeconds(0.5f);
        explosion.Play();
        if (explosion.isPlaying)
            Debug.Log("playing animation");
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Hit());
       // Destroy(gameObject);
    }
}

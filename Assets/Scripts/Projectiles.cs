using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class Projectiles : MonoBehaviour
{
	private Rigidbody enemyRigidbody;
	private float velocity = 100.0f;

	private float destroyDelay = 3.0f;
	
    void Start()
    {
		enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        StartCoroutine(DisableProjectile());
    }
    private void FixedUpdate()
    {
        enemyRigidbody.velocity = transform.forward * velocity;
    }
    IEnumerator DisableProjectile()
    {
        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player touched !");
            gameObject.SetActive(false);
		}
	}
}

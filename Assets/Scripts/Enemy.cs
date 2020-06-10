using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	//waiting for kevin give a name to the game object player private Transform player;
	private Transform player;
	private UnityEngine.AI.NavMeshAgent enemyNavMeshAgent;
	private Animator enemyAnimator;

	public GameObject projectilesPrefab;
	public Transform projectileSpawner;

	public float detectionRange = 60.0f;
	public float attackRange = 200.0f;
	public float distanceFromPlayer;

	private float shotCooldown = 1.0f;
	private float timeSinceLastShot = 0f;

	void Start()
	{
		player = GameObject.Find("Player_v2").GetComponent<Transform>();
		enemyAnimator = GetComponent<Animator>();

	}

	void Update()
	{
		distanceFromPlayer = Vector3.Distance(transform.position, player.position);


		if (distanceFromPlayer <= attackRange)
		{
			enemyAnimator.SetBool("isAttacking", true);

			Shoot();
		}

	}

	void Shoot()
	//Instanciate a projectile
	{
		transform.LookAt(player.position);
        projectileSpawner.LookAt(player.position);
		if (timeSinceLastShot >= shotCooldown)
		{
			Instantiate(projectilesPrefab, projectileSpawner.position, projectileSpawner.transform.rotation);
			timeSinceLastShot = 0;
		}
		else
			timeSinceLastShot += Time.deltaTime;
	}
}

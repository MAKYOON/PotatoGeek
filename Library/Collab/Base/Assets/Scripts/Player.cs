using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] int amountToPool;
    [SerializeField] float jumpHeight;
    [SerializeField] float rotateSpeed = 5;
    [SerializeField] float speed;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Image[] images;



    Rigidbody playerRb;
    Animator playerAnimator;
    List<GameObject> pooledObjects = new List<GameObject>();

    float maxSpeed = 25.0f;
    float timer = 0;
    float timeBetweenShots = 0.1f;
    bool isGrounded;
    bool canJump;
    int powerUpSpeedLevel = 0;
    bool[] powerUpActivated = new bool[4];

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        Cursor.visible = false;
        CreatePoolOfProjectile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGrounded && transform.position.y > collision.transform.position.y)
            isGrounded = true;
    }
	
    void Update()
    {
        timer += Time.deltaTime;

        //float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        //transform.Rotate(0, horizontal, 0);
		
		//Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (Input.GetKeyDown(KeyCode.Q))
            transform.rotation = Quaternion.Euler(0, -90, 0);
        else if (Input.GetKey(KeyCode.Z))
        {
            transform.rotation = new Quaternion(0,Camera.main.transform.rotation.y,0,transform.rotation.w);
        }
        else if (Input.GetKeyDown(KeyCode.S))
            transform.rotation = Quaternion.Euler(0, -180, 0);

        //Fire input
        if (Input.GetMouseButton(0))
        {
            if (timer >= timeBetweenShots)
            {
                FireProjectile();
            }
        }

		//Increase speed
        if (Input.GetKeyDown(KeyCode.O) && powerUpSpeedLevel < 3)
        {
            powerUpSpeedLevel++;
            maxSpeed += 7.5f;
			playerAnimator.speed += playerAnimator.speed*0.2f;

		}
           
		//
        if (powerUpSpeedLevel >= 0)
        {
            powerUpActivated[powerUpSpeedLevel] = true;

            for (int i = 0; i < images.Length; i++)
            {
                if (powerUpActivated[i])
                    images[i].gameObject.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            playerRb.AddForce(transform.forward * speed, ForceMode.VelocityChange);



        if (verticalInput > 0 || horizontalInput != 0)
            playerAnimator.SetBool("isRunningForward", true);
        else if (verticalInput < 0)
            playerAnimator.SetBool("isRunningBackward", true);
        else
        {
            playerAnimator.SetBool("isRunningForward", false);
            playerAnimator.SetBool("isRunningBackward", false);
        }

        playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed);
        if (isGrounded && canJump)
        {
            playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
            canJump = false;
        }
    }

    void CreatePoolOfProjectile()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.SetParent(GameObject.Find("ProjectileStock").transform);
            projectile.SetActive(false);
            pooledObjects.Add(projectile);
        }
    }

    //returns the GameObject that we are going to set active to display
    private GameObject GetPooledProjectile()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    void FireProjectile()
    {
        timer = 0;
        GameObject projectile = GetPooledProjectile();
        if (projectile != null)
        {
            projectile.transform.position = projectileSpawn.position;
            projectile.transform.rotation = projectileSpawn.rotation;
            projectile.SetActive(true);
        }
    }
}

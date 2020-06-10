using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable 0649
public class Player : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField] int amountToPool; //amount of projectile to create object pooling
    [SerializeField] float jumpHeight; //jump force of robot
    [SerializeField] float speed; //speed of robot
    [SerializeField] Transform projectileSpawn; //where the projectile will spawn
    [SerializeField] GameObject projectilePrefab; //prefab of the projectile
    [SerializeField] Image[] imageSpeed; //graphical display of the speed level
    [SerializeField] Image[] imageJump; //graphical display of the jump level
    [SerializeField] Image displayStats; //when the user presses TAB, it displays all of his current upgrades
    [SerializeField] Image story; //to display the story between cinematic and level

    //sounds
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip repairSound;
     
    Rigidbody playerRb;
    Animator playerAnimator;
    List<GameObject> pooledObjects = new List<GameObject>();
    AudioSource audioSource;

    float maxSpeed = 25.0f;
    float timer = 0;
    float timeBetweenShots = 0.1f;
    bool isGrounded;
    bool canJump;
    int powerUpSpeedLevel = 0; //level of current speed upgrade
    bool[] speedActivated = new bool[4]; //table of bool for speed => each time we increment the speed level, speedActivated[powerUpSpeedLevel] becomes true
    int powerUpJumpLevel = 0; //level of current jump upgrade
    bool[] jumpActivated = new bool[4]; //table of bool for jump => each time we increment the jump level, jumpActivated[powerUpJumpLevel] becomes true

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        Cursor.visible = false;
        CreatePoolOfProjectile();
        StartCoroutine(DisplayStory());
        InvokeRepeating("PlayWalkSound", 0.0f, 0.5f);
    }
    IEnumerator DisplayStory()
    {
        yield return new WaitForSeconds(12.0f);
        story.gameObject.SetActive(false);

    }
    private void OnCollisionEnter(Collision collision)
    {   
        //handle jump
        if (collision.gameObject.CompareTag("Ground") && !isGrounded && transform.position.y > collision.transform.position.y)
            isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if we collect a speed power up, increase speed and animation
        if (other.gameObject.CompareTag("PowerUpSpeed"))
        {
            audioSource.PlayOneShot(repairSound, 1.0f);
            powerUpSpeedLevel++;
            maxSpeed += 7.5f;
            playerAnimator.speed += playerAnimator.speed * 0.2f;
            Destroy(other.gameObject);
        }
        //do the same for jump collectible
        if (other.gameObject.CompareTag("PowerUpJump"))
        {
            audioSource.PlayOneShot(repairSound, 1.0f);
            powerUpJumpLevel++;
            jumpHeight *= 1.5f;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("PowerUpShoot"))
        {
            audioSource.PlayOneShot(repairSound, 1.0f);
            projectileSpawn = gameObject.transform.Find("UpgradeShootSpawn").transform;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "EndLevel")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    void RotateCharacter()
    {
        //Bidirectional
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Z))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y - 45, 0);
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Z))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + 45, 0);
        else if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.S))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + 225, 0);
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + 135, 0);
        //mono
        else if (Input.GetKey(KeyCode.D))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + 90, 0);
        else if (Input.GetKey(KeyCode.Q))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y - 90, 0);
        else if (Input.GetKey(KeyCode.Z))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        else if (Input.GetKey(KeyCode.S))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y - 180, 0);
    }

    void Update()
    {
        //the timer is used to make sure the player cant spam the shots
        timer += Time.deltaTime;

		//Jump input
;        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            canJump = true;
        }

        RotateCharacter();
		
		//Fire input
		if (Input.GetMouseButton(0))
        {
            if (timer >= timeBetweenShots)
            {
                FireProjectile();
                audioSource.PlayOneShot(shootSound, 1.0f);
            }
        }

        //display stats
        if (Input.GetKey(KeyCode.Tab))
            DisplayUpgrade();
        else
            displayStats.gameObject.SetActive(false);
    }

    void DisplayUpgrade()
    {
        displayStats.gameObject.SetActive(true);
        //handle display of speed according to the level
        if (powerUpSpeedLevel >= 0)
        {
            //we set the according index of the table of bool to true
            speedActivated[powerUpSpeedLevel] = true;
            //then we loop through the images, and if the bool is true, then we display an additional bar
            for (int i = 0; i < imageSpeed.Length; i++)
            {
                if (speedActivated[i])
                    imageSpeed[i].gameObject.SetActive(true);
            }
        }

        //handle display of jump according to the level, basically the same function for the speed 
        if (powerUpJumpLevel >= 0)
        {
            jumpActivated[powerUpJumpLevel] = true;

            for (int i = 0; i < imageJump.Length; i++)
            {
                if (jumpActivated[i])
                    imageJump[i].gameObject.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        //make sure player moves in local forward
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            playerRb.AddForce(transform.forward * speed, ForceMode.VelocityChange);

        //animation
        if (verticalInput != 0 || horizontalInput != 0)
        {
            playerAnimator.SetBool("isRunningForward", true);
        }
        else
            playerAnimator.SetBool("isRunningForward", false);

        //this is to limit the player's speed (doesnt go higher than maxSpeed, more or less)
        playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed);

        //jump if we are on the ground
        if (isGrounded && canJump)
        {
            playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
            canJump = false;
            audioSource.PlayOneShot(jumpSound, 1.0f);
        }

    }

    void PlayWalkSound()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 && isGrounded)
        {
            audioSource.PlayOneShot(walkSound, 1.0f);
        }
    }

    //create pool of projectile at the beginning of the game
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

    //get a projectile from the pool and sets it active
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

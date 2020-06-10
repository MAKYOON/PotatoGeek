using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cinematic_Player : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] Transform targetLoot;
    [SerializeField] Image text1;
    [SerializeField] Image text2;
    [SerializeField] Image text3;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip deathSound;
    Animator animator;
    bool hasToMove;
    float speed = 150.0f;
    float offSet;
    bool isRunningCoroutine;
    bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCinematic());
        animator = GetComponent<Animator>();
        offSet = transform.position.z - Camera.main.transform.position.z;
    }

    IEnumerator StartCinematic()
    { 
        yield return new WaitForSeconds(5.0f);
        text1.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.5f);
        animator.SetBool("isAttacking", true);
        audioSource.PlayOneShot(shootSound, 1.0f);
        Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        projectilePrefab.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(4.0f);
        
        text2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        text2.gameObject.SetActive(false);
        hasToMove = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasToMove && !isDead)
        {
            transform.LookAt(targetLoot);
            transform.Translate(transform.forward*speed*Time.deltaTime);
            animator.SetBool("isRunning", true);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z - offSet);
        }

        if (transform.position.z > 375)
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -7.0f,3.5f), ForceMode.VelocityChange);

        if (transform.position.y < -70 && !isRunningCoroutine)
        {
            StartCoroutine(Death());
            isDead = true;
        }

        
    }

    IEnumerator Death()
    {
        isRunningCoroutine = true;
        text3.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        text3.gameObject.SetActive(false);
        explosion.Play();
        audioSource.PlayOneShot(deathSound, 1.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("PlateformLVL1");
    }

}

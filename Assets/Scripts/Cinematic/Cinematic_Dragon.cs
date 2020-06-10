using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cinematic_Dragon : MonoBehaviour
{
    [SerializeField] ParticleSystem[] flames;
    [SerializeField] GameObject loot;
    [SerializeField] AudioClip spitFlame;
    [SerializeField] AudioClip explosionDeath;
    Animator animator;
    [SerializeField] AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayCinematic());
        animator = GetComponent<Animator>();
    }

    IEnumerator PlayCinematic()
    {
        yield return new WaitForSeconds(6.0f);
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].Play();
        }
        yield return new WaitForSeconds(0.1f);
        audioSource.PlayOneShot(spitFlame, 1.5f);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(2.0f);
        audioSource.PlayOneShot(explosionDeath, 2.0f);
        animator.SetBool("isDead", true);
        GetComponent<Rigidbody>().AddForce(Vector3.down);
        GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
        loot.SetActive(true);
        Physics.gravity += new Vector3(0, 3, 0);



    }

    // Update is called once per frame
    void Update()
    {

    }


}

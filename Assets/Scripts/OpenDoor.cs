using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (door) { 
            if (door.transform.position.y - door.GetComponent<MovePlateform>().maxRange.y < 0)
            {
                Destroy(door);
            }
        }

        if (isOpen)
        {
            transform.localScale = new Vector3(0, 0, 0);
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((collision.gameObject.name == "Player_v2" && collision.transform.position.y > transform.position.y)||(collision.gameObject.name == "Projectile(Clone)")) && !isOpen)
        {
            isOpen = true;
            door.GetComponent<MovePlateform>().enabled = true;
        }
    }
}

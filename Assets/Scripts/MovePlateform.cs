using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class MovePlateform : MonoBehaviour
{
    public float speed;
    float startTime;
    float journeyLength;

    public Vector3 maxRange;
    Vector3 initialPos;
    Vector3 maxPos;
    

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        maxPos = initialPos + maxRange;
        startTime = Time.time;
        journeyLength = Vector3.Distance(initialPos,maxPos);
    }

    // Update is called once per frame
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        
        transform.position = Vector3.Lerp(initialPos, maxPos, fractionOfJourney);

        if (transform.position == maxPos)
        {
            maxRange *= -1;
            Start();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player_v2")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Player_v2")
        {
            collision.transform.SetParent(transform.parent.parent);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowThing : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0, 0.5f,0);
        transform.position += new Vector3(0, 0.25f, 0);
	}
}

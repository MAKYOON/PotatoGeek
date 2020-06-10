using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowThing : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
		transform.localScale = transform.localScale + Vector3.up * 0.1f;
		transform.position = transform.position + Vector3.up*0.1f;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowThing : MonoBehaviour
{
	public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
		transform.localScale = transform.localScale + Vector3.up * speed * Time.deltaTime;
		transform.position = transform.position + Vector3.up* speed * Time.deltaTime;
	}
}

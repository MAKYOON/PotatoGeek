using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class CameraController : MonoBehaviour
{
	public Vector3 offset;
	public Transform player;

	private float mouseXSensitivity = 5.0f;
	private float mouseYSensitivity = 1f;
	private float clampAngle = 20.0f; //How high / low the player is able to look

	private float mouseX;
	private float mouseY;

	private float height;
	private float distance = 15.0f;

    void Awake()
    {
		//Sets the offset of the camera. Y a little uper and Z relative distance we want between player and camera.
		offset = new Vector3(0, player.position.y, distance);
	}
	
    void LateUpdate()
    {
		//Vertical axis
		mouseY = -Input.GetAxis("Mouse Y") * mouseYSensitivity;
		height = offset.y + mouseY;

		//Prevents the player from looking too higher/lower
		if (height > clampAngle) height = clampAngle;
		else if (height < -clampAngle) height = -clampAngle;
		
		//Horizontal axis
		mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity; //Stores the input
		offset = Quaternion.AngleAxis(mouseX, Vector3.up) * offset; //Makes the camera turns around the Y axis

		//Apply the input (vertical and horizontal) to the camera rotation
		offset.Set(offset.x, height, offset.z); //Used to refresh the height
		transform.position = player.position + offset; //Place the camera
		transform.LookAt(player.position); //Rotate the camera
	}

	private void Update()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
}

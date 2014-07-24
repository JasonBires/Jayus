﻿using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
	public float speed;
	public float rotationSpeed;
	
	private float xRotation;
	private float yRotation;

	private TimeController timeControl;

	public AudioSource forwardMove;
	public AudioSource timeReverse;

	// Use this for initialization
	void Start () {
		xRotation = 0;
		yRotation = 0;
		timeControl = new TimeController ();
	}

	/* Update is called once per frame
	 * If T is pressed and timeControl's backing array isn't spent, rewind time
	 * Else change the object as normal and record the object's rotation and translation in timeControl
	 */
	void Update () {
		if (Input.GetKey (KeyCode.T) && !timeControl.isEmpty()) {
			Vector3[] temp = timeControl.readOff();
			transform.Translate (-temp[0]);
			transform.localEulerAngles = temp[1];
			if (!timeReverse.isPlaying) {
				timeReverse.Play();
				forwardMove.Stop();
			}
		} else {
			if (!forwardMove.isPlaying) {
				timeReverse.Stop();
				forwardMove.Play();
			}
			float translationZ = Input.GetAxis ("Vertical") * speed;
			float translationX = Input.GetAxis ("Horizontal") * speed;
			translationZ *= Time.deltaTime;
			translationX *= Time.deltaTime;
			Vector3 translateBy = new Vector3(translationX, 0, translationZ);
			transform.Translate (translationX, 0, translationZ);

			yRotation -= Input.GetAxis ("Mouse Y") * rotationSpeed * Time.deltaTime;
			yRotation = Mathf.Clamp (yRotation, -80, 80);
			xRotation += Input.GetAxis ("Mouse X") * rotationSpeed * Time.deltaTime;
			xRotation = xRotation % 360;
			Vector3 rotateBy = new Vector3(yRotation, xRotation, 0);
			transform.localEulerAngles = new Vector3 (yRotation, xRotation, 0);

			timeControl.addFrame (translateBy, rotateBy);
		}
	}
}

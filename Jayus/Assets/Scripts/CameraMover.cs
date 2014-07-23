using UnityEngine;
using System.Collections;

public class TimeController {
	private Vector3[] posStorage;
	private Vector3[] posStorageBuffer;
	private Vector3[] rotStorage;
	private Vector3[] rotStorageBuffer;
	private bool hasBeenDumped = false;

	private int size = 0;
	private int storeSize = 1599;
	private const int MAX_SIZE = 1600;

	public TimeController() {
		posStorageBuffer = new Vector3[1600];
		rotStorageBuffer = new Vector3[1600];
	}

	private bool isFull() {
		return size == MAX_SIZE;
	}

	public bool hasGottenDumped() {
		return hasBeenDumped;
	}

	private void bufferDump() {
		posStorage = posStorageBuffer;
		rotStorage = rotStorageBuffer;
		hasBeenDumped = true;
		size = 0;
		storeSize = 1599;
	}

	public void addFrame(Vector3 pos, Vector3 rot) {
		posStorageBuffer [size] = pos;
		rotStorageBuffer [size] = rot;
		size ++;
		if (isFull ()) {
			bufferDump ();
		}
	}

	public Vector3[] readOff() {
		Vector3[] posRotArray = new Vector3[2];
		if (hasBeenDumped) {
			if(size != 0) {
				posRotArray[0] = posStorageBuffer[size - 1];
				posRotArray[1] = rotStorageBuffer[size - 1];
				size--;
			} else if (storeSize != -1) {
				posRotArray[0] = posStorage[storeSize];
				posRotArray[1] = rotStorage[storeSize];
				storeSize--;
			}
		}
		return posRotArray;
	}
}

public class CameraMover : MonoBehaviour {
	public float speed;
	public float rotationSpeed;

	private float xRotation;
	private float yRotation;

	private TimeController timeControl;

	// Use this for initialization
	void Start () {
		xRotation = 0;
		yRotation = 0;
		timeControl = new TimeController ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.T) && timeControl.hasGottenDumped()) {
			Vector3[] temp = timeControl.readOff();
			transform.Translate (-temp[0]);
			transform.localEulerAngles = temp[1];
		} else {
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

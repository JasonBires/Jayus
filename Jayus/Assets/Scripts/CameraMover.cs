using UnityEngine;
using System.Collections;

public class TimeController {
	private Vector3[] posStorageBuffer;
	private Vector3[] rotStorageBuffer;
	private int size;
	private const int MAX_SIZE = 3200;
	private Vector3 SIGNAL_EMPTY = new Vector3 (-9000, 1000, -90000);

	public TimeController() {
		posStorageBuffer = new Vector3[MAX_SIZE];
		for (int i = 0; i < MAX_SIZE; i++) {
			posStorageBuffer [i] = SIGNAL_EMPTY;
		}
		rotStorageBuffer = new Vector3[MAX_SIZE];
		size = 0;
	}
	//Set a temp variable for size-- 
	public bool isEmpty() {
		int checkSize = ensureCorrectMinusSize(size);
		return posStorageBuffer [checkSize] == SIGNAL_EMPTY;
	}

	public void addFrame(Vector3 pos, Vector3 rot) {
		posStorageBuffer [size] = pos;
		rotStorageBuffer [size] = rot;
		size = (size + 1) % MAX_SIZE;
	}

	public Vector3[] readOff() {
		Vector3[] posRotArray = new Vector3[2];
		size = ensureCorrectMinusSize (size);
		if(posStorageBuffer[size] != SIGNAL_EMPTY) {
			posRotArray[0] = posStorageBuffer[size];
			posRotArray[1] = rotStorageBuffer[size];
			posStorageBuffer[size] = SIGNAL_EMPTY;
		}
		return posRotArray;
	}

	private int ensureCorrectMinusSize(int sizeIn) {
		if (sizeIn == 0) {
			sizeIn = MAX_SIZE - 1;
		} else {
			sizeIn--;
		}
		return sizeIn;
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

	/* Update is called once per frame
	 * If T is pressed and timeControl's backing array isn't spent, rewind time
	 * Else change the object as normal and record the object's rotation and translation in timeControl
	 */
	void Update () {
		if (Input.GetKey (KeyCode.T) && !timeControl.isEmpty()) {
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

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

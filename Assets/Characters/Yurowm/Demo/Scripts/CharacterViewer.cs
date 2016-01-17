using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterViewer : MonoBehaviour {
	
	private Vector3 lastPosition = Vector3.zero;
	public Transform targetForCamera;

	private Vector3 deltaPosition;

	void Awake () {
		deltaPosition = Camera.main.transform.position - targetForCamera.position;
	}

	void Update () {
		if (Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width / 2)
			transform.Rotate(0, -300f * (Input.mousePosition - lastPosition).x / Screen.width, 0);
		lastPosition = Input.mousePosition;
	}

	void LateUpdate () {
		Camera.main.transform.position += (targetForCamera.position + deltaPosition - Camera.main.transform.position) * Time.deltaTime * 5;
	}
}

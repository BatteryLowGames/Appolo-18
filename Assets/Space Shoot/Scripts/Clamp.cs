using UnityEngine;
using System.Collections;

public class Clamp : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Mathf.Clamp (transform.position.x, -70f, 70f),Mathf.Clamp (transform.position.y, -70f, 70f), Mathf.Clamp (transform.position.z, -70f, 70f));
		transform.position = (Vector3.MoveTowards (transform.position, endPos, 5f));

	}
}

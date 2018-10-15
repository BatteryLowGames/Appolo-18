using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public ParticleSystem ps;

    public void OnTriggerEnter(Collider col)
	{
		if (col.transform.tag=="Allien") {
			GameManager.instance.GameOver (ps);
		}

    }
}

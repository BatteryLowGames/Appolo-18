using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "Bullet")
        {
            Destroy(col.gameObject);
        }
    }
}

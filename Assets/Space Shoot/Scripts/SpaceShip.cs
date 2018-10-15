using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour {

    public List<Transform> targetPositions = new List<Transform>();
    private int index = 0;
    public float speed = 5;
    public float rotateSpeed = 5;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position != targetPositions[index].position)
        {
            Vector3 targetDir = targetPositions[index].position - transform.position;

            // The step size is equal to speed times frame time.
            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 2.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position = Vector3.MoveTowards(transform.position, targetPositions[index].position, speed * Time.deltaTime);

        }
        else
        {
            if (index == targetPositions.Count - 1)
            {
                index = 0;
            }
            else
                index++;
            //Debug.Log(targetPositions.Count);

        }

    }
}

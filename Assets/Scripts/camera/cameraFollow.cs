using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;

    public float followSpeed = 10f;
    public float lookSpeed = 10f;

    public void LookAtTarget()
    {
        Vector3 lookDirection = objectToFollow.position - transform.position;
        Quaternion cameraRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, lookSpeed * Time.deltaTime);
    }
    public void MoveToTarget()
    {
        Vector3 targetPostion = objectToFollow.position +
                                objectToFollow.forward * offset.z +
                                objectToFollow.right * offset.x +
                                objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPostion, followSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }
}

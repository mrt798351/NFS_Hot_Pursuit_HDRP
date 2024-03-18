using System;
using UnityEngine;
public class Car : MonoBehaviour
{
    private float originaYPos;
    private bool hasOriginalYPosBeenSet;

    public virtual void Start()
    {
        Invoke("SetOriginalYPos", 1.5f);
    }

    public virtual void Update()
    {
        if (hasOriginalYPosBeenSet)
        {
            Vector3 positionWithConstantYPos = new Vector3(transform.position.x, originaYPos, transform.position.z);
            transform.position = positionWithConstantYPos;
        }
    }

    private void SetOriginalYPos()
    {
        originaYPos = transform.position.y;
        hasOriginalYPosBeenSet = true;
    }
}
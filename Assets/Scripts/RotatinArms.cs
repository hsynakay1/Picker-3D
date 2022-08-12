using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatinArms : MonoBehaviour
{
    enum DirectionOfRotation {left, right }
    [SerializeField] private DirectionOfRotation directionOfRotation;
    private int direction;

    private void Start()
    {
        switch (directionOfRotation)
        {
            case DirectionOfRotation.right:
                direction = 1;
                break;
            case DirectionOfRotation.left:
                direction = -1;
                break;
        }
    }
    void Update()
    {
        transform.Rotate(0, 0, direction, Space.Self);
    }
}

using System.Collections;
using System.Collections.Generic;
using Panda.Examples.Move;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInDirection : MonoBehaviour
{
    public float speed = 10f;
    public Direction direction;
    public enum Direction
    {
        FORWARD, 
        BACKWARD, 
        RIGHT, 
        LEFT, 
        UP, 
        DOWN
    }

    private Vector3 moveDirection;

    void Start()
    {
        switch (direction)
        {
            case Direction.FORWARD:
                moveDirection = transform.forward;
                break;
            case Direction.BACKWARD:
                moveDirection = -transform.forward;
                break;
            case Direction.RIGHT:
                moveDirection = transform.right;
                break;
            case Direction.LEFT:
                moveDirection = -transform.right;
                break;
            case Direction.UP:
                moveDirection = transform.up;
                break;
            case Direction.DOWN:
                moveDirection = -transform.up;
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

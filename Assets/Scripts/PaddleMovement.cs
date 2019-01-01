using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField] float MovementStep;
    [SerializeField] float MovementSpeedKeyboard;
    [SerializeField] Transform paddleTransform;
    [SerializeField] Paddle paddle;

    [SerializeField] bool Move = false;
    [SerializeField] bool MoveDirection = true;

    public void FindAndAssignPaddle() {
        paddle = FindObjectOfType<Paddle>();
        if (paddle) {
            paddleTransform = paddle.transform;
        } else if (!paddle) print("PaddleMovement/FindAndAssignPaddle: paddle not found");
    }

    public void StartMovingLeft() {
        Move = true;
        MoveDirection = false;
    }

    public void StartMovingRight() {
        Move = true;
        MoveDirection = true;
    }

    public void StopMovement() {
        Move = false;
    }

    private void Update() {
        if(Move) {
            var movement = paddleTransform.position += new Vector3((MoveDirection ? MovementStep : -MovementStep) * MovementSpeedKeyboard * Time.deltaTime, 0, 0);
            var newPos = new Vector3(Mathf.Clamp(movement.x, paddle.leftConstraint, paddle.rightConstraint), movement.y, movement.z);
            paddleTransform.position = newPos;
        }
    }
}

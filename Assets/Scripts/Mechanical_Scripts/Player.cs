using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Player {

    public Vector3 velocity;
    public float gravity = 20f;
    public Vector3 gravityDirection = new Vector3(0,1,0);

    // varibles need for air resistance 
    Vector3 dampingDirection;
    public float drag;

    // varible for limiting the max speed 
    public float maximumSpeed;

    public void ApplyGravity() {
        velocity -= gravityDirection * gravity * Time.deltaTime * 60;
    } 

    // add air resistance 
    public void ApplyDamping() {
        dampingDirection = -velocity;
        dampingDirection *=drag;
        velocity += dampingDirection;
    }
    // limit the max speed 
    public void CapMaxSpeed() {
        velocity = Vector3.ClampMagnitude(velocity, maximumSpeed);
    }
}

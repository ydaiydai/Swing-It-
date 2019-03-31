using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Pendulum {

    public Transform player_tr;
    public Tether tether;
    public Arm arm;
    public Player player;

    Vector3 previousPostion;

    public void Initialize() {
        player_tr.transform.parent = tether.tether_tr;
        arm.length = Vector3.Distance(player_tr.transform.localPosition, tether.position);
    }

    public Vector3 MovePlayer(Vector3 pos, float time) {
        player.velocity += GetConstrainedVelocity(pos, previousPostion, time);
        // apply gravity
        player.ApplyGravity();
        // apply air resistance 
        player.ApplyDamping();
        // apply limiting max speed
        player.CapMaxSpeed();

        pos += player.velocity * time;

        // check the if the distance > then arm.length 
        // this is checking if the distance < the arm.length 
        // without this checking the bob keeps dapping around 
        if (Vector3.Distance(pos, tether.position) < arm.length) {
            // keep the new pos is on the curvey path
            pos = Vector3.Normalize(pos - tether.position) * arm.length;
            // at the same time, need to also update new distance between pos and tether
            arm.length = (Vector3.Distance(pos, tether.position));
            return pos;
        }

        previousPostion = pos;

        return pos; 
    }

    // Simple overriding 
    public Vector3 MovePlayer(Vector3 pos, Vector3 prevPos, float time) {
        player.velocity += GetConstrainedVelocity(pos, previousPostion, time);
        // apply gravity
        player.ApplyGravity();
        // apply air resistance 
        player.ApplyDamping();
        // apply limiting max speed
        player.CapMaxSpeed();

        pos += player.velocity * time;

        if (Vector3.Distance(pos, tether.position) < arm.length) {
            pos = Vector3.Normalize(pos - tether.position) * arm.length;
            arm.length = (Vector3.Distance(pos, tether.position));
            return pos;
        }
        
        previousPostion = pos;

        return pos; 
    }

    public Vector3 GetConstrainedVelocity(Vector3 currentPos, Vector3 previousPostion, float time){
        float distanceToTether;
        Vector3 constrainedPosition; 
        Vector3 predictedPosition;

        distanceToTether = Vector3.Distance(currentPos, tether.position);

        if (distanceToTether > arm.length) {
            // A vector pointing from player to the constrained position
            constrainedPosition = Vector3.Normalize(currentPos - tether.position) * arm.length;

            // Basically is the velocity 
            // player's velocity when it is constrained
            // The direction is the tengent of the swing curvey 
            predictedPosition = (constrainedPosition - previousPostion) / time;

            return predictedPosition;
        } 
        return Vector3.zero;
    }

    // let player switch tether 
    public void SwitchTether(Vector3 newPosition) {
        player_tr.transform.parent = null;
        tether.tether_tr.position = newPosition;
        player_tr.transform.parent = tether.tether_tr;
        tether.position = tether.tether_tr.InverseTransformPoint(newPosition);
        arm.length = Vector3.Distance(player_tr.transform.localPosition, tether.position);
    }


    // when no connection player falls 
    public Vector3 Fall(Vector3 pos, float time) {
        player.ApplyGravity();
        player.ApplyDamping();
        player.CapMaxSpeed();

        pos += player.velocity * time;
        return pos;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controls : MonoBehaviour
{
    public enum RotationAxis{
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxis axes = RotationAxis.MouseX;

    // limit the camera vertical angle 
    public float minimumVert = -90.0f;
    public float maximumVert = 90.0f;
    public float sensHorizontal = 5.0f;
    public float sensVertical = 5.0f;
    public float _rotationX = 0;

    void Start () {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        if (axes == RotationAxis.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensHorizontal, 0);
        } else if ((axes == RotationAxis.MouseY)) {
            _rotationX -= Input.GetAxis("Mouse Y") * sensVertical;
        } 
        // Clamps the vertical angle within the min and max limits in 45 degree
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        
        float RotationY = transform.localEulerAngles.y;
        transform.localEulerAngles = new Vector3(_rotationX, RotationY, 0);
    }
}

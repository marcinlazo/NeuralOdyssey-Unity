using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 7.5f;
    public float jumpForce = 10f;
    public float gravity = 25f;

    public float mouseYClamp = 89f;
    public float speed_mouseX = 5f;
    public float speed_mouseY = 5f;
    float mouseX, mouseY;

    CharacterController con;

	void Start ()
    {
        Prepare();
	}
	void Prepare()
    {
        con = GetComponent<CharacterController>();
    }

	void Update () {
        InputKeyboard();
        InputMouse();
	}
    void InputKeyboard()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement);
        movement *= speed;
        if (con.isGrounded)
        {
            movement.y = -1;
            if (Input.GetButtonDown("Jump"))
            {
                movement.y = jumpForce;
            }
        }
        movement.y -= gravity * Time.deltaTime;
        con.Move(movement * Time.deltaTime);
    }
    void InputMouse()
    {
        mouseX += Input.GetAxis("Mouse X") * speed_mouseX;
        mouseY += Input.GetAxis("Mouse Y") * speed_mouseY * -1;// * (PlayerData.invertCamera ? -1 : 1);
        mouseY = Mathf.Clamp(mouseY, -mouseYClamp, mouseYClamp);

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }
}

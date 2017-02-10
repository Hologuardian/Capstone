using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour {

    public float speed = 2;
    public float sensitivity = 2f;

    public CharacterController player;
    public Camera eyes;

    float moveFB; // front and back
    float moveLR; // left and right

    float rotX;
    float rotY;

	// Use this for initialization
	void Start () {

        player = GetComponent<CharacterController>();
       

        
	}
	
	// Update is called once per frame
	void Update () {

        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;
        Debug.Log(eyes.transform.eulerAngles.x);
        if (eyes.transform.eulerAngles.x > 180f)
        {
            rotY = (rotY > 0) ? 0 : rotY;
            

        }
        else if (eyes.transform.eulerAngles.x <= 0f)
        {
            rotY = (rotY < 0) ? 0 : rotY;
        }

        Vector3 movement = new Vector3(moveLR, 0, moveFB);

        transform.Rotate(0, rotX, 0);


        eyes.transform.Rotate(-rotY, 0, 0);

      

        movement = transform.rotation * movement;

        player.Move(movement * Time.deltaTime); 
	}
}

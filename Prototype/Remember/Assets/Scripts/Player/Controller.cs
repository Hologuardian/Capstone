using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.W))
        {
            MoveForward();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveBackward();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
	}

    void MoveForward()
    {

    }

    void MoveLeft()
    {

    }

    void MoveBackward()
    {

    }

    void MoveRight()
    {

    }

    void LeftClick()
    {

    }

    void RightClick()
    {

    }
}

using UnityEngine;
using System.Collections;
using System;

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
        throw new NotImplementedException();
    }

    void MoveLeft()
    {
        throw new NotImplementedException();
    }

    void MoveBackward()
    {
        throw new NotImplementedException();
    }

    void MoveRight()
    {
        throw new NotImplementedException();
    }

    void LeftClick()
    {
        throw new NotImplementedException();
    }

    void RightClick()
    {
        throw new NotImplementedException();
    }
}

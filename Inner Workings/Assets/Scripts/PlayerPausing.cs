using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson; 

public class PlayerPausing : MonoBehaviour
{
    public GameObject pausing;
    private RigidbodyFirstPersonController controller;
    float xSen = 0;
    float ySen = 0;

	void Start ()
    {
        controller = GetComponent<RigidbodyFirstPersonController>();
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
	}

    public void TogglePause()
    {
        if (pausing.activeSelf)
        {
            pausing.SetActive(false);
            controller.mouseLook.lockCursor = true;
            controller.mouseLook.XSensitivity = xSen;
            controller.mouseLook.YSensitivity = ySen;
        }
        else
        {
            xSen = controller.mouseLook.XSensitivity;
            ySen = controller.mouseLook.YSensitivity;
            controller.mouseLook.XSensitivity = 0;
            controller.mouseLook.YSensitivity = 0;
            pausing.SetActive(true);
            controller.mouseLook.lockCursor = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	void Start ()
    {
        StartCoroutine(swapScene(4.0f));
	}

    private IEnumerator swapScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Main Menu");
    }
}

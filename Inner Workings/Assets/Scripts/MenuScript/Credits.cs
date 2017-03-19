using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	void Start ()
    {
        StartCoroutine(swapScene(72.0f));
	}
	
	void Update ()
    {
		
	}

    private IEnumerator swapScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Main Menu");
    }
}

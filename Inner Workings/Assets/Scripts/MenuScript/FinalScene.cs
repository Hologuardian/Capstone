using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{

	void Start ()
    {

        StartCoroutine(swapScene(24.0f));
    }

    void Update()
    {

    }

    private IEnumerator swapScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Credits");
    }
}

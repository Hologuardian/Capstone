using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEnterForWorld : MonoBehaviour 
{
    public int num;

	// Use this for initialization
	void Start ()
    {
		
	}

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            LoadingSceneManager.LoadScene(num);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}

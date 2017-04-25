using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelScript : MonoBehaviour {

    public string LevelToLoad;
    public float radius = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var collider in Physics2D.OverlapCircleAll(transform.position, radius))
            {
                if (collider.CompareTag("Player"))
                {
                    SceneManager.LoadScene(LevelToLoad);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

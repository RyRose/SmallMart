using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public float radius;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var collider in Physics2D.OverlapCircleAll(transform.position, radius))
            {
                if (collider.CompareTag("Player"))
                {
                    Destroy(gameObject);
                }
            }
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

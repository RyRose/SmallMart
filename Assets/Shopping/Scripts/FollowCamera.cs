using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject target;

    public float easing;

	void Update () {
        var position = transform.position;
        var new_position = Vector3.Lerp(position, target.transform.position, easing);
        new_position.z = position.z;
        transform.position = new_position;
	}
}

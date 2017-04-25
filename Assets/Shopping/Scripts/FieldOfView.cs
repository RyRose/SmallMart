using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [Range(0.1f, 1000f)]
    public float radius = 1;

    [Range(1.0f, 360f)]
    public int fov = 90;//90 degrees

    public Vector2 direction;

    //used to test the FOV
    public Transform testPoint;

    private Vector2 leftLineFOV;
    private Vector2 rightLineFOV;

    private Mesh mesh;

    private Enemy enemy;
    private EdgeCollider2D edgeColliders;

	void Start () {
        enemy = GetComponentInParent<Enemy>();
        mesh = GetComponent<MeshFilter>().mesh;

        var direction = Vector3.right * radius;
        var left = Quaternion.Euler(0, 0, -fov / 2) * direction;
        var right = Quaternion.Euler(0, 0, fov / 2) * direction;

        mesh.vertices = new Vector3[] { Vector3.zero, left, right };
        mesh.triangles = new int[] { 0, 2, 1 };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) };
	}



    void Update()
    {
        var d = Vector3.right * radius;
        var left = Quaternion.Euler(0, 0, -fov / 2) * d;
        var right = Quaternion.Euler(0, 0, fov / 2) * d;

        mesh.vertices = new Vector3[] { Vector3.zero, left, right };

        direction = transform.rotation * Vector2.right;
        if (testPoint != null)
        {
            rightLineFOV = RotatePointAroundTransform(direction.normalized * radius, -fov / 2);
            leftLineFOV = RotatePointAroundTransform(direction.normalized * radius, fov / 2);
//            Debug.DrawRay(transform.position, testPoint.position - transform.position, Color.black);
            if (InsideFOV(new Vector2(testPoint.position.x, testPoint.position.y)) && Physics2D.Raycast(transform.position, testPoint.position - transform.position).transform.tag == "Player")
            {
                enemy.Pursue(testPoint.gameObject.GetComponent<Player>());
            }
        }
    }

    public bool InsideFOV(Vector2 playerPos)
    {
        float squaredDistance = ((playerPos.x - transform.position.x) * (playerPos.x - transform.position.x)) + ((playerPos.y - transform.position.y) * (playerPos.y - transform.position.y));
        if (radius * radius >= squaredDistance)
        {
            float signLeftLine = (leftLineFOV.x) * (playerPos.y - transform.position.y) - (leftLineFOV.y) * (playerPos.x - transform.position.x);
            float signRightLine = (rightLineFOV.x) * (playerPos.y - transform.position.y) - (rightLineFOV.y) * (playerPos.x - transform.position.x);
            if (fov <= 180)
            {
                if (signLeftLine <= 0 && signRightLine >= 0)
                    return true;
            }
            else
            {
                if (!(signLeftLine >= 0 && signRightLine <= 0))
                    return true;
            }
        }
        return false;
    }

    //Rotate point (px, py) around point (ox, oy) by angle theta you'll get:
    //p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
    //p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy
    private Vector2 RotatePointAroundTransform(Vector2 p, float angles)
    {
        return new Vector2(Mathf.Cos((angles) * Mathf.Deg2Rad) * (p.x) - Mathf.Sin((angles) * Mathf.Deg2Rad) * (p.y),
                           Mathf.Sin((angles) * Mathf.Deg2Rad) * (p.x) + Mathf.Cos((angles) * Mathf.Deg2Rad) * (p.y));
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction.normalized*radius);

        rightLineFOV = RotatePointAroundTransform(direction.normalized*radius, -fov/2);
        leftLineFOV = RotatePointAroundTransform(direction.normalized*radius, fov/2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rightLineFOV);
        Gizmos.DrawRay(transform.position, leftLineFOV);

        Vector2 p = rightLineFOV;
        for(int i = 1; i <= 20; i++) {
            float step = fov/20;
            Vector2 p1 = RotatePointAroundTransform(direction.normalized*radius, -fov/2 + step*(i));
            Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y) + p, p1-p);
            p = p1;
        }
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y) + p, leftLineFOV - p);
}

}

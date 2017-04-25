using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public bool Frozen
    {
        get; set;
    }

    public float speed;

    private Rigidbody2D rigidbody;

    private Animator animator;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        float horizontal, vertical;
        if (!Frozen)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

        } else
        {
            horizontal = 0;
            vertical = 0;
        }

        animator.SetInteger("Direction", GetDirection(horizontal, vertical));
        rigidbody.AddForce(new Vector2(horizontal, vertical) * speed * Time.deltaTime, ForceMode2D.Impulse);
	}

    private int GetDirection(float h, float v)
    {
        if (h > 0 && v == 0) // right
        {
            return 0;
        } else if (h < 0 && v == 0) // left
        {
            return 2;
        } else if (v > 0) // up
        {
            return 1;
        } else if (v < 0) // down
        {
            return 3;
        } else
        {
            return 4;
        }
    }

    public IEnumerator EndGame()
    {
        Dialogue d = GetComponent<HasDialogueScript>().GetDialogue();
        GetComponent<PlayDialogue>().SpawnDialog(d);
        yield return new WaitUntil(() => !GetComponent<PlayDialogue>().inDialogue);
        print("load scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

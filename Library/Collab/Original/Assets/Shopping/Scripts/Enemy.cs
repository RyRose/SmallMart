using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Patrol,
    Returning,
    Nothing,
    Pursuit
}
public class Enemy : MonoBehaviour {

    public Vector2 target;

    public State state;
    public float speed;

    public bool harmless;

    private Dialogue dialogue;

    private Rigidbody2D rigidbody;

    private Vector2 patrolTo;
    private Vector2 original;

    private bool inDialogue;

    private Animator animator;

    private void Start()
    {
        inDialogue = false;
        original = transform.position;
        patrolTo = target;
        rigidbody = GetComponent<Rigidbody2D>();
        dialogue = GetComponent<HasDialogueScript>().GetDialogue();
        animator = GetComponent<Animator>();
    }

    void Update () {
        Vector2 pos = transform.position;
        switch (state)
        {
            case State.Patrol:
                if (Vector2.Distance(target, transform.position) < 1f)
                {
                    if (target == patrolTo)
                    {
                        target = original;
                    } else
                    {
                        target = patrolTo;
                    }
                }
                rigidbody.AddForce((target - pos).normalized * speed * Time.deltaTime);
                break;
            case State.Pursuit:
                rigidbody.AddForce((target - pos).normalized * speed * Time.deltaTime);
                break;
            case State.Returning:
                target = original;
                rigidbody.AddForce((target - pos).normalized * speed * Time.deltaTime);
                break;
            case State.Nothing:
                break;
            default:
                break;
        }

        animator.SetInteger("Direction", GetDirection(rigidbody.velocity.x, rigidbody.velocity.y));
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

    public void Pursue(Player player)
    {
        player.Frozen = true;
        target = player.transform.position;
        state = State.Pursuit;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !inDialogue && !(state == State.Returning)) 
        {
            var player = collision.gameObject.GetComponent<PlayDialogue>();
            if (harmless)
            {
                StartCoroutine(StartDialogue(collision.gameObject.GetComponent<Player>(), player));
            } else
            {
                StartCoroutine(EndLevel(collision.gameObject.GetComponent<Player>(), player));
            }
        }
    }

    private IEnumerator StartDialogue(Player player, PlayDialogue dialogue)
    {
        state = State.Nothing;
        inDialogue = true;
        dialogue.SpawnDialog(this.dialogue);
        yield return new WaitUntil(() => !dialogue.inDialogue);
        Destroy(transform.GetChild(0).gameObject); // Destroys the FOV triangle.
        player.Frozen = false;
        state = State.Returning;
        inDialogue = false;
    }

    private IEnumerator EndLevel(Player player, PlayDialogue dialogue)
    {
        state = State.Nothing;
        inDialogue = true;
        dialogue.SpawnDialog(this.dialogue);
        yield return new WaitUntil(() => !dialogue.inDialogue);
        yield return player.EndGame();
    }
}

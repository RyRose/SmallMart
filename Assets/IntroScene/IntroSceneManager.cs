using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour {

    public GameObject introDialogue;
    public GameObject outroDialogue;
    private bool temp = true;
    public GameObject[] enemies;

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.GetComponent<Player>().Frozen = true;
        var dialogue = introDialogue.GetComponent<HasDialogueScript>().GetDialogue();
        playerObject.GetComponent<PlayDialogue>().SpawnDialog(dialogue);
        yield return new WaitUntil(() => !playerObject.GetComponent<PlayDialogue>().inDialogue);
        playerObject.GetComponent<Player>().Frozen = false;
    }

    void Update () {
        int count = 0;
        foreach(var enemy in enemies)
        {
            count += enemy.transform.childCount;
        }

        if (count == 0 && temp) // end level
        {
            temp = false;
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (!playerObject.GetComponent<PlayDialogue>().inDialogue)
            {
                playerObject.GetComponent<Player>().Frozen = true;
                var dialogue = outroDialogue.GetComponent<HasDialogueScript>().GetDialogue();
                playerObject.GetComponent<PlayDialogue>().SpawnDialog(dialogue);
                StartCoroutine(PlayOutro());
            }
        }
	}

    private IEnumerator PlayOutro()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        yield return new WaitUntil(() => !playerObject.GetComponent<PlayDialogue>().inDialogue);
        SceneManager.LoadScene("_GasStation");
    }
}

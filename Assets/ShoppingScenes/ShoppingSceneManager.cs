using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShoppingSceneManager : MonoBehaviour {

    public string nextSceneName;

    public GameObject introDialogue;
    public GameObject outroDialogue;

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

    private bool temp = true;

    void Update () {
        if (GameObject.FindGameObjectsWithTag("Item").Length == 0 && temp)
        {
            temp = false;
            StartCoroutine(PlayOutro());
        }
	}

    private IEnumerator PlayOutro()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.GetComponent<Player>().Frozen = true;
        yield return new WaitUntil(() => !playerObject.GetComponent<PlayDialogue>().inDialogue);
        playerObject.GetComponent<PlayDialogue>().SpawnDialog(outroDialogue.GetComponent<HasDialogueScript>().GetDialogue());
        yield return new WaitUntil(() => !playerObject.GetComponent<PlayDialogue>().inDialogue);
        SceneManager.LoadScene(nextSceneName);
    }
}
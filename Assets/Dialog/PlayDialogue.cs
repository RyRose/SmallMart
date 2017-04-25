using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayDialogue : MonoBehaviour {
    public bool inDialogue;

    AudioSource Aud;
    private GameObject BackgroundCanvas;
    Dialogue CurrentDialogue;
    public KeyCode NextDialogueKey;
	// Use this for initialization
	void Start () {
        Aud = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (inDialogue && Input.GetKeyDown(NextDialogueKey))
        {
            ContinueDialog();
        }
	}
    public void SpawnDialog(Dialogue d)
    {
        inDialogue = true;
        BackgroundCanvas = Instantiate(Resources.Load("BackgroundCanvas")) as GameObject;
        BackgroundCanvas.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = d.GetText();
        CurrentDialogue = d;
        var clip = CurrentDialogue.GetAudio();
        if (clip != null)
        {
            Aud.clip = clip;
            Aud.Play();
        }
    }

    public void ContinueDialog()
    {
        if (CurrentDialogue.HasNext())
        {
            CurrentDialogue = CurrentDialogue.GetNext();
            var a = BackgroundCanvas.GetComponentInChildren<Canvas>();
            var b = a.GetComponentInChildren<Text>();
            b.text = CurrentDialogue.GetText();
            Aud.clip = CurrentDialogue.GetAudio();
            if (Aud.clip != null)
            {
                Aud.Play();
            }
        } else
        {
            inDialogue = false;
            Destroy(BackgroundCanvas);
        }
    }
    
}

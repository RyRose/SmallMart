using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasDialogueScript : MonoBehaviour {
    public string[] strings;
    public AudioClip[] sounds;
    public Dialogue d;
    
    void Awake () {
        d = new Dialogue(strings[0],sounds[0]);
        for (int i = 1; i < strings.Length; i++)
        {
            d.AddNext(new Dialogue(strings[i],sounds[i]));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Dialogue GetDialogue()
    {
        return d;
    }

}

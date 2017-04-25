using UnityEngine;

public class Dialogue {

    private string text;
    private Dialogue next = null;
    private AudioClip Sound = null;

    public Dialogue(string s, AudioClip sound)
    {
        text = s;
        Sound = sound;
    }
    public Dialogue(string[] strings, AudioClip[] sounds)
    {
        this.text = strings[0];
        for(int i = 1; i < strings.Length; i++)
        {
            this.AddNext(new Dialogue(strings[i], sounds[i]));
        }
    }
    public void AddNext(Dialogue d)
    {
        if(next == null)
        {
            next = d;
        } else
        {
            next.AddNext(d);
        }    
    }
    public Dialogue GetNext()
    {
        return next;
    }
    public bool HasNext()
    {
        return next != null;
    }
    public string GetText()
    {
        return text;
    }
    public AudioClip GetAudio()
    {
        return Sound;
    }
}


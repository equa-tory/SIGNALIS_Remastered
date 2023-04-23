using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class OstManager : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] ost;
    int choosedOst;

    bool nextOstStarted;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        choosedOst = Random.Range(0, ost.Length);
        PlayOst(ost[choosedOst]);
    }

    private void Update()
    {
        if (source.time >= ost[choosedOst].length && !nextOstStarted) { Invoke(nameof(NextOst),1f); nextOstStarted = true; }
    }

    public void NextOst()
    {
        choosedOst++;
        if (choosedOst >= ost.Length) choosedOst = 0;
        PlayOst(ost[choosedOst]);

        nextOstStarted=false;
    }

    private void PlayOst(AudioClip _clip)
    {
        source.clip = _clip;
        source.Play();
    }


}

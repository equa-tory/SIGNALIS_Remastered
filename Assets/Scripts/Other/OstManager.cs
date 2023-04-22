using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class OstManager : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] ost;
    int choosedOst;


    private void Awake()
    {
        source = GetComponent<AudioSource>();
        choosedOst = Random.Range(0, ost.Length);
        PlayOst(ost[choosedOst]);
    }

    private void Update()
    {
        if (source.time >= ost[choosedOst].length + 1f) NextOst();
        if (choosedOst >= ost.Length - 1) choosedOst = 0;

    }

    public void NextOst()
    {
        choosedOst++;
        PlayOst(ost[choosedOst]);
    }

    private void PlayOst(AudioClip _clip)
    {
        source.clip = _clip;
        source.Play();
    }


}

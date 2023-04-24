using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Animator anim;

    private void Awake() {
        Instance = this;

        anim = GetComponent<Animator>();
    }

    public void Fade(){
        anim.SetTrigger("Fade");
    }
}

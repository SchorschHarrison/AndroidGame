using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    
    public GameObject next;

    private Animator wallAnim;

    public void StartAppear()
    {
        if(wallAnim == null)
        {
            wallAnim = GetComponentInChildren<Animator>();
        }
        wallAnim.SetBool("appear", true);
    }

}

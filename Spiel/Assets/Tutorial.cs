using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    Animator textAnimator;
    TextMesh textMesh;
    bool jumped;
    void Start()
    {
        jumped = false;
        textAnimator = GetComponentInChildren<Animator>();
        textMesh = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerInput.current.jumpPressed && !jumped)
        {
            jumped = true;
            textAnimator.SetTrigger("Blink");
            StartCoroutine("ChangeText");
            GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>().GameStart();
        }


    }

    IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.35f);
        textAnimator.ResetTrigger("Blink");
        textMesh.text = "Great!";
        yield return new WaitForSeconds(2f);
        textAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    public void BlinkFinished()
    {
        Debug.Log("lol");
    }
}

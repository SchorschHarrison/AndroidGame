using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput current;

    public bool jumpPressed;

    void Start()
    {
        if(PlayerInput.current == null)
        {
            PlayerInput.current = this;
        }
        jumpPressed = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        jumpPressed = (Input.touchCount > 0) || (Input.GetMouseButton(0));
    }
}

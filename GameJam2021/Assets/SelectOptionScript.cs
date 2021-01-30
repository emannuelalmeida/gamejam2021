using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOptionScript : MonoBehaviour
{
    
    enum MenuOption { Start, Load, Options, Credits, Quit}
    MenuOption option = 0;
    GameObject menuSelector = GameObject.Find("Menu Selector"); 

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            option--;
        else if (Input.GetKey(KeyCode.DownArrow))
            option++;

        if (option < 0)
            option = MenuOption.Quit;
        else if ((int)option > 3)
            option = MenuOption.Start;
    }
}

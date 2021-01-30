using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOptionScript : MonoBehaviour
{
    
    enum MenuOption { Start, Load, Options, Credits, Quit}
    MenuOption option = 0;
    GameObject menuSelector;
    Vector3 initialPosition;
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    void Start()
    {
        menuSelector = GameObject.Find("Menu Selector");
        initialPosition = menuSelector.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            option--;
            audioSource.PlayOneShot(audioClipArray[0]);
        }     
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            option++;
            audioSource.PlayOneShot(audioClipArray[0]);
        } else if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(audioClipArray[1]);
        }

        if (option < 0)
            option = MenuOption.Quit;
        else if ((int)option > 4)
            option = MenuOption.Start;

        menuSelector.transform.position = 
            new Vector3(initialPosition.x, initialPosition.y - (int)option * 1.4f, initialPosition.z);


    }
}

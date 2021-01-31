using UnityEngine;
using UnityEngine.SceneManagement;

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
            ProcessOption();
        }

        if (option < 0)
            option = MenuOption.Quit;
        else if ((int)option > 4)
            option = MenuOption.Start;

        menuSelector.transform.position = 
            new Vector3(initialPosition.x, initialPosition.y - (int)option * 1.4f, initialPosition.z);

    }

    private void ProcessOption()
    {
        Debug.Log(option);
        switch (option)
        {
            case MenuOption.Quit:
                Application.Quit();
                break;
            case MenuOption.Start:
                SceneManager.LoadScene("External");
                break;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehavior : MonoBehaviour {

    public Canvas MainMenu;
    public Canvas Lobbies;
    public Canvas TeamPicker;
    public Canvas QuitMenu;

    public GameObject orange;
    public GameObject blue;

    private bool team = true;


    // Use this for initialization
    void Start () {
        Lobbies.enabled = false;
        TeamPicker.enabled = false;
        QuitMenu.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape") && !MainMenu.enabled && !Lobbies.enabled && !TeamPicker.enabled)
        {
            QuitMenu.enabled = true;
        }
        if (team)
        {
            orange.GetComponent<RectTransform>().sizeDelta = new Vector2(184, 34.5f);
            blue.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        }
        else
        {
            blue.GetComponent<RectTransform>().sizeDelta = new Vector2(184, 34.5f);
            orange.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        }
    }

    public void selOrange()
    {
        team = true;
    }

    public void selBlue()
    {
        team = false;
    }

    public void ShutDown ()
    {
        Application.Quit();
    }

    public void StartGame ()
    {
        //game go here
    }

    public void StopGame()
    {
        //stop teh gamez
    }
}

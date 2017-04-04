using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehavior : MonoBehaviour {

    public Canvas MainMenu;
    public Canvas QuitMenu;

	// Use this for initialization
	void Start () {
        QuitMenu.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape") && !MainMenu.enabled)
            QuitMenu.enabled = true;
    }

    public void ShutDown ()
    {
        Application.Quit();
    }

    public void StartGame ()
    {
        //game go here
    }
}

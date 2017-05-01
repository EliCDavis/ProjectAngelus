using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    [SerializeField]
    private Animator m_MenuAnimator;
    [SerializeField]
    private bool isStartMenu = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) && isStartMenu)
        {
            isStartMenu = false;
            SwitchStartMenu();
        }
	}

    public void SwitchStartMenu()
    {
        m_MenuAnimator.SetTrigger("SpaceLeave");
        m_MenuAnimator.SetTrigger("StartMenu");
    }

    public void SwitchMatchMenu()
    {
        m_MenuAnimator.SetTrigger("MatchManager");
    }

    public void SwitchQuitMenu()
    {
        m_MenuAnimator.SetTrigger("QuitMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string _scene)
    {
        StartCoroutine(WaitLoadScene(_scene));
    }

    IEnumerator WaitLoadScene(string _scene)
    {
        FadingManager.m_Singleton.BeginFade(1);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(_scene);
    }
}

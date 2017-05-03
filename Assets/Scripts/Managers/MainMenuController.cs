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
    [SerializeField]
    AudioSource transition;


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
        transition.Play();
        m_MenuAnimator.SetTrigger("SpaceLeave");
        m_MenuAnimator.SetTrigger("StartMenu");
    }

    public void SwitchMatchMenu()
    {
        transition.Play();
        m_MenuAnimator.SetTrigger("MatchManager");
    }

    public void SwitchQuitMenu()
    {
        transition.Play();
        m_MenuAnimator.SetTrigger("QuitMenu");
    }

    public void QuitGame()
    {
        transition.Play();
        Application.Quit();
    }

    public void LoadScene(string _scene)
    {
        StartCoroutine(WaitLoadScene(_scene));
    }

    IEnumerator WaitLoadScene(string _scene)
    {
        FadingManager.m_Singleton.BeginFade(1);
        transition.Play();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(_scene);
    }
}

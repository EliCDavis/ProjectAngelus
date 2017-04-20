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
        SceneManager.LoadScene(_scene);
    }
}

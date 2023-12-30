using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }    
    
    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }    
    
    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level3");
    }    
    
    public void PlayLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void QuitGame()
    {
        #if UNITY_WEBPLAYER || UNITY_WEBGL
            Application.OpenURL("about:blank"); 
        #else
            Application.Quit();
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Texture2D cursorImageMenu;
    public Animator transisi;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImageMenu, new Vector2(30,10), CursorMode.ForceSoftware);
    }

    public void LoadScenePilih(string Scene)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadScenes(Scene));
    }

    IEnumerator LoadScenes(string sceneTerpilih)
    {
        transisi.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        AudioListener.pause = false;
        SceneManager.LoadScene(sceneTerpilih);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

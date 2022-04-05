using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Texture2D cursorImageMenu;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImageMenu, new Vector2(30,10), CursorMode.ForceSoftware);
    }

    public void PilihGame(int pilihanGame)
    {
        SceneManager.LoadScene("Game "+ pilihanGame);
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}

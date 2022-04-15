using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool IsPause = false;
    public GameObject PauseMenu;
    public GameObject ButtonMusic;
    public GameObject ButtonPause;
    public GameObject HideObjectPause;

    public void OnButtonPause()
    {
        Debug.Log(IsPause);
        //if (IsPause)
        //{
        //    Resume();
        //}
        //else
        //{
        //    Stop();
        //}

        Stop();
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //IsPause = false;
        AudioListener.pause = SoundManager.muted;
        if(HideObjectPause != null) HideObjectPause.SetActive(true);

        ButtonMusic.GetComponent<Button>().interactable = true;
        ButtonPause.GetComponent<Button>().interactable = true;
    }

    void Stop()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        //IsPause = true;
        AudioListener.pause = true;
        if (HideObjectPause != null) HideObjectPause.SetActive(false);

        ButtonMusic.GetComponent<Button>().interactable = false;
        ButtonPause.GetComponent<Button>().interactable = false;
    }

}

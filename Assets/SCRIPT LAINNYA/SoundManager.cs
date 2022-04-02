using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject Sound;
    [SerializeField] Sprite SoundON;
    [SerializeField] Sprite SoundOFF;
    public static bool muted = false;

    void Start()
    {
        if (PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }

        UpdateIcon();
        AudioListener.pause = muted;
    }

    public void OnButtonPress()
    {
        if (muted)
        {
            muted = false;
            AudioListener.pause = false;
        }
        else
        {
            muted = true;
            AudioListener.pause = true;
        }

        Save();
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (muted)
        {
            //SoundON.enabled = false;
            //SoundOFF.enabled = true;
            Sound.GetComponent<Image>().sprite = SoundOFF;
        }
        else
        {
            //SoundON.enabled = true;
            //SoundOFF.enabled = false;
            Sound.GetComponent<Image>().sprite = SoundON;
        }
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }

}

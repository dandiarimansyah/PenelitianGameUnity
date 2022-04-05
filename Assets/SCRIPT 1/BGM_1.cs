using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_1 : MonoBehaviour
{
    private static BGM_1 BGMusic1;
    private void Awake()
    {
        if (BGMusic1 == null)
        {
            BGMusic1 = this;
            //DontDestroyOnLoad(BGMusic1);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

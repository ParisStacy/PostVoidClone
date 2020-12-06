using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrosshairTriggers : MonoBehaviour
{


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Death() {
        SceneManager.LoadScene("PeterScene");
        Cursor.lockState = CursorLockMode.None;
    }
}

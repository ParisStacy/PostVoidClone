using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrosshairTriggers : MonoBehaviour
{

    [SerializeField]
    AudioSource crosshairSource;
    [SerializeField]
    AudioClip clocktickClip, deathSound;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Tick() {

    }

    public void Death() {
        SceneManager.LoadScene("PeterScene");
        Cursor.lockState = CursorLockMode.None;
    }
}

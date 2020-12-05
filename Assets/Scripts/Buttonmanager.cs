using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttonmanager : MonoBehaviour
{
    public void ButtonMoveScene(string level)
    {
        SceneManager.LoadScene(level);
    }
}

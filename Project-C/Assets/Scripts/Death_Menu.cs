using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death_Menu : MonoBehaviour
{

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

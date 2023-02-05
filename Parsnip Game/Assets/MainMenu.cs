using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ActivateOptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

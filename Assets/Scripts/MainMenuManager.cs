using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    float time = 0;
    float maxTime = 5;
    public Image splashscreen;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= maxTime)
        {
            time += Time.deltaTime;
        }
        if (time >= maxTime && splashscreen.gameObject.activeSelf)
        {
            splashscreen.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Cinematic");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

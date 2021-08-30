using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioClip menuBgm;
    private void Start()
    {
        AudioManager.Instance.PlayMusic(menuBgm);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}

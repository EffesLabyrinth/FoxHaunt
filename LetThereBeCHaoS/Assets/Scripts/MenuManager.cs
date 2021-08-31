using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioClip menuBgm;
    [SerializeField] Animator anim;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(menuBgm);
        StartCoroutine(CreepyEffect());
    }
    public void StartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator CreepyEffect()
    {
        while (true)
        {
            float randomTimer = Random.Range(10f, 24f);
            Debug.Log(randomTimer);
            yield return new WaitForSeconds(randomTimer);
            anim.Play("creepy", 0, 0);
        }
    }
}

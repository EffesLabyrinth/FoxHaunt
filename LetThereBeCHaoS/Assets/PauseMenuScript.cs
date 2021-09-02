using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] Animator anim;

    //sound
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    //snapping
    [SerializeField] bool isSnapping;
    [SerializeField] GameObject checkMark;

    public void OpenPauseMenu()
    {
        gameObject.SetActive(true);

        //sound
        float value;
        if( mixer.GetFloat("BgmVol", out value))
        {
            musicSlider.value = value;
        }
        if (mixer.GetFloat("SfxVol", out value))
        {
            sfxSlider.value = value;
        }

        //snapping
        isSnapping = PlayerManager.Instance.controller.enableSnapToEnemy;
        checkMark.SetActive(isSnapping);
        Time.timeScale = 0;
    }
    public void ReturnToGame()
    {
        anim.Play("PauseMenuClose");
    }
    public void DisablePanel()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ChangeMusicVol()
    {
        mixer.SetFloat("BgmVol", musicSlider.value);
    }
    public void ChangeSfxVol()
    {
        mixer.SetFloat("SfxVol", sfxSlider.value);
    }

    public void EnableSnapping()
    {
        isSnapping = !isSnapping;
        checkMark.SetActive(isSnapping);
        PlayerManager.Instance.controller.enableSnapToEnemy = isSnapping;
    }
}

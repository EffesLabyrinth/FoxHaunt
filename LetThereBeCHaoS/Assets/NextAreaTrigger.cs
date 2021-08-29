using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextAreaTrigger : MonoBehaviour
{
    [SerializeField] int currentLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("level", currentLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            
        }
    }
}

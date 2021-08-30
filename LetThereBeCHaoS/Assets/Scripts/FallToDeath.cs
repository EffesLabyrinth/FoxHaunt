using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallToDeath : MonoBehaviour
{
    Transform target;
    void Start()
    {
        target = PlayerManager.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

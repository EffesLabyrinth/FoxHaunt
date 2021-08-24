using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    Queue<GameObject> queue;
    [SerializeField] float timer;
    float startTimer;
    [SerializeField] SpriteRenderer spriteRend;
    public void SetPool(Queue<GameObject> pool)
    {
        queue = pool;
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRend.sprite = sprite;
    }
    private void Start()
    {
        InitializeFade();
    }
    private void OnEnable()
    {
        InitializeFade();
    }
    IEnumerator FadeTimer()
    {
        float ratio;
        do
        {
            startTimer -= Time.deltaTime;
            ratio = Mathf.Clamp01(startTimer / timer);
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, ratio);
            if (startTimer > 0) yield return null;
        } while (startTimer > 0);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        queue.Enqueue(gameObject);
    }
    void InitializeFade()
    {
        startTimer = timer;
        StartCoroutine(FadeTimer());
        spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1);
    }
}

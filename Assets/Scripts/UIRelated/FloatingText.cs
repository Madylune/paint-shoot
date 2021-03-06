﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private float speed, duration;

    [SerializeField]
    private Text text;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        if (Time.timeScale > 0f)
        {
            Move();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public IEnumerator FadeOut()
    {
        float startAlpha = text.color.a;
        float rate = 1.0f / duration;
        float progress = 0.0f;

        while (progress < 1.0)
        {
            Color tmp = text.color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            text.color = tmp;
            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingCamera : MonoBehaviour
{
    public GameObject fadeScreen;
    public float fadeDuration = 2.0f;

    public Color fadeColor;

    private Renderer _rend;
    private bool _fadingIn = false;
    


    private void Start()
    {
        _rend = fadeScreen.GetComponent<Renderer>();
    }

    public void FadeIn()
    {
        fadeScreen.SetActive(false);
        _fadingIn = true;
        Fade(1,0);
    }


    public void FadeOut()
    {
        fadeScreen.SetActive(true);
        Fade(0,1);
    }
    
    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0.0f;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            _rend.material.SetColor("_Color", newColor);
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
            
        _rend.material.SetColor("_Color", newColor2);
        
        if (_fadingIn)
        {
            fadeScreen.SetActive(false);
            _fadingIn = false;
        }
    }
}

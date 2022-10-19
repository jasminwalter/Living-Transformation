using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ObjectTransitions : MonoBehaviour
{

    private new SkinnedMeshRenderer _rend;
    public GameObject obj2Mesh;
    
    // public Material placeholderMat;
    //
    public Material[] newMats;
    //
    // public Material defaultMat;
    // public Material objectDark;
    // public Material objectColor;
    // public Material objectGlow;

    public int transitions;
    public bool upwards;
    public bool makeTransition = false;

    public Animator objectAnim1;
    public float slowDownDuration = 1.0f;
    public float speedUpDuration = 2.0f;

    public float fadeBlackDuration = 2.0f;
    public float fadeBack2Normal = 2.0f;
    public Color fadeColor;

    public float fadeOutDuration = 2.0f;
    public float fadeInDuration = 2.0f;

    private float shrinkingDuration = 0.7f;
    private float growingDuration = 2.0f;
    private float shakeDuration = 2.0f;
    
    public Material material1;
    public Material material2;
    public float duration = 2.0f;
    public Renderer rend;
    
    

    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rend = obj2Mesh.GetComponent<SkinnedMeshRenderer>();
        transitions = 0;
        upwards = true;

        objectAnim1 = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.O))
        // {
        //     StartCoroutine(FadeToBlackMaterial());
        //     
        // }
        //
        // if (Input.GetKey(KeyCode.P))
        // {
        //     StartCoroutine(FadeToWhiteMaterial());
        // }
        if (makeTransition)
        {
            StartCoroutine(Transitioning(objectAnim1));
            makeTransition = false;

            // / other transition effects
            // ping-pong between the materials over the duration
            // float lerp = Mathf.PingPong(Time.time, duration) / duration;
            // rend.material.Lerp(material1, material2, lerp);

            // StartCoroutine(Transitioning(objectAnim1));
            // makeTransition = false;
            // StartCoroutine(FadeToBlackMaterial());
        }
        
    }

    public void ChangeLiveMat(int newMatNum)
    {
        // Find the material in the array
         Material materialToUse = newMats[newMatNum];


         _rend.material = materialToUse;

    }
    

    private IEnumerator Transitioning(Animator objectAnim)
    {

        StartCoroutine(SlowDownAnimation(objectAnim));
        yield return new WaitForSeconds(slowDownDuration);

        if (upwards)
        {
            transitions++;
            if(transitions == 2)
            {
                upwards = false;
            }
        }
        else
        {
            transitions--;
            if (transitions == 0)
            {
                upwards = true;
            }
        }
        // Fade to black transition
        // StartCoroutine(FadeToBlackMaterial());
        // yield return new WaitForSeconds(fadeBlackDuration);
        //
        //
        // ChangeLiveMat(transitions);
        //
        // StartCoroutine(FadeToWhiteMaterial());
        // yield return new WaitForSeconds(fadeBack2Normal);

        // StartCoroutine(FadeToBlackMaterial());
        // yield return new WaitForSeconds(fadeBlackDuration);
        // StartCoroutine(FadeOutRoutine());
        // yield return new WaitForSeconds(fadeOutDuration);

        // Color newColorNext = fadeColor;
        // newColorNext.a = 1;
        //     
        // newMats[transitions].SetColor("_Color", newColorNext);
        //
        // ChangeLiveMat(transitions);

        //
        // StartCoroutine(FadeInRoutine());
        // yield return new WaitForSeconds(fadeInDuration);
        StartCoroutine(ShakeObject());
        yield return new WaitForSeconds(shakeDuration);
        
        StartCoroutine(ShrinkModel());
        yield return new WaitForSeconds(shrinkingDuration);
        
        ChangeLiveMat(transitions);
        
        StartCoroutine(GrowModel());
        yield return new WaitForSeconds(growingDuration);

        
        StartCoroutine(SpeedUpAnimation(objectAnim));

        yield return null;
    }

    private IEnumerator SlowDownAnimation(Animator currentAnim)
    {
        float timer = 0.0f;
        while (timer <= slowDownDuration)
        {
            currentAnim.speed =  Mathf.Lerp(1, 0, timer / slowDownDuration);
            
            timer += Time.deltaTime;
            yield return null;
        }

        currentAnim.speed = 0;

        yield return null;
    }
    
    private IEnumerator SpeedUpAnimation(Animator currentAnim)
    {
        float timer = 0.0f;
        while (timer <= speedUpDuration)
        {
            currentAnim.speed =  Mathf.Lerp(0, 1, timer / speedUpDuration);
            
            timer += Time.deltaTime;
            yield return null;
        }

        currentAnim.speed = 1;

        yield return null;
    }

    private IEnumerator ShakeObject()
    {
        Vector3 initialPosition = transform.position;
        float timer = 0.0f;
        
        Vector3 directionOfShake = transform.up;
        float amplitude = 0.02f; // the amount it moves
        float frequency = 20.0f;
        
        
        while (timer <= shakeDuration)
        {

            transform.position = initialPosition + directionOfShake *
                (-amplitude + Mathf.PingPong(frequency * Time.deltaTime, 2.0f * amplitude));
            timer += Time.deltaTime;

            yield return null;
        }

        transform.position = initialPosition;
            

        yield return null;
    }

    private IEnumerator ShrinkModel()
    {
        float timer = 0.0f;
        
        while (timer <= shrinkingDuration)
        {
            
            float shrinkVal = Mathf.Lerp(1, 0, timer / shrinkingDuration);
            this.GetComponent<Transform>().localScale = new Vector3(shrinkVal, shrinkVal, shrinkVal);
            timer += Time.deltaTime;
            yield return null;
        }
        
        this.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        
        yield return null;
    }

    private IEnumerator GrowModel()
    {
        float timer = 0.0f;
        
        while (timer <= growingDuration)
        {
            
            float shrinkVal = Mathf.Lerp(0, 1, timer / growingDuration);
            this.GetComponent<Transform>().localScale = new Vector3(shrinkVal, shrinkVal, shrinkVal);
            timer += Time.deltaTime;
            yield return null;
        }
        
        this.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        
        yield return null;
    }

    #region Unused Transition Effects

        private IEnumerator FadeToBlackMaterial()
    {
        
        float timer = 0.0f;
        
        while (timer <= fadeBlackDuration)
        {
            // fade out color
            Color newColor = fadeColor;
            newColor = Color.Lerp(Color.white, Color.black, timer / fadeBlackDuration);
            _rend.material.color = newColor;
            
            float newFloat = Mathf.Lerp(1.0f, 0.0f, timer / fadeBlackDuration);
            _rend.material.SetFloat("_Glossiness",newFloat);
            // _rend.material.Lerp(objectColor, objectGlow, timer/fadeBlackDuration);
            
            timer += Time.deltaTime;
            yield return null;
        }

            
        _rend.material.color = Color.black;
    
    
    
        yield return null;
    }

    private IEnumerator SwitchMaterials()
    {
        float timer = 0.0f;
        
        while (timer <= fadeBlackDuration)
        {

            // Material newColor = objectColor;
            // newColor.Lerp(objectColor, objectGlow, timer / fadeBlackDuration);
            // _rend.material = newColor;
            
            timer += Time.deltaTime;
            yield return null;
        }


        // _rend.material = objectGlow;
    }
    
    
    private IEnumerator FadeToWhiteMaterial()
    {
        
        float timer = 0.0f;
        
        while (timer <= fadeBack2Normal)
        {
            Color newColor = fadeColor;
            newColor = Color.Lerp(Color.black, Color.white, timer / fadeBack2Normal);
            _rend.material.color = newColor;
            
            timer += Time.deltaTime;
            yield return null;
        }


        _rend.material.color = Color.white;
    
        yield return null;
    }
    
    // private IEnumerator FadeOutMaterial()
    // {
    //     float timer = 0.0f;
    //     float hue;
    //     float saturation;
    //     float value;
    //
    //
    //     Color.RGBToHSV(placeholderMat.color, out hue, out saturation, out value);
    //     
    //     
    //     while (timer <= fadeBlackDuration)
    //     {
    //         var newValue = Mathf.Lerp(100, 0, timer / fadeBlackDuration);
    //         Color newColor = Color.HSVToRGB(hue, saturation, newValue);
    //         
    //         // newColor = Color.HSVToRGB()
    //         placeholderMat.SetColor("_Color", newColor);
    //             
    //         timer += Time.deltaTime;
    //         yield return null;
    //     }
    //         
    //     Color newColor2 = fadeColor;
    //     newColor2 = Color.black;
    //             
    //     placeholderMat.SetColor("_Color", newColor2);
    //
    // }
    //
    // private IEnumerator FadeBackMaterial()
    // {
    //     float timer = 0.0f;
    //     float hue;
    //     float saturation;
    //     float value;
    //
    //
    //     Color.RGBToHSV(placeholderMat.color, out hue, out saturation, out value);
    //
    //
    //     while (timer <= fadeBack2Normal)
    //     {
    //         var newValue = Mathf.Lerp(0, 100, timer / fadeBlackDuration);
    //         Color newColor = Color.HSVToRGB(hue, saturation, newValue);
    //
    //         // newColor = Color.HSVToRGB()
    //         placeholderMat.SetColor("_Color", newColor);
    //
    //         timer += Time.deltaTime;
    //         yield return null;
    //     }
    //
    //     Color newColor2 = fadeColor;
    //     newColor2 = Color.white;
    //
    //     placeholderMat.SetColor("_Color", newColor2);
    //     yield return null;
    // }
    
    // private IEnumerator FadeOutRoutine()
    // {
    //     float timer = 0.0f;
    //     while (timer <= fadeOutDuration)
    //     {
    //         Color newColor = fadeColor;
    //         newColor.a = Mathf.Lerp(1, 0, timer / fadeOutDuration);
    //         _rend.material.SetColor("_Color", newColor);
    //         
    //         timer += Time.deltaTime;
    //         yield return null;
    //     }
    //     
    //     Color newColor2 = fadeColor;
    //     newColor2.a = 0;
    //         
    //     _rend.material.SetColor("_Color", newColor2);
    //     
    // }
    //
    // private IEnumerator FadeInRoutine()
    // {
    //     float timer = 0.0f;
    //     while (timer <= fadeInDuration)
    //     {
    //         Color newColor = fadeColor;
    //         newColor.a = Mathf.Lerp(0, 1, timer / fadeInDuration);
    //         _rend.material.SetColor("_Color", newColor);
    //         
    //         timer += Time.deltaTime;
    //         yield return null;
    //     }
    //     
    //     Color newColor2 = fadeColor;
    //     newColor2.a = 1;
    //         
    //     _rend.material.SetColor("_Color", newColor2);
    //     
    // }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitchMaterials : MonoBehaviour
{
    public Material material1;
    public Material material2;
    public float duration = 2.0f;
    public Renderer rend;
    public bool start = false;
    public float fadeBlackDuration = 2.0f;
    public Color fadeColor;
    public GameObject obj2Mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = obj2Mesh.GetComponent<SkinnedMeshRenderer>();
        rend.material = material1;
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            // ping-pong between the materials over the duration
            // float lerp = Mathf.PingPong(Time.time, duration) / duration;
            // switchingMaterial.Lerp(material1, material2, lerp);
            // rend.material = switchingMaterial;
            StartCoroutine(FadeToBlackMaterial());
            start = false;
        }

        
    }
    
    
    private IEnumerator FadeToBlackMaterial()
    {
        
        float timer = 0.0f;
        
        while (timer <= fadeBlackDuration)
        {
            // fade out color
            Color newColor = fadeColor;
            newColor = Color.Lerp(Color.white, Color.black, timer / fadeBlackDuration);
            // rend.material.color = newColor;
            rend.material.SetColor("_BaseColor", newColor);
            float newFloat = Mathf.Lerp(1.0f, 0.0f, timer / fadeBlackDuration);
            rend.material.SetFloat("_Smoothness",newFloat);
            rend.material.SetFloat("_BumpScale", newFloat);
            rend.material.SetFloat("_OcclusionStrength",newFloat);
            rend.material.SetFloat("_Metallic", newFloat);
            // _rend.material.Lerp(objectColor, objectGlow, timer/fadeBlackDuration);
            
            timer += Time.deltaTime;
            yield return null;
        }

            
        // rend.material.color = Color.black;
    
    
    
        yield return null;
    }
}

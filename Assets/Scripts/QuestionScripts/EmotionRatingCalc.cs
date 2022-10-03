using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class EmotionRatingCalc : MonoBehaviour
{

    public QuestionsManager questionsManager;
    public GameObject startPoint;

    public GameObject endPoint;
    public TextMeshPro ratingText;

    public float completeDistance;
    public float multiplier;
    public float emotionRating = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        completeDistance = Vector3.Distance(startPoint.GetComponent<Transform>().position,
            endPoint.GetComponent<Transform>().position);
        multiplier = completeDistance / 5;
    }

    // Update is called once per frame
    void Update()
    {
        emotionRating = Vector3.Distance(startPoint.GetComponent<Transform>().position,
            this.transform.position) / multiplier;
        ratingText.text = emotionRating.ToString("0.00");
        questionsManager.GetComponent<QuestionsManager>().currentEmotionRating = emotionRating;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionRatingCalc : MonoBehaviour
{
    public QuestionsManager questionsManager;
    public GameObject negPoint;
    public GameObject posPoint;

    public TextMeshPro ratingText2;

    private float _completeDistance;
    
    private float _multiplier;
    private float _connectionRating = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _completeDistance = Vector3.Distance(negPoint.GetComponent<Transform>().position,
            posPoint.GetComponent<Transform>().position);
        _multiplier = _completeDistance / 10;
    }

    // Update is called once per frame
    void Update()
    {
        _connectionRating = (Vector3.Distance(negPoint.GetComponent<Transform>().position,
            this.transform.position) / _multiplier) -5.0f;
        ratingText2.text = _connectionRating.ToString("0.00");
        
        QuestionsManager.Instance.connectionRating = _connectionRating;
    }
}

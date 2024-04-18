using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    #region References

    public GameObject _TeleportHitPoint1;
    public GameObject _TeleportHitPoint2;

    //teleportation training bools
    public bool hitpoint1 = false;
    public bool hitpoint2 = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //td: set active true for the objects

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "local_player")
        {
            if (gameObject == _TeleportHitPoint1)
            {
                hitpoint1 = true;
                Debug.Log("HitPoint1 reached");
            }
            if (gameObject == _TeleportHitPoint2) 
            {
                hitpoint2 = true;
                Debug.Log("HitPoint2 reached");
            }
        }
    }

   
}

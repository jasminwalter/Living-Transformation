using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarScaler : MonoBehaviour
{
    public Transform vrCamera;
    private float defaultHeight;
    private float playerHeight;
    public bool setPlayerHeight;

    public bool scaleAvatarNow = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scaleAvatarNow)
        {
            ScaleAvatar();
            scaleAvatarNow = false;
        }

        if (setPlayerHeight)
        {
            playerHeight = vrCamera.localPosition.y;
            Debug.Log("Player height " + playerHeight);
            setPlayerHeight = false;
        }
    }

    public void ScaleAvatar()
    {
        
        float scaleParam = playerHeight / defaultHeight;
        transform.localScale = new Vector3(scaleParam, scaleParam, scaleParam);
    }
    
}

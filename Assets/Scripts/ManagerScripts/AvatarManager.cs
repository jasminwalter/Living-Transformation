using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    [Header("Game Objects")] 
    public GameObject currentAvatar;

    public GameObject female1;
    public GameObject female2;
    public GameObject female3;
    public GameObject female4;
    
    public GameObject male1;
    public GameObject male2;
    public GameObject male3;
    public GameObject male4;
    
    public GameObject nonbinary1;
    public GameObject nonbinary2;
    public GameObject nonbinary3;
    public GameObject nonbinary4;

    public GameObject mirror;
    private Vector3 mirrorPosition = new Vector3(-11.11f,-48.41115f,8.718639f);
    private Quaternion mirrorRotation = Quaternion.Euler(90, 90, 0);
    private Vector3 mirrorMovePos = new Vector3(0f,-48.41115f,0f);
    private Quaternion mirrorMoveRot = Quaternion.Euler(90, 180, 0);
    
    void Start()
    {
        currentAvatar = female2;
        currentAvatar.SetActive(true);
    }

    public void StartSelection()
    {
        mirror.transform.position = mirrorMovePos;
        mirror.transform.rotation = mirrorMoveRot;
    }
    
    public void DisableAvatar()
    {
        currentAvatar.SetActive(false);
    }

    public void EndSelection()
    {
        mirror.transform.position = mirrorPosition;
        mirror.transform.rotation = mirrorRotation;
    }
}

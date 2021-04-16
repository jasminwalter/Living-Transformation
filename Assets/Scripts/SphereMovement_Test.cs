//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Controls for the non-VR debug camera
//
//=============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Camera ) )]
	public class SphereMovement_Test : MonoBehaviour
	{
		
		// script active if in 2D fallback mode
		// gaze sphere is moved according to mouse cursor position
		public Camera cam;
		public GameObject gazeSphere;
		
		// until here 
		public float speed = 4.0f;
		public float shiftSpeed = 16.0f;
		public bool showInstructions = true;
		public float RotateSpeed = 80f;

		private Vector3 startEulerAngles;
		private Vector3 startMousePosition;
		private float realTime;
		//-----------------------------------------------------
		void Start()
		{
			gazeSphere = GameObject.Find("local_gazeSphere");
        
		}

		//-------------------------------------------------
		void OnEnable()
		{
			realTime = Time.realtimeSinceStartup;
			
		}


		//-------------------------------------------------
		void Update()
		{
			// Raycasting and moving gaze sphere according to mouse position on screen

			Ray gazeRay = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(gazeRay, out hit, 100f))
			{
				gazeSphere.transform.position = hit.point;
			}

			// rotate player up down if arrows up down are pressed
			float forward = 0.0f;
			if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) )
			{

				transform.Rotate(-Vector3.right * RotateSpeed * Time.deltaTime);
				
			}
			if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) )
			{

				transform.Rotate(Vector3.right * RotateSpeed * Time.deltaTime);
			}

			// rotate player according to input via arrows left right (original code of player)
			if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) )
			{
				transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime,Space.World);
			}
			if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) )
			{
				transform.Rotate(-Vector3.up * RotateSpeed * Time.deltaTime,Space.World);
			}
			

			float realTimeNow = Time.realtimeSinceStartup;
			float deltaRealTime = realTimeNow - realTime;
			realTime = realTimeNow;

			// do a fast rotation if right mouse button is pressed
			Vector3 mousePosition = Input.mousePosition;

			if ( Input.GetMouseButtonDown( 1 ) /* right mouse */)
			{
				startMousePosition = mousePosition;
				startEulerAngles = transform.localEulerAngles;
			}

			if ( Input.GetMouseButton( 1 ) /* right mouse */)
			{
				Vector3 offset = mousePosition - startMousePosition;
				transform.localEulerAngles = startEulerAngles + new Vector3( -offset.y * 360.0f / Screen.height, offset.x * 360.0f / Screen.width, 0.0f );
			}

		}
		


		//-------------------------------------------------
		void OnGUI()
		{
			if ( showInstructions )
			{
				GUI.Label(new Rect(10.0f, 10.0f, 600.0f, 400.0f),
					"WASD or Arrow Keys to rotate the camera\n" +
					"Fast rotation with right mouse click\n");
			}
		}
	}
}

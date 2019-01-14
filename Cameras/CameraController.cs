﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    // Zoom In
    public float normalRate = 90f;
    public float zoomInRate = 72f;
    public float zoomInTime = 0.2f;
    public float zoomInDelay = 0f;
    public iTween.EaseType zoomInType = iTween.EaseType.spring;

    public float zoomStayTime = 1f;

    public float zoomOutTime = 0.2f;
    public float zoomOutDelay = 0f;
    public iTween.EaseType zoomOutType = iTween.EaseType.linear;

    public float rotate = 2f;

    public float shakeRotation = 1f;
    public float shakeTime = 0.1f;

    // Ref
    public CameraEffect cameraEffect;
    public Camera cam;
    public GameObject followTarget;

    [SerializeField]
    public bool isZooming = false;

	// Use this for initialization
	void Awake () 
    {
        cam = GetComponent<Camera>();
        cameraEffect = GetComponent<CameraEffect>();
	}

    private void Start()
    {
        followTarget = Object.FindObjectOfType<PlayerManager>().gameObject;
    }

    // Update is called once per frame
    void Update ()
    {
        if(!Commander.instance.IsBattle)
        {
            MoveCamera();
        }
	}

    void MoveCamera()
    {
        if(followTarget)
        {
            this.gameObject.transform.position = followTarget.transform.position + new Vector3(0, 0, -10f);
        }
        else
        {
            followTarget = PlayerManager.instance.gameObject;
            this.gameObject.transform.position = followTarget.transform.position + new Vector3(0, 0, -10f);
        }
    }

    public void BattleZoomIn()
    {
        if(isZooming || Commander.instance.IsActing)
        {
            return;
        }
        Debug.Log("Zoom Started!");
        StartCoroutine(BattleZoomInRoutine());
    }


    void UpdateZoom(float updateValue)
    {
        cam.fieldOfView = updateValue;
    }

    // If attack, rotate > 0, zoomInTime > zoomOutTime
    // If defense, rotate < 0, ZoomInTime < ZoomOutTime
    IEnumerator BattleZoomInRoutine()
    {
        isZooming = true;
        if(cameraEffect != null)
        {
            cameraEffect.EnableCameraBlur(true);
        }


        iTween.ValueTo(gameObject, iTween.Hash(
            "from", normalRate,
            "to", zoomInRate,
            "time", zoomInTime,
            "delay", zoomInDelay,
            "easetype", zoomInType,
            "unupdatetarget", gameObject,
            "onupdate", "UpdateZoom"
        ));

        iTween.RotateTo(gameObject, iTween.Hash(
            "z", rotate,
            "time", zoomInTime,
            "easetype", zoomInType
        ));

        yield return new WaitForSeconds(zoomInTime + zoomInDelay + zoomStayTime);

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", zoomInRate,
            "to", normalRate,
            "time", zoomOutTime,
            "delay", zoomOutDelay,
            "easetype", zoomOutType,
            "unupdatetarget", gameObject,
            "onupdate", "UpdateZoom"
        ));

        iTween.RotateTo(gameObject, iTween.Hash(
            "z", 0f,
            "time", zoomOutTime,
            "easetype", zoomOutType
        ));

        yield return new WaitForSeconds(zoomOutTime + zoomOutDelay);

        if(cameraEffect != null)
        {
            cameraEffect.EnableCameraBlur(false);
        }

        isZooming = false;
    }
}
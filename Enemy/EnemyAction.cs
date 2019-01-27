using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : CharacterAction {

    BaseEnemy baseEnemy;

    public void Initiate()
    {
        animator = GetComponent<Animator>();
        body = this.transform.GetChild(0).gameObject;
        cameraController = Camera.main.GetComponent<CameraController>();
        subCameraController = GameObject.FindWithTag("SubCamera").GetComponent<CameraController>();

        actInTime = cameraController.zoomInDelay;
        actStayTime = cameraController.zoomInTime + cameraController.zoomStayTime;
        actOutTime = cameraController.zoomOutTime;

        baseEnemy = GetComponent<BaseEnemy>();
    }



    // Do nothing
    protected override void Start()
    {
       
    }

    public override void MoveForwardAction()
    {

    }

    public override void MoveBackWardAction()
    {

    }

    public override void StopAction()
    {

    }
}

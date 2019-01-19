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
        moveInTime = cameraController.zoomInTime + cameraController.zoomStayTime;
        moveOutTime = cameraController.zoomOutTime;

        baseEnemy = GetComponent<BaseEnemy>();
    }


    protected override void MoveToStage()
    {
        SetMoveSpace(baseEnemy.Position);

        iTween.MoveBy(body, iTween.Hash(
            "x", moveSpace,
            "time", moveInTime
        ));
    }

    protected override void MoveToTargetStage()
    {
        body.transform.localPosition = new Vector3(targetMoveSpace, 0, 0);

        iTween.MoveBy(body, iTween.Hash(
            "x", -targetMoveSpace / 2,
            "time", moveInTime
        ));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour {

    protected Animator animator;
    protected GameObject body;

    public bool isActing = false;


    public float baseMoveSpace = 1f;
    public float correctionSpace = 3f;
    public float targetMoveSpace = 5f;
    // Calculate later on
    public float moveSpace;
    public float moveInTime;
    public float moveOutTime;
    //public iTween.EaseType moveEaseType;

    public float scaleTime = 0.1f;
    public float readyActionDelay = 0.35f;

    // Ref
    protected CameraController cameraController;
    protected CameraController subCameraController;
    protected BaseCharacter baseCharacter;

	// Use this for initialization
	protected virtual void Start () 
    {
        baseCharacter = GetComponent<BaseCharacter>();
        animator = GetComponent<Animator>();
        body = this.transform.GetChild(0).gameObject;
        cameraController = Camera.main.GetComponent<CameraController>();
        subCameraController = GameObject.FindWithTag("SubCamera").GetComponent<CameraController>();

        // Default Setting
        moveInTime = cameraController.zoomInTime + cameraController.zoomStayTime;
        moveOutTime = cameraController.zoomOutTime;
    }

    public void TestAct()
    {
        Act(ActionType.MainAttack);
    }

    public void TestHit()
    {
        Act(ActionType.Hit);
    }

    public void Act(ActionType actionType)
    {
        switch (actionType)
        {
            // Attack
            case ActionType.MainAttack:
                StartCoroutine(ActionRoutine(actionType.ToString()));
                break;

            case ActionType.SubAttack:
                StartCoroutine(ActionRoutine(actionType.ToString()));
                break;

            case ActionType.Buff:
                StartCoroutine(ActionRoutine(actionType.ToString()));
                break;

            // Defense
            case ActionType.Hit:
                StartCoroutine(HitRoutine(actionType.ToString()));
                break;

            // Do not move Position

            case ActionType.Idle:
                break;

            default:
                break;
        }
    }

    protected virtual IEnumerator ActionRoutine(string action)
    {
        // Zoom the camera
        baseCharacter.cursor.SetActive(false);
        cameraController.BattleZoomIn();
        subCameraController.BattleZoomIn();

        Commander.instance.IsActing = true;

        SwitchLayer("Actor");
        animator.SetTrigger(action);

        MoveToStage();

        yield return new WaitForSeconds(moveInTime);

        MoveBackToPosition();

        // Wait until move out action is over, and then reset its transform.position.
        // Wait more than moveOutTime by 0.5 second in order to avoid some transform related bug.
        yield return new WaitForSeconds(moveOutTime + 0.5f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger(action);
        SwitchLayer("Character");

        while(cameraController.isZooming || subCameraController.isZooming)
        {
            yield return null;
        }
        Commander.instance.IsActing = false;
    }

    protected virtual IEnumerator HitRoutine(string action)
    {

        SwitchLayer("Actor");
        animator.SetTrigger(action);

        MoveToTargetStage();

        yield return new WaitForSeconds(moveInTime);

        MoveFromTargetStage();

        // Wait until move out action is over, and then reset its transform.position.
        // Wait more than moveOutTime by 0.5 second in order to avoid some transform related bug.
        yield return new WaitForSeconds(moveOutTime + 0.5f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger(action);
        SwitchLayer("Character");
    }

    protected virtual void MoveToTargetStage()
    {
        body.transform.localPosition = new Vector3(targetMoveSpace, 0, 0);

        iTween.MoveBy(body, iTween.Hash(
            "x", -targetMoveSpace / 2,
            "time", moveInTime
        ));
    }

    protected virtual void MoveFromTargetStage()
    {
        var towardOriginVector = Vector3.zero - body.transform.localPosition;

        iTween.MoveBy(body, iTween.Hash(
            "x", towardOriginVector.x,
            "time", moveOutTime
        ));
    }

    // ActionRoutine Related
    protected virtual void MoveToStage()
    {
        SetMoveSpace(baseCharacter.Position);

        iTween.MoveBy(body, iTween.Hash(
            "x", moveSpace,
            "time", moveInTime
        ));
    }

    protected virtual void MoveBackToPosition()
    {
        iTween.MoveBy(body, iTween.Hash(
            "x", -moveSpace,
            "time", moveOutTime
        ));
    }

    protected virtual void SetMoveSpace(int currentPosition)
    {
        var temp = (float)currentPosition;
        moveSpace = baseMoveSpace + temp * correctionSpace;
    }

    protected virtual void SwitchLayer(string layerName)
    {
        this.gameObject.layer = LayerMask.NameToLayer(layerName);
        this.body.layer = LayerMask.NameToLayer(layerName);
    }



    // Apply this method to active unit and its confirmed targets
    public virtual void ReadyAction()
    {
        if(!Commander.instance.IsBattle)
        {
            return;
        }

        Debug.Log(this.gameObject.name + " is ready to act");
        StartCoroutine(ReadyRoutine());
    }

    protected virtual IEnumerator ReadyRoutine()
    {

        iTween.ScaleTo(body, iTween.Hash(
            "x", 1.2f,
            "y", 1.2f,
            "z", 1.2f,
            "time", scaleTime,
            "easetype", iTween.EaseType.linear
        ));

        yield return new WaitForSeconds(readyActionDelay);

        iTween.ScaleTo(body, iTween.Hash(
            "x", 1f,
            "y", 1f,
            "z", 1f,
            "time", scaleTime,
            "easetype", iTween.EaseType.linear
        ));
    }


    // Outside of battle
    public virtual void MoveForwardAction()
    {
        animator.SetBool("Walk", true);
        animator.SetFloat("Direction", 1);
    }

    public virtual void MoveBackWardAction()
    {
        animator.SetBool("Walk", true);
        animator.SetFloat("Direction", -1);
    }

    public virtual void StopAction()
    {
        animator.SetBool("Walk", false);
        animator.SetFloat("Direction", 1);
    }
}

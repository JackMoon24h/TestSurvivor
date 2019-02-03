using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour 
{
    protected Animator animator;
    protected GameObject body;

    public bool isActing = false;

    public float baseMoveSpace = 1f;
    public float correctionSpace = 3f;
    public float targetMoveSpace = 5f;

    // Attack Action Related =================================================
    public float actOffsetBase = -5.5f;
    public float actOffset = 3.5f;
    public float actMoveOffset = 7f;
    public float actInPos;
    public float actOutPos;
    public iTween.EaseType easeTypeIn = iTween.EaseType.linear;
    public iTween.EaseType easeTypeStay = iTween.EaseType.easeOutExpo;
    public iTween.EaseType easeTypeOut = iTween.EaseType.easeOutExpo;
    public float actInTime; // Time for moving to act start position
    public float actStayTime; // Time for moving to act end position
    public float actOutTime; // Time for moving to original position

    // Defense Action Related =================================================
    public Vector3 targetStage = new Vector3(5.5f, 0, 0);
    public Vector3 buffTargetStage = new Vector3(6.5f, 0, 0);
    public float targetMoveOffset = -2.5f;

    public float scaleTime = 0.1f;
    public float readyActionDelay = 0.35f;

    // Ref
    protected CameraController cameraController;
    protected CameraController subCameraController;
    protected Actor actor;

	// Use this for initialization
	protected virtual void Start () 
    {
        actor = GetComponent<Actor>();
        animator = GetComponent<Animator>();
        body = this.transform.GetChild(0).gameObject;
        cameraController = Camera.main.GetComponent<CameraController>();
        subCameraController = GameObject.FindWithTag("SubCamera").GetComponent<CameraController>();

        // Default Setting
        actInTime = cameraController.zoomInDelay;
        actStayTime = cameraController.zoomInTime + cameraController.zoomStayTime;
        actOutTime = cameraController.zoomOutTime;
    }

    // For Enemies Only
    public void Initiate()
    {
        actor = GetComponent<Actor>();
        animator = GetComponent<Animator>();
        body = this.transform.GetChild(0).gameObject;
        cameraController = Camera.main.GetComponent<CameraController>();
        subCameraController = GameObject.FindWithTag("SubCamera").GetComponent<CameraController>();
    }

    public void Act(ActionType actionType)
    {
        switch (actionType)
        {
            // Attack
            case ActionType.MainAttack:
                StartCoroutine(ActionRoutine(actionType.ToString()));
                break;

            case ActionType.Pick:
                StartCoroutine(ActionRoutine(actionType.ToString()));
                break;

                // Defensive Animation
            case ActionType.SubAttack:
                StartCoroutine(TargetActionRoutine(actionType.ToString()));
                break;

            case ActionType.Buff:
                StartCoroutine(BuffActionRoutine(actionType.ToString()));
                break;

            case ActionType.Hit:
                StartCoroutine(TargetActionRoutine(actionType.ToString()));
                break;

            case ActionType.CriticalHit:
                StartCoroutine(TargetActionRoutine(actionType.ToString()));
                break;

            case ActionType.Dodge:
                StartCoroutine(TargetActionRoutine(actionType.ToString()));
                break;

            // Do not move Position
            case ActionType.Buffed:
                StartCoroutine(BuffedActionRoutine());
                break;

            default:
                break;
        }
    }


    protected virtual IEnumerator ActionRoutine(string action)
    {
        // Zoom the camera
        actor.cursor.SetActive(false);
        cameraController.BattleZoomIn();
        subCameraController.BattleZoomIn();
        Commander.instance.IsActing = true;
        SwitchLayer("Actor");
        animator.SetTrigger(action);

        // Set
        SetActPosition(actor.Position);

        MoveToStage();
        yield return new WaitForSeconds(actInTime);

        MoveToDestination();
        yield return new WaitForSeconds(actStayTime);

        MoveBackToPosition();
        yield return new WaitForSeconds(actOutTime + 0.2f);

        // Just in case
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger(action);
        SwitchLayer("Character");

        while (cameraController.isZooming || subCameraController.isZooming)
        {
            yield return null;
        }
        Commander.instance.IsActing = false;
    }

    protected virtual IEnumerator BuffActionRoutine(string action)
    {
        actor.cursor.SetActive(false);
        cameraController.BattleZoomIn();
        subCameraController.BattleZoomIn();
        Commander.instance.IsActing = true;
        SwitchLayer("Actor");
        animator.SetTrigger(action);

        body.transform.localPosition = buffTargetStage;
        yield return new WaitForSeconds(actInTime);

        iTween.MoveBy(body, iTween.Hash(
            "x", -1f,
            "isLocal", true,
            "time", actStayTime,
            "easetype", easeTypeStay
        ));
        yield return new WaitForSeconds(actStayTime);

        MoveBackToPosition();
        yield return new WaitForSeconds(actOutTime + 0.2f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger(action);
        SwitchLayer("Character");

        while (cameraController.isZooming || subCameraController.isZooming)
        {
            yield return null;
        }
        Commander.instance.IsActing = false;
    }

    protected virtual IEnumerator TargetActionRoutine(string action)
    {

        SwitchLayer("Actor");
        animator.SetTrigger(action);

        body.transform.localPosition = targetStage;
        yield return new WaitForSeconds(actInTime);

        iTween.MoveBy(body, iTween.Hash(
            "x", targetMoveOffset,
            "isLocal", true,
            "time", actStayTime,
            "easetype", easeTypeStay
        ));
        yield return new WaitForSeconds(actStayTime);

        MoveBackToPosition();
        yield return new WaitForSeconds(actOutTime + 0.2f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger(action);
        SwitchLayer("Character");
    }

    protected virtual IEnumerator BuffedActionRoutine()
    {
        SwitchLayer("Actor");
        //animator.SetTrigger(action);

        body.transform.localPosition = buffTargetStage;
        yield return new WaitForSeconds(actInTime);

        iTween.MoveBy(body, iTween.Hash(
            "x", -1f,
            "isLocal", true,
            "time", actStayTime,
            "easetype", easeTypeStay
        ));
        yield return new WaitForSeconds(actStayTime);

        MoveBackToPosition();
        yield return new WaitForSeconds(actOutTime + 0.2f);
        body.transform.localPosition = Vector3.zero;

        //animator.ResetTrigger(action);
        SwitchLayer("Character");
    }

    protected virtual void MoveToStage()
    {
        // Move to Stage
        iTween.MoveTo(body, iTween.Hash(
            "x", actInPos,
            "isLocal", true,
            "time", actInTime,
            "easetype", easeTypeIn
        ));
    }

    protected virtual void MoveToDestination()
    {
        // Move Forward!
        iTween.MoveTo(body, iTween.Hash(
            "x", actOutPos,
            "isLocal", true,
            "time", actStayTime,
            "easetype", easeTypeStay
        ));
    }

    // Common Action
    protected virtual void MoveBackToPosition()
    {
        // Move back to original position
        iTween.MoveTo(body, iTween.Hash(
            "x", Vector3.zero.x,
            "isLocal", true,
            "time", actOutTime,
            "easetype", easeTypeOut
        ));
    }

    protected virtual void SetActPosition(int currentPosition)
    {
        actInPos = actOffset * (currentPosition - 1) + actOffsetBase;
        actOutPos = actInPos + actMoveOffset;

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

    public void Dead()
    {
        StartCoroutine(DeadRoutine());
    }

    public IEnumerator DeadRoutine()
    {
        SwitchLayer("Actor");
        animator.SetTrigger("Dead");

        body.transform.localPosition = targetStage;
        yield return new WaitForSeconds(actInTime);

        iTween.MoveBy(body, iTween.Hash(
            "x", targetMoveOffset,
            "isLocal", true,
            "time", actStayTime,
            "easetype", easeTypeStay
        ));
        yield return new WaitForSeconds(actStayTime);

        MoveBackToPosition();
        yield return new WaitForSeconds(actOutTime + 0.2f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger("Dead");
        SwitchLayer("Character");

        if (this.gameObject.tag == "Enemy")
        {
            EnemyManager.instance.characterList.RemoveAt(this.actor.Position - 1);
            EnemyManager.instance.SetPositions(EnemyManager.instance.characterList);
            Commander.instance.narrator.Narrate("Singular Strike...!!");
        }
        else
        {
            PlayerManager.instance.characterList.RemoveAt(this.actor.Position - 1);
            PlayerManager.instance.SetPositions(PlayerManager.instance.characterList);
            Commander.instance.narrator.Narrate("Slowly, gently...life is taken...");
        }

        Destroy(this.gameObject);
        Commander.instance.IsActing = false;
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

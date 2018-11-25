using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour {

    Animator animator;
    GameObject body;
    public bool isActing = false;
    public float moveInTime;
    public float moveOutTime;
    public float baseMoveSpace = 1f;
    public float correctionSpace = 2f;
    public float moveSpace;
    public float scaleTime = 0.1f;

    // Ref
    CameraController cameraController;
    CameraController subCameraController;
    BaseCharacter baseCharacter;

	// Use this for initialization
	void Start () 
    {
        animator = GetComponent<Animator>();
        body = this.transform.GetChild(0).gameObject;
        baseCharacter = GetComponent<BaseCharacter>();
        cameraController = Camera.main.GetComponent<CameraController>();
        subCameraController = GameObject.FindWithTag("SubCamera").GetComponent<CameraController>();
        moveInTime = cameraController.zoomInTime + cameraController.zoomStayTime;
        moveOutTime = cameraController.zoomOutTime;
    }

    void MoveToStage()
    {
        SetMoveSpace(baseCharacter.Position);

        iTween.MoveBy(body, iTween.Hash(
            "x", moveSpace,
            "time", moveInTime
        ));
    }

    void MoveBackToPosition()
    {
        iTween.MoveBy(body, iTween.Hash(
            "x", -moveSpace,
            "time", moveOutTime
        ));
    }

    void SetMoveSpace(int currentPosition)
    {
        var temp = (float)currentPosition;
        moveSpace = baseMoveSpace + temp * correctionSpace;
    }

    void SwitchLayer(string layerName)
    {
        this.gameObject.layer = LayerMask.NameToLayer(layerName);
        this.body.layer = LayerMask.NameToLayer(layerName);
    }

    public void Hit()
    {
        StartCoroutine(HitRoutine());

    }

    IEnumerator HitRoutine()
    {
        while (!cameraController.isZooming || !subCameraController.isZooming)
        {
            yield return null;
        }

        SwitchLayer("Actor");
        animator.SetTrigger("Hit");

        MoveToStage();

        yield return new WaitForSeconds(moveInTime);

        MoveBackToPosition();

        // Wait until move out action is over, and then reset its transform.position.
        // Wait more than moveOutTime by 0.5 second in order to avoid some transform related bug.
        yield return new WaitForSeconds(moveOutTime + 0.5f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger("Hit");
        SwitchLayer("Character");
    }

    public void Shoot()
    {
        Debug.Log("Shoot!");
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while(!cameraController.isZooming || !subCameraController.isZooming)
        {
            yield return null;
        }

        Commander.instance.IsActing = true;

        SwitchLayer("Actor");
        animator.SetTrigger("Shoot");

        MoveToStage();

        yield return new WaitForSeconds(moveInTime);

        MoveBackToPosition();

        // Wait until move out action is over, and then reset its transform.position.
        // Wait more than moveOutTime by 0.5 second in order to avoid some transform related bug.
        yield return new WaitForSeconds(moveOutTime + 0.5f);
        body.transform.localPosition = Vector3.zero;

        animator.ResetTrigger("Shoot");
        SwitchLayer("Character");

        Commander.instance.IsActing = false;
    }


    public void ReadyAction()
    {
        Debug.Log(this.gameObject.name + " is ready to act");
        StartCoroutine(ReadyRoutine());
    }

    IEnumerator ReadyRoutine()
    {
        iTween.ScaleTo(body, iTween.Hash(
            "x", 1.2f,
            "y", 1.2f,
            "z", 1.2f,
            "time", scaleTime,
            "easetype", iTween.EaseType.linear
        ));

        yield return new WaitForSeconds(0.2f);

        iTween.ScaleTo(body, iTween.Hash(
            "x", 1f,
            "y", 1f,
            "z", 1f,
            "time", scaleTime,
            "easetype", iTween.EaseType.linear
        ));
    }

    public void MoveForwardAction()
    {
        animator.SetBool("Walk", true);
        animator.SetFloat("Direction", 1);
    }

    public void MoveBackWardAction()
    {
        animator.SetBool("Walk", true);
        animator.SetFloat("Direction", -1);
    }

    public void StopAction()
    {
        animator.SetBool("Walk", false);
        animator.SetFloat("Direction", 1);
    }
}

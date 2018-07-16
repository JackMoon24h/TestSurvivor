using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimManager : MonoBehaviour 
{
    GameManager gameManager;
    SquadManager squadManager;
    [HideInInspector]public Animator animator;
    [HideInInspector]public Animator enemyAnimator;
    Camera mainCamera;

    public iTween.EaseType easeTypeIn = iTween.EaseType.linear;
    public iTween.EaseType easeTypeStay = iTween.EaseType.easeOutExpo;
    public iTween.EaseType easeTypeOut = iTween.EaseType.linear;

    public float easeTimeIn = 2f;
    public float easeTimeStay = 2f;
    public float easeTimeOut = 2f;

    public float delay = 2.5f;

    public Vector3 offsetOriginToStart = new Vector3(-3.5f, 0f, 0f);
    public Vector3 offsetStartToEnd = new Vector3(6f, 0f, 0f);
    public Vector3 offsetEndToOrigin = new Vector3(-2.5f, 0f, 0f);

    public Vector3 originPos;
    public Vector3 actStartPos;
    public Vector3 actEndPos;

    public Vector3 targetOriginPos;
    public Vector3 targetActStartPos;
    public Vector3 targetActEndPos;

    float scaleX = 1.3f;
    float scaleY = 1.3f;

	// Use this for initialization
	void Awake () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(squadManager.squadMover.isMoving)
        {
            animator.SetBool("walk", true);
            if(squadManager.squadInput.Direction > 0f)
            {
                animator.SetFloat("direction", 1);
            }
            else 
            {
                animator.SetFloat("direction", -1);
            }
        }
        else
        {
            animator.SetBool("walk", false);
        }
	}


    // This should be called after battle started
    void GetActPos()
    {
        this.originPos = this.transform.position;
        this.actStartPos = this.originPos + offsetOriginToStart;
        this.actEndPos = this.actStartPos + offsetStartToEnd;
    }

    void GetTargetActPos(List<GameObject> targets)
    {
        
    }

    public IEnumerator PickRoutine(GameObject item)
    {
        animator.SetTrigger("pick");
        gameManager.UpdateNarration("Picking....");

        yield return new WaitForSeconds(delay);

        animator.ResetTrigger("pick");
        gameManager.UpdateNarration("Pick Ended");
    }

    public IEnumerator ShootRoutine(List<GameObject> target)
    {
        animator.SetTrigger("skill_shoot");

        gameManager.UpdateNarration("Annimation Comes Here");

        iTween.ShakeRotation(gameManager.mainCamera.gameObject, iTween.Hash(
            "z", 3f,
            "time", 1f,
            "delay", 0f
        ));


        yield return new WaitForSeconds(delay);
        gameManager.IsActing = false;
        animator.ResetTrigger("skill_shoot");
        gameManager.UpdateNarration("Next Turn");
    }

    void ZoomCamera()
    {
        
    }

    //public IEnumerator ShootRoutine(List<GameObject> targets)
    //{

    //    animator.SetTrigger("skill_shoot");

    //    foreach (var enemy in targets)
    //    {
    //        var enemyAnim = enemy.transform.GetChild(0).GetComponent<Animator>();
    //        enemyAnim.SetTrigger("Hit");
    //    }


    //    // Move to Start position

    //    iTween.MoveBy(this.gameObject, iTween.Hash(
    //        "amount", offsetOriginToStart,
    //        "time", easeTimeIn,
    //        "easetype", easeTypeIn
    //    ));

    //    Debug.Log("Moved to Start Stage");

    //    iTween.MoveTo(gameObject, iTween.Hash(
    //        "x", actStartPos.x,
    //        "y", actStartPos.y,
    //        "z", actStartPos.z,
    //        "time", easeTimeIn,
    //        "easetype", easeTypeIn
    //    ));

    //    foreach(var enemy in targets)
    //    {
    //        iTween.MoveTo(enemy, iTween.Hash(
    //            "x", targetActStartPos.x,
    //            "y", targetActStartPos.y,
    //            "z", targetActStartPos.z,
    //            "time", easeTimeIn,
    //            "easetype", easeTypeIn
    //        ));
    //    }

    //    // Move to End position

    //    iTween.MoveBy(this.gameObject, iTween.Hash(
    //        "amount", offsetStartToEnd,
    //        "time", easeTimeIn,
    //        "easetype", easeTypeIn
    //    ));

    //    Debug.Log("Moved to End Stage");

    //    iTween.MoveTo(gameObject, iTween.Hash(
    //        "x", actEndPos.x,
    //        "y", actEndPos.y,
    //        "z", actEndPos.z,
    //        "time", easeTimeStay,
    //        "easetype", easeTypeStay
    //    ));

    //    foreach (var enemy in targets)
    //    {
    //        iTween.MoveTo(enemy, iTween.Hash(
    //            "x", targetActEndPos.x,
    //            "y", targetActEndPos.y,
    //            "z", targetActEndPos.z,
    //            "time", easeTimeStay,
    //            "easetype", easeTimeStay
    //        ));
    //    }

    //    // Scale
    //    iTween.ScaleTo(this.gameObject, iTween.Hash(
    //        "x", 2f,
    //        "y", 2f,
    //        "time", 1f,
    //        "delay", 0.1f,
    //        "easetype", easeTypeStay
    //    ));

    //    Debug.Log("Scaled");

    //    foreach (var enemy in targets)
    //    {
    //        iTween.ScaleTo(enemy, iTween.Hash(
    //            "x", scaleX,
    //            "y", scaleY,
    //            "time", 1f,
    //            "delay", 0.1f,
    //            "easetype", easeTypeStay
    //        ));
    //    }

    //    // Shake Camera

    //    iTween.ShakeRotation(gameManager.mainCamera.gameObject, iTween.Hash(
    //        "z", 3f,
    //        "time", 1f,
    //        "delay", 0.5f
    //    ));

    //    // Back to original position

    //    iTween.MoveBy(this.gameObject, iTween.Hash(
    //        "amount", offsetEndToOrigin,
    //        "time", easeTimeOut,
    //        "easetype", easeTypeOut
    //    ));

    //    Debug.Log("Moved to Original Stage");

    //    iTween.MoveTo(gameObject, iTween.Hash(
    //        "x", originPos.x,
    //        "y", originPos.y,
    //        "z", originPos.z,
    //        "time", easeTimeOut,
    //        "easetype", easeTypeOut
    //    ));

    //    foreach (var enemy in targets)
    //    {
    //        iTween.MoveTo(enemy, iTween.Hash(
    //            "x", targetOriginPos.x,
    //            "y", targetOriginPos.y,
    //            "z", targetOriginPos.z,
    //            "time", easeTimeOut,
    //            "easetype", easeTypeOut
    //        ));
    //    }

    //    // Back to original scale

    //    iTween.ScaleTo(this.gameObject, iTween.Hash(
    //        "x", 1f,
    //        "y", 1f,
    //        "time", 1f,
    //        "delay", 0.1f,
    //        "easetype", easeTypeOut
    //    ));

    //    foreach (var enemy in targets)
    //    {
    //        iTween.ScaleTo(enemy, iTween.Hash(
    //            "x", 1f,
    //            "y", 1f,
    //            "time", 1f,
    //            "delay", 0.1f,
    //            "easetype", easeTypeOut
    //        ));
    //    }

    //    yield return new WaitForSeconds(delay);

    //    animator.ResetTrigger("skill_shoot");

    //    foreach (var enemy in targets)
    //    {
    //        var enemyAnim = enemy.transform.GetChild(0).GetComponent<Animator>();
    //        enemyAnim.ResetTrigger("Hit");
    //    }

    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.IsActing = false;

    //    gameManager.UpdateNarration("How did you feel...?");
    //}
}

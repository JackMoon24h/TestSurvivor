using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour 
{

    Camera mainCamera;
    private bool m_inputEnabled;
    public bool InputEnabled
    {
        get
        {
            return m_inputEnabled;
        }

        set
        {
            m_inputEnabled = value;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(m_inputEnabled)
        {
            // This is the method of getting object by touch for [Perspective Camera]
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 screenPos = Input.mousePosition;
                //Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
                Ray ray = mainCamera.ScreenPointToRay(screenPos);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null)
                {
                    PlayerManager.instance.clickedObject = this.GetObjectInfo(hit.collider);

                    switch(PlayerManager.instance.clickedObject.tag)
                    {
                        case "Survivor":
                            if(!Commander.instance.IsBattle)
                            {
                                var temp = PlayerManager.instance.clickedObject.GetComponent<BaseCharacter>();

                                if(PlayerManager.instance.swapBtn.isBtnPressed && temp.isSwapTarget)
                                {
                                    PlayerManager.instance.Swap(PlayerManager.instance.characterList, PlayerManager.instance.activeCharacter.Position, temp.Position);
                                }
                                else if(PlayerManager.instance.swapBtn.isBtnPressed && !temp.isSwapTarget)
                                {
                                    // Do Nothing
                                }
                                else
                                {
                                    PlayerManager.instance.SetActiveCharacter(temp);
                                    Debug.Log("Active Character : " + PlayerManager.instance.activeCharacter.gameObject.name);
                                }
                            }
                            else
                            {
                                var pTarget = PlayerManager.instance.clickedObject.GetComponent<BaseCharacter>();
                                if(pTarget.isTargeted)
                                {
                                    PlayerManager.instance.ConfirmAllyTarget(pTarget);
                                }
                                else if (PlayerManager.instance.swapBtn.isBtnPressed && pTarget.isSwapTarget)
                                {
                                    PlayerManager.instance.Swap(PlayerManager.instance.characterList, PlayerManager.instance.activeCharacter.Position, pTarget.Position);
                                }
                            }
                            break;
                        case "Enemy":
                            // Player can click enemies only in the battle
                            var eTarget = PlayerManager.instance.clickedObject.GetComponent<BaseEnemy>();
                            if(eTarget.isTargeted)
                            {
                                // Player set this enemy as a target and confirmed its command
                                PlayerManager.instance.ConfirmEnemyTarget(eTarget);
                            }
                            else
                            {
                                Debug.Log("You clicked an Enemy");
                            }

                            break;
                        case "Object":
                            if(!Commander.instance.IsBattle)
                            {
                                Debug.Log("Clicked Object : " + PlayerManager.instance.clickedObject.name);
                                // Investigate the object
                            }
                            // If in the battle, ignore it
                            break;
                        default:
                            Debug.Log("Default :: Clicked Object : " + PlayerManager.instance.clickedObject.name);
                            break;
                    }
                }
                else
                {
                    Debug.Log("No Object Found");
                }
            }
        }

    }

    public GameObject GetObjectInfo(Collider2D col)
    {
        return col.gameObject;
    }
}

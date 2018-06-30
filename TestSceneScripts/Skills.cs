using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour {

	Manager gameManager;
	BaseSurvivor caster;
	GameObject statusWindow;

	[SerializeField]List<GameObject> existingTargetCursors = new List<GameObject>();

	public enum Target
	{
		SELF,
		SURVIVOR,
		SURVIVORGROUP,
		ENEMY,
		ENEMYGROUP
	}

	public enum Type
	{
		DAMAGE,
		HEAL, // Heal Health
		CURE, // Cure Mental
		STATUSCHANGE,
	}

	// Add effect when skill type is status-change
	public enum Effect
	{
		NONE,
		BUFF,
		INFECT,
		BLEED,
		STUN,
		FEAR,
		MOVE
	}
		
	public Target skillTarget;
	public bool[] availablePositions = new bool[4];
	public bool[] castRange = new bool[4];

	public Type skillType;
	public Effect skillEffect;

	void Start(){
		gameManager = Manager.instance;
		statusWindow = GameObject.Find ("Window");
	}

	public void OnClickEnter(){
		this.DrawTarget ();
	}

	public void SetCaster(BaseSurvivor survivor){
		this.caster = survivor;
	}

	public void DrawTarget(){

		this.gameManager.confirmBtn.SetActive (false);

		if (!gameManager.isBattle)
			return;

		if (gameManager.activeSurvivor != this.caster)
			return;

		if(gameManager.targetCursors.Count > 0){
			for(int i = 0; i < gameManager.targetCursors.Count; i++){
				Destroy(gameManager.targetCursors[i].gameObject);
			}
			gameManager.targetCursors.Clear ();
			gameManager.enemyTargetList.Clear ();
		}

		// If caster is in the right position, draw its target range
		if(this.availablePositions[caster.currentPos - 1])
		{
			for(int i = 0; i < castRange.Length; i++){
				if(castRange[i]){
					var target = gameManager.GetEnemyInPos (i+1);
					if(target)
					{
						target.isTargeted = true;
						// In order for instantiated cursor which uses animation to spawn at the right position, you must assign it to a gameobject. Unless you do that, the cursor will spawn always at the same position because of animation
						gameManager.targetCursors.Add(Instantiate (gameManager.targetCursorPrefab, target.transform.position + new Vector3(0.25f, 3.75f, 0f), Quaternion.identity));
						gameManager.enemyTargetList.Add (target);

						this.caster.turnState = BaseSurvivor.TurnState.SELECTING;
						this.gameManager.confirmBtn.SetActive (true);
						this.gameManager.activeSkill = this;
					}
				}
			}
		}
	}




}

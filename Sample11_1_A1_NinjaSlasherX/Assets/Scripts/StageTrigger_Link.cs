﻿using UnityEngine;
using System.Collections;

public class StageTrigger_Link : MonoBehaviour {

	// === 外部パラメータ（インスペクタ表示） =====================
	public string 	jumpSceneName;
	public string 	jumpLabelName;

	public int 		jumpDir 			= 0;
	public bool		jumpInput 			= true; // fales = AutoJump
	public float	jumpDelayTime 		= 0.0f;

	// === 内部パラメータ ======================================
	Transform		 playerTrfm;
	PlayerController playerCtrl;
	
	// === コード（Monobehaviour基本機能の実装） ================
	void Awake() {
		playerTrfm = PlayerController.GetTranform();
		playerCtrl = playerTrfm.GetComponent<PlayerController> ();
	}

	void OnTriggerEnter2D_PlayerEvent (GameObject go) {
		if (!jumpInput) {
			Jump ();
		}
	}

	// === コード（シーンジャンプの実装） ========================
	public void Jump() {
		// ジャンプ先設定
		if (jumpSceneName == "") {
			jumpSceneName = Application.loadedLevelName;
		}

		// チェックポイント
		PlayerController.checkPointEnabled   = true;
		PlayerController.checkPointLabelName = jumpLabelName;
		PlayerController.checkPointSceneName = jumpSceneName;
		PlayerController.checkPointHp 		 = PlayerController.nowHp;

		playerCtrl.ActionMove (0.0f);
		playerCtrl.activeSts = false;

		Invoke("JumpWork",jumpDelayTime);
	}

	void JumpWork() {
		playerCtrl.activeSts = true;

		if (Application.loadedLevelName == jumpSceneName) {
			// シーン内ジャンプ
			GameObject[] stageLinkList = GameObject.FindGameObjectsWithTag ("EventTrigger");
			foreach (GameObject stageLink in stageLinkList) {
				if (stageLink.GetComponent<StageTrigger_CheckPoint>().labelName == jumpLabelName) {
					playerTrfm.position = stageLink.transform.position;
					playerCtrl.groundY 	= playerTrfm.position.y;
					Camera.main.transform.position = new Vector3(playerTrfm.position.x,playerTrfm.position.y,-10.0f);
					break;
				}
			}
		} else {
			// シーン外ジャンプ
			Application.LoadLevel (jumpSceneName);
		}
	}
}

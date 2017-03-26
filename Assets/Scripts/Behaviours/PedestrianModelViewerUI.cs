﻿using UnityEngine;
using System.Collections;
using SanAndreasUnity.Behaviours;
using SanAndreasUnity.Importing.Animation;

public class PedestrianModelViewerUI : MonoBehaviour {

	public	Pedestrian	pedestrian = null ;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void loadModel(int id, AnimGroup group, AnimIndex index) {
		pedestrian.Load (id);
		pedestrian.PlayAnim (group, index, PlayMode.StopAll);
	}

	void	OnGUI() {

		if (null == pedestrian)
			return;

		// ----------------------------------------------------------------------

		GUILayout.Label ("Current model ID: " + pedestrian.PedestrianId);
		GUILayout.Label ("Current model name: " + ((pedestrian.Definition != null) ? pedestrian.Definition.ModelName : "(null!)"));

		if (GUILayout.Button ("Next model")) {
			loadModel (pedestrian.PedestrianId + 1, pedestrian.AnimGroup, pedestrian.animIndex);
			// TODO max model count?
		}

		if (GUILayout.Button ("Previous model")) {
			int newId = pedestrian.PedestrianId - 1;
			if (newId < 0) {
				newId = 0;
			}
			loadModel (newId, pedestrian.AnimGroup, pedestrian.animIndex);
		}

		// ----------------------------------------------------------------------

		GUILayout.Label ("Current anim group: " + pedestrian.AnimGroup);

		if (GUILayout.Button ("Next anim group")) {
			AnimGroup newGroup = pedestrian.AnimGroup + 1;
			if (newGroup >= AnimGroup.MyWalkCycle) {
				newGroup = AnimGroup.WalkCycle;
			}
			pedestrian.animIndex = (newGroup == AnimGroup.WalkCycle) ? AnimIndex.Idle : AnimIndex.Walk;
			loadModel (pedestrian.PedestrianId, newGroup, pedestrian.animIndex);
		}

		if (GUILayout.Button ("Previous anim group")) {
			AnimGroup newGroup = pedestrian.AnimGroup - 1;
			if (newGroup <= AnimGroup.None) {
				newGroup = AnimGroup.Car;
			}
			pedestrian.animIndex = (newGroup == AnimGroup.WalkCycle) ? AnimIndex.Idle : AnimIndex.Walk;
			loadModel (pedestrian.PedestrianId, newGroup, pedestrian.animIndex);
		}

		// ----------------------------------------------------------------------
		/*
		GUILayout.Label ("Current anim index: " + (int)pedestrian.AnimGroup);

		if (GUILayout.Button ("Next anim index")) {
			AnimIndex newIndex = pedestrian.animIndex + 1;
			if (newIndex >= AnimGroup.MyWalkCycle) {
				newIndex = AnimGroup.WalkCycle;
			}
			loadModel (pedestrian.PedestrianId, pedestrian.AnimGroup, newIndex);
		}

		if (GUILayout.Button ("Previous anim index")) {
			AnimIndex newIndex = pedestrian.animIndex - 1;
			if (newIndex <= AnimGroup.None) {
				newIndex = AnimGroup.Car;
			}
			loadModel (pedestrian.PedestrianId, pedestrian.AnimGroup, newIndex);
		}
		*/
	}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestSkinnedMesh : MonoBehaviour
{
	[SerializeField]
	SkinnedMeshRenderer original;

	#region UNITYC_CALLBACK

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//UpdateMeshRenderer(original);
		}
	}

//#if UNITY_EDITOR
//	void OnDrawGizmosSelected()
//	{
//		var meshrenderer = GetComponentInChildren<SkinnedMeshRenderer>();
//		Vector3 before = meshrenderer.bones[0].position;
//		for (int i = 0; i < meshrenderer.bones.Length; i++)
//		{
//			Gizmos.DrawLine(meshrenderer.bones[i].position, before);
//			UnityEditor.Handles.Label(meshrenderer.bones[i].transform.position, i.ToString());
//			before = meshrenderer.bones[i].position;
//		}
//	}
//#endif

	#endregion


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LuaZygote : LuaClient
{
	protected new void Awake()
	{
		Instance = this;
	}

	protected System.Collections.IEnumerator Start()
	{
		yield return null;
		Init();
#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded += OnSceneLoaded;
#endif
	}

}

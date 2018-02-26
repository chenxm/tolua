using App;
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
		Debug.Log(LuaConst.luaResDir);
#if !UNITY_EDITOR
		FileUpdater updater = new FileUpdater();
		yield return updater.Update(LuaConst.luaResDir, "http://10.0.16.223/appdata/lua", "test", 3);
#else
		yield return null;
#endif
		LuaResLoader loader = new LuaResLoader();
		Init();
		loader.PrintSearchPath();


#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded += OnSceneLoaded;
#endif
	}


}

using UnityEngine;
using System;
using System.Collections;
using LuaInterface;

//使用Unity的Coroutine机制
public class TestCoroutineQueue : MonoBehaviour 
{
    public TextAsset luaFile = null;
    private LuaState lua = null;

	void Awake () 
    {
#if UNITY_5 || UNITY_2017
		Application.logMessageReceived += ShowTips;
#else
        Application.RegisterLogCallback(ShowTips);
#endif        
        new LuaResLoader();
        lua  = new LuaState();
        lua.Start();
		LuaBinder.Bind(lua);
		DelegateFactory.Init();

		lua["GlobalBehaviour"] = this;

		lua.DoString(luaFile.text, "TestLuaCoroutineQueue.lua");
        LuaFunction f = lua.GetFunction("TestCortinue");
        f.Call();
        f.Dispose();
        f = null;        
    }

    void OnApplicationQuit()
    {
        lua.Dispose();
        lua = null;
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif
    }

    string tips = null;

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

}

//#define FAKE_GENERATE

using System;
using UnityEngine;
using LuaInterface;
#if FAKE_GENERATE
public static class LuaBinder
{
	public static void Bind(LuaState L)
	{
		throw new NotImplementedException();
	}
}

public class DelegateFactory
{
	public delegate Delegate DelegateCreate(LuaFunction func, LuaTable self, bool flag);
	public static System.Collections.Generic.Dictionary<Type, DelegateCreate> dict = new System.Collections.Generic.Dictionary<Type, DelegateCreate>();

	public static void Init()
	{
		throw new NotImplementedException();
	}

	public static Delegate CreateDelegate(Type t, LuaFunction func = null)
	{
		throw new NotImplementedException();
	}

	public static Delegate RemoveDelegate(Delegate obj, LuaFunction func)
	{
		throw new NotImplementedException();
	}

	public static Delegate RemoveDelegate(Delegate obj, Delegate dg)
	{
		throw new NotImplementedException();
	}
}
#endif
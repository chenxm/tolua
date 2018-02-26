using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;


public class LuaCoroutineQueue
{
	private List<object> routines = new List<object>();

	public UnityEngine.Coroutine Start(MonoBehaviour mb)
	{
		return mb.StartCoroutine(Run());
	}

	public void Join(object routine)
	{
		this.routines.Add(routine);
	}

	public void Clear()
	{
		this.routines.Clear();
	}

	private IEnumerator Run()
	{
		for (int i = 0; i < this.routines.Count; i++)
		{
			object r = this.routines[i];
			if (r is IEnumerator)
			{
				yield return r;
			}
			else if (r is YieldInstruction)
			{
				yield return r;
			}
			else if (r is LuaFunction)
			{
				((LuaFunction)r).Call();
			}
		}
	}
}

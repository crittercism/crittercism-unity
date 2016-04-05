#if UNITY_IPHONE

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

public static class TestLib
{
	[DllImport("__Internal")]
	private static extern void TestLib_crashMe ();

	public static void crashMe ()
	{
		try {
			TestLib_crashMe();
		} catch {
			Debug.Log ("TestLib Unity plugin failed to initialize.");
		}
	}
}

#endif

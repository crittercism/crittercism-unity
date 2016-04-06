#if !UNITY_EDITOR && UNITY_IOS
#define CRITTERCISM_IOS
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

public static class CrittercismIOS
{
#if CRITTERCISM_IOS
	[DllImport("__Internal")]
	private static extern void Crittercism_EnableWithAppID (string appID, bool enableServiceMonitoring);

	[DllImport("__Internal")]
	private static extern bool Crittercism_LogHandledException (string name, string reason, string stack, int platformId);

	[DllImport("__Internal")]
	private static extern void Crittercism_LogUnhandledException (string name, string reason, string stack, int platformId);

	[DllImport("__Internal")]
	private static extern bool Crittercism_LogNetworkRequest(string method,
	                                                         string uriString,
	                                                         double latency,
	                                                         int bytesRead,
	                                                         int bytesSent,
	                                                         int responseCode,
	                                                         int errorCode);
	
	[DllImport("__Internal")]
	private static extern void Crittercism_SetAsyncBreadcrumbMode (bool writeAsync);

	[DllImport("__Internal")]
	private static extern void Crittercism_LeaveBreadcrumb (string breadcrumb);

	[DllImport("__Internal")]
	private static extern void Crittercism_SetUsername (string key);

	[DllImport("__Internal")]
	private static extern void Crittercism_SetValue (string value, string key);

	[DllImport("__Internal")]
	private static extern void Crittercism_SetOptOutStatus (bool status);

	[DllImport("__Internal")]
	private static extern bool Crittercism_DidCrashOnLastLoad();
	
	[DllImport("__Internal")]
	private static extern bool Crittercism_GetOptOutStatus ();

	[DllImport("__Internal")]
	private static extern void Crittercism_BeginUserflow (string name);

	[DllImport("__Internal")]
	private static extern void Crittercism_BeginUserflowWithValue (string name, int value);

	[DllImport("__Internal")]
	private static extern void Crittercism_EndUserflow (string name);

	[DllImport("__Internal")]
	private static extern void Crittercism_FailUserflow (string name);
	
	[DllImport("__Internal")]
	private static extern void Crittercism_CancelUserflow (string name);

	[DllImport("__Internal")]
	private static extern void Crittercism_SetUserflowValue (string name, int value);

	[DllImport("__Internal")]
	private static extern int Crittercism_GetUserflowValue (string name);
#endif // CRITTERCISM_IOS

	// Crittercism-ios CRPluginException.h defines crPlatformId crUnityId = 0 .
	private const int crUnityId = 0;
	
	// Reporting uncaught C# Exception's as crashes (red blips)?
	private static volatile bool logUnhandledExceptionAsCrash = false;

	/// <summary>
	/// Initializes Crittercism.  Crittercism must be initialized before any other calls may be
	/// made to Crittercism.  Once Crittercism is initialized, any crashes will be reported to
	/// Crittercism.
	/// </summary>
	/// <param name="appID">A Crittercism app identifier.  The app identifier may be found
	/// in the Crittercism web portal under "App Settings".</param>
	public static void Init (string appID)
	{
#if CRITTERCISM_IOS
		if (appID == null) {
			Debug.Log ("Crittercism given a null app ID");
			return;
		}
		try {
			Crittercism_EnableWithAppID (appID, true);
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
#if UNITY_5 || UNITY_5_3_OR_NEWER
			Application.logMessageReceived += OnLogMessageReceived;
#else
			Application.RegisterLogCallback (OnLogMessageReceived);
#endif
			Debug.Log ("CrittercismIOS: Sucessfully Initialized");
		} catch {
			Debug.Log ("Crittercism Unity plugin failed to initialize.");
		}
#endif
	}

	private static string StackTrace (Exception e)
	{
#if CRITTERCISM_IOS
		// Allowing for the fact that the "name" and "reason" of the outermost
		// exception e are already shown in the Crittercism portal, we don't
		// need to repeat that bit of info.  However, for InnerException's, we
		// will include this information in the StackTrace .  The horizontal
		// lines (hyphens) separate InnerException's from each other and the
		// outermost Exception e .
		string answer = e.StackTrace;
		// Using seen for cycle detection to break cycling.
		List<Exception> seen = new List<Exception> ();
		seen.Add (e);
		if (answer != null) {
			// There has to be some way of telling where InnerException ie stacktrace
			// ends and main Exception e stacktrace begins.  This is it.
			answer = ((e.GetType ().FullName + " : " + e.Message + "\r\n")
				+ answer);
			Exception ie = e.InnerException;
			while ((ie != null) && (seen.IndexOf(ie) < 0)) {
				seen.Add (ie);
				answer = ((ie.GetType ().FullName + " : " + ie.Message + "\r\n")
					+ (ie.StackTrace + "\r\n")
					+ answer);
				ie = ie.InnerException;
			}
		} else {
			answer = "";
		}
		return answer;
#else
        return "";
#endif
	}

	/// <summary>
	/// Log an exception that has been handled in code.
	/// This exception will be reported to the Crittercism portal.
	/// </summary>
	/// <param name="e">A caught exception that should be reported to Crittercism.</param>
	public static void LogHandledException (Exception e)
	{
#if CRITTERCISM_IOS
		if (e == null) {
			return;
		}
		Crittercism_LogHandledException (e.GetType ().FullName, e.Message, StackTrace (e), crUnityId);
#endif
	}

	/// <summary>
	/// Check if the user has opted out of Crittercism.  If a user is opted out, then no data will be
	/// sent to Crittercism.
	/// </summary>
	/// <returns>True if the user has opted out of Crittercism</returns>
	public static bool GetOptOut ()
	{
#if CRITTERCISM_IOS
		return Crittercism_GetOptOutStatus ();
#else
    return true;
#endif
	}

	/// <summary>
	/// Changes whether the user is opted in or out of reporting data to Crittercism.
	/// </summary>
	/// <param name="isOptedOut">True to opt out of sending data to Crittercism</param>
	public static void SetOptOut (bool isOptedOut)
	{
#if CRITTERCISM_IOS
		Crittercism_SetOptOutStatus (isOptedOut);
#endif
	}

	/// <summary>
	/// Set the Username of the current user.
	/// </summary>
	/// <param name="username">The user name to set</param>
	public static void SetUsername (string username)
	{
#if CRITTERCISM_IOS
		Crittercism_SetUsername (username);
#endif
	}

	/// <summary>
	/// Tell Crittercism to associate the given value/key pair with the current
	/// device UUID.
	/// <param name="val">The metadata value to set</param>
	/// <param name="key">The key to associate with the given metadata<c/param>
	/// <example>SetValue("5", "Game Level")</example>
	/// </summary>
	public static void SetValue (string val, string key)
	{
#if CRITTERCISM_IOS
		Crittercism_SetValue (val, key);
#endif
	}

	/// <summary>
	/// Log a breadcrumb.  Breadcrumbs are used for tracking local events.  Breadcrumbs
	/// will be attached to handled exceptions and crashes, which will allow diagnosing
	/// which events lead up to a crash.
	/// </summary>
	/// <param name="breadcrumb">The breadcrumb text to append to the breadcrumb trail</param>
	/// <example>LeaveBreadcrumb("Game started");</example>
	public static void LeaveBreadcrumb (string breadcrumb)
	{
#if CRITTERCISM_IOS
		Crittercism_LeaveBreadcrumb (breadcrumb);
#endif
	}

	public static void LogNetworkRequest (string method,
	                                      string uriString,
	                                      double latency,
	                                      int bytesRead,
	                                      int bytesSent,
	                                      HttpStatusCode responseCode,
	                                      WebExceptionStatus exceptionStatus)
	{
#if CRITTERCISM_IOS
		Crittercism_LogNetworkRequest (method, uriString, latency, bytesRead, bytesSent, (int)responseCode, (int)exceptionStatus);
#endif
	}

	/// <summary>
	/// Did the application crash on the previous load?
	/// </summary>
	public static bool DidCrashOnLastLoad ()
	{
#if CRITTERCISM_IOS
		bool answer = false;
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			answer = Crittercism_DidCrashOnLastLoad ();
		}
		return answer;
#else
    return false;
#endif
	}

	/// <summary>
	/// Init and begin a userflow with a default value.
	/// </summary>
	public static void BeginUserflow (string name)
	{
#if CRITTERCISM_IOS
		Crittercism_BeginUserflow (name);
#endif
	}

	[Obsolete("BeginTransaction is deprecated, please use BeginUserflow instead.")]
	public static void BeginTransaction (string name)
	{
#if CRITTERCISM_IOS
		BeginUserflow (name);
#endif
	}

	/// <summary>
	/// Init and begin a userflow with an input value.
	/// </summary>
	public static void BeginUserflow (string name, int value)
	{
#if CRITTERCISM_IOS
		Crittercism_BeginUserflowWithValue (name, value);
#endif
	}

	[Obsolete("BeginTransaction is deprecated, please use BeginUserflow instead.")]
	public static void BeginTransaction (string name, int value)
	{
		BeginUserflow (name, value);
	}
	
	/// <summary>
	/// Cancel a userflow as if it never existed.
	/// </summary>
	public static void CancelUserflow (string name)
	{
#if CRITTERCISM_IOS
		Crittercism_CancelUserflow (name);
#endif
	}

	[Obsolete("CancelTransaction is deprecated, please use CancelUserflow instead.")]
	public static void CancelTransaction (string name)
	{
		CancelUserflow (name);
	}

	/// <summary>
	/// End an already begun userflow successfully.
	/// </summary>
	public static void EndUserflow (string name)
	{
#if CRITTERCISM_IOS
		Crittercism_EndUserflow (name);
#endif
	}

	[Obsolete("EndTransaction is deprecated, please use EndUserflow instead.")]
	public static void EndTransaction (string name)
	{
		EndUserflow (name);
	}
	
	/// <summary>
	/// End an already begun userflow as a failure.
	/// </summary>
	public static void FailUserflow (string name)
	{
#if CRITTERCISM_IOS
		Crittercism_FailUserflow (name);
#endif
	}

	[Obsolete("FailTransaction is deprecated, please use FailUserflow instead.")]
	public static void FailTransaction (string name)
	{
		FailUserflow (name);
	}
	
	/// <summary>
	/// Set the currency cents value of a userflow.
	/// </summary>
	public static void SetUserflowValue (string name, int value)
	{
#if CRITTERCISM_IOS
		Crittercism_SetUserflowValue (name, value);
#endif
	}

	[Obsolete("SetTransactionValue is deprecated, please use SetUserflowValue instead.")]
	public static void SetTransactionValue (string name, int value)
	{
		SetUserflowValue (name, value);
	}

	/// <summary>
	/// Get the currency cents value of a userflow.
	/// </summary>
	public static int GetUserflowValue (string name)
	{
#if CRITTERCISM_IOS
		return Crittercism_GetUserflowValue (name);
#else
        return -1;
#endif
	}

	[Obsolete("GetTransactionValue is deprecated, please use GetUserflowValue instead.")]
	public static int GetTransactionValue (string name)
	{
		return GetUserflowValue (name);
	}

	private static void OnUnhandledException (object sender, UnhandledExceptionEventArgs args)
	{
#if CRITTERCISM_IOS
		if (args == null || args.ExceptionObject == null) {
			return;
		}
		try {
			Exception e = args.ExceptionObject as Exception;
			if (e != null) {
				// Should never get here since the Init() call would have bailed on the same if statement
				Crittercism_LogUnhandledException (e.GetType ().FullName, e.Message, StackTrace (e), crUnityId);
			}
		} catch {
			if (Debug.isDebugBuild == true) {
				Debug.Log ("CrittercismIOS: Failed to log exception");
			}
		}
#endif
	}

	/// <summary>
	/// Report uncaught C# Exception's as crashes (red blips) iff value is true .
	/// </summary>
	public static void SetLogUnhandledExceptionAsCrash (bool value)
	{
#if CRITTERCISM_IOS
		logUnhandledExceptionAsCrash = value;
#endif
	}

	/// <summary>
	/// Reporting uncaught C# Exception's as crashes (red blips)?
	/// </summary>
	public static bool GetLogUnhandledExceptionAsCrash ()
	{
		return logUnhandledExceptionAsCrash;
	}

	private static void OnLogMessageReceived (String name, String stack, LogType type)
	{
#if CRITTERCISM_IOS
		if (type == LogType.Exception) {
			if (logUnhandledExceptionAsCrash) {
				Crittercism_LogUnhandledException (name, name, stack, crUnityId);
			} else {
				Crittercism_LogHandledException (name, name, stack, crUnityId);
			}
		}
#endif
	}
}


#if UNITY_ANDROID

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public static class CrittercismAndroid
{
	/// <summary>
	/// Show debug and log messaged in the console in release mode.
	/// If true CrittercismIOS logs will not appear in the console.
	/// </summary>
	static bool _ShowDebugOnOnRelease = true;
	private static bool isInitialized = false;
	private static readonly string CRITTERCISM_CLASS = "com.crittercism.app.Crittercism";
	private static AndroidJavaClass mCrittercismsPlugin = null;

	/// <summary>
	/// Description:
	/// Start Crittercism for Unity, will start crittercism for android if it is not already active.
	/// Parameters:
	/// appID: Crittercisms Provided App ID for this application
	/// </summary>
	public static void Init (string appID)
	{
		Init (appID, new CrittercismConfig ());
	}

	public static void Init (string appID, CrittercismConfig config)
	{
		if (isInitialized) {
			UnityEngine.Debug.Log ("CrittercismAndroid is already initialized.");
			return;
		}
		UnityEngine.Debug.Log ("Initializing Crittercism with app id " + appID);
		mCrittercismsPlugin = new AndroidJavaClass (CRITTERCISM_CLASS);
		if (mCrittercismsPlugin == null) {
			UnityEngine.Debug.Log ("CrittercismAndroid failed to initialize.  Unable to find class " + CRITTERCISM_CLASS);
			return;
		}
		using (var cls_UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
			using (var objActivity = cls_UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
				PluginCallStatic ("initialize", objActivity, appID, config.GetAndroidConfig ());
			}
		}
		System.AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		Application.logMessageReceived += OnLogMessageReceived;
		isInitialized = true;
		UnityEngine.Debug.Log ("CrittercismAndroid only supports the Android platform. Crittercism will not be enabled");
	}

	private static string StackTrace (System.Exception e)
	{
		// Allowing for the fact that the "name" and "reason" of the outermost
		// exception e are already shown in the Crittercism portal, we don't
		// need to repeat that bit of info.  However, for InnerException's, we
		// will include this information in the StackTrace .  The horizontal
		// lines (hyphens) separate InnerException's from each other and the
		// outermost Exception e .
		string answer = e.StackTrace;
		// Using seen for cycle detection to break cycling.
		List<System.Exception> seen = new List<System.Exception> ();
		seen.Add (e);
		if (answer != null) {
			// There has to be some way of telling where InnerException ie stacktrace
			// ends and main Exception e stacktrace begins.  This is it.
			answer = ((e.GetType ().FullName + " : " + e.Message + "\r\n")
				+ answer);
			System.Exception ie = e.InnerException;
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
	}
	
	/// <summary>
	/// Log an exception that has been handled in code.
	/// This exception will be reported to the Crittercism portal.
	/// </summary>
	public static void LogHandledException (System.Exception e)
	{
		string name = e.GetType ().FullName;
		string message = e.Message;
		string stack = StackTrace (e);
		PluginCallStatic ("_logHandledException", name, message, stack);
	}

	private static void LogUnhandledException (System.Exception e)
	{
		string name = e.GetType ().FullName;
		string message = e.Message;
		string stack = StackTrace (e);
		PluginCallStatic (logUnhandledExceptionAsCrash ? "_logCrashException" : "_logHandledException", name, message, stack);
	}

	public static void LogNetworkRequest(string method,
                                         string uriString,
	                                     long latency,
	                                     long bytesRead,
	                                     long bytesSent,
	                                     HttpStatusCode responseCode,
	                                     WebExceptionStatus exceptionStatus) {
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("logNetworkRequest", method, uriString, latency, bytesRead, bytesSent, (int)responseCode, (int)exceptionStatus);
	}

	/// <summary>
	/// Retrieve whether the user is optted out of Crittercism.
	/// </summary>
	public static bool GetOptOut ()
	{
		if (!isInitialized) {
			return false;
		}
		return PluginCallStatic<bool> ("getOptOutStatus");
	}


	/// <summary>
	/// Set if whether the user is opting to use crittercism
	/// </summary></param>
	public static void SetOptOut (bool optOutStatus)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic<bool> ("setOptOutStatus", optOutStatus);
	}

	/// <summary>
	/// Did the application crash on the previous load?
	/// </summary>
	public static bool DidCrashOnLastLoad ()
	{
		if (!isInitialized) {
			return false;
		}
		return PluginCallStatic<bool> ("didCrashOnLastLoad");
	}

	/// <summary>
	/// Set the Username of the user
	/// This will be reported in the Crittercism Meta.
	/// </summary>
	public static void SetUsername (string username)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("setUsername", username);
	}

	/// <summary>
	/// Add a custom value to the Crittercism Meta.
	/// </summary>
	public static void SetMetadata (string[] keys, string[] values)
	{
		if (!isInitialized) {
			return;
		}
		if (keys.Length != values.Length) {
			UnityEngine.Debug.Log ("Crittercism.SetMetadata given arrays of different lengths");
			return;
		}
		for (int i = 0; i < keys.Length; i++) {
			SetValue (keys [i], values [i]);
		}
	}

	public static void SetValue (string key, string value)
	{
		if (!isInitialized) {
			return;
		}
		using (var jsonObject = new AndroidJavaObject ("org.json.JSONObject")) {
			jsonObject.Call<AndroidJavaObject> ("put", key, value);

			//TODO: using AndroidJavaClass and AndroidJavaObject can be really expensive in C#
			//consider add a overload method void setMetadata(string key, string value) in java side
			PluginCallStatic ("setMetadata", jsonObject);
		}
	}

	/// <summary>
	/// Leave a breadcrumb for tracking.
	/// </summary>
	public static void LeaveBreadcrumb (string breadcrumb)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("leaveBreadcrumb", breadcrumb);
	}
		
	/// <summary>
	/// Begin a userflow to track ex. login
	/// </summary>
	public static void BeginUserflow (string userflowName)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("beginTransaction", userflowName);
	}

	[Obsolete("BeginTransaction is deprecated, please use BeginUserflow instead.")]
	public static void BeginTransaction (string userflowName)
	{
		BeginUserflow (userflowName);
	}
	
	/// <summary>
	/// Cancel a userflow as if it never existed.
	/// </summary>
	public static void CancelUserflow (string userflowName)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("cancelTransaction", userflowName);
	}

	[Obsolete("CancelTransaction is deprecated, please use CancelUserflow instead.")]
	public static void CancelTransaction (string userflowName)
	{
		CancelUserflow (userflowName);
	}
		
	/// <summary>
	/// Ends a tracked userflow ex. login was successful
	/// </summary>
	public static void EndUserflow (string userflowName)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("endTransaction", userflowName);
	}

	[Obsolete("EndTransaction is deprecated, please use EndUserflow instead.")]
	public static void EndTransaction (string userflowName)
	{
		EndUserflow (userflowName);
	}

	/// <summary>
	/// Fails a tracked userflow ex. login error
	/// </summary>
	public static void FailUserflow (string userflowName)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("failTransaction", userflowName);
	}

	[Obsolete("FailTransaction is deprecated, please use FailUserflow instead.")]
	public static void FailTransaction (string userflowName)
	{
		FailUserflow (userflowName);
	}

	/// <summary>
	/// Set a value for a userflow ex. shopping cart value
	/// </summary>
	public static void SetUserflowValue (string userflowName, int value)
	{
		if (!isInitialized) {
			return;
		}
		PluginCallStatic ("setTransactionValue", userflowName, value);
	}

	[Obsolete("SetTransactionValue is deprecated, please use SetUserflowValue instead.")]
	public static void SetTransactionValue (string userflowName, int value)
	{
		SetUserflowValue (userflowName, value);
	}

	/// <summary>
	/// Get the current value of the tracked userflow
	/// </summary>
	public static int GetUserflowValue (string userflowName)
	{
		if (!isInitialized) {
			return -1;
		}
		return PluginCallStatic<int> ("getTransactionValue", userflowName);
	}

	[Obsolete("GetTransactionValue is deprecated, please use GetUserflowValue instead.")]
	public static int GetTransactionValue (string userflowName)
	{
		return GetUserflowValue (userflowName);
	}

	private static void OnUnhandledException (object sender, System.UnhandledExceptionEventArgs args)
	{
		if (!isInitialized || args == null || args.ExceptionObject == null) {
			return;
		}
		System.Exception e = args.ExceptionObject as System.Exception;
		if (e != null) {
			LogUnhandledException (e);
		}
	}

	private static volatile bool logUnhandledExceptionAsCrash = false;
	
	public static void SetLogUnhandledExceptionAsCrash (bool value)
	{
		logUnhandledExceptionAsCrash = value;
	}
	
	public static bool GetLogUnhandledExceptionAsCrash ()
	{
		return logUnhandledExceptionAsCrash;
	}

	private static void OnLogMessageReceived (string name, string stack, LogType type)
	{
		if (LogType.Exception != type) {
			return;
		}
		if (!isInitialized) {
			return;
		}
		if (LogType.Exception == type) {
			if (Application.platform == RuntimePlatform.Android) {
				if (logUnhandledExceptionAsCrash) {
					PluginCallStatic ("_logCrashException", name, name, stack);
				} else {
					stack = (new Regex("\r\n")).Replace(stack, "\n\tat");
					PluginCallStatic ("_logHandledException", name, name, stack);
				}
			}
		}
	}

	private static void PluginCallStatic (string methodName, params object[] args)
	{
		mCrittercismsPlugin.CallStatic (methodName, args);
	}

	private static RetType PluginCallStatic<RetType> (string methodName, params object[] args)
	{
		return mCrittercismsPlugin.CallStatic<RetType> (methodName, args);
	}
}
#endif

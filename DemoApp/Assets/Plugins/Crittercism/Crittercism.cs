﻿using System;
using System.Net;
using UnityEngine;

public class Crittercism : MonoBehaviour
{
    /// <summary>
    /// Your Crittercism App ID.  Every app has a special identifier that allows Crittercism
    /// to associate error monitoring data with your app.  Your app ID can be found on the
    /// "App Settings" page of the app you are trying to monitor.
    /// See the Crittercism portal https://app.crittercism.com/
    /// </summary>
    /// <example>A real app ID looks like this:  5671d3b6f7c78a7243000a05</example>

    public string CrittercismiOSAppID = "YOUR IOS APP ID GOES HERE";

    public string CrittercismAndroidAppID = "YOUR ANDROID APP ID GOES HERE";

    void Awake ()
    {
        #if UNITY_IPHONE
        CrittercismIOS.Init (CrittercismiOSAppID);
        #elif UNITY_ANDROID
        CrittercismAndroid.Init (CrittercismAndroidAppID);
        #endif
    }

	/// <summary>
	/// Log an exception that has been handled in code.
	/// This exception will be reported to the Crittercism portal.
	/// </summary>
	/// <param name="e">A caught exception that should be reported to Crittercism.</param>
	public static void LogHandledException (Exception e)
	{
#if UNITY_IPHONE
		CrittercismIOS.LogHandledException (e);
#elif UNITY_ANDROID
		CrittercismAndroid.LogHandledException (e);
#endif
	}

	/// <summary>
	/// Check if the user has opted out of Crittercism.  If a user is opted out, then no data will be
	/// sent to Crittercism.
	/// </summary>
	/// <returns>True if the user has opted out of Crittercism</returns>
	public static bool GetOptOut ()
	{
#if UNITY_IPHONE
		return CrittercismIOS.GetOptOut ();
#elif UNITY_ANDROID
		return CrittercismAndroid.GetOptOut ();
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
#if UNITY_IPHONE
		CrittercismIOS.SetOptOut (isOptedOut);
#elif UNITY_ANDROID
		CrittercismAndroid.SetOptOut (isOptedOut);
#endif	
	}

	/// <summary>
	/// Set the Username of the current user.
	/// </summary>
	/// <param name="username">The user name to set</param>
	public static void SetUsername (string username)
	{
#if UNITY_IPHONE
		CrittercismIOS.SetUsername (username);
#elif UNITY_ANDROID
		CrittercismAndroid.SetUsername (username);
#endif
	}

	/// <summary>
	/// Tell Crittercism to associate the given value/key pair with the current
	/// device UUID.
	/// <param name="val">The metadata value to set</param>
	/// <param name="key">The key to associate with the given metadata<c/param>
	/// <example>SetValue("5", "Game Level")</example>
	/// </summary>
	public static void SetValue (string key, string value)
	{
#if UNITY_IPHONE
		CrittercismIOS.SetValue (value, key);
#elif UNITY_ANDROID
		CrittercismAndroid.SetMetadata (new string[] {key}, new string[] {value});
#endif
	}
    
	/// <summary>
	/// Tell Crittercism to associate the given value/key pair with the current
	/// device UUID.
	/// <param name="val">The metadata value to set</param>
	/// <param name="key">The key to associate with the given metadata<c/param>
	/// <example>SetValue("5", "Game Level")</example>
	/// </summary>
	public static void SetMetadata (string[] keys, string[] values)
	{
#if UNITY_IPHONE
        int length = keys.Length;
        for (int i = 0; i < length; i++) {
            string key = keys[i];
            string value = values[i];
    		CrittercismIOS.SetValue (value, key);   
        }
#elif UNITY_ANDROID
		CrittercismAndroid.SetMetadata (keys, values);
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
#if UNITY_IPHONE
		CrittercismIOS.LeaveBreadcrumb (breadcrumb);
#elif UNITY_ANDROID
		CrittercismAndroid.LeaveBreadcrumb (breadcrumb);
#endif
	}

	public static void LogNetworkRequest (string method,
										  string uriString,
										  double latencyInSeconds,
										  int bytesRead,
										  int bytesSent,
										  HttpStatusCode responseCode,
										  WebExceptionStatus exceptionStatus)
	{
#if UNITY_IPHONE
		CrittercismIOS.LogNetworkRequest (method,
 										  uriString,
										  latencyInSeconds,
										  bytesRead,
										  bytesSent,
										  responseCode,
										  exceptionStatus);
#elif UNITY_ANDROID
		CrittercismAndroid.LogNetworkRequest (method,
											  uriString,
											  (long)latencyInSeconds*1000,
											  bytesRead,
											  bytesSent,
											  responseCode,
											  exceptionStatus);
#endif
	}

	public static void LogNetworkRequest (string method,
										  string uriString,
										  long latencyInMilliseconds,
										  int bytesRead,
										  int bytesSent,
										  HttpStatusCode responseCode,
										  WebExceptionStatus exceptionStatus)
	{
#if UNITY_IPHONE
		CrittercismIOS.LogNetworkRequest (method,
										  uriString,
										  (double)latencyInMilliseconds/1000.0,
										  bytesRead,
										  bytesSent,
										  responseCode,
										  exceptionStatus);
#elif UNITY_ANDROID
		CrittercismAndroid.LogNetworkRequest (method,
											  uriString,
											  latencyInMilliseconds,
											  bytesRead,
											  bytesSent,
											  responseCode,
											  exceptionStatus);
#endif
	}

	/// <summary>
	/// Did the application crash on the previous load?
	/// </summary>
	public static bool DidCrashOnLastLoad ()
	{
#if UNITY_IPHONE
		return CrittercismIOS.DidCrashOnLastLoad ();
#elif UNITY_ANDROID
		return CrittercismAndroid.DidCrashOnLastLoad ();
#else
		return false;
#endif
	}

	/// <summary>
	/// Init and begin a transaction with a default value.
	/// </summary>
	public static void BeginTransaction (string name)
	{
#if UNITY_IPHONE
		CrittercismIOS.BeginTransaction (name);
#elif UNITY_ANDROID
		CrittercismAndroid.BeginTransaction (name);
#endif
	}

	/// <summary>
	/// Init and begin a transaction with an input value.
	/// </summary>
	public static void BeginTransaction (string name, int value)
	{
#if UNITY_IPHONE
		CrittercismIOS.BeginTransaction (name, value);
#elif UNITY_ANDROID
		CrittercismAndroid.BeginTransaction (name);
		CrittercismAndroid.SetTransactionValue (name, value);
#endif
	}

	/// <summary>
	/// Cancel a transaction as if it never existed.
	/// </summary>
	public static void CancelTransaction (string name)
	{
#if UNITY_IPHONE
		CrittercismIOS.CancelTransaction (name);
#elif UNITY_ANDROID
		CrittercismAndroid.CancelTransaction (name);
#endif
	}

	/// <summary>
	/// End an already begun transaction successfully.
	/// </summary>
	public static void EndTransaction (string name)
	{
#if UNITY_IPHONE
		CrittercismIOS.EndTransaction (name);
#elif UNITY_ANDROID
		CrittercismAndroid.EndTransaction (name);
#endif
	}

	/// <summary>
	/// End an already begun transaction as a failure.
	/// </summary>
	public static void FailTransaction (string name)
	{
#if UNITY_IPHONE
		CrittercismIOS.FailTransaction (name);
#elif UNITY_ANDROID
		CrittercismAndroid.FailTransaction (name);
#endif
	}

	/// <summary>
	/// Set the currency cents value of a transaction.
	/// </summary>
	public static void SetTransactionValue (string name, int value)
	{
#if UNITY_IPHONE
		CrittercismIOS.SetTransactionValue (name, value);
#elif UNITY_ANDROID
		CrittercismAndroid.SetTransactionValue (name, value);
#endif
	}

	/// <summary>
	/// Get the currency cents value of a transaction.
	/// </summary>
	public static int GetTransactionValue (string name)
	{
#if UNITY_IPHONE
		return CrittercismIOS.GetTransactionValue (name);
#elif UNITY_ANDROID
		return CrittercismAndroid.GetTransactionValue (name);
#else
		return -1;
#endif
	}

	/// <summary>
	/// Report uncaught C# Exception's as crashes (red blips) iff value is true .
	/// </summary>
	public static void SetLogUnhandledExceptionAsCrash (bool value)
	{
#if UNITY_IPHONE
		CrittercismIOS.SetLogUnhandledExceptionAsCrash (value);
#elif UNITY_ANDROID
		CrittercismAndroid.SetLogUnhandledExceptionAsCrash (value);
#endif	
	}

	/// <summary>
	/// Reporting uncaught C# Exception's as crashes (red blips)?
	/// </summary>
	public static bool GetLogUnhandledExceptionAsCrash ()
	{
#if UNITY_IPHONE
		return CrittercismIOS.GetLogUnhandledExceptionAsCrash ();
#elif UNITY_ANDROID
		return CrittercismAndroid.GetLogUnhandledExceptionAsCrash ();
#else
		return false;
#endif
	}
}

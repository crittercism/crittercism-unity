using System;
using System.Net;

public class Crittercism
{
	public static void Init (string appID)
	{
#if UNITY_IPHONE
		CrittercismIOS.Init (appID);
#elif UNITY_ANDROID
		CrittercismAndroid.Init (appID);
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism will not be enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
#else
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
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
		UnityEngine.Debug.Log ("Crittercism Unity only supports iOS and Android. Crittercism is not enabled");
		return false;
#endif
	}
}


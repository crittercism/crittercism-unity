#if !UNITY_EDITOR && UNITY_ANDROID
#define CRITTERCISM_ANDROID
#endif

using UnityEngine;
using System.Collections;

public class CrittercismConfig
{
#if CRITTERCISM_ANDROID    
	private static readonly string CRITTERCISM_CONFIG_CLASS = "com.crittercism.app.CrittercismConfig";
	private AndroidJavaObject mCrittercismConfig = null;
#endif

	public CrittercismConfig ()
	{
#if CRITTERCISM_ANDROID
		mCrittercismConfig = new AndroidJavaObject (CRITTERCISM_CONFIG_CLASS);
#endif
	}

	public AndroidJavaObject GetAndroidConfig ()
	{
#if CRITTERCISM_ANDROID
		return mCrittercismConfig;
#else
    return null;
#endif
	}

	public string GetCustomVersionName ()
	{
#if CRITTERCISM_ANDROID
		return CallConfigMethod<string> ("getCustomVersionName");
#else
    return "";
#endif
	}

	public void SetCustomVersionName (string customVersionName)
	{
#if CRITTERCISM_ANDROID
		CallConfigMethod ("setCustomVersionName", customVersionName);
#endif
	}

	public bool IsLogcatReportingEnabled ()
	{
#if CRITTERCISM_ANDROID
		return CallConfigMethod<bool> ("isLogcatReportingEnabled");
#else
    return false;
#endif
	}

	public void SetLogcatReportingEnabled (bool shouldCollectLogcat)
	{
#if CRITTERCISM_ANDROID
		CallConfigMethod ("setLogcatReportingEnabled", shouldCollectLogcat);
#endif
	}

	public bool IsServiceMonitoringEnabled ()
	{
#if CRITTERCISM_ANDROID
		return CallConfigMethod<bool> ("isServiceMonitoringEnabled");
#else
    return false;
#endif
	}

	public void SetServiceMonitoringEnabled (bool isServiceMonitoringEnabled)
	{
#if CRITTERCISM_ANDROID
		CallConfigMethod ("setServiceMonitoringEnabled", isServiceMonitoringEnabled);
#endif 
	}

	void CallConfigMethod (string methodName, params object[] args)
	{
#if CRITTERCISM_ANDROID
		mCrittercismConfig.Call (methodName, args);
#endif
	}

	RetType CallConfigMethod<RetType> (string methodName, params object[] args)
	{
#if CRITTERCISM_ANDROID
		return mCrittercismConfig.Call<RetType> (methodName, args);
#else
        return default(RetType);
#endif
	}
}


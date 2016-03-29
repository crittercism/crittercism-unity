#if UNITY_ANDROID

using UnityEngine;
using System.Collections;

public class CrittercismConfig
{
	private static readonly string CRITTERCISM_CONFIG_CLASS = "com.crittercism.app.CrittercismConfig";
	private AndroidJavaObject mCrittercismConfig = null;

	public CrittercismConfig ()
	{
		mCrittercismConfig = new AndroidJavaObject (CRITTERCISM_CONFIG_CLASS);
	}

	public AndroidJavaObject GetAndroidConfig ()
	{
		return mCrittercismConfig;
	}

	public string GetCustomVersionName ()
	{
		return CallConfigMethod<string> ("getCustomVersionName");
	}

	public void SetCustomVersionName (string customVersionName)
	{
		CallConfigMethod ("setCustomVersionName", customVersionName);
	}

	public bool IsLogcatReportingEnabled ()
	{
		return CallConfigMethod<bool> ("isLogcatReportingEnabled");
	}

	public void SetLogcatReportingEnabled (bool shouldCollectLogcat)
	{
		CallConfigMethod ("setLogcatReportingEnabled", shouldCollectLogcat);
	}

	public bool IsServiceMonitoringEnabled ()
	{
		return CallConfigMethod<bool> ("isServiceMonitoringEnabled");
	}

	public void SetServiceMonitoringEnabled (bool isServiceMonitoringEnabled)
	{
		CallConfigMethod ("setServiceMonitoringEnabled", isServiceMonitoringEnabled);
	}

	void CallConfigMethod (string methodName, params object[] args)
	{
		mCrittercismConfig.Call (methodName, args);
	}

	RetType CallConfigMethod<RetType> (string methodName, params object[] args)
	{
		return mCrittercismConfig.Call<RetType> (methodName, args);
	}
}

#endif
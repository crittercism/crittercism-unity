using System;
using System.Net;
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Networking;

public class CrittercismTestGUI : MonoBehaviour
{
    
    private CrittercismTestGUI ()
    {
        Debug.Log ("DidCrashOnLastLoad: " + Crittercism.DidCrashOnLastLoad ());
        Crittercism.SetLogUnhandledExceptionAsCrash (true);
    }

    public void LogNetworkRequest ()
    {
        // Crittercism automatically logs network performance information for both UnityWebRequest and the WWW apis
        StartCoroutine (UnityWebRequestGet ());
        StartCoroutine (UnityWebRequestPost ());
        StartCoroutine (WWWRequest ());
        
        // If you use a 3rd party networking API, you may log network performance information like this:
        Crittercism.LogNetworkRequest (
            "GET",
            "http://myurl.com",
            250,                  // latency in milliseconds
            1024,                 // number of bytes received
            2048,                 // number of bytes sent
            (HttpStatusCode)200,  // http status code
            WebExceptionStatus.Success);
    }

    public void LeaveBreadcrumb ()
    {
        Crittercism.LeaveBreadcrumb ("BreadCrumb");

    }

    public void SetMetadata ()
    {
        Crittercism.SetUsername ("MommaCritter");
        Crittercism.SetValue ("Game Level", "Status");
        Crittercism.SetValue ("5", "Crashes a lot");
    }

    public void Crash ()
    {
        crashInnerException ();
    }

    public void LogHandledException ()
    {
        try {
            crashInnerException ();
        } catch (System.Exception e) {
            Crittercism.LogHandledException (e);
        }
    }

    public void BeginUserflow()
    {
        Crittercism.BeginUserflow ("UnityUserflow");
    }

    public void EndUserflow()
    {
        Crittercism.EndUserflow ("UnityUserflow");
    }

    public void FailUserflow()
    {
        Crittercism.FailUserflow ("UnityUserflow");
    }

    public void CancelUserflow()
    {
        Crittercism.CancelUserflow ("UnityUserflow");
    }

    public void IncrementUserflowValue()
    {
        int value = Crittercism.GetUserflowValue ("UnityUserflow");
        if (value < 0) {
            value = 0;
        }

        value += 1;

        Crittercism.SetUserflowValue ("UnityUserflow", value);
    }

    private void DeepError (int n)
    {
        if (n == 0) {
            throw new Exception ("Deep Inner Exception");
        } else {
            DeepError (n - 1);
        }
    }

    private void crashInnerException ()
    {
        try {
            DeepError (4);
        } catch (Exception ie) {
            throw new Exception ("Outer Exception", ie);
        }
    }
        

    private IEnumerator UnityWebRequestGet ()
    {
        using (UnityWebRequest www = UnityWebRequest.Get ("http://httpbin.org/status/418")) {
            yield return www.Send ();

            if (www.isError) {
                Debug.Log (www.error);
            } else {
                Debug.Log (www.downloadHandler.text);
            }
        }
    }

    private IEnumerator UnityWebRequestPost ()
    {
        WWWForm form = new WWWForm ();
        form.AddField ("fieldName", "fieldValue");
        form.AddField ("test", "data");

        using (UnityWebRequest www = UnityWebRequest.Post ("http://httpbin.org/post", form)) {
            yield return www.Send ();

            if (www.isError) {
                Debug.Log (www.error);
            } else {
                Debug.Log ("Form upload complete!");
            }
        }
    }

    private IEnumerator WWWRequest ()
    {
        WWW www = new WWW ("http://httpbin.org/get");
        yield return www;
        // check for errors
        if (www.error == null) {
            Debug.Log ("WWW Ok!: " + www.text);
        } else {
            Debug.Log ("WWW Error: " + www.error);
        }    
    }
}

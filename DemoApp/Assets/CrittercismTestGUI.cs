using System;
using System.Net;
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Networking;

public class CrittercismTestGUI : MonoBehaviour {

    private CrittercismTestGUI() {
        Debug.Log ("DidCrashOnLastLoad: " + Crittercism.DidCrashOnLastLoad ());
        Crittercism.SetLogUnhandledExceptionAsCrash (false);
    }

    public void OnGUI () {
        GUIStyle customStyle = new GUIStyle (GUI.skin.button);
        customStyle.fontSize = 30;
        const int numberOfButtons = 15;
        int screenButtonHeight = Screen.height / numberOfButtons;

        if (GUI.Button (new Rect (0, 0, Screen.width, screenButtonHeight), "Set Username", customStyle)) {
            Crittercism.SetUsername ("MommaCritter");
        }
        if (GUI.Button (new Rect (0, screenButtonHeight, Screen.width, screenButtonHeight), "Set Metadata", customStyle)) {
            Crittercism.SetValue ("Game Level", "5");
            Crittercism.SetValue ("Status", "Crashes a lot");
        }
        if (GUI.Button (new Rect (0, 2 * screenButtonHeight, Screen.width, screenButtonHeight), "Leave Breadcrumb", customStyle)) {
            Crittercism.LeaveBreadcrumb ("This is an illuminating piece of information");
        }
        if (GUI.Button (new Rect (0, 3 * screenButtonHeight, Screen.width, screenButtonHeight), "Log Network Request", customStyle)) {

            // Crittercism automatically logs network performance information for both UnityWebRequest and the WWW apis
            StartCoroutine (UnityWebRequestGet ());
            StartCoroutine (UnityWebRequestPost ());
            StartCoroutine(WWWRequest());

            // If you use a 3rd party networking API, you may log network performance information like this:
            Crittercism.LogNetworkRequest(
                "GET",
                "https://myurl.com",
                250,                  // latency in milliseconds
                1024,                 // number of bytes received
                2048,                 // number of bytes sent
                (HttpStatusCode)200,  // http status code
                WebExceptionStatus.Success);
        }
        if (GUI.Button (new Rect (0, 4 * screenButtonHeight, Screen.width, screenButtonHeight), "C# Unhandled Exception (Crash)", customStyle)) {
            // (iOS) This will cause an app crash while in 'Fast no Exceptions' mode 
            // (iOS) and an exception in 'Slow and Safe' mode
            crashInnerException ();
            // causeNullPointerException ();
        }
        if (GUI.Button (new Rect (0, 5 * screenButtonHeight, Screen.width, screenButtonHeight), "C# Handled Exception", customStyle)) {
            try {
                crashInnerException ();
            } catch (System.Exception e) {
                Crittercism.LogHandledException (e);
            }
        }
        if (GUI.Button (new Rect (0, 6 * screenButtonHeight, Screen.width, screenButtonHeight), "Begin Userflow", customStyle)) {
            Crittercism.BeginUserflow ("UnityUserflow");
        }
        if (GUI.Button (new Rect (0, 7 * screenButtonHeight, Screen.width, screenButtonHeight), "End Userflow", customStyle)) {
            Crittercism.EndUserflow ("UnityUserflow");
        }
        if (GUI.Button (new Rect (0, 8 * screenButtonHeight, Screen.width, screenButtonHeight), "Fail Userflow", customStyle)) {
            Crittercism.FailUserflow ("UnityUserflow");
        }
        if (GUI.Button (new Rect (0, 9 * screenButtonHeight, Screen.width, screenButtonHeight), "Cancel Userflow", customStyle)) {
            Crittercism.CancelUserflow ("UnityUserflow");
        }
        if (GUI.Button (new Rect (0, 10 * screenButtonHeight, Screen.width, screenButtonHeight), "Increment Userflow Value", customStyle)) {
            int value = Crittercism.GetUserflowValue ("UnityUserflow");

            if (value <= 0) {
                // Value hasn't been set yet
                value = 1;
            }

            value++;
            Crittercism.SetUserflowValue ("UnityUserflow", value);
        }
        if (GUI.Button (new Rect (0, 11 * screenButtonHeight, Screen.width, screenButtonHeight), "Log unhandled exceptions as CRASH", customStyle)) {
            Crittercism.SetLogUnhandledExceptionAsCrash (true);
        }
        if (GUI.Button (new Rect (0, 12 * screenButtonHeight, Screen.width, screenButtonHeight), "Log unhandled exceptions as EXCEPTION", customStyle)) {
            Crittercism.SetLogUnhandledExceptionAsCrash (false);
        }
        if (GUI.Button (new Rect (0, 13 * screenButtonHeight, Screen.width, screenButtonHeight), "Set opt-out to TRUE", customStyle)) {
            Crittercism.SetOptOut (true);
        }
        if (GUI.Button (new Rect (0, 14 * screenButtonHeight, Screen.width, screenButtonHeight), "Set opt-out to FALSE", customStyle)) {
            Crittercism.SetOptOut (false);
        }
    }

    IEnumerator UnityWebRequestGet() {
        using(UnityWebRequest www = UnityWebRequest.Get("https://httpbin.org/status/418")) {
            yield return www.Send();

            if(www.isError) {
                Debug.Log(www.error);
            } else {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    IEnumerator UnityWebRequestPost() {
        WWWForm form = new WWWForm ();
        form.AddField ("fieldName", "fieldValue");
        form.AddField ("test", "data");

        using(UnityWebRequest www = UnityWebRequest.Post("https://httpbin.org/post", form)) {
            yield return www.Send();

            if(www.isError) {
                Debug.Log(www.error);
            }
            else {
                Debug.Log("Form upload complete!");
            }
        }
    }

    IEnumerator WWWRequest() {
        WWW www = new WWW("https://httpbin.org/get");
        yield return www;
        // check for errors
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.data);
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    }

    public void DeepError (int n) {
        if (n == 0) {
            throw new Exception ("Deep Inner Exception");
        } else {
            DeepError (n - 1);
        }
    }

    public void crashInnerException () {
        try {
            DeepError (4);
        } catch (Exception ie) {
            throw new Exception ("Outer Exception", ie);
        }
    }

    void causeNullPointerException () {
        object o = null;
        o.GetHashCode ();
    }
}
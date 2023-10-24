using System.Collections.Generic;
using UnityEngine;
public class errorManager : MonoBehaviour
{
    //Instance
    private static errorManager instance = null;
    public static errorManager Instance { get { return instance; } }
    //Error Reports
    private List<errorReport> errorReports = new List<errorReport>();
    void Awake()
    {
        //Instatiate
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    //Create Error Report
    public void createErrorReport(string _scriptName, string _methodName, errorType _errType)
    {
        errorReport newErrorReport = new errorReport(_scriptName, _methodName, _errType);
        newErrorReport.reportError();
        errorReports.Add(newErrorReport);
    }
}

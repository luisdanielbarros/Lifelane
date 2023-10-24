using UnityEngine;
public class errorReport
{
    //Script Name
    protected string scriptName { private set; get; }
    //Method Name
    protected string methodName { private set; get; }
    //Error Type
    protected errorType errType { private set; get; }
    //Constructor
    public errorReport(string _scriptName, string _methodName, errorType _errType)
    {
        scriptName = _scriptName;
        methodName = _methodName;
        errType = _errType;
    }
    //Report Error
    public void reportError()
    {
        Debug.Log("Error: " + errType + " at method " + methodName + " in script " + scriptName + ".");
    }
}

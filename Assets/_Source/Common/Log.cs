using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Log
{
    public static void Debug(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
        )
    {
        string sourceFileName = System.IO.Path.GetFileName(sourceFilePath);

        string writeString = " [DEBUG] " + 
            sourceFileName + ":" +
            memberName + ": ";
        writeString += message;

        UnityEngine.Debug.Log(writeString);
    }

    public static void Info(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
        )
    {
        string sourceFileName = System.IO.Path.GetFileName(sourceFilePath);

        string writeString = " [INFO] " +
            sourceFileName + ":" +
            memberName + ": ";
        writeString += message;

        UnityEngine.Debug.Log(writeString);
    }

    public static void Error(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
        )
    {
        string sourceFileName = System.IO.Path.GetFileName(sourceFilePath);

        string writeString = " [ERROR] " +
            sourceFileName + ":" +
            memberName + ": ";
        writeString += message;

        UnityEngine.Debug.Log(writeString);
    }
}

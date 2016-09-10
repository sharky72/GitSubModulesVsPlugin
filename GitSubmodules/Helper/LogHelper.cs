using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class for things to log
    /// </summary>
    internal static class LogHelper
    {
        /// <summary>
        /// Write a <see cref="string"/> to a log file
        /// </summary>
        /// <param name="messages">The message to log</param>
        /// <param name="memberName">The member name that hat call this method</param>
        /// <param name="codelineNumber">The code line number from this this method have called</param>
        internal static void Log(string messages, [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int codelineNumber = 0)
        {
            using(var fileStream = new StreamWriter(new FileStream("D:\\GitSubmodule.log",
                                                                   FileMode.Append,
                                                                   FileAccess.Write)))
            {
                fileStream.WriteLine("{0:HH:m:ss:fff}: {1,-20} - {2:000} - {3}",
                                     DateTime.Now,
                                     memberName,
                                     codelineNumber,
                                     messages);
            }
        }

        internal static void Log(Exception exception, [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int codelineNumber = 0)
        {
            Log(exception.ToString(), memberName, codelineNumber);
        }
    }
}

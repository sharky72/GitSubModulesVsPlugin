using System;
using System.Threading;
using System.Threading.Tasks;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier handle with tasks
    /// </summary>
    internal static class TaskHelper
    {
        /// <summary>
        /// Run a <see cref="Action"/> asynchronous [exact the same as Task.Run(Action) in .NET Framework 4.5]
        /// </summary>
        /// <param name="action"></param>
        internal static void Run(Action action)
        {
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }
}

using Microsoft.Win32.TaskScheduler;
using System.Diagnostics;

namespace iRacingApplicationVersionManger
{
    public static class DeElevatedProcess
    {
        const string taskName = "iRacingReplayOverlay";

        public static void Start(string command)
        {
            using (var ts = new TaskService())
            {
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "start " + taskName + " limited user";
                td.Actions.Add(new ExecAction(command));
                td.Settings.Priority = ProcessPriorityClass.Normal;
                td.Principal.RunLevel = TaskRunLevel.LUA;
                td.Settings.AllowDemandStart = true;
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StopIfGoingOnBatteries = false;

                var ret = ts.RootFolder.RegisterTaskDefinition(taskName, td);

                var fooTask = ts.FindTask(taskName, true);
                fooTask.Run();

                ts.RootFolder.DeleteTask(taskName);
            }
        }
    }
}

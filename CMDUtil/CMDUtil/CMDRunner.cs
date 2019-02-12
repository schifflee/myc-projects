using System;
using System.Diagnostics;

namespace CMDUtil
{
    public class CMDRunner:IDisposable
    {
        public CMDRunner()
        {
            InitializeSettings();
        }
        private string outputData;
        private string errorData;
        private readonly Process process = new Process();

        private void InitializeSettings()
        {
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            process.BeginOutputReadLine();
            process.OutputDataReceived += (sender, e) => outputData += e.Data;

            process.BeginErrorReadLine();
            process.ErrorDataReceived += (sender, e) => errorData += e.Data;
        }

        public void Send(params string[] commands)
        {
            errorData = string.Empty;
            foreach (var command in commands)
            {
                process.StandardInput.WriteLine(command);
            }
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

            if (string.IsNullOrEmpty(errorData) == false)
            {
                throw new InvalidOperationException(errorData);
            }
        }

        public void Dispose()
        {
            process.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

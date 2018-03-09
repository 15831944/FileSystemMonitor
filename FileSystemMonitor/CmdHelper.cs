using System.Diagnostics;

namespace FileSystemMonitor
{
    /// <summary>
    ///     命令提示字元Helper
    /// </summary>
    public class CmdHelper
    {
        /// <summary>
        ///     建構子
        /// </summary>
        /// <param name="isShowWindow">是否顯示命令提示視窗</param>
        public CmdHelper(bool isShowWindow = false)
        {
            IsShowWindow = isShowWindow;
        }

        /// <summary>
        ///     是否顯示命令提示視窗
        /// </summary>
        private bool IsShowWindow { get; }

        /// <summary>
        ///     執行命令提示字元並回傳執行結果文字
        /// </summary>
        /// <param name="cmdText">指令</param>
        /// <returns>結果文字</returns>
        public string ExecCmd(string cmdText)
        {
            var cmd = new Process();

            var startInfo = GetProStartInfo(cmdText, true);

            cmd.StartInfo = startInfo;
            cmd.Start();

            var strOutput = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();

            return strOutput;
        }

        /// <summary>
        ///     執行命令提示字元
        /// </summary>
        /// <param name="cmdText">指令</param>
        public void ExecNonCmd(string cmdText)
        {
            var cmd = new Process();

            var startInfo = GetProStartInfo(cmdText, false);

            cmd.StartInfo = startInfo;
            cmd.Start();

            cmd.WaitForExit();
        }

        /// <summary>
        ///     取得Process 初始參數
        /// </summary>
        /// <param name="cmdText">命令字元</param>
        /// <param name="isReturnOutput">是否要輸出結果</param>
        /// <returns></returns>
        private ProcessStartInfo GetProStartInfo(string cmdText, bool isReturnOutput)
        {
            var startInfo = new ProcessStartInfo();

            if (!IsShowWindow)
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            if (isReturnOutput)
                startInfo.RedirectStandardOutput = true;

            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c" + cmdText;

            return startInfo;
        }
    }
}
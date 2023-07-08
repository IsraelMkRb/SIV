using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlServices
{
    class Logs
    {
        private static readonly string Path = $"TMP\\CSR{DateTime.Now:ddMMyyyy}.log";
        public static void Write(string EventLog)
        {
            try
            {
                if (!File.Exists(path: Path)) { DirectoryInfo dr= Directory.CreateDirectory("TMP");  }

                using (StreamWriter streamWriter = new StreamWriter(path: Path))
                {
                    streamWriter.WriteLine($"[{DateTime.Now:dd-MM-yyyyy hh-mm-ss}] info: {EventLog}");
                }
            }
            catch (Exception) { throw; }
        }
    }
}

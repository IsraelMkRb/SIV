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
        private static readonly string _Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"TMP");
        private static readonly string LogFile = $"\\CSR{DateTime.Now:ddMMyyyy}.log";
        public static void Write(string EventLog)
        {
            try
            {
                if (!Directory.Exists(path: _Path)) Directory.CreateDirectory(_Path);
                File.AppendAllText(_Path + LogFile, $"[{DateTime.Now:dd-MM-yyyyy hh-mm-ss}] Info: {EventLog} \n");
            }
            catch (Exception) { throw; }
        }

        public static void Write(Exception Ex)
        {
            try
            {
                if (!Directory.Exists(path: _Path)) Directory.CreateDirectory(_Path);
                File.AppendAllText(_Path + LogFile, $"[{DateTime.Now:dd-MM-yyyyy hh-mm-ss}] Error: {Ex.Message}  origen: {Ex.StackTrace} \n");
            }
            catch (Exception) { throw; }
        }
    }
}

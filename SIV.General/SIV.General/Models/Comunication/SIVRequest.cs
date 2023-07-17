using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entities
{
    public enum CodeResponse
    {
        OK,
        Error
    }

    public class SIVRequest
    {
        public string Method { get; set; }
        public int TerminalSource { get; set; }
        public SIVParameters Parameters { get; set; }
    }

    public class SIVResponse
    {
        public CodeResponse Code { get; set; }
        public object Data { get; set; }
    }
}
    
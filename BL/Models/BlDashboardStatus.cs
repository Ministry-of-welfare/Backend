using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class BlDashboardStatus
    {
        public int Waiting { get; }
        public int InProgress { get; }
        public int Success { get; }
        public int Error { get; }
        public int Other { get; }

        public BlDashboardStatus(int waiting, int inProgress, int success, int error, int other)
        {
            Waiting = waiting;
            InProgress = inProgress;
            Success = success;
            Error = error;
            Other = other;
        }
    }
}
    

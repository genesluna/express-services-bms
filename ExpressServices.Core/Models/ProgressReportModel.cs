using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressServices.Core.Models
{
    public class ProgressReportModel
    {
        public int PercentageComplete { get; set; } = 0;

        public string CurrentTask { get; set; }
    }
}

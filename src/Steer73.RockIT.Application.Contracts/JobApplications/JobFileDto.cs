using Microsoft.VisualBasic.FileIO;
using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steer73.RockIT.JobApplications
{
    public class JobFileDto
    {
        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public JobFileType JobFileType { get; set; }
    }
}

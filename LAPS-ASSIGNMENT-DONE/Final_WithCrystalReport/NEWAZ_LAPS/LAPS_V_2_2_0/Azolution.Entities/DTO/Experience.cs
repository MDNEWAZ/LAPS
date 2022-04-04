using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{

    [Serializable]

    public class Experience
    {
        public int ExperienceID { get; set; }
        public string Company { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remarks { get; set; }


    }
}

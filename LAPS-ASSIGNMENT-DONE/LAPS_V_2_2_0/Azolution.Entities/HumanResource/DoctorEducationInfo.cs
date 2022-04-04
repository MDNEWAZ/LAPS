using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
   public class DoctorEducationInfo
    {
        public int DoctorEduInfiId { get; set; }
        public string Exam { get; set; }
        public DateTime Year { get; set; }
        public string Institute { get; set; }
        public decimal Result { get; set; }
        public int DoctorId { get; set; }
    }
}

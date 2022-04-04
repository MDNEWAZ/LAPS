using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
  public  class EducationDetail
    {
        public int EducationID { get; set; }
        public string Exam { get; set; }
        public int Year { get; set; }
        public string Institute { get; set; }
        public string  Result { get; set; }

    }
}

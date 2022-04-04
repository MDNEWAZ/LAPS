using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
    [Serializable]
   public class PersonalDetails
    {

       public int PersonalDetailsId { get; set; }
       public string FirstName { get; set; }

       public string LastName { get; set; }

       public string FatherName { get; set; }

       public string MotherName { get; set; }

       public DateTime DateOfBirth { get; set; }

       public int Gender { get; set; }

       public int Maritalstatus { get; set; }

       public string NationalIdNo { get; set; }

       public int Religion { get; set; }
       
       public string Mobile { get; set; }

       public string Address { get; set; }

       public string GenderName { get; set; }

       public string ReligionName { get; set; }

       public string MaritalstatusName { get; set; }


       
    }
}

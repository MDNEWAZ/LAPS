using Azolution.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.Service.Interface
{
   public interface IPersonalDetailsRepository
    {
       string SavePersonDetails(PersonalDetails objPerson);
       GridEntity<PersonalDetails> GetPersonalSummary(GridOptions options);

       string DeletePersonalInfo(PersonalDetails objPerson);
        
    }
}

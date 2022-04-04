using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class PersonalDetailsService : IPersonalDetailsRepository
    {
        PersonalDetailsDataService  _personalDetailsDataService= new PersonalDetailsDataService();
        public string SavePersonDetails(PersonalDetails objPerson)
        {
       
            return _personalDetailsDataService.SavePersonDetails(objPerson);
        }

        public string DeletePersonalInfo(PersonalDetails objPerson)
        {

            return _personalDetailsDataService.DeletePersonalInfo( objPerson);
        }

        public GridEntity<PersonalDetails> GetPersonalSummary( GridOptions options)
        {
            return _personalDetailsDataService.GetPersonalSummary(options);
        }

    }
}

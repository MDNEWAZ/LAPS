using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;

namespace Azolution.Entities.Sale
{
    public class Due
    {
        public int DueId { get; set; }
        public decimal FromDue { get; set; }
        public decimal ToDue { get; set; }

        public string Color { get; set; }
        public int Status { get; set; }
        public string EntryDate { get; set; }
        public string Updated { get; set; }
        public int UserId { get; set; }
        public int UpdateBy { get; set; }

        public int CompanyId { get; set; }
        public Company ACompany { get; set; }

        public int AllTypeId { get; set; }
        public AllType AAllType { get; set; }
    }
}

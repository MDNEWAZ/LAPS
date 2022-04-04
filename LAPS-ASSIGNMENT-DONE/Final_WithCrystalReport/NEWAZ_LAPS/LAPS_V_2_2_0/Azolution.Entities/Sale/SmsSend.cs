using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SmsSend
    {
        public int SmsId { get; set; }
        public string Contant { get; set; }
        public string LicenseType { get; set; }
        public string LicenseNo { get; set; }
        public string SendDate { get; set; }
        public string Mobile { get; set; }
        public int Flag { get; set; }
        public int IsRead { get; set; }
        public Product AProduct { get; set; }
    }
}

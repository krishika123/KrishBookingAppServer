using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBookkeepingAuth.Models
{
    public class Company
    {
        public string CompanyName { get; set; }
        public string CompanyTin { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string GhPostGps { get; set; }
        public string BusinessRegistrationNumber { get; set; }
        public string PostalAddress { get; set; }
        public string City { get; set; }
        public Guid TransactionCurrency { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
    }
}

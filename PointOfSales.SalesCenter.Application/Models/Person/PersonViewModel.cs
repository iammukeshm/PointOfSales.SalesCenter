using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointOfSales.SalesCenter.Application.Models.Person
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string _LastName;
        public string FirstName { get; set; }
        public string LastName {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = value;
                this.Name = $"{this.FirstName} {this._LastName}";
                this.Initials = $"{this.FirstName[0]}{this._LastName[0]}";
            }
        }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NickName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] Image { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

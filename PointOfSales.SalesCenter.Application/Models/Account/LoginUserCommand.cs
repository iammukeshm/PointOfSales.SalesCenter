using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.SalesCenter.Application.Models.Account
{
    public class LoginUserCommand
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}

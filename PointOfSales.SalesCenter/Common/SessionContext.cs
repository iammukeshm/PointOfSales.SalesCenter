using CoreLibrary.Helpers;
using PointOfSales.SalesCenter.Application.Constants;
using PointOfSales.SalesCenter.Application.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSales.SalesCenter.Common
{
    internal class SessionContext
    {
        public static string Name { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string UserName { get; set; }
        public static string Email { get; set; }
        public static string PhoneNumber { get; set; }
        public static ApiHelper ApiHelper { get; set; }
        public static bool IsAuthenticated { get; set; } = false;

        public static bool IsAdmin { get; set; } = false;

        public static List<string> Roles{ get; set; }

        public static void Kill()
        {
            Name = string.Empty;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            ApiHelper = null;
            IsAuthenticated = false;
            IsAdmin = false;
            Roles = null;
        }

        internal static void Authenticate(LoggedInUser data, string accessToken)
        {
            Name = $"{data.FirstName} {data.LastName}";
            UserName = data.UserName;
            FirstName = data.FirstName;
            LastName = data.LastName;
            Email = data.Email;
            PhoneNumber = data.PhoneNumber;
            Roles = data.Roles;
            ApiHelper = new ApiHelper(ApplicationSettings.ApiBaseAddress);
            ApiHelper.AddJwtAuthorization(data.Token);
            IsAuthenticated = true;
            if(data.Roles.Any(a=>a == AuthorizationConstants.Roles.Administrator.ToString()))
            {
                IsAdmin = true;
            }
        }
    }
}

using CoreLibrary.Helpers;

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

        public static void Authenticate(string firstName, string lastName, string userName,string email, string phoneNumer, string token)
        {
            Name = $"{firstName} {lastName}";
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumer;
            ApiHelper = new ApiHelper(ApplicationSettings.ApiBaseAddress);
            ApiHelper.AddJwtAuthorization(token);
            IsAuthenticated = true;
        }
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
        }
    }
}

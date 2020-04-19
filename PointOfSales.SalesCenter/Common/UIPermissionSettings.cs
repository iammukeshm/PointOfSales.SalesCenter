using PointOfSales.SalesCenter.Application.Constants;
using PointOfSales.SalesCenter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.SalesCenter.Common
{
    public class UIPermissionSettings
    {
        public static List<AuthorizationConstants.Roles> SalesCenterPermissions = new List<AuthorizationConstants.Roles>
        {
            AuthorizationConstants.Roles.Administrator,
            AuthorizationConstants.Roles.Operator
        };

    }
}

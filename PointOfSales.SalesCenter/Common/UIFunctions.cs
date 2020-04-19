using PointOfSales.SalesCenter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointOfSales.SalesCenter.Common
{
    public class UIFunctions
    {
        internal static bool IsUserAuthorizedForView(Views view)
        {
            if(!SessionContext.IsAdmin)
            {
                bool IsAuthorized = false;
                switch(view)
                {
                    case Views.SalesCenter:
                        IsAuthorized = UIPermissionSettings.SalesCenterPermissions.Any(x => SessionContext.Roles.Any(y => y == x.ToString()));
                        break;
                }
                return IsAuthorized;
            }
            else
            {
                //Add
                return true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates.Models
{
    public class Toggles
    {
        public const string SendNotificationVariableName = "Toggle_SendNotification";

        public static bool SendNotification
        {
            get
            {
                var toggle_SendNotification = Environment.GetEnvironmentVariable(SendNotificationVariableName);
                if (!string.IsNullOrEmpty(toggle_SendNotification) && bool.TryParse(toggle_SendNotification, out bool result))
                {
                    return result;
                }

                return false;
            }
        }
    }
}

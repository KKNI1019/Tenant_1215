using System;
using System.Collections.Generic;
using System.Text;

namespace tenantApp.Popups
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}

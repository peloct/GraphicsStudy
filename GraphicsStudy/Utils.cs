using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphicsStudy
{
    public static class Utils
    {
        public static T FindName<T>(string name) where T : class
        {
            return Application.Current.MainWindow.FindName(name) as T;
        }
    }
}

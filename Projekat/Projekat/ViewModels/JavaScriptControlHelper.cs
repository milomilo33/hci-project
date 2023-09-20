using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat.ViewModels
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
	{
        Window prozor;
        public JavaScriptControlHelper(Window w)
        {
            prozor =  w;
        }
        /*
        public void RunFromJavascript(string param)
        {
            prozor.doThings(param);
        }*/
    }
}

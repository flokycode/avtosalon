using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Prodazha_avto
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static user270_dbEntities Context { get; } = new user270_dbEntities();

        public static CarUser CurrentUser { get;set; } =null;
        


    }
}

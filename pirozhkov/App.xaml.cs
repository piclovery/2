using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace pirozhkov

{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static pirozhkov_spEntities Context { get;  } = new pirozhkov_spEntities();
        public static User CurrentUser { get; set; } = null;
    }
}

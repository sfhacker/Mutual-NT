
namespace EIV.MainApp
{
    using UserControlBase;
    using System.Windows;
    using System;
    using System.Reflection;

    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private const string USER_CONTROL_TYPE = "EIV.UserControl.Ejemplos.UControl1";
        private const string USER_CONTROL_TYPE = "EIV.UserControl.Ejemplos.UControl2";

        private IUserControlBase testUserControl1 = null;

        public MainWindow()
        {
            InitializeComponent();

            // change the path here
            Assembly userControlAssembly = Assembly.LoadFrom("C:\\msys\\1.0\\src\\temp\\EIV.UserControl.Ejemplos.dll");
            if (userControlAssembly != null)
            {
                this.testUserControl1 = Activator.CreateInstance(userControlAssembly.GetType(USER_CONTROL_TYPE)) as IUserControlBase;
                if (this.testUserControl1 != null)
                {
                    this.testUserControl1.Initialize();

                    // It should show the user control here
                    this.contentControl.Content = this.testUserControl1.MainInterface;
                }
            }
        }
    }
}

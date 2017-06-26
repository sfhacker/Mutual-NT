
namespace EIV.MutualNT
{
    using UI.UserTemplates.Interface;
    using System.Reflection;
    using System.Windows;
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string USER_TEMPLATES_ASSEMBLY = "..\\..\\..\\EIV.UI.UserTemplates.Tests\\bin\\Debug\\EIV.UI.UserTemplates.Tests.dll";
        private const string PAISES_FORM = "EIV.UI.UserTemplates.Tests.Paises";
        private const string PRESTAMOS_FORM = "EIV.UI.UserTemplates.Tests.PrestamosPendientesForm";
        private const string LOCALIDADES_FORM = "EIV.UI.UserTemplates.Tests.LocalidadesForm";

        private Assembly userControlAssembly = null;
        private UserTemplateInterface genericUserControl = null;

        public MainWindow()
        {
            InitializeComponent();

            this.userControlAssembly = Assembly.LoadFrom(USER_TEMPLATES_ASSEMBLY);
        }

        private void btnPaises_Click(object sender, RoutedEventArgs e)
        {
            // change the path here
            if (this.userControlAssembly != null)
            {
                this.genericUserControl = Activator.CreateInstance(userControlAssembly.GetType(PAISES_FORM)) as UserTemplateInterface;
                if (this.genericUserControl != null)
                {
                    this.genericUserControl.Initialize();

                    // It should show the user control here
                    /* this.contentControl.Content = this.testUserControl1.MainInterface; */
                    /* this.Content = this.testUse rControl1.MainInterface.Content; */


                    /* this.Content = this.genericUserControl.MainInterface; */
                    this.CreateNewWindow(this.genericUserControl.MainInterface);
                }
            }
        }

        private void CreateNewWindow(System.Windows.Controls.UserControl winContent)
        {
            Window newWin = new Window();

            newWin.Title = "Ventana Dinamica";
            newWin.Owner = this;

            newWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            newWin.Content = winContent;

            // Modal
            //newWin.ShowDialog();

            // Non-Modal
            newWin.Show();
        }

        private void btnPrestamos_Click(object sender, RoutedEventArgs e)
        {
            // change the path here
            if (this.userControlAssembly != null)
            {
                this.genericUserControl = Activator.CreateInstance(userControlAssembly.GetType(PRESTAMOS_FORM)) as UserTemplateInterface;
                if (this.genericUserControl != null)
                {
                    this.genericUserControl.Initialize();

                    /* this.Content = this.genericUserControl.MainInterface; */
                    this.CreateNewWindow(this.genericUserControl.MainInterface);
                }
            }
        }

        private void btnLocalidades_Click(object sender, RoutedEventArgs e)
        {
            if (this.userControlAssembly != null)
            {
                this.genericUserControl = Activator.CreateInstance(userControlAssembly.GetType(LOCALIDADES_FORM)) as UserTemplateInterface;
                if (this.genericUserControl != null)
                {
                    this.genericUserControl.Initialize();

                    this.CreateNewWindow(this.genericUserControl.MainInterface);
                }
            }
        }
    }
}
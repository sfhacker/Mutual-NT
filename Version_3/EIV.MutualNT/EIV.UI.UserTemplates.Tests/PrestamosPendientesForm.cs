
namespace EIV.UI.UserTemplates.Tests
{
    using Base;
    using Grid;
    using OData.Client.Context;
    using OData.Client.Interface;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;

    public sealed class PrestamosPendientesForm : UserTemplateBase<com.cairone.odataexample.PrestamoPendiente>
    {
        private string DEFAULT_SKIN = "/EIV.UI.UserTemplates;component/Skins/BlueSkin.xaml";

        private static IODataServiceContext service = null;

        public PrestamosPendientesForm() : base()
        {
            service = ODataServiceContext.Instance;

            string serviceURL = ReadSetting("webServiceURL");
            if (string.IsNullOrEmpty(serviceURL))
            {
                // TODO: fix this
                serviceURL = "http://192.168.1.174:8080/odataexample/odata/appexample.svc/";
            }
            //com.carione
            bool rst = service.Connect<com.cairone.odataexample.ODataExample>(serviceURL);
            if (!rst)
            {
                return;
            }

            this.SetGridColumns(this.GenerateGridColumns());

            this.SetGridItems(this.LoadAllPrestamos());

            this.SetDefaultSkin(DEFAULT_SKIN);
        }

        // ok
        private IList<GridColumn> GenerateGridColumns()
        {
            IList<GridColumn> cols = new List<GridColumn>();

            cols.Add(new GridColumn() { BindingName = "clave", Header = "Clave", UniqueName = "prestamo_clave" });
            cols.Add(new GridColumn() { BindingName = "fechaAlta", Header = "Alta", UniqueName = "prestamo_nombre" });
            cols.Add(new GridColumn() { BindingName = "prestamo", Header = "Prestamo", UniqueName = "prestamo_importe" });
            cols.Add(new GridColumn() { BindingName = "intereses", Header = "Intereses", UniqueName = "prestamo_interes" });

            return cols;
        }

        private ObservableCollection<com.cairone.odataexample.PrestamoPendiente> LoadAllPrestamos()
        {
            //Pais newPais = null;

            //ObservableCollection <Pais> query = new ObservableCollection<Pais>();
            ObservableCollection<com.cairone.odataexample.PrestamoPendiente> query = null;

            if (service == null)
            {
                return query;
            }
            if (!service.IsConnected)
            {
                return query;
            }

            query = service.LoadEntityItems<com.cairone.odataexample.PrestamoPendiente>("PrestamosPendientes");

            //newPais = new Pais() { id = 1, nombre = "Argentina", prefijo = "1234" };
            //query.Add(newPais);

            return query;
        }

        private string ReadSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];

                return result;
            }
            catch (ConfigurationErrorsException)
            {
                /* Console.WriteLine("Error reading app settings"); */
                //this.errorMessage = "Invalid App.config file.";
            }

            return null;
        }
    }
}
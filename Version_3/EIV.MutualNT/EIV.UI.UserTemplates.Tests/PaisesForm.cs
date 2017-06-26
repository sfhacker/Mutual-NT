
namespace EIV.UI.UserTemplates.Tests
{
    using OData.Client.Context;
    using OData.Client.Interface;
    using EIV.UI.UserTemplates.Base;
    using Grid;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    //using com.cairone.odataexample;

    public sealed class Paises : UserTemplateBase<com.cairone.odataexample.Pais>
    {
        public const string ODATA_SERVICE_URL_JAVA = "http://192.168.1.174:8080/odataexample/odata/appexample.svc/";

        private static IODataServiceContext service = null;

        public Paises() : base()
        {
            service = ODataServiceContext.Instance;

            //com.carione
            bool rst = service.Connect<com.cairone.odataexample.ODataExample>(ODATA_SERVICE_URL_JAVA);
            if (!rst)
            {
                return;
            }

            this.SetGridColumns(this.GenerateGridColumns());

            this.SetGridItems(this.LoadAllPaises());
        }

        // ok
        private IList<GridColumn> GenerateGridColumns()
        {
            IList<GridColumn> cols = new List<GridColumn>();

            cols.Add(new GridColumn() { BindingName = "id", Header = "Id", UniqueName = "pais_id", DataType = typeof(int) });
            cols.Add(new GridColumn() { BindingName = "nombre", Header = "Nombre", UniqueName = "pais_nombre" });
            cols.Add(new GridColumn() { BindingName = "prefijo", Header = "Prefijo", UniqueName = "pais_prefijo", DataFormat = "{00-00000000-00}" });
            cols.Add(new GridColumn() { BindingName = "dominio", Header = "Dominio", UniqueName = "pais_dominio", IsComboBox = true });
            cols.Add(new GridColumn() { BindingName = "editable", Header = "Editable", UniqueName = "pais_editable", IsCheckBox = true });

            return cols;
        }

        private ObservableCollection<com.cairone.odataexample.Pais> LoadAllPaises()
        {
            //Pais newPais = null;

            //ObservableCollection <Pais> query = new ObservableCollection<Pais>();
            ObservableCollection<com.cairone.odataexample.Pais> query = null;

            if (service == null)
            {
                return query;
            }
            if (!service.IsConnected)
            {
                return query;
            }

            // We need to pass some filters here!!!!!!
            query = service.LoadEntityItems<com.cairone.odataexample.Pais>("Paises");

            //newPais = new Pais() { id = 1, nombre = "Argentina", prefijo = "1234" };
            //query.Add(newPais);

            return query;
        }
    }

    //public sealed class Pais
    //{
    //    public Pais()
    //    {

    //    }

    //    public int id { get; set; }
    //    public string nombre { get; set; }

    //    public string prefijo { get; set; }

    //    public bool editable { get; set; }
    //}
}
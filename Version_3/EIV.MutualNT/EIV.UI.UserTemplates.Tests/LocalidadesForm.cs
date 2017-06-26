
namespace EIV.UI.UserTemplates.Tests
{
    using EIV.UI.UserTemplates.Base;
    using Grid;
    using OData.Client.Context;
    using OData.Client.Interface;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class LocalidadesForm : UserTemplateBase<com.cairone.odataexample.Localidad>
    {
        private const string WEB_SERVICE_URL = "http://192.168.1.174:8080/odataexample/odata/appexample.svc/";

        private string DEFAULT_SKIN = "/EIV.UI.UserTemplates;component/Skins/BlueSkin.xaml";

        private static IODataServiceContext service = null;

        public LocalidadesForm() : base()
        {
            service = ODataServiceContext.Instance;

            bool rst = service.Connect<com.cairone.odataexample.ODataExample>(WEB_SERVICE_URL);
            if (!rst)
            {
                return;
            }

            this.SetGridColumns(this.GenerateGridColumns());

            this.SetGridItems(this.LoadAllLocalidades());

            this.SetDefaultSkin(DEFAULT_SKIN);
        }

        private IList<GridColumn> GenerateGridColumns()
        {
            IList<GridColumn> cols = new List<GridColumn>();

            cols.Add(new GridColumn() { BindingName = "paisId", Header = "Pais Id", UniqueName = "localidad_paisId", IsVisible = false });
            cols.Add(new GridColumn() { BindingName = "provinciaId", Header = "Provincia Id", UniqueName = "localidad_provinciaId", IsVisible = false });
            cols.Add(new GridColumn() { BindingName = "localidadId", Header = "Localidad Id", UniqueName = "localidad_localidadId" });

            cols.Add(new GridColumn() { BindingName = "nombre", Header = "Nombre", UniqueName = "localidad_nombre" });
            cols.Add(new GridColumn() { BindingName = "cp", Header = "Cod. Postal", UniqueName = "localidad_cp" });
            cols.Add(new GridColumn() { BindingName = "prefijo", Header = "Prefijo", UniqueName = "localidad_prefijo" });

            return cols;
        }

        private ObservableCollection<com.cairone.odataexample.Localidad> LoadAllLocalidades()
        {
            ObservableCollection<com.cairone.odataexample.Localidad> query = null;

            if (service == null)
            {
                return query;
            }
            if (!service.IsConnected)
            {
                return query;
            }

            query = service.LoadEntityItems<com.cairone.odataexample.Localidad>("Localidades");

            return query;
        }
    }
}
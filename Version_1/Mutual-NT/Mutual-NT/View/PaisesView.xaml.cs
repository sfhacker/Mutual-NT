
namespace Mutual_NT.View
{
    using com.cairone.odataexample;
    using Microsoft.OData.Client;
    using ServiceContext;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using Telerik.Windows.Controls;
    using Telerik.Windows.Controls.GridView;
    using ViewModel;

    using System.Configuration;

    /// <summary>
    /// Interaction logic for PaisesView.xaml
    /// </summary>
    public partial class PaisesView : Window
    {
        private const string ODATA_SERVICE_URL_JAVA = "http://localhost:8080/odata/appexample.svc/";

        // App.config file
        private const string ODATA_SERVICE_APP_KEY = "webServiceURL";

        // Proxy class
        private ODataExample context = null;

        private ObservableCollection<MenuItem> rowContextMenuItems;

        // Delegate that returns void for the query result callback.
        private delegate void OperationResultCallback();

        //private TrackableEntities entityList = null;
        private CustomViewModel customViewModel = null;

        private ODataServiceContext serviceContext = null;


        public PaisesView()
        {
            InitializeComponent();

            this.txtId.Text = "";
            this.txtNombre.Text = "";

            this.customViewModel = new CustomViewModel();

            this.ServiceConnect();

            this.SetGridSource();

            this.InitializeRowContextMenuItems();
        }

        private void gridView_Loaded(object sender, RoutedEventArgs e)
        {
            // Works ok
            this.dataPager.SetBinding(RadDataPager.SourceProperty, new Binding("Items") { Source = this.gridView });

            // Put this in the view
            this.dataPager.PageSize = 5;
        }

        private void gridView_DataLoading(object sender, Telerik.Windows.Controls.GridView.GridViewDataLoadingEventArgs e)
        {
            this.statusInfo.Text = "Cargando datos ...";

            this.gridView.IsBusy = true;
        }

        private void gridView_DataLoaded(object sender, System.EventArgs e)
        {
            this.statusInfo.Text = "Carga de datos completa!";

            this.gridView.IsBusy = false;
        }
        private void ServiceConnect()
        {
            // value from App.config file
            string appServiceUrl = this.ReadSetting(ODATA_SERVICE_APP_KEY);
            if (string.IsNullOrEmpty(appServiceUrl))
            {
                // Refactor
                this.gridView.IsEnabled = false;
                this.statusInfo.Text = "Invalid Web Service URL.";
                this.btnAceptar.IsEnabled = false;

                return;
            }
            try
            {
                /* Uri serviceUrl = new Uri(ODATA_SERVICE_URL_JAVA); */
                Uri serviceUrl = new Uri(appServiceUrl);

                this.context = new ODataExample(serviceUrl);

                context.Format.UseJson();

                this.serviceContext = new ODataServiceContext(this.context);
            }
            catch (Exception ex)
            {
                this.statusInfo.Text = ex.Message;
            }
        }

        private void SetGridSource()
        {
            ObservableCollection<Pais> itemSource = this.LoadPaises();
            if (itemSource != null)
            {
                this.gridView.ItemsSource = itemSource;
            }
        }

        private ObservableCollection<Pais> LoadPaises()
        {
            if (this.context == null)
            {
                return null;
            }

            try
            {
                // 1.- Query
                // The service is down or any other network related issue(s)
                var paises = this.context.Paises.ToList();

                return new ObservableCollection<Pais>(paises);
            }
            catch (Exception ex)
            {
                this.statusInfo.Text = ex.Message;

                // Should i disable the grid here?
                this.gridView.IsEnabled = false;
                this.btnAceptar.IsEnabled = false;
            }

            return null;
        }

        private void GridContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (this.ClickedHeader != null)
            {
                //this.GridContextMenu.ItemsSource = this.headerContextMenuItems;
            }
            else if (this.ClickedRow != null)
            {
                //this.radGridView.SelectedItem = this.ClickedRow.DataContext;
                //foreach (var item in this.rowContextMenuItems)
                //{
                //    item.IsEnabled = true;
                //}
                this.GridContextMenu.ItemsSource = this.rowContextMenuItems;
            }
            else
            {
                //foreach (var item in this.rowContextMenuItems)
                //{
                //    if (!item.Text.Equals("Add"))
                //    {
                //        item.IsEnabled = false;
                //    }
                //}
                //this.GridContextMenu.ItemsSource = this.rowContextMenuItems;
            }
        }

        private GridViewHeaderCell ClickedHeader
        {
            get
            {
                return this.GridContextMenu.GetClickedElement<GridViewHeaderCell>();
            }
        }
        private GridViewRow ClickedRow
        {
            get
            {
                return this.GridContextMenu.GetClickedElement<GridViewRow>();
            }
        }

        private void GridContextMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MenuItem item = (e.OriginalSource as RadMenuItem).DataContext as MenuItem;
            switch (item.Text)
            {
                case "Detalle":
                    if (this.checkBox.IsChecked.HasValue)
                    {
                        if (this.checkBox.IsChecked.Value)
                        {
                            return;
                        }
                    }
                    //this.gridView.BeginInsert();
                    RadTabItem detalleTab = this.paisesTab.Items[1] as RadTabItem;
                    detalleTab.IsEnabled = true;
                    this.checkBox.IsEnabled = false;

                    //detalleTab.Visibility = Visibility.Visible;
                    //detalleTab.Focus();
                    //this.txtId.Text = 
                    if (this.ClickedRow != null)
                    {
                        // Clicked not on a valid row
                        if (this.ClickedRow.DataContext == null)
                        {
                            return;
                        }
                        // To refactor!
                        // Clicked on a new row but it is empty
                        Pais pais = this.ClickedRow.DataContext as Pais;
                        if (pais == null)
                        {
                            return;
                        }

                        this.txtId.Text = pais.id.ToString();
                        this.txtNombre.Text = pais.nombre;
                        this.txtPrefijo.Text = pais.prefijo.HasValue ? pais.prefijo.Value.ToString() : "";
                    }

                    this.paisesTab.SelectedItem = detalleTab;
                    break;
                case "Provincias":
                    this.gridView.BeginEdit();
                    break;
            }
        }

        private void InitializeRowContextMenuItems()
        {
            ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();
            MenuItem addItem = new MenuItem();
            addItem.Text = "Detalle";
            items.Add(addItem);
            
            MenuItem separatorItem = new MenuItem();
            separatorItem.IsSeparator = true;
            items.Add(separatorItem);

            MenuItem deleteItem = new MenuItem();
            deleteItem.Text = "Provincias";
            items.Add(deleteItem);

            this.rowContextMenuItems = items;
        }

        // Single save operation
        // Async mode
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            int prefijo = -1;

            DataServiceResponse resp = null;

            if (string.IsNullOrEmpty(this.txtNombre.Text))
            {
                return;
            }
            int paisId = int.Parse(this.txtId.Text);

            // Refactor
            // Which properties can be changed?
            // What about the UI validation?
            // The validation should be written in the user control itself by .... someone!
            Pais updatePais = this.context.Paises.ByKey(paisId).GetValue();
            if (updatePais != null)
            {
                updatePais.nombre = this.txtNombre.Text;
                if (!string.IsNullOrEmpty(this.txtPrefijo.Text))
                {
                    bool rst = int.TryParse(this.txtPrefijo.Text, out prefijo);
                }
                updatePais.prefijo = prefijo;

                this.context.UpdateObject(updatePais);
                this.context.BeginSaveChanges(OnSaveChangesCompleted, null);
            }
        }

        private void OnSaveChangesCompleted(IAsyncResult result)
        {
            DataServiceResponse resp = null;

            // Use the Dispatcher to ensure that the operation returns in the UI thread.
            this.Dispatcher.BeginInvoke(new OperationResultCallback(delegate
            {
                try
                {
                    // Complete the save changes operation.
                    //result.AsyncWaitHandle.WaitOne();     // ????

                    resp = this.context.EndSaveChanges(result);
                    this.gridView.Rebind();

                    this.statusInfo.Text = "Datos actualizados exitosamente";
                }
                catch (DataServiceRequestException ex)
                {
                    this.statusInfo.Text = string.Format("Error actualizando datos: {0}", ex.ToString());

                    // ????
                    // if the prev operation fails, then
                    // the next one fails as well (either same or different entity)
                    // Is this a service issue?
                    this.context.CancelRequest(result);
                }

                RadTabItem listadoTab = this.paisesTab.Items[0] as RadTabItem;
                this.paisesTab.SelectedItem = listadoTab;

            }), null);
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rst;
            DataServiceResponse resp = null;

            rst = System.Windows.MessageBox.Show("Se procedera a eliminar este registro. Desea continuar?", "Pais::Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rst == MessageBoxResult.Yes)
            {
                int paisId = int.Parse(this.txtId.Text);

                Pais deletePais = this.context.Paises.ByKey(paisId).GetValue();
                if (deletePais != null)
                {
                    this.context.DeleteObject(deletePais);
                    this.context.BeginSaveChanges(OnSaveChangesCompleted, null);
                }
            }
        }

        private void paisesTab_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            RadSelectionChangedEventArgs eArgs = e as RadSelectionChangedEventArgs;
            // We now have access to e.AddedItems and e.RemovedItems 

            string tabItem = ((sender as RadTabControl).SelectedItem as RadTabItem).Header as string;

            if (string.IsNullOrEmpty(tabItem))
            {
                return;
            }
            if (tabItem.Equals("Detalle"))
            {
                var one = this.gridView.SelectedItem;
                if (one != null)
                {
                    Pais pais = one as Pais;
                    if (pais != null)
                    {
                        this.txtId.Text = pais.id.ToString();
                        this.txtNombre.Text = pais.nombre;
                        this.txtPrefijo.Text = pais.prefijo.HasValue ? pais.prefijo.Value.ToString() : "";
                    }
                }
            }
        }

        // Batch mode
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            IAsyncResult batchRst = null;

            MessageBoxResult rst;

            int totalChanges = this.customViewModel.TotalChanges;
            if (totalChanges < 1)
            {
                return;
            }

            rst = System.Windows.MessageBox.Show(string.Format("Se guardaran todos los cambios realizados hasta el momento: {0}. Desea proceder?", totalChanges), "Pais::Guardar cambios", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rst == MessageBoxResult.Yes)
            {
                this.statusInfo.Text = "Enviando cambios ... ";

                this.serviceContext.ProcessOperations(this.customViewModel.Items);

                batchRst = this.context.BeginSaveChanges(SaveChangesOptions.BatchWithSingleChangeset | SaveChangesOptions.ReplaceOnUpdate, OnSaveChangesCompleted, null);

                this.customViewModel.Items.Clear();
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void gridView_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            Pais newPais = null;

            if (e.EditAction == GridViewEditAction.Cancel)
            {
                e.UserDefinedErrors.Clear();

                return;
            }

            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                //Add the new entry to the data base.
                //this.radGridView1.SelectedRows[0].Cells["Picture Name"].Value;

                GridViewRow row = e.Row;
                if (e.Row is GridViewNewRow)
                {
                    newPais = e.NewData as Pais;
                    if (newPais == null)
                    {
                        return;
                    }

                    // TODO:
                    //paisViewModel.Insert(newPais);

                    // Some sort of validation here
                    // empty fields, etc
                    // this is entity specific
                    // The UI should handle this
                    if (string.IsNullOrEmpty(newPais.nombre))
                    {
                        return;
                    }
                    this.customViewModel.InsertEntity<Pais>(newPais, "Paises");

                    //System.Windows.MessageBox.Show(string.Format("New Pais: {0} {1}", newPais.id, newPais.nombre));
                }

                return;
            }

            

            if (e.EditOperationType == GridViewEditOperationType.Edit)
            {
                GridViewRow row = e.Row;

                newPais = e.NewData as Pais;
                if (newPais == null)
                {
                    return;
                }

                // TODO:
                //bool rst = this.customViewModel.HasModifications(newPais, e.OldValues);
                //if (!rst)
               // {
                //    return;
                //}

                //System.Windows.MessageBox.Show(string.Format("Update Pais: {0} {1}", newPais.id, newPais.nombre));

                //paisViewModel.Update(newPais);
                //this.entityList.ProcessEntity<Pais>(newPais, TrackableEntity.Operation.Update);
                this.customViewModel.UpdateEntity<Pais>(newPais);
            }

        }

        private void gridView_Deleting(object sender, GridViewDeletingEventArgs e)
        {
            if (e.Items != null)
            {
                List<Pais> paises = e.Items.Cast<Pais>().ToList();

                if (paises != null)
                {
                    if (paises.Count > 0)
                    {
                        this.customViewModel.RemoveEntities<Pais>(paises);
                    }
                }
            }
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
                this.statusInfo.Text = "Invalid App.config file.";
            }

            return null;
        }

        private void txtNombre_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            MessageBox.Show("Here", "Nombre");
        }

        private void GridViewDataColumn_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            MessageBox.Show("Here", "Nombre");
        }
    }
}
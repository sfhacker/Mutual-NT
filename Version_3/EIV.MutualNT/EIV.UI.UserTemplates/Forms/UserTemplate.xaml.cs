
namespace EIV.UI.UserTemplates.Forms
{
    using Grid;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Telerik.Windows.Controls;
    using Telerik.Windows.Controls.GridView;

    /// <summary>
    /// Interaction logic for UserTemplate.xaml
    /// </summary>
    public partial class UserTemplate : UserControl
    {
        private string DEFAULT_SKIN = "/EIV.UI.UserTemplates;component/Skins/GreenSkin.xaml";

        private string gridStatusInfo = null;

        private string defaultSkinName = null;

        public UserTemplate()
        {
            InitializeComponent();
        }

        public string GridStatusInfo
        {
            get
            {
                return this.gridStatusInfo;
            }
        }

        public void UpdateGridCols(IList<GridColumn> gridCols)
        {
            GridViewDataColumn newCol = null;

            if (this.gridView.Columns.Count != 0)
            {
                return;
            }

            foreach (GridColumn col in gridCols)
            {
                if (col.IsComboBox)
                {
                    this.gridView.Columns.Add(this.CreateComboBoxGridColumn(col));

                    continue;
                }
                if (col.IsCheckBox)
                {
                    this.gridView.Columns.Add(this.CreateCheckBoxGridColumn(col));

                    continue;
                }

                this.gridView.Columns.Add(this.CreateTextGridColumn(col));
            }
        }

        private GridViewDataColumn CreateTextGridColumn(GridColumn col)
        {
            GridViewDataColumn newCol = new GridViewDataColumn();

            newCol.DataMemberBinding = new System.Windows.Data.Binding(col.BindingName);
            newCol.Header = col.Header;
            newCol.UniqueName = col.UniqueName;
            newCol.DataType = col.DataType;
            newCol.IsVisible = col.IsVisible;
            if (!string.IsNullOrEmpty(col.DataFormat))
            {
                newCol.DataFormatString = col.DataFormat;
                newCol.DataMemberBinding.StringFormat = col.DataFormat;
            }
            return newCol;
        }

        private GridViewComboBoxColumn CreateComboBoxGridColumn(GridColumn col)
        {
            GridViewComboBoxColumn newCol = new GridViewComboBoxColumn();

            newCol.DataMemberBinding = new System.Windows.Data.Binding(col.BindingName);
            newCol.Header = col.Header;
            newCol.UniqueName = col.UniqueName;

            return newCol;
        }

        private GridViewCheckBoxColumn CreateCheckBoxGridColumn(GridColumn col)
        {
            GridViewCheckBoxColumn newCol = new GridViewCheckBoxColumn();

            newCol.DataMemberBinding = new System.Windows.Data.Binding(col.BindingName);
            newCol.Header = col.Header;
            newCol.UniqueName = col.UniqueName;

            return newCol;
        }

        public void SetDefaultSkin(string skinName)
        {
            this.defaultSkinName = skinName;
        }

        // private
        public void SetGridSource<T>(ObservableCollection<T> itemSource) where T : class
        {
            //if (!this.gridView.IsLoaded)
            //{
            //    return;
            //}
            // What if I want to reset the grid source?
            if (itemSource == null)
            {
                return;
            }
            this.gridView.ItemsSource = itemSource;
        }

        private void ApplySkinFromPath(string skinPath)
        {
            Uri skinDictUri = new Uri(skinPath, UriKind.Relative);

            // Tell the Application to load the skin resources.
            this.ApplySkin(skinDictUri);
        }

        private void ApplySkin(Uri skinDictionaryUri)
        {
            // Load the ResourceDictionary into memory.
            ResourceDictionary skinDict = Application.LoadComponent(skinDictionaryUri) as ResourceDictionary;

            Collection<ResourceDictionary> mergedDicts = base.Resources.MergedDictionaries;

            // Remove the existing skin dictionary, if one exists.
            // NOTE: In a real application, this logic might need
            // to be more complex, because there might be dictionaries
            // which should not be removed.
            if (mergedDicts.Count > 0)
                mergedDicts.Clear();

            // Apply the selected skin so that all elements in the
            // application will honor the new look and feel.
            mergedDicts.Add(skinDict);
        }

        #region Grid Events
        private void gridView_DataLoading(object sender, Telerik.Windows.Controls.GridView.GridViewDataLoadingEventArgs e)
        {
            this.gridStatusInfo = "Cargando datos ...";

            GridViewDataControl dataControl = (GridViewDataControl)sender;
            if (dataControl != null)
            {
                dataControl.IsBusy = true;
            }
        }

        private void gridView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GridViewDataControl dataControl = (GridViewDataControl)sender;
            if (dataControl != null)
            {
                this.dataPager.SetBinding(RadDataPager.SourceProperty, new Binding("Items") { Source = this.gridView });

                // TODO:
                /*
                // CreateDataPager(dataControl);
                this.BindDataPager(this.gridPager, dataControl);

                ObservableCollection<Pais> items = this.LoadPaises();

                SetGridSource(dataControl, items);
                */

                //this.ApplySkinFromPath(".\\Skins\\BlueSkin.xaml");
                if (string.IsNullOrEmpty(this.defaultSkinName))
                {
                    this.ApplySkinFromPath(DEFAULT_SKIN);
                } else
                {
                    this.ApplySkinFromPath(this.defaultSkinName);
                }
                
            }
        }

        private void gridView_Deleting(object sender, Telerik.Windows.Controls.GridViewDeletingEventArgs e)
        {
            if (e.Items != null)
            {
                // TODO:
                /* User can delete more than one row at the same time */
                /*List<Pais> paises = e.Items.Cast<Pais>().ToList();

                if (paises != null)
                {
                    if (paises.Count > 0)
                    {
                        /* this.customViewModel.RemoveEntities<Pais>(paises); */
                  /*  }
                }*/
            }
        }

        private void gridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            // TODO:
            //Pais newPais = null;

            if (e.EditAction == GridViewEditAction.Cancel)
            {
                e.UserDefinedErrors.Clear();

                return;
            }

            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                GridViewRow row = e.Row;
                if (e.Row is GridViewNewRow)
                {
                    object cellValue = e.NewData;

                    // TODO:
                    /*
                    newPais = e.NewData as Pais;
                    if (newPais == null)
                    {
                        return;
                    }*/

                    // Some sort of validation here
                    // empty fields, etc
                    // this is entity specific
                    // The UI should handle this
                    /*
                    if (string.IsNullOrEmpty(newPais.nombre))
                    {
                        return;
                    }*/

                    // TODO: ......
                    /* this.customViewModel.InsertEntity<Pais>(newPais, "Paises"); */

                    //System.Windows.MessageBox.Show(string.Format("New Pais: {0} {1}", newPais.id, newPais.nombre));
                }

                return;
            }

            if (e.EditOperationType == GridViewEditOperationType.Edit)
            {
                GridViewRow row = e.Row;

                /*
                newPais = e.NewData as Pais;
                if (newPais == null)
                {
                    return;
                }*/

                // TODO: 
                /* this.customViewModel.UpdateEntity<Pais>(newPais); */
            }
        }

        private void gridView_DataLoaded(object sender, System.EventArgs e)
        {
            this.gridStatusInfo = "Carga de datos completa!";

            GridViewDataControl dataControl = (GridViewDataControl)sender;
            if (dataControl != null)
            {
                dataControl.IsBusy = false;
            }
        }

        private void mainTab_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnSalir_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Window someForm = (Window) this.Parent;
            if (someForm != null)
            {
                someForm.Close();
            }
        }

        private void btnGuardar_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        #endregion Grid Events
    }
}
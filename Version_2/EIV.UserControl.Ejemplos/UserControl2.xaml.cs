
using System;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EIV.UserControl.Ejemplos
{
    /// <summary>
    /// Lógica de interacción para UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : System.Windows.Controls.UserControl
    {
        public UserControl2()
        {
            InitializeComponent();

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

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            IAsyncResult batchRst = null;

            MessageBoxResult rst;

            rst = System.Windows.MessageBox.Show(string.Format("Se guardaran todos los cambios realizados hasta el momento: {0}. Desea proceder?", 0), "Pais::Guardar cambios", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rst == MessageBoxResult.Yes)
            {
                this.statusInfo.Text = "Enviando cambios ... ";
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            int prefijo = -1;
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rst;

            rst = System.Windows.MessageBox.Show("Se procedera a eliminar este registro. Desea continuar?", "Pais::Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rst == MessageBoxResult.Yes)
            {
                int paisId = int.Parse(this.txtId.Text);
            }
        }

        private void txtNombre_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            MessageBox.Show("Here", "Nombre");
        }

        private void GridViewDataColumn_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            MessageBox.Show("Here", "Nombre");
        }

        private void gridView_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                e.UserDefinedErrors.Clear();

                return;
            }
        }

        private void gridView_Deleting(object sender, GridViewDeletingEventArgs e)
        {
            if (e.Items != null)
            {
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
        }
    }
}


namespace EIV.UI.UserTemplates.Grid
{
    using System;
    public sealed class GridColumn
    {
        public GridColumn()
        {
            this.DataType = typeof(string);
            this.IsVisible = true;
        }

        public Type DataType { get; set; }

        // GridViewDataColumn
        public string BindingName { get; set; }

        public string Header { get; set; }

        public string UniqueName { get; set; }

        public bool IsVisible { get; set; }

        public bool IsReadOnly { get; set; }

        // This could be improved (e.g. inheritance)
        public bool IsComboBox { get; set; }

        public bool IsCheckBox { get; set; }

        // eg cuit
        // {0:dd, MMM, yyyy}
        // {00-00000000-00}
        public string DataFormat { get; set; }

    }
}
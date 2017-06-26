
namespace EIV.UI.UserTemplates.Base
{
    using Grid;
    using Interface;
    using System.Collections.Generic;

    using Forms;
    using System.Windows.Controls;
    using System.Collections.ObjectModel;

    public abstract class UserTemplateBase<T> : UserTemplateInterface where T : class
    {
        // private System.Windows.Controls.UserControl userForm = null;
        private UserTemplate userTemplateForm = null;

        // Grid/GridColumn.cs
        private IList<GridColumn> gridColumns;

        private ObservableCollection<T> gridItems = null;

        private string defaultSkin = null;

        public UserControl MainInterface
        {
            get
            {
                //return this.userForm;
                return this.userTemplateForm;
            }
        }

        public void Initialize()
        {
            //this.userForm = new UserTemplate();
            this.userTemplateForm = new UserTemplate();
            if (this.userTemplateForm != null)
            {
                this.userTemplateForm.UpdateGridCols(this.gridColumns);
                this.userTemplateForm.SetGridSource<T>(gridItems);
                this.userTemplateForm.SetDefaultSkin(this.defaultSkin);
            }
        }

        public void Dispose()
        {
            //this.userForm = null;
            this.userTemplateForm = null;
        }

        public void SetGridColumns(IList<GridColumn> cols)
        {
            this.gridColumns = cols;
        }

        public void SetGridItems(ObservableCollection<T> gridItems)
        {
            this.gridItems = gridItems;

            //this.userTemplateForm.SetGridSource<T>(gridItems);
        }

        public void SetDefaultSkin(string skinName)
        {
            this.defaultSkin = skinName;
        }
    }
}
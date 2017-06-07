
namespace EIV.UserControl.Ejemplos
{
    using System;
    using System.Windows.Controls;
    using EIV.UserControlBase;
    public sealed class UControl1 : IUserControlBase
    {
        private System.Windows.Controls.UserControl  myMainInterface = null;

        public UControl1()
        {
            this.myMainInterface = new UserControl1();
        }
        public UserControl MainInterface
        {
            get
            {
                return this.myMainInterface;
            }
        }

        public void Dispose()
        {
        }

        public void Initialize()
        {
        }
    }
}
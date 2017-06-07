

namespace EIV.UserControl.Ejemplos
{
    using System.Windows.Controls;
    using EIV.UserControlBase;

    public sealed class UControl2 : IUserControlBase
    {
        private System.Windows.Controls.UserControl myMainInterface = null;

        public UControl2()
        {
            this.myMainInterface = new UserControl2();
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

namespace EIV.UserControlBase
{
    public interface IUserControlBase
    {
        //System.Windows.Forms.UserControl MainInterface {get;}
        //System.Windows.Forms.Form MainInterface { get; }
        System.Windows.Controls.UserControl MainInterface { get; }

        void Initialize();
        void Dispose();
    }
}
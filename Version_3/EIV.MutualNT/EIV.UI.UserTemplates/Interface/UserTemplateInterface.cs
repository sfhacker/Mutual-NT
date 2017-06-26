
namespace EIV.UI.UserTemplates.Interface
{
    public interface UserTemplateInterface
    {
        System.Windows.Controls.UserControl MainInterface { get; }

        void Initialize();

        void Dispose();
    }
}
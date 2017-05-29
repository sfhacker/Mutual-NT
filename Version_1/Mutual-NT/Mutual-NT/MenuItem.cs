
namespace Mutual_NT
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    public sealed class MenuItem : INotifyPropertyChanged
    {
        private bool isEnabled = true;
        private bool isSeparator = false;
        private string text;

        private ObservableCollection<MenuItem> subItems;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                if (this.isEnabled != value)
                {
                    this.isEnabled = value;
                    this.OnNotifyPropertyChanged("IsEnabled");
                }
            }
        }
        public bool IsSeparator
        {
            get
            {
                return this.isSeparator;
            }
            set
            {
                if (this.isSeparator != value)
                {
                    this.isSeparator = value;
                    this.OnNotifyPropertyChanged("IsSeparator");
                }
            }
        }
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnNotifyPropertyChanged("Text");
                }
            }
        }
        public ObservableCollection<MenuItem> SubItems
        {
            get
            {
                if (this.subItems == null)
                {
                    this.subItems = new ObservableCollection<MenuItem>();
                }
                return this.subItems;
            }
            set
            {
                if (this.subItems != value)
                {
                    this.subItems = value;
                    this.OnNotifyPropertyChanged("SubItems");
                }
            }
        }
        private void OnNotifyPropertyChanged(string ptopertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(ptopertyName));
            }
        }
    }
}
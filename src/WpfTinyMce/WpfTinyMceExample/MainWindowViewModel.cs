using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTinyMceExample
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _Html;

        public string Html
        {
            get { return _Html; }
            set
            {
                if (value == _Html)
                    return;

                _Html = value;
                OnPropertyChanged(nameof(Html));
            }
        }

    }
}

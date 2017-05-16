using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTinyMceExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _VM;
        public MainWindow()
        {
            InitializeComponent();
            _VM = new MainWindowViewModel();
            DataContext = _VM;
        }

        private void LoadSampleDocButton_Click(object sender, RoutedEventArgs e)
        {
            _VM.Html = "<html><body><p>This is a sample document.</p><p><i>Italic text looks like this.</i></p></body></html>";
        }

        private void ShowHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_VM.Html);
        }
    }
}

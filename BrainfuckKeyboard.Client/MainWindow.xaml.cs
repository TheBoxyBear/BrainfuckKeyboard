using BrainfuckKeyboard.Engine;

using System.Windows;
using System.Windows.Controls;

namespace BrainfuckKeyboard.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BrainfuckEngine engine = new();

        public MainWindow()
        {
            InitializeComponent();
            engine.OutputReceived += (_, output) => OutputBox.Text += output;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            engine.AddToken(((string)((Button)sender).Content)[0]);
            engine.ResumeExecution();
        }
    }
}

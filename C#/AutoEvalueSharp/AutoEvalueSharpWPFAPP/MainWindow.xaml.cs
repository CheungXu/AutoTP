using AutoEvalueSharp.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoEvalueSharpWPFAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StartButton.IsEnabled = false;
        }

        public bool IsPrepared
        {
            get => !(string.IsNullOrEmpty(DetectTextBox.Text)
                || string.IsNullOrEmpty(TrueTextBox.Text)
                || string.IsNullOrEmpty(OutputTextBox.Text));
        }


        private void DetectButton_Click(object sender, RoutedEventArgs e)
        {
            DetectTextBox.Text = ChooseDirectory();
            StartButton.IsEnabled = IsPrepared;
        }

        private void TrueButton_Click(object sender, RoutedEventArgs e)
        {
            TrueTextBox.Text = ChooseDirectory();
            StartButton.IsEnabled = IsPrepared;
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text = ChooseDirectory();
            StartButton.IsEnabled = IsPrepared;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string detectDir = DetectTextBox.Text;
            string trueDir = TrueTextBox.Text;
            string outputDir = OutputTextBox.Text;

            try
            {
                ResultParser parser = new ResultParser(detectDir, trueDir, outputDir);
                parser.Run();

                System.Windows.MessageBox.Show("完成", "提示");
            }
            catch (FormatException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "错误");
            }
        }

        private string ChooseDirectory()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            return dialog.SelectedPath;
        }
    }
}

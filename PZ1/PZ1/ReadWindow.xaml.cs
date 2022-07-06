using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public ReadWindow(String naziv)
        {
            InitializeComponent();

            string str = "";
            foreach (var s in naziv.Split())
            {
                str += s;
            }

            string putanja = "../../Rtf datoteka/";
            string konacnaPutanja = putanja + naziv + ".rtf";
            if (!File.Exists(konacnaPutanja))
                return;
            using (FileStream fileStream = new FileStream(konacnaPutanja, FileMode.Open))
            {
                TextRange textRange = new TextRange(viewRichTextBox.Document.ContentStart, viewRichTextBox.Document.ContentEnd);
                textRange.Load(fileStream, DataFormats.Rtf);
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

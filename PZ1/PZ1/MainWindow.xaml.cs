using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Classes;



namespace PZ1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static BindingList<LegendeNBALige> Legende { get; set; }

        public MainWindow()
        {
            
            if (Legende == null)
            {
                Legende = new BindingList<LegendeNBALige>();
            }
            DataContext = this;
            InitializeComponent();
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult defaultResult;
            if (MessageBox.Show("Da li ste sigurni da želite da izađete? " + "Podaci neće biti sačuvani ako nastavite!",
               "Oprez", MessageBoxButton.YesNo, MessageBoxImage.Warning, defaultResult = MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            AddWindow newWindow = new AddWindow("");
            newWindow.ShowDialog();
            dataGridLegende.Items.Refresh();

        }


        private void buttonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(((LegendeNBALige)dataGridLegende.SelectedItem).Putanja);

            addWindow.textBoxIme.Text = Legende[dataGridLegende.SelectedIndex].ImeIPrezime;
            addWindow.textBoxBrGodina.Text = Legende[dataGridLegende.SelectedIndex].BrojGodina.ToString();
            addWindow.imageAddWindow.Source = new BitmapImage(new Uri(Legende[dataGridLegende.SelectedIndex].Slika));
            addWindow.datePicker.SelectedDate = Legende[dataGridLegende.SelectedIndex].Datum;

            addWindow.textBoxIme.Foreground = Brushes.Black;
            addWindow.textBoxBrGodina.Foreground = Brushes.Black ;

            loadFromRtfDocument(addWindow);

            addWindow.ButtonAddWindowDODAJ.Content = "IZMENI";
            addWindow.labelNazivDodavanjeIgraca.Content = "IZMENA IGRAČA";
            addWindow.index = dataGridLegende.SelectedIndex;
            addWindow.ShowDialog();
            
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult defaultResult;
            if (MessageBox.Show("Da li ste sigurni da želite da obrišete " + Legende[dataGridLegende.SelectedIndex].ImeIPrezime + " igrača? ",
               "Oprez", MessageBoxButton.YesNo, MessageBoxImage.Warning, defaultResult = MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                File.Delete(((LegendeNBALige)dataGridLegende.SelectedItem).Putanja);
                Legende.RemoveAt(dataGridLegende.SelectedIndex);
                dataGridLegende.Items.Refresh();
            }
            
        }

        private void buttonProcitaj_Click(object sender, RoutedEventArgs e)
        {

                ReadWindow readWindow = new ReadWindow(((LegendeNBALige)dataGridLegende.SelectedItem).ImeIPrezime);
            
                readWindow.viewImage.Source = new BitmapImage(new Uri(Legende[dataGridLegende.SelectedIndex].Slika));
                readWindow.DataContext = dataGridLegende.SelectedItem;

                readWindow.viewLabelIme.Content = Legende[dataGridLegende.SelectedIndex].ImeIPrezime;
                readWindow.viewLabelBrojGodina.Content = Legende[dataGridLegende.SelectedIndex].BrojGodina.ToString();
                readWindow.viewLabelDatum.Content = Legende[dataGridLegende.SelectedIndex].Datum.ToString();

                readWindow.ShowDialog();
                dataGridLegende.Items.Refresh();
            

        }
        public void loadFromRtfDocument(AddWindow addWindow)
        {
            string naziv = "";
            foreach (var s in addWindow.textBoxIme.Text.Split())
            {
                naziv += s;
            }

            string putanja = "../../Rtf datoteka/";
            string konacnaPutanja = putanja + naziv + ".rtf";
            using (FileStream fileStream = new FileStream(konacnaPutanja, FileMode.Open))
            {
                TextRange textRange = new TextRange(addWindow.richTextBox1.Document.ContentStart, addWindow.richTextBox1.Document.ContentEnd);
                textRange.Load(fileStream, DataFormats.Rtf);
                
            }
        }

        
    }
}

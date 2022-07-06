using Classes;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public bool boolSlika = false;
        public int index = 0;
        public string putanjaZaBrisanje;

        public static BindingList<double> fontSize = new BindingList<double> { 8, 9, 11, 12, 14, 16, 18, 22, 25 };

        public AddWindow(String putanjaDokumenta)
        {
            InitializeComponent();
            putanjaZaBrisanje = putanjaDokumenta;
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = fontSize;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonAddWindowIZADJI_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult defaultResult;
            if (MessageBox.Show("Da li ste sigurni da želite da izađete iz ovog prozora? ",
               "Oprez", MessageBoxButton.YesNo, MessageBoxImage.Warning, defaultResult = MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void ButtonAddWindowDODAJ_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                if (ButtonAddWindowDODAJ.Content.Equals("DODAJ"))
                {

                    String putanja = saveRtfDocument();
                    MainWindow.Legende.Add(new LegendeNBALige(Int32.Parse(textBoxBrGodina.Text), textBoxIme.Text, (DateTime)datePicker.SelectedDate, imageAddWindow.Source.ToString(),putanja));
                    this.Close();
                }
                else
                {
                    File.Delete(putanjaZaBrisanje);
                    string putanja = saveRtfDocument();
                    MainWindow.Legende[index] = new LegendeNBALige(Int32.Parse(textBoxBrGodina.Text), textBoxIme.Text, (DateTime)datePicker.SelectedDate, imageAddWindow.Source.ToString(), putanja) ;
                    MainWindow.Legende.ResetBindings();
                    this.Close();

                }
            }
            else
            {
                MessageBox.Show("Polja nisu dobro popunjena!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool validate()
        {
            bool result = true;
            if (ButtonAddWindowDODAJ.Content.Equals("DODAJ"))
            {
                if (LegendeNBALige.imena.Contains(textBoxIme.Text.Trim().ToUpper()))
                {

                    result = false;
                    textBoxIme.BorderBrush = Brushes.Red;
                    textBoxIme.BorderThickness = new Thickness(1);
                    labelImeGreska.Content = "Igrač sa ovim imenom već postoji!";
                    MessageBox.Show("GRESKA: Igrač sa ovim imenom " + textBoxIme.Text + " vec postoji. Pokusajte sa drugim imenom.",
                                    "Greska", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            if (textBoxIme.Text.Trim().Equals("") || textBoxIme.Text.Trim().Equals("Unesite ime i prezime..."))
            {
                result = false;
                labelImeGreska.Content = "Unesite ime i prezime!";
                textBoxIme.BorderBrush = Brushes.Red;
                textBoxIme.BorderThickness = new Thickness(2);
            }
            else
            {
                labelImeGreska.Content = "";
                textBoxIme.BorderBrush = Brushes.Gray;
            }
            TextRange range = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            if (range.Text.Trim().Length.Equals(0))
            {
                result = false;
                labelRichGreska.Content = "Unesite tekst!";
                richTextBox1.BorderBrush = Brushes.Red;
                richTextBox1.BorderThickness = new Thickness(2);

                
            }
            else
            {
                labelRichGreska.Content = "";
                richTextBox1.BorderBrush = Brushes.Gray;

            }

            if(textBoxBrGodina.Text.Trim().Equals("") || textBoxBrGodina.Text.Trim().Equals("Unesite godine..."))
            {
                result = false;
                labelGodineGreska.Content = "Unesite godine!";
                textBoxBrGodina.BorderThickness = new Thickness(2);
                textBoxBrGodina.BorderBrush = Brushes.Red;
            }
            else
            {
                labelGodineGreska.Content = "";
                textBoxBrGodina.BorderBrush = Brushes.Gray;

               
                try
                {
                    UInt64.Parse(textBoxBrGodina.Text.Trim());
                    labelGodineGreska.Content = "";
                }
                catch (Exception e)
                {
                    result = false;
                    textBoxBrGodina.BorderBrush = Brushes.Red;
                    textBoxBrGodina.BorderThickness = new Thickness(2);
                    labelGodineGreska.Content = "Unesite pravilno godine!";
                    Console.WriteLine(e.Message);
                }

            }

            if (datePicker.SelectedDate == null)
            {
                result = false;
                labelDatumGreska.Content = "Unesite datum!";
                datePicker.BorderBrush = Brushes.Red;

            }
            else
            {
                labelDatumGreska.Content = "";
                datePicker.BorderBrush = Brushes.Gray;
            }

            if (!boolSlika && ButtonAddWindowDODAJ.Content.Equals("DODAJ"))
            {
                labelSlikaGreska.Content = "Dodajte sliku!";
                Browse.BorderBrush = Brushes.Red;
                Browse.BorderThickness = new Thickness(2);
                result = false;
            }
            else
            {
                labelSlikaGreska.Content = "";
                Browse.BorderBrush = Brushes.Gray;
            }




            return result;
        }

        private void textBoxIme_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Trim().Equals(String.Empty))
            {
                textBoxIme.Text = "Unesite ime i prezime...";
                textBoxIme.Foreground = Brushes.LightSlateGray;
            }
        }

        private void textBoxIme_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Trim().Equals("Unesite ime i prezime..."))
            {
                textBoxIme.Text = "";
                textBoxIme.Foreground = Brushes.Black;
            }

        }

        private void Button_Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imageAddWindow.Source = new BitmapImage(fileUri);
                boolSlika = true;
            }
        }

        private void textBoxBrGodina_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxBrGodina.Text.Trim().Equals("Unesite godine..."))
            {
                textBoxBrGodina.Text = "";
                textBoxBrGodina.Foreground = Brushes.Black;
            }
        }

        private void textBoxBrGodina_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxBrGodina.Text.Trim().Equals(String.Empty))
            {
                textBoxBrGodina.Text = "Unesite godine...";
                textBoxBrGodina.Foreground = Brushes.LightSlateGray;
            }
        }

        public String saveRtfDocument()
        {


            String naziv = "";
            foreach (var s in textBoxIme.Text.Split())
            {
                naziv += s;

            }

            String putanja = "../../Rtf datoteka/";
            String konacnaPutanja = putanja + naziv + ".rtf";
            using (FileStream fileStream = new FileStream(konacnaPutanja, FileMode.Create))
            {
                TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                textRange.Save(fileStream, System.Windows.DataFormats.Rtf);
                LegendeNBALige.imena.Add(naziv.ToUpper());

            }

            return konacnaPutanja;
        }

        private void richTextBox1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp;

            temp = richTextBox1.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;

            cmbFontSize.SelectedItem = richTextBox1.Selection.GetPropertyValue(Inline.FontSizeProperty);

            temp = richTextBox1.Selection.GetPropertyValue(Inline.FontWeightProperty);
            buttonBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = richTextBox1.Selection.GetPropertyValue(Inline.FontStyleProperty);
            buttonItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = richTextBox1.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            buttonUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = ColorLabel.Background;

            ColorLabel.Background = (SolidColorBrush)temp;
            
            
        }

        private void richTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            string trText = textRange.Text.Trim();

            int wordCount = 0;

            foreach (string word in trText.Split(' '))
            {
                if (word != "")
                {
                    wordCount++;
                }
            }

            textBoxWordsCount.Text = wordCount.ToString();
        }

        private void buttonColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!richTextBox1.Selection.IsEmpty)
                {
                    richTextBox1.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
                    ColorLabel.Background = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                }
            }
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null && !richTextBox1.Selection.IsEmpty)
            {
                richTextBox1.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.SelectedItem);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null && !richTextBox1.Selection.IsEmpty)
            {
                richTextBox1.Selection.ApplyPropertyValue(Inline.FontFamilyProperty,cmbFontFamily.SelectedItem);
            }
        }
    }
}

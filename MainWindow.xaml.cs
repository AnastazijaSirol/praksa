using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Markup;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool running = false;

        private readonly Translation translation;
        public MainWindow()
        {
            InitializeComponent();
            translation = new Translation();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dobro.IsChecked == true)
            {
                MessageBox.Show(Properties.Resources.sveDobro);
            }
            else if (lose.IsChecked == true)
            {
                MessageBox.Show(Properties.Resources.sveNeDobro);
            }
        }

        private void promjena_Click(object sender, RoutedEventArgs e)
        {
            if (running)
            {
                tekst_promjena.Text = Properties.Resources.zaustavljeno;
                promjena.Content = Properties.Resources.pokreni;
            }
            else
            {
                tekst_promjena.Text = Properties.Resources.pokrenuto;
                promjena.Content = Properties.Resources.bt_promjena;
            }

            running = !running;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.Resources.nastavak, " ", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                odgovor.Text = Properties.Resources.pozitivno;
            }
            else
            {
                odgovor.Text = Properties.Resources.negativno;
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingSpinner.Visibility = Visibility.Visible; 

                if (btn_jezik.Content.ToString() == "Engleski")
                {
                    await Task.Run(() => translation.TranslateResources()); 
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("hr-HR");
                }

                ReloadResources();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri prevođenju: " + ex.Message);
            }
            finally
            {
                LoadingSpinner.Visibility = Visibility.Hidden; 
            }

            Refresh(); 
        }

        private void ReloadResources()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
        }

        private void Refresh()
        {
            var currentWindow = Application.Current.MainWindow;

            if (currentWindow != null)
            {
                MainWindow newWindow = new MainWindow();
                newWindow.Left = currentWindow.Left;
                newWindow.Top = currentWindow.Top;
                newWindow.Width = currentWindow.Width;
                newWindow.Height = currentWindow.Height;
                newWindow.WindowState = currentWindow.WindowState;

                currentWindow.Close();

                Application.Current.MainWindow = newWindow;
                newWindow.Show();
            }
        }
    }
}

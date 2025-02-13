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
            translation= new Translation(); 
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
            MessageBoxResult result = MessageBox.Show(Properties.Resources.nastavak, "Pristanak", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                odgovor.Text = Properties.Resources.pozitivno;
            }
            else
            {
                odgovor.Text = Properties.Resources.negativno;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (btn_jezik.Content.ToString() == "Engleski")
            {
                translation.TranslateResources();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("hr-HR");
            }
            RefreshUI();
        }

        private void RefreshUI()
        {
            var currentWindow = Application.Current.MainWindow;

            if (currentWindow != null)
            {
                MainWindow newWindow = new MainWindow();

                currentWindow.Close();

                Application.Current.MainWindow = newWindow;
                newWindow.Show();
            }
        }
    }
}

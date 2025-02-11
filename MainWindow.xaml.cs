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
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Xml;

namespace demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool running = false;
        string currentLanguage = "hr";
        string svedobro = Properties.Strings.sveDobro;
        string svenedobro = Properties.Strings.sveNeDobro;
        string zaustavljeno = Properties.Strings.zaustavljeno;
        string pokreni = Properties.Strings.pokreni;
        string pokrenuto = Properties.Strings.pokrenuto;
        string promjena_2 = Properties.Strings.bt_promjena;
        string pozitivno = Properties.Strings.pozitivno;
        string negativno = Properties.Strings.negativno;
        string nastavak = Properties.Strings.nastavak;

        public MainWindow()
        {
            InitializeComponent();
            SetTextFromResources();
        }

        private void SetTextFromResources()
        {
            appName.Content = Properties.Strings.AppName;
            dobro.Content = Properties.Strings.Dobro;
            lose.Content = Properties.Strings.Lose;
            btn_ispis.Content = Properties.Strings.bt_ispis;
            promjena.Content = Properties.Strings.bt_promjena;
            tekst_promjena.Text = Properties.Strings.tekst_promjena;
            btn_posalji.Content = Properties.Strings.bt_posalji;
            lista1.Content = Properties.Strings.lista1;
            lista2.Content = Properties.Strings.lista2;
            lista3.Content = Properties.Strings.lista3;
            lista4.Content = Properties.Strings.lista4;
            lista5.Content = Properties.Strings.lista5;
            btn_jezik.Content = Properties.Strings.jezik;
            svedobro = Properties.Strings.sveDobro;
            svenedobro = Properties.Strings.sveNeDobro;
            zaustavljeno = Properties.Strings.zaustavljeno;
            pokreni = Properties.Strings.pokreni;
            pokrenuto = Properties.Strings.pokrenuto;
            promjena_2 = Properties.Strings.bt_promjena;
            pozitivno = Properties.Strings.pozitivno;
            negativno = Properties.Strings.negativno;
            nastavak = Properties.Strings.nastavak;

        }
        private void SaveTranslationToResx(string inputText, string translatedText, string languageCode)
        {
            string resourceFilePath = $@"C:\Users\anastazijas\source\repos\demo\Properties\Strings_en.resx";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(resourceFilePath);

                XmlNode? rootNode = xmlDoc.SelectSingleNode("root");
                if (rootNode == null)
                {
                    MessageBox.Show("Error: Invalid RESX file format.");
                    return;
                }

                XmlNodeList dataNodes = xmlDoc.GetElementsByTagName("data");
                bool entryUpdated = false;

                foreach (XmlNode dataNode in dataNodes)
                {
                    XmlNode? nameNode = dataNode?["value"];
                    if (nameNode != null && nameNode.InnerText == inputText)
                    {
                        XmlNode? valueNode = dataNode?.SelectSingleNode("value");
                        if (valueNode != null)
                        {
                            valueNode.InnerText = translatedText;
                            entryUpdated = true;
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"Value node not found for resource key: {inputText}");
                            return;
                        }
                    }
                }

                if (!entryUpdated)
                {
                    MessageBox.Show($"Resource key '{inputText}' not found to update its value.");
                }

                xmlDoc.Save(resourceFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving translation to resource file: {ex.Message}");
            }
        }
        public string TranslateText(string inputText, string srcLang, string targetLang)
        {
            try
            {
                string pythonExePath = @"C:\ProgramData\anaconda3\python.exe";
                string pythonScriptPath = @"C:\Users\anastazijas\Desktop\translate.py";

                string arguments = $"\"{pythonScriptPath}\" \"{inputText}\" {srcLang} {targetLang}";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pythonExePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process? process = Process.Start(startInfo);

                if (process == null)
                {
                    return "Error: Process could not be started.";
                }

                using (process)
                using (StreamReader outputReader = process.StandardOutput)
                using (StreamReader errorReader = process.StandardError)
                {
                    string result = outputReader.ReadToEnd();
                    string error = errorReader.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        return $"Error from Python script: {error}";
                    }

                    SaveTranslationToResx(inputText, result.Trim(), targetLang);

                    return result.Trim();
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dobro.IsChecked == true)
            {
                MessageBox.Show(svedobro);
            }
            else if (lose.IsChecked == true)
            {
                MessageBox.Show(svenedobro);
            }
        }

        private void promjena_Click(object sender, RoutedEventArgs e)
        {
            if (running)
            {
                tekst_promjena.Text = zaustavljeno;
                promjena.Content = pokreni;
            }
            else
            {
                tekst_promjena.Text = pokrenuto;
                promjena.Content = promjena_2;
            }

            running = !running;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(nastavak, "Pristanak", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                odgovor.Text = pozitivno;
            }
            else
            {
                odgovor.Text = negativno;
            }
        }

        private string GetTranslationFromResx(string originalText, string languageCode)
        {
            string resourceFilePath = @"C:\Users\anastazijas\source\repos\demo\Properties\Strings_en.resx";
            string sourceFilePath = @"C:\Users\anastazijas\source\repos\demo\Properties\Strings.resx";

            try
            {
                if (!File.Exists(resourceFilePath))
                {
                    File.Copy(sourceFilePath, resourceFilePath);
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sourceFilePath);

                XmlNodeList dataNodes = xmlDoc.GetElementsByTagName("data");

                string? resourceKey = string.Empty;

                foreach (XmlNode dataNode in dataNodes)
                {
                    XmlNode? valueNode = dataNode.SelectSingleNode("value");
                    if (valueNode != null && valueNode.InnerText.Equals(originalText))
                    {
                        XmlNode? nameNode = dataNode.Attributes?["name"];
                        if (nameNode != null)
                        {
                            resourceKey = nameNode.Value;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(resourceKey))
                {
                    MessageBox.Show("Key not found for the given value.");
                    return string.Empty;
                }

                xmlDoc.Load(resourceFilePath);
                dataNodes = xmlDoc.GetElementsByTagName("data");

                foreach (XmlNode dataNode in dataNodes)
                {
                    XmlNode? nameNode = dataNode.Attributes?["name"];
                    if (nameNode != null && nameNode.Value != null)
                    {
                        if (nameNode.Value.Equals(resourceKey))
                        {
                            XmlNode? valueNode = dataNode.SelectSingleNode("value");
                            if (valueNode != null)
                            {
                                string translatedValue = valueNode.InnerText ?? string.Empty;
                                if (translatedValue.Equals(originalValue))
                                {
                                    return null;
                                }
                                return translatedValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting translation from resource file: {ex.Message}");
            }

            return string.Empty;
        }
        private void Translate(string targetLanguage)
        {
            appName.Content = GetTranslationFromResx(Properties.Strings.AppName, targetLanguage);
            if (string.IsNullOrEmpty(appName.Content.ToString()))
            {
                appName.Content = TranslateText(Properties.Strings.AppName, currentLanguage, targetLanguage);
            }

            dobro.Content = GetTranslationFromResx(Properties.Strings.Dobro, targetLanguage);
            if (string.IsNullOrEmpty(dobro.Content.ToString()))
            {
                dobro.Content = TranslateText(Properties.Strings.Dobro, currentLanguage, targetLanguage);
            }

            lose.Content = GetTranslationFromResx(Properties.Strings.Lose, targetLanguage);
            if (string.IsNullOrEmpty(lose.Content.ToString()))
            {
                lose.Content = TranslateText(Properties.Strings.Lose, currentLanguage, targetLanguage);
            }

            btn_ispis.Content = GetTranslationFromResx(Properties.Strings.bt_ispis, targetLanguage);
            if (string.IsNullOrEmpty(btn_ispis.Content.ToString()))
            {
                btn_ispis.Content = TranslateText(Properties.Strings.bt_ispis, currentLanguage, targetLanguage);
            }

            promjena.Content = GetTranslationFromResx(Properties.Strings.bt_promjena, targetLanguage);
            if (string.IsNullOrEmpty(promjena.Content.ToString()))
            {
                promjena.Content = TranslateText(Properties.Strings.bt_promjena, currentLanguage, targetLanguage);
            }

            tekst_promjena.Text = GetTranslationFromResx(Properties.Strings.tekst_promjena, targetLanguage);
            if (string.IsNullOrEmpty(tekst_promjena.Text))
            {
                tekst_promjena.Text = TranslateText(Properties.Strings.tekst_promjena, currentLanguage, targetLanguage);
            }

            btn_posalji.Content = GetTranslationFromResx(Properties.Strings.bt_posalji, targetLanguage);
            if (string.IsNullOrEmpty(btn_posalji.Content.ToString()))
            {
                btn_posalji.Content = TranslateText(Properties.Strings.bt_posalji, currentLanguage, targetLanguage);
            }

            lista1.Content = GetTranslationFromResx(Properties.Strings.lista1, targetLanguage);
            if (string.IsNullOrEmpty(lista1.Content.ToString()))
            {
                lista1.Content = TranslateText(Properties.Strings.lista1, currentLanguage, targetLanguage);
            }

            lista2.Content = GetTranslationFromResx(Properties.Strings.lista2, targetLanguage);
            if (string.IsNullOrEmpty(lista2.Content.ToString()))
            {
                lista2.Content = TranslateText(Properties.Strings.lista2, currentLanguage, targetLanguage);
            }

            lista3.Content = GetTranslationFromResx(Properties.Strings.lista3, targetLanguage);
            if (string.IsNullOrEmpty(lista3.Content.ToString()))
            {
                lista3.Content = TranslateText(Properties.Strings.lista3, currentLanguage, targetLanguage);
            }

            lista4.Content = GetTranslationFromResx(Properties.Strings.lista4, targetLanguage);
            if (string.IsNullOrEmpty(lista4.Content.ToString()))
            {
                lista4.Content = TranslateText(Properties.Strings.lista4, currentLanguage, targetLanguage);
            }

            lista5.Content = GetTranslationFromResx(Properties.Strings.lista5, targetLanguage);
            if (string.IsNullOrEmpty(lista5.Content.ToString()))
            {
                lista5.Content = TranslateText(Properties.Strings.lista5, currentLanguage, targetLanguage);
            }

            svedobro = GetTranslationFromResx(Properties.Strings.sveDobro, targetLanguage);
            if (string.IsNullOrEmpty(svedobro))
            {
                svedobro = TranslateText(Properties.Strings.sveDobro, currentLanguage, targetLanguage);
            }

            svenedobro = GetTranslationFromResx(Properties.Strings.sveNeDobro, targetLanguage);
            if (string.IsNullOrEmpty(svenedobro))
            {
                svenedobro = TranslateText(Properties.Strings.sveNeDobro, currentLanguage, targetLanguage);
            }

            zaustavljeno = GetTranslationFromResx(Properties.Strings.zaustavljeno, targetLanguage);
            if (string.IsNullOrEmpty(zaustavljeno))
            {
                zaustavljeno = TranslateText(Properties.Strings.zaustavljeno, currentLanguage, targetLanguage);
            }

            pokreni = GetTranslationFromResx(Properties.Strings.pokreni, targetLanguage);
            if (string.IsNullOrEmpty(pokreni))
            {
                pokreni = TranslateText(Properties.Strings.pokreni, currentLanguage, targetLanguage);
            }

            pokrenuto = GetTranslationFromResx(Properties.Strings.pokrenuto, targetLanguage);
            if (string.IsNullOrEmpty(pokrenuto))
            {
                pokrenuto = TranslateText(Properties.Strings.pokrenuto, currentLanguage, targetLanguage);
            }

            pozitivno = GetTranslationFromResx(Properties.Strings.pozitivno, targetLanguage);
            if (string.IsNullOrEmpty(pozitivno))
            {
                pozitivno = TranslateText(Properties.Strings.pozitivno, currentLanguage, targetLanguage);
            }

            negativno = GetTranslationFromResx(Properties.Strings.negativno, targetLanguage);
            if (string.IsNullOrEmpty(negativno))
            {
                negativno = TranslateText(Properties.Strings.negativno, currentLanguage, targetLanguage);
            }

            nastavak = GetTranslationFromResx(Properties.Strings.nastavak, targetLanguage);
            if (string.IsNullOrEmpty(nastavak))
            {
                nastavak = TranslateText(Properties.Strings.nastavak, currentLanguage, targetLanguage);
            }

            promjena_2 = GetTranslationFromResx(Properties.Strings.bt_promjena, targetLanguage);
            if (string.IsNullOrEmpty(promjena_2))
            {
                promjena_2 = TranslateText(Properties.Strings.bt_promjena, currentLanguage, targetLanguage);
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string targetLanguage;

            if (btn_jezik.Content.ToString() == "Engleski")
            {
                targetLanguage = "en";
                btn_jezik.Content = "Croatian";

                Translate(targetLanguage);
                
                currentLanguage = "en";
            }
            else
            {
                targetLanguage = "hr";
                btn_jezik.Content = "Engleski";

                SetTextFromResources();

                currentLanguage = "hr";
            }
        }
    }
}

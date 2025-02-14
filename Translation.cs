using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace test
{
    internal class Translation
    {
        public void TranslateResources()
        {
            string resourcesFilePath = @"C:\Users\anastazijas\source\repos\test\Properties\Resources.resx";
            string translatedFilePath = @"C:\Users\anastazijas\source\repos\test\Properties\Resources.en.resx";

            XElement resources = XElement.Load(resourcesFilePath);

            if (!File.Exists(translatedFilePath))
            {
                using (FileStream fs = File.Create(translatedFilePath)) { }

                XDocument emptyResx = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("root",
                        new XElement("resheader", new XAttribute("name", "resmimetype"),
                            new XElement("value", "text/microsoft-resx")),
                        new XElement("resheader", new XAttribute("name", "version"),
                            new XElement("value", "2.0")),
                        new XElement("resheader", new XAttribute("name", "reader"),
                            new XElement("value", "System.Resources.ResXResourceReader, System.Windows.Forms")),
                        new XElement("resheader", new XAttribute("name", "writer"),
                            new XElement("value", "System.Resources.ResXResourceWriter, System.Windows.Forms"))
                    )
                );

                emptyResx.Save(translatedFilePath);  
            }

            XElement translatedResources = XElement.Load(translatedFilePath);

            foreach (var data in resources.Descendants("data"))
            {
                string name = data.Attribute("name").Value;
                string value = data.Element("value")?.Value;

                if (!string.IsNullOrEmpty(value))
                {
                    var translatedData = translatedResources.Descendants("data")
                        .FirstOrDefault(d => d.Attribute("name")?.Value == name);

                    if (translatedData != null)
                    {
                        string translated = translatedData.Element("value")?.Value;

                        if (translated != value)
                        {
                            continue;
                        }
                    }

                    string translatedValue = TranslateText(value, "hr", "en");

                    var existingTranslatedData = translatedResources.Descendants("data")
                        .FirstOrDefault(d => d.Attribute("name")?.Value == name);

                    if (existingTranslatedData != null)
                    {
                        existingTranslatedData.Element("value").Value = translatedValue;
                    }
                    else
                    {
                        translatedResources.Add(new XElement("data",
                            new XAttribute("name", name),
                            new XElement("value", translatedValue)
                        ));
                    }
                }
            }

            translatedResources.Save(translatedFilePath);
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

                Process process = Process.Start(startInfo);

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

                    return result.Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error translating: {ex.Message}");
                return ex.Message;
            }
        }
    }
}

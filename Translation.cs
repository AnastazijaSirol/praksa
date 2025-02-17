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
            string untranslatedFilePath = @"C:\Users\anastazijas\source\repos\test\Properties\Untranslated.resx";

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

            XElement untranslatedResources = new XElement("root",
            resources.Descendants("data")
                .Where(data =>
                    !translatedResources.Descendants("data").Any(d =>
                        d.Attribute("name")?.Value == data.Attribute("name")?.Value))
                .Select(data => new XElement("data",
                    new XAttribute("name", data.Attribute("name")?.Value),
                    new XElement("value", data.Element("value")?.Value)
                ))
            );

            if (!untranslatedResources.Elements("data").Any())
            {
                return;
            }

            untranslatedResources.Save(untranslatedFilePath);

            string translatedXml = "";

            try
            {
                translatedXml = TranslateText(untranslatedFilePath, "hr", "en");
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            if (string.IsNullOrEmpty(translatedXml))
            {
                Console.WriteLine("Greška u prijevodu.");
                return;
            }

            XElement translatedXElement = XElement.Parse(translatedXml);

            foreach (var data in translatedXElement.Descendants("data"))
            {
                string name = data.Attribute("name")?.Value;
                string translatedValue = data.Element("value")?.Value;

                var existingEntry = translatedResources.Descendants("data")
                    .FirstOrDefault(d => d.Attribute("name")?.Value == name);

                if (existingEntry == null)
                {
                    translatedResources.Add(new XElement("data",
                        new XAttribute("name", name),
                        new XElement("value", translatedValue)
                    ));
                }
            }

            translatedResources.Save(translatedFilePath);
        }

        public string TranslateText(string filePath, string srcLang, string targetLang)
        {
            try
            {
                string pythonExePath = @"C:\ProgramData\anaconda3\python.exe";
                string pythonScriptPath = @"C:\Users\anastazijas\Desktop\translate.py";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pythonExePath,
                    Arguments = $"\"{pythonScriptPath}\" \"{filePath}\" {srcLang} {targetLang}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process == null) return null;

                    using (StreamReader outputReader = process.StandardOutput)
                    {
                        string result = outputReader.ReadToEnd();
                        process.WaitForExit();
                        return result.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri pozivanju prijevoda: {ex.Message}");
                return null;
            }
        }
    }
}


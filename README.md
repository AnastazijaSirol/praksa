# Praksa

```
private string GetTranslationFromResx(string originalText, string languageCode)
{
    string resourceFilePath = $@"C:\Users\anastazijas\source\repos\demo\Properties\Strings_{languageCode}.resx";
    string sourceFilePath = @"C:\Users\anastazijas\source\repos\demo\Properties\Strings.resx";

    try
    {
        if (!File.Exists(resourceFilePath))
        {
            File.Copy(sourceFilePath, resourceFilePath);
        }

        XmlDocument sourceXml = new XmlDocument();
        sourceXml.Load(sourceFilePath);
        XmlNodeList sourceDataNodes = sourceXml.GetElementsByTagName("data");

        string? resourceKey = null;
        string? originalValue = null;

        foreach (XmlNode dataNode in sourceDataNodes)
        {
            XmlNode? valueNode = dataNode.SelectSingleNode("value");
            if (valueNode != null && valueNode.InnerText.Equals(originalText))
            {
                XmlNode? nameNode = dataNode.Attributes?["name"];
                if (nameNode != null)
                {
                    resourceKey = nameNode.Value;
                    originalValue = valueNode.InnerText;
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty(resourceKey))
        {
            MessageBox.Show("Key not found for the given value.");
            return string.Empty;
        }

        XmlDocument translatedXml = new XmlDocument();
        translatedXml.Load(resourceFilePath);
        XmlNodeList translatedDataNodes = translatedXml.GetElementsByTagName("data");

        foreach (XmlNode dataNode in translatedDataNodes)
        {
            XmlNode? nameNode = dataNode.Attributes?["name"];
            if (nameNode != null && nameNode.Value == resourceKey)
            {
                XmlNode? valueNode = dataNode.SelectSingleNode("value");
                if (valueNode != null)
                {
                    string translatedValue = valueNode.InnerText ?? string.Empty;
                    
                    if (translatedValue.Equals(originalValue))
                    {
                        return string.Empty;
                    }
                    
                    return translatedValue;
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
```

microsoft tehnologija

.net 4.8 framework

wpf

10-20 labela?

resorce datoteke?

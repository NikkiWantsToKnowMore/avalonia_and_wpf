using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;
using System.Xml.XPath;

namespace WPFForExperiment.Properties
{
    /// <summary>
    /// Провайдер настроек
    /// </summary>
    public class CustomFileSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string DEFAULT_CONFIG_NAME = "user.config";
        private string _appName = "WPFForExperiment";
        private bool _useEncryption = false;
        private string _configPath = "";

        #region SettingsProvider Implementation

        public override string ApplicationName
        {
            get => _appName;
            set => _appName = value;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                config = new NameValueCollection();

            base.Initialize(!string.IsNullOrEmpty(name) ? name : GetType().Name, config);

            // Чтение параметров конфигурации
            _useEncryption = bool.Parse(config["encryptSensitiveData"] ?? "false");

            string configFileName = config["configFileName"] ?? "user.config";
            bool encryptData = bool.Parse(config["encryptSensitiveData"] ?? "false");

            //string customFileName = config["configFileName"]!;
            _configPath = GetConfigFilePath(configFileName ?? DEFAULT_CONFIG_NAME);

            Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
        }

        public override SettingsPropertyValueCollection GetPropertyValues( SettingsContext context,SettingsPropertyCollection properties)
        {
            var values = new SettingsPropertyValueCollection();

            if (!File.Exists(_configPath))
                return CreateDefaultValues(properties, values);

            try
            {
                var doc = new XmlDocument();
                doc.Load(_configPath);

                string[] pathParts = GetConfigSectionName(context).Split('.');
                XmlNode currentNode = doc.DocumentElement!;

                // Находим узел настроек
                foreach (string part in pathParts)
                {
                    currentNode = FindNode(currentNode, part);
                    if (currentNode == null) break;
                }

                // Заполняем значения
                foreach (SettingsProperty property in properties)
                {
                    var value = new SettingsPropertyValue(property);

                    if (currentNode != null)
                    {
                        XmlNode valueNode = FindNode(currentNode, property.Name);
                        if (valueNode != null)
                        {
                            value.PropertyValue = ConvertValue(
                                valueNode.InnerText,
                                property.PropertyType,
                                property.DefaultValue);
                        }
                    }

                    values.Add(value);
                }
            }
            catch (Exception ex)
            {
                throw new SettingsException("Failed to load settings", ex);
            }

            return values;
        }

        private XmlNode FindNode(XmlNode parentNode, string nodeName)
        {
            foreach (XmlNode child in parentNode.ChildNodes)
            {
                if (child.Name == nodeName)
                {
                    return child;
                }
            }
            return null!;
        }

        private object ConvertValue(string value, Type targetType, object defaultValue)
        {
            try
            {
                if (targetType == typeof(double))
                    return double.Parse(value, CultureInfo.InvariantCulture);

                if (targetType == typeof(int))
                    return int.Parse(value);

                if (targetType == typeof(bool))
                    return bool.Parse(value);

                // Для остальных типов
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                // Возвращаем значение по умолчанию при ошибке
                return defaultValue; 
            }
        }


        public override void SetPropertyValues(
                SettingsContext context,
            SettingsPropertyValueCollection values)
        {
            XmlDocument doc = new XmlDocument();

            // Загружаем существующий файл или создаем новый
            if (File.Exists(_configPath))
            {
                try
                {
                    doc.Load(_configPath);
                }
                catch (XmlException)
                {
                    // Если файл поврежден, создаем новый документ
                    doc = new XmlDocument();
                    doc.AppendChild(doc.CreateElement("configuration"));
                }
            }
            else
            {
                doc.AppendChild(doc.CreateElement("configuration"));
            }

            // Получаем корневой элемент
            XmlNode root = doc.DocumentElement!;

            // Создаем иерархию узлов для настроек
            string[] pathParts = GetConfigSectionName(context).Split('.');
            XmlNode currentNode = root;

            foreach (string part in pathParts)
            {
                currentNode = FindOrCreateNode(currentNode, part);
            }

            // Сохраняем значения
            foreach (SettingsPropertyValue value in values)
            {
                if (value.IsDirty)
                {
                    XmlNode valueNode = FindOrCreateNode(currentNode, value.Name);
                    valueNode.InnerText = value.PropertyValue?.ToString() ?? string.Empty;
                }
            }

            // Сохраняем документ
            SaveConfigDocument(doc);
        }
        private void SaveConfigDocument(XmlDocument doc)
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                // Сохраняем во временный файл
                using (var writer = XmlWriter.Create(tempFile, new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8
                }))
                {
                    doc.Save(writer);
                }

                // Создаем директорию, если не существует
                Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);

                // Заменяем старый файл
                File.Copy(tempFile, _configPath, true);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }
        private XmlNode FindOrCreateNode(XmlNode parentNode, string nodeName)
        {
            // Проверка валидности имени узла
            if (string.IsNullOrWhiteSpace(nodeName))
            {
                throw new ArgumentException("Node name cannot be null or empty", nameof(nodeName));
            }

            // Ручной поиск существующего узла
            foreach (XmlNode child in parentNode.ChildNodes)
            {
                if (child.Name.Equals(nodeName, StringComparison.Ordinal))
                {
                    return child;
                }
            }

            // Создание нового узла с проверкой документа
            if (parentNode.OwnerDocument == null)
            {
                throw new InvalidOperationException("Parent node does not belong to a document");
            }

            XmlNode newNode = parentNode.OwnerDocument.CreateElement(nodeName.Trim());
            return parentNode.AppendChild(newNode)!;
        }
        #endregion

        #region IApplicationSettingsProvider Implementation

        public SettingsPropertyValue GetPreviousVersion(
            SettingsContext context,
            SettingsProperty property)
        {
            string oldConfigPath = FindPreviousConfigVersion();
            if (!File.Exists(oldConfigPath))
                return null!;

            try
            {
                var oldDoc = new XmlDocument();
                oldDoc.Load(oldConfigPath);

                var oldSettingsNode = GetSettingsNode(oldDoc, GetConfigSectionName(context));
                if (oldSettingsNode?.SelectSingleNode(property.Name) is XmlNode oldNode)
                {
                    return new SettingsPropertyValue(property)
                    {
                        PropertyValue = _useEncryption && IsEncrypted(property)
                            ? DecryptValue(oldNode.InnerText)
                            : oldNode.InnerText
                    };
                }
            }
            catch 
            { 
            }

            return null!;
        }

        public void Reset(SettingsContext context)
        {
            if (File.Exists(_configPath))
            {
                try 
                { 
                    File.Delete(_configPath); 
                }
                catch 
                { 
                }
            }
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            string oldConfigPath = FindPreviousConfigVersion();
            if (!File.Exists(oldConfigPath))
                return;

            try
            {
                var oldValues = GetPropertyValues(context, properties);
                SetPropertyValues(context, oldValues);
            }
            catch 
            { 
            }
        }

        #endregion

        #region Helper Methods

        private string GetConfigFilePath(string fileName)
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                ApplicationName,
                fileName);
        }

        private string FindPreviousConfigVersion()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                ApplicationName);

            if (!Directory.Exists(appDataPath))
                return null!;

            // Поиск предыдущей версии (например, по дате изменения)
            var dirInfo = new DirectoryInfo(appDataPath);
            var configFile = dirInfo.GetFiles("*.config")
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault(f => f.FullName != _configPath);

            return configFile?.FullName!;
        }

        private string GetConfigSectionName(SettingsContext context)
        {
            string groupName = context["GroupName"] as string ?? "DefaultGroup";
            string settingsKey = context["SettingsKey"] as string ?? "DefaultSettings";

            // Удаляем запрещенные символы из имен
            groupName = SanitizeXmlName(groupName);
            settingsKey = SanitizeXmlName(settingsKey);

            return $"{groupName}.{settingsKey}";
        }

        private string SanitizeXmlName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Default";

            // Удаляем все недопустимые для XML имена символы
            var validChars = name.Where(c =>
                char.IsLetterOrDigit(c) || c == '.' || c == '_').ToArray();

            string sanitized = new string(validChars);

            // Если после очистки имя пустое - возвращаем значение по умолчанию
            return string.IsNullOrEmpty(sanitized) ? "Default" : sanitized;
        }
        private XmlNode GetSettingsNode(XmlDocument doc, string sectionName)
        {
            // Безопасное преобразование имени секции в XPath
            string xpath = ConvertToValidXPath(sectionName);

            // Поиск узла с проверкой ошибок
            try
            {
                // Сначала ищем как прямого потомка configuration
                XmlNode node = doc.SelectSingleNode($"/configuration/{xpath}")!;

                // Если не нашли, ищем в любом месте документа
                return node ?? doc.SelectSingleNode($"//{xpath}")!;
            }
            catch (XPathException)
            {
                // Fallback: линейный поиск по узлам
                return FindNodeRecursive(doc.DocumentElement!, sectionName.Split('.'));
            }
        }

        private string ConvertToValidXPath(string sectionName)
        {
            // Экранируем специальные символы XPath
            string[] parts = sectionName.Split('.');
            return string.Join("/", parts.Select(p => p.Replace("'", "''")));
        }

        private XmlNode FindNodeRecursive(XmlNode parent, string[] pathParts, int index = 0)
        {
            if (parent == null || index >= pathParts.Length)
                return null!;

            foreach (XmlNode child in parent.ChildNodes)
            {
                if (child.Name == pathParts[index])
                {
                    if (index == pathParts.Length - 1)
                        return child;

                    return FindNodeRecursive(child, pathParts, index + 1);
                }
            }
            return null!;
        }

        private SettingsPropertyValueCollection CreateDefaultValues(
            SettingsPropertyCollection properties,
            SettingsPropertyValueCollection values)
        {
            foreach (SettingsProperty property in properties)
            {
                values.Add(new SettingsPropertyValue(property)
                {
                    PropertyValue = property.DefaultValue
                });
            }
            return values;
        }

        private bool IsEncrypted(SettingsProperty property)
        {
            return property.Attributes[typeof(EncryptedSettingAttribute)] != null;
        }
        private object DecryptValue(string value)
        {
            byte[] protectedBytes = Convert.FromBase64String(value);
            byte[] bytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedSettingAttribute : Attribute { }

    public class SettingsException : Exception
    {
        public SettingsException(string message, Exception inner)
            : base(message, inner) { }
    }
}

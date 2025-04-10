using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentAvaloniaApp.Properties
{
    [SettingsProvider(typeof(CustomFileSettingsProvider))]
    public sealed class AppSettings : ApplicationSettingsBase
    {
        private static AppSettings _default = new AppSettings();
        public static AppSettings Default => _default;

        [UserScopedSetting]
        [DefaultSettingValue("light")]
        public string Theme
        {
            get => (string)this[nameof(Theme)];
            set => this[nameof(Theme)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("800")]
        public double WindowWidth
        {
            get
            {
                object value = this[nameof(WindowWidth)];
                if (value is string strValue) // Если значение пришло как строка
                {
                    return double.Parse(strValue, CultureInfo.InvariantCulture);
                }
                return (double)value; // Если уже double
            }
            set => this[nameof(WindowWidth)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("200")]
        public double WindowHeight
        {
            get
            {
                object value = this[nameof(WindowHeight)];
                if (value is string strValue) // Если значение пришло как строка
                {
                    return double.Parse(strValue, CultureInfo.InvariantCulture);
                }
                return (double)value; // Если уже double
            }
            set => this[nameof(WindowHeight)] = value;
        }
    }
}

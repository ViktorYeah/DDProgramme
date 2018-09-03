using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DoubleMastersDegreeProgramme.Models
{
    public class CustomStringLocalizer : IStringLocalizer
    {
        Dictionary<string, Dictionary<string, string>> resources;

        public CustomStringLocalizer()
        {
            // dictionary for en
            Dictionary<string, string> enDict = new Dictionary<string, string> { };

            // dictionary for ua
            Dictionary<string, string> ukDict = new Dictionary<string, string> { };

            // resource dictionary
            resources = new Dictionary<string, Dictionary<string, string>>
            {
                {"en", enDict },
                {"uk", ukDict }
            };
        }
        
        // Choose resourse by key
        public LocalizedString this[string name]
        {
            get
            {
                var currentCulture = CultureInfo.CurrentUICulture;
                string val = "";
                if (resources.ContainsKey(currentCulture.Name))
                {
                    if (resources[currentCulture.Name].ContainsKey(name))
                    {
                        val = resources[currentCulture.Name][name];
                    }
                }
                return new LocalizedString(name, val);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }
    }
}

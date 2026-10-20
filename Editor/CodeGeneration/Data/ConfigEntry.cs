using System;
using System.Collections.Generic;
using System.Text;

namespace RoRSkinBuilder.Data
{
    [Serializable]
    public class ConfigEntry
    {
        public string name;
        public string description;
        public bool enabledByDefault;
    }
}

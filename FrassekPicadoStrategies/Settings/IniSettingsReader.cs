using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Settings
{
    internal class IniSettingsReader
    {
        private Dictionary<string, Dictionary<string, string>> _entryGroups = new Dictionary<string, Dictionary<string, string>>();

        public string IniFilePath { get; private set; }

        public IniSettingsReader(string iniFilePath)
        {
            IniFilePath = iniFilePath;
            InternalRead();
        }

        private void InternalRead()
        {
            var lines = File.ReadAllLines(IniFilePath);
            string currentGroup = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("[")) // Group name found
                {
                    currentGroup = line.Replace("[", "").Replace("]", "");
                    _entryGroups.Add(currentGroup, new Dictionary<string, string>());
                    continue;
                }
                else
                {
                    string[] keyValue = ClearWhitespace(line).Split('=');
                    _entryGroups[currentGroup].Add(keyValue[0], keyValue[1]);
                }
            }
        }

        public string FindValue(string groupName, string keyName)
        {
            return _entryGroups[groupName][keyName];
        }

        public Dictionary<string, string> GetGroup(string groupName)
        {
            return _entryGroups[groupName];
        }

        private string ClearWhitespace(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in s)
                if (c != ' ')
                    sb.Append(c);

            return sb.ToString();

        }


    }
}

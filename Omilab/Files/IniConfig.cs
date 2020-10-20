using System;
using System.Collections.Generic;
using System.IO;
using Omilab.Security;

namespace Omilab.Files
{   
    public class Group
    {
        public string GroupName { get; set; }
        public Dictionary<string, string> Members { get; set; }

        public Group()
        {
            Members = new Dictionary<string, string>();
        }

        public void SetValue(string key, string value)
        {
            if (Members.ContainsKey(key))
            {
                Members[key] = value;
            }
            else
            {
                Members.Add(key, value);
            }
        }

        public string GetValue(string key)
        {
            if (Members.ContainsKey(key))
            {
                return Members[key];
            }
            else
            {
                return "";
            }
        }

    }//end class

    public static class IniConfig
    {
        public static Dictionary<string, Group> groups = new Dictionary<string, Group>();

        public static void SetValue(string groupName, string key, string value)
        {
            if (groups.ContainsKey(groupName))
            {   
                groups[groupName].SetValue(key, value);
            }
            else
            {
                var tempGroup = new Group();
                tempGroup.GroupName = groupName;
                tempGroup.SetValue(key, value);
                groups.Add(groupName, tempGroup);                
            }
        }

        public static string GetValue(string groupName,string key)
        {
            if (groups.ContainsKey(groupName))
            {
                return groups[groupName].GetValue(key);
            }
            else
            {
                return "";
            }
        }
    
        public static void Write(string path)
        {
            string contents = "";

            foreach(KeyValuePair<string,Group> kvp in groups)
            {
                contents += "[" + kvp.Value.GroupName + "]" + Environment.NewLine;

                foreach(KeyValuePair<string,string> kvp2 in kvp.Value.Members)
                {
                    contents += kvp2.Key + " = " + kvp2.Value + Environment.NewLine;
                }
                contents += Environment.NewLine;
            }

            File.WriteAllText(path, contents);            
        }

        public static void Write(string path, string secret)
        {
            string contents = "";

            foreach (KeyValuePair<string, Group> kvp in groups)
            {
                contents += "[" + kvp.Value.GroupName + "]" + Environment.NewLine;

                foreach (KeyValuePair<string, string> kvp2 in kvp.Value.Members)
                {
                    contents += kvp2.Key + " = " + kvp2.Value + Environment.NewLine;
                }
                contents += Environment.NewLine;
            }

            string encryptedText = Crypto.Encrypt(contents, secret);
            File.WriteAllText(path, encryptedText);
        }              

        public static void Read(string path, string secret)
        {
            string encryptedText = File.ReadAllText(path);
            string decData = Crypto.Decrypt(encryptedText, secret);

            File.WriteAllText("temp", decData);
            Read("temp");

            File.Delete("temp");
        }

        public static void Read(string path)
        {
            if(File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                ReadLines(lines);
            }
        }

        private static void ReadLines(string[] lines)
        {
            if(lines.Length > 0)
            {
                string currentGroup = "";
                groups.Clear();
                  
                foreach (string linea in lines)
                {
                    if(linea != "")
                    {
                        int startIndex = linea.IndexOf('[');
                        int endIndex = linea.LastIndexOf(']');

                        if (startIndex >= 0 && endIndex <= linea.Length)
                        {
                            currentGroup = linea.Substring(startIndex + 1, endIndex - 1);                               
                            groups.Add(currentGroup, new Group());
                        }
                        else
                        {
                            string[] kvp = linea.Split('=');

                            if(kvp.Length >= 2)
                            {
                                string key = kvp[0].Trim();
                                string value = "";

                                if(kvp.Length == 2)
                                {
                                    value = kvp[1].Trim();
                                }
                                else if(kvp.Length > 2)
                                {
                                    for(int i = 1;i < kvp.Length; i++)
                                    {
                                        value += kvp[i].Trim();
                                    }
                                }
                                groups[currentGroup].SetValue(key, value);
                            }

                        }
                    }
                        
                }
            }                
            
        }


    } //end class
} // end namespace
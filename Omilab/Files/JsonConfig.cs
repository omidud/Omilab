using System.Collections.Generic;
using System.IO;
using Omilab.Security;
using Newtonsoft.Json;

namespace Omilab.Files
{
    public static class JsonConfig
    {
        private static Dictionary<string, string> dataDic = new Dictionary<string, string>();

        public static void Reset()
        {
            dataDic.Clear();
        }

        public static void SetValue(string key, string value)
        {
            if (dataDic.ContainsKey(key))
            {
                dataDic[key] = value;
            }
            else
            {
                dataDic.Add(key, value);
            }
        }

        public static string GetValue(string key)
        {
            if (dataDic.ContainsKey(key))
            {
                return dataDic[key];
            }
            else
            {
                return "";
            }
        }

        public static void Read(string path)
        {
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                dataDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }
        }

        public static void Read(string path, string secret)
        {
            if (File.Exists(path))
            {
                string encryptedText = File.ReadAllText(path);
                string jsonString = Crypto.Decrypt(encryptedText, secret); 
                dataDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }
        }

        public static void Write(string path)
        {
            string jsonString = JsonConvert.SerializeObject(dataDic);
            File.WriteAllText(path, jsonString);
        }

        public static void Write(string path, string secret)
        {
            string jsonString = JsonConvert.SerializeObject(dataDic);
            string encryptedText = Crypto.Encrypt(jsonString, secret);
            File.WriteAllText(path, encryptedText);
        }

       

        
    } //end class
} //end namespace



using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Raden_Booster.Utils.Config
{
    internal class RegistryConfig
    {
        private string AppName;
        private RegistryHive RegistryRoot;
        private Dictionary<string, object> Configurations;
        const char SPLIT_CHAR = '\\';
        public RegistryConfig(string ApplicationName, RegistryHive Hive)
        {
            AppName = ApplicationName;
            RegistryRoot = Hive;
            Configurations = new Dictionary<string, object>();
        }

        private RegistryKey MyRegistry(bool Write = false)
        {
            RegistryKey RegKey = RegistryKey.OpenBaseKey(RegistryRoot, RegistryView.Default).OpenSubKey("SOFTWARE", true);
            if (IsContainKey(RegKey, AppName)){
                return  RegKey.OpenSubKey(AppName, Write);
            }
            else
            {
                RegKey.CreateSubKey(AppName);
                return RegKey.OpenSubKey(AppName, Write);
            }            
        }

        private bool IsContainKey(RegistryKey RegKey, string SubKey)
        {
            return RegKey.GetSubKeyNames().Contains(SubKey);
        }

        public void Load()
        {
            Queue<string> Key = new Queue<string>();
            foreach (string RegName in MyRegistry().GetValueNames())
            {
                try
                {
                    Object value = MyRegistry().GetValue(RegName);
                    Configurations.Add(RegName, value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error:" + ex.StackTrace.ToString());
                }
            }
            foreach (string KeyName in MyRegistry().GetSubKeyNames())
            {
                try
                {
                    Key.Enqueue(KeyName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error:" + ex.StackTrace.ToString());
                }
            }
            while (Key.Count > 0)
            {
                string key = Key.Dequeue();
                foreach (string RegName in MyRegistry().OpenSubKey(key).GetValueNames())
                {
                    try
                    {
                        Object value = MyRegistry().OpenSubKey(key).GetValue(RegName);
                        Configurations.Add(key + SPLIT_CHAR + RegName, value);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error:" + ex.StackTrace.ToString());
                    }
                }
                foreach (string KeyName in MyRegistry().OpenSubKey(key).GetSubKeyNames())
                {
                    try
                    {
                        Key.Enqueue(key + SPLIT_CHAR + KeyName);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error:" + ex.StackTrace.ToString());
                    }
                }
            }
        }

        private string GetKeyName(string KeyPath)
        {
            string[] Key = KeyPath.Split(SPLIT_CHAR);
            if (Key.Count() > 1)
                return Key.Last();
            else
                return KeyPath;
        }

        private string GetParentKey(string KeyPath)
        {
            string[] Key = KeyPath.Split(SPLIT_CHAR);
            if (Key.Count() > 1)
                return String.Join(SPLIT_CHAR.ToString(), Key.Take(Key.Count() - 1));
            else
                return null;
        }

        public object Get(string Key, object DefaultValue = null)
        {
            if (Configurations.ContainsKey(Key))
                return Configurations[Key];
            else
                return DefaultValue;
        }

        public T Get<T>(string Key, T DefaultValue = default(T))
        {
            if (Configurations.ContainsKey(Key))
            {
                if (typeof(T) == typeof(bool))
                {
                    if (Configurations[Key].GetType() == typeof(int))
                        return (T)(Object)((int)Configurations[Key] == 1);
                    else
                        return DefaultValue;
                }
                return (T)Configurations[Key];
            }
            else
                return DefaultValue;
        }

        public void Set(string Key, Object value)
        {
            if (value.GetType() == typeof(bool))
            {
                if (Configurations.ContainsKey(Key))
                    Configurations[Key] = (bool)value ? 1 : 0;
                else
                    Configurations.Add(Key, (bool)value ? 1 : 0);
            }
            else
            {
                if (Configurations.ContainsKey(Key))
                    Configurations[Key] = value;
                else
                    Configurations.Add(Key, value);
            }
        }

        public void Remove(string Key)
        {
            if (Configurations.ContainsKey(Key)) Configurations.Remove(Key);
        }

        public Dictionary<string, object> GetConfig()
        {
            return Configurations;
        }

        private string GetRootKey(string Key)
        {
            if (Key == null) return null;
            return Key.Split(SPLIT_CHAR).FirstOrDefault();
        }
        private string[] TraceFromRoot(string Key)
        {
            if (GetParentKey(Key) == null) return Array.Empty<string>();
            List<string> PossibleKey = new List<string>();
            string[] Keys = Key.Split(SPLIT_CHAR);
            for (int i = 0; i < Keys.Count(); i++)
            {
                PossibleKey.Add(String.Join(SPLIT_CHAR.ToString(), Keys.Take(i + 1)));
            }
            return PossibleKey.ToArray();
        }

        public void Save()
        {
            foreach (var row in Configurations)
            {
                var PathSplit = TraceFromRoot(row.Key);
                for (int i = 0; i < PathSplit.Length - 1; i++)
                {
                    string Path = PathSplit[i];
                    if (!IsContainKey(MyRegistry(), Path))
                        MyRegistry(true).CreateSubKey(GetKeyName(Path));
                }
                if (GetParentKey(row.Key) != null)
                {
                    if (row.Value.GetType() == typeof(string))
                        MyRegistry().OpenSubKey(GetParentKey(row.Key), true).SetValue(GetKeyName(row.Key), row.Value.ToString(), RegistryValueKind.String);
                    else if (typeof(IEnumerable).IsAssignableFrom(row.Value.GetType()))
                        MyRegistry().OpenSubKey(GetParentKey(row.Key), true).SetValue(GetKeyName(row.Key), (IEnumerable<string>)row.Value, RegistryValueKind.MultiString);
                    else if (row.Value.GetType() == typeof(int))
                        MyRegistry().OpenSubKey(GetParentKey(row.Key), true).SetValue(GetKeyName(row.Key), row.Value, RegistryValueKind.DWord);
                    else if (row.Value.GetType() == typeof(float) || row.Value.GetType() == typeof(double))
                        MyRegistry().OpenSubKey(GetParentKey(row.Key), true).SetValue(GetKeyName(row.Key), row.Value.ToString(), RegistryValueKind.String);
                }
                else
                {
                    if (row.Value.GetType() == typeof(string))
                        MyRegistry(true).SetValue(GetKeyName(row.Key), row.Value.ToString(), RegistryValueKind.String);
                    else if (typeof(IEnumerable).IsAssignableFrom(row.Value.GetType()))
                        MyRegistry(true).SetValue(GetKeyName(row.Key), (IEnumerable<string>)row.Value, RegistryValueKind.MultiString);
                    else if (row.Value.GetType() == typeof(int))
                        MyRegistry(true).SetValue(GetKeyName(row.Key), row.Value, RegistryValueKind.DWord);
                    else if (row.Value.GetType() == typeof(float) || row.Value.GetType() == typeof(double))
                        MyRegistry(true).SetValue(GetKeyName(row.Key), row.Value.ToString(), RegistryValueKind.String);
                }
            }
        }
    }
}

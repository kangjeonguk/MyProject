using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;


public class XMLLoader : MonoBehaviour 
{
    public const string PATH_DIR = "XmlDoc/";

    public static bool LoadXmlToList<T>(string path, ref List<T> list) where T : class, new()
    {
        XmlElement root = null;
        if (!XMLLoader.LoadRootElement(PATH_DIR + path, out root))
        {
            Debug.LogError("Failed to load" + path + "Table!");
            return false;
        }

        if (list == null)
            list = new List<T>();
        else
            list.Clear();

        for (int dataIdx = 0; dataIdx < root.ChildNodes.Count; ++dataIdx)
        {
            XmlElement node = root.ChildNodes[dataIdx] as XmlElement;
            FieldInfo[] fieldArr = typeof(T).GetFields();
            T data = new T();
            for (int i = 0; i < fieldArr.Length; ++i)
            {
                FieldInfo fi = fieldArr[i];
                string value = node.GetAttribute(fi.Name);
                if (!string.IsNullOrEmpty(value))
                    SetValue<T>(fi, data, value);
            }

            list.Add(data);
        }

        return true;
    }

    public static bool LoadXmlKeyValueIntToDict<T>(string path, string keyValue, ref Dictionary<int, T> dict) where T : class, new()
    {
        XmlElement root = null;
        if (!XMLLoader.LoadRootElement(PATH_DIR + path, out root))
        {
            Debug.LogError("Failed to load" + path + "Table!");
            return false;
        }

        if (dict == null)
            dict = new Dictionary<int, T>();

        for (int dataIdx = 0; dataIdx < root.ChildNodes.Count; ++dataIdx)
        {
            XmlElement node = root.ChildNodes[dataIdx] as XmlElement;
            FieldInfo[] fieldArr = typeof(T).GetFields();
            T data = new T();

            string strValue = node.GetAttribute(keyValue);
            if (string.IsNullOrEmpty(strValue))
            {
                Debug.LogError("Failed to Dict Load key Name is :" + strValue);
                return false;
            }
            else
            {
                for (int i = 0; i < fieldArr.Length; ++i)
                {
                    FieldInfo fi = fieldArr[i];
                    string value = node.GetAttribute(fi.Name);
                    if (!string.IsNullOrEmpty(value))
                        SetValue<T>(fi, data, value);
                }

                int key = Convert.ToInt32(strValue);
                dict.Add(key, data);
            }
        }
        
        return true;
    }

    public static bool LoadXmlKeyValueStringToDict<T>(string path, string keyValue, ref Dictionary<string, T> dict) where T : class, new()
    {
        XmlElement root = null;
        if (!XMLLoader.LoadRootElement(PATH_DIR + path, out root))
        {
            Debug.LogError("Failed to load" + path + "Table!");
            return false;
        }

        if (dict == null)
            dict = new Dictionary<string, T>();
        else
            dict.Clear();

        for (int dataIdx = 0; dataIdx < root.ChildNodes.Count; ++dataIdx)
        {
            XmlElement node = root.ChildNodes[dataIdx] as XmlElement;
            FieldInfo[] fieldArr = typeof(T).GetFields();
            T data = new T();

            string strValue = node.GetAttribute(keyValue);
            if (string.IsNullOrEmpty(strValue))
            {
                Debug.LogError("Failed to Dict Load key Name is :" + strValue);
                return false;
            }
            else
            {
                for (int i = 0; i < fieldArr.Length; ++i)
                {
                    FieldInfo fi = fieldArr[i];
                    string value = node.GetAttribute(fi.Name);
                    if (!string.IsNullOrEmpty(value))
                        SetValue<T>(fi, data, value);
                }
                
                dict.Add(strValue, data);
            }
        }

        return true;
    }

    private static void SetValue<T>(FieldInfo fi, T t, string value) where T : class, new()
    {
        if (fi != null)
        {
            if (fi.FieldType == typeof(string))
                fi.SetValue(t, value);
            else if (fi.FieldType == typeof(Int32))
                fi.SetValue(t, Convert.ToInt32(value));
            else if (fi.FieldType == typeof(bool))
                fi.SetValue(t, Convert.ToBoolean(value));
            else if (fi.FieldType == typeof(Single))
                fi.SetValue(t, Convert.ToSingle(value));
            else if (fi.FieldType == typeof(Int16))
                fi.SetValue(t, Convert.ToInt16(value));
            else if (fi.FieldType == typeof(Int64))
                fi.SetValue(t, Convert.ToInt64(value));
            else if (fi.FieldType == typeof(UInt16))
                fi.SetValue(t, Convert.ToUInt16(value));
            else if (fi.FieldType == typeof(UInt32))
                fi.SetValue(t, Convert.ToUInt32(value));
            else if (fi.FieldType == typeof(UInt64))
                fi.SetValue(t, Convert.ToUInt64(value));
            else if (fi.FieldType == typeof(byte))
                fi.SetValue(t, Convert.ToByte(value));
            else if (fi.FieldType == typeof(char))
                fi.SetValue(t, Convert.ToChar(value));
            else if (fi.FieldType == typeof(DateTime))
                fi.SetValue(t, Convert.ToDateTime(value));
            else if (fi.FieldType == typeof(decimal))
                fi.SetValue(t, Convert.ToDecimal(value));
            else if (fi.FieldType == typeof(double))
                fi.SetValue(t, Convert.ToDouble(value));
            else if (fi.FieldType == typeof(sbyte))
                fi.SetValue(t, Convert.ToSByte(value));             
            else
                Debug.LogError("DTXMLLoader: Field of type [" + fi.FieldType.ToString() + "] not supported");
        }
    }

    private static bool LoadRootElement(string path, out XmlElement root)
    {
        root = null;
        TextAsset tempObj = Resources.Load(path) as TextAsset;

        if (string.IsNullOrEmpty(tempObj.text))
            return false;

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(tempObj.text);
        root = doc.DocumentElement;        
        return true;
    }
}

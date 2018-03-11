using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using UnityEditor;
using System;


public class RootInstance
{

    private static RootInstance sinstance = null;
    private static GameObject mainRoots = null;

    public static RootInstance instance
    {
        get
        {
            if (sinstance == null)
            {
                sinstance = new RootInstance();
                if (mainRoots == null)
                {
                    mainRoots = new GameObject("_SingletonObjects");
                }
            }
            return sinstance;
        }
    }

    public T GetInstance<T>() where T : MonoBehaviour
    {
        string instanceName = (typeof(T)).ToString();
        instanceName = instanceName.Substring(2);
        string path = string.Format("02. Prefabs/01. Instance/{0}", instanceName);
        GameObject instancePrefab = Resources.Load(path) as GameObject;

        T obj = null;

        if (instancePrefab == null)
            obj = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
        else
        {
            GameObject cloneObj = GameObject.Instantiate(instancePrefab);
            obj = cloneObj.GetComponent<T>();
        }

        obj.transform.parent = mainRoots.transform;
        return obj;
    }
}

public class BaseInstance<T> : MonoBehaviour where T : BaseInstance<T>
{

    private static T sinstance = null;

    public static T instance
    {
        get
        {
            if (sinstance == null)
            {
                sinstance = RootInstance.instance.GetInstance<T>();
                sinstance.InitInstance();
            }
            return sinstance;
        }
    }

    public void Call()
    {

    }

    public void DestorySelf()
    {
        GameObject.DestroyImmediate(instance.gameObject);
        sinstance = null;
    }

    void Update()
    {
        if (instance != null)
        {
            UpdateInstance();
        }
    }

    public virtual void InitInstance()
    {
        
    }

    public virtual void UpdateInstance()
    {

    }

}

public class BaseDataInstance<T> : MonoBehaviour where T : BaseDataInstance<T>
{

    private static T sinstance = null;

    public static T instance
    {
        get
        {
            if (sinstance == null)
            {
                sinstance = RootInstance.instance.GetInstance<T>();
                sinstance.LoadData();
            }
            return sinstance;
        }
    }

    public void DestorySelf()
    {
        GameObject.DestroyImmediate(instance.gameObject);
        sinstance = null;
    }

    public void Call()
    {

    }

    public virtual void LoadData()
    {

    }

    #region XML Attribute Node
    public XmlNodeList GetXmlValue_NodeList(XmlNode node, string attr, string debugString)
    {
        XmlNode child = node.SelectSingleNode(attr);
        if (child == null)
        {
            Debug.LogWarning(string.Format("XML Attribute NULL: {0}, {1}", debugString, attr));
            return null;
        }

        return child.ChildNodes;
    }

    public string GetXmlValue_String(XmlNode node, string attr, string debugString)
    {
        XmlNode child = node.Attributes[attr];
        if (child == null)
        {
            Debug.LogWarning(string.Format("XML Attribute NULL: {0}, {1}", debugString, attr));
            return null;
        }

        return child.Value;
    }

    public string GetXmlValue_StringWithNull(XmlNode node, string attr, string debugString)
    {
        XmlNode child = node.Attributes[attr];
        if (child == null)
        {
            return null;
        }

        return child.Value;
    }

    public long GetXmlValue_Long(XmlNode node, string attr, string debugString)
    {
        string str = GetXmlValue_String(node, attr, debugString);
        if (str == null)
            return 0;

        return long.Parse(str);
    }

    public int GetXmlValue_Int(XmlNode node, string attr, string debugString)
    {
        string str = GetXmlValue_String(node, attr, debugString);
        if (str == null)
            return 0;

        return int.Parse(str);
    }

    public bool GetXmlValue_Int(XmlNode node, string attr, out int value, string debugString)
    {
        string str = GetXmlValue_StringWithNull(node, attr, debugString);
        if (str == null)
        {
            value = 0;
            return false;
        }

        value = int.Parse(str);
        return true;
    }

    public float GetXmlValue_Float(XmlNode node, string attr, string debugString)
    {
        string str = GetXmlValue_String(node, attr, debugString);
        if (str == null)
            return 0.0f;

        return float.Parse(str);
    }

    public Texture GetXmlValue_Texture(XmlNode node, string attr, string debugString)
    {
        string str = GetXmlValue_String(node, attr, debugString);
        if (str == null)
        {
            Debug.LogWarning("Not Find Texture: " + str);
            return null;
        }

        return Resources.Load(str) as Texture;
    }

    public GameObject GetXmlValue_GameObject(XmlNode node, string attr, string debugString)
    {
        string str = GetXmlValue_String(node, attr, debugString);
        if (str == null)
        {
            Debug.LogWarning("Not Find Texture: " + str);
            return null;
        }

        return Resources.Load(str) as GameObject;
    }
    #endregion

    #region XML Attribute Element

    protected bool LoadRootElement(string path, out XmlElement root)
    {
        root = null;
        string totalPath = string.Format("Tables/{0}", path);
        TextAsset textAsset = Resources.Load(totalPath, typeof(TextAsset)) as TextAsset;
        if (textAsset == null)
        {
            Debug.Log(string.Format("Can not found {0}.xml", path));
            return false;
        }

        if (string.IsNullOrEmpty(textAsset.text))
            return false;

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;

        return true;
    }

    protected bool LoadRootElementFromResources(string path, out XmlElement root)
    {
        root = null;

        TextAsset textAsset = Resources.Load(path) as TextAsset;
        if (textAsset == null)
            return false;

        if (string.IsNullOrEmpty(textAsset.text))
            return false;

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;

        return true;
    }

    protected int GetInt(XmlElement element, string attrName)
    {
        if (element.HasAttribute(attrName))
        {
            string str = element.GetAttribute(attrName);
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            return XmlConvert.ToInt32(str);
        }

        return int.MaxValue;
    }

    protected object GetEnum(XmlElement element, System.Type enumType, string attrName)
    {
        if (element.HasAttribute(attrName))
        {
            string str = element.GetAttribute(attrName);
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            try
            {
                object en = System.Enum.Parse(enumType, str);
                if (System.Enum.IsDefined(enumType, en))
                {
                    return en;
                }

                return 0;
            }
            catch (System.ArgumentException)
            {
                return 0;
            }
        }

        return 0;
    }

    protected string GetString(XmlElement element, string attrName)
    {
        if (element.HasAttribute(attrName))
        {
            string str = element.GetAttribute(attrName);
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            return str;
        }

        return null;
    }

    protected bool GetBool(XmlElement element, string attrName)
    {
        if (element.HasAttribute(attrName))
        {
            string str = element.GetAttribute(attrName);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            str = str.ToLower();
            if (str.Contains("true"))
            {
                return true;
            }

            if (str.Contains("false"))
            {
                return false;
            }
        }

        return false;
    }

    protected float GetFloat(XmlElement element, string attrName)
    {
        if (element.HasAttribute(attrName))
        {
            string str = element.GetAttribute(attrName);
            if (string.IsNullOrEmpty(str))
            {
                return 0.0f;
            }

            return (float)XmlConvert.ToDouble(str);
        }

        return float.MaxValue;
    }

    #endregion
}


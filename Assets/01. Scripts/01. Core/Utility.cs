using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Utility
{
    public static GameObject CreateObject(Object obj)
    {
        GameObject ret = GameObject.Instantiate(obj) as GameObject;
        return ret;
    }

    public static GameObject CreateObjectWithAddChild(Object obj, GameObject parent)
    {
        GameObject ret = GameObject.Instantiate(obj) as GameObject;
        Utility.AddChild(parent, ret);
        return ret;
    }

    public static void AddChild(GameObject parent, GameObject child)
    {
        AddChild(parent.transform, child.transform);
    }

    public static void AddChild(GameObject parent, Transform child)
    {
        AddChild(parent.transform, child);
    }

    public static void AddChild(Transform parent, GameObject child)
    {
        AddChild(parent, child.transform);
    }

    public static void AddChild(Transform parent, Transform child)
    {
        Vector3 localPos = child.localPosition;
        Quaternion localRot = child.localRotation;
        Vector3 localScl = child.localScale;

        child.parent = parent;

        child.localPosition = localPos;
        child.localRotation = localRot;
        child.localScale = localScl;
    }

    public static void AddChildIdentity(GameObject parent, GameObject child)
    {
        AddChildIdentity(parent.transform, child.transform);
    }

    public static void AddChildIdentity(GameObject parent, Transform child)
    {
        AddChildIdentity(parent.transform, child);
    }

    public static void AddChildIdentity(Transform parent, GameObject child)
    {
        AddChildIdentity(parent, child.transform);
    }

    public static void AddChildIdentity(Transform parent, Transform child)
    {
        child.parent = parent;

        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;
    }

    public static Vector3 ConvertToZZeroVector3(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0.0f);
    }

    public static Vector2 ConvertToZZeroVector2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static void Destory(Object obj)
    {
        Object.Destroy(obj);
        obj = null;
    }

    public static void Destory(Object obj, float t)
    {
        Object.Destroy(obj, t);
        obj = null;
    }

    public static void DestroyImmediate(Object obj)
    {
        Object.DestroyImmediate(obj);
        obj = null;
    }

    public static void DestroyImmediate(Object obj, bool allow)
    {
        Object.DestroyImmediate(obj, allow);
    }

    public static T EnumParse<T>(string s)
    {
        return (T)System.Enum.Parse(typeof(T), s);
    }

    public static void ClearChild(Transform obj)
    {
        for (int i = 0; i < obj.childCount; ++i)
        {
            Transform child = obj.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void ClearChild(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; ++i)
        {
            Transform child = obj.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static float GetCalcRateValue(float value)
    {
        float standardRate = 1280.0f / 720.0f;
        float currentRate = (float)Screen.width / (float)Screen.height;

        float oriValue = value;
        float calcValue = (currentRate / standardRate) * oriValue;
        float returnValue = oriValue + (oriValue - calcValue);
        return returnValue;
    }

    public static object GetEnum(System.Type enumType, string attrName)
    {
        if (string.IsNullOrEmpty(attrName))
        {
            return 0;
        }

        try
        {
            object en = System.Enum.Parse(enumType, attrName, false);
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

    public static Color ChangeRGBColor(Color color)
    {
        color.r = (float)(color.r) / 255.0f;
        color.g = (float)(color.g) / 255.0f;
        color.b = (float)(color.b) / 255.0f;
        return color;
    }

    public static Color ChangeRGBColorWithAlpha(Color color)
    {
        color.r = (float)(color.r) / 255.0f;
        color.g = (float)(color.g) / 255.0f;
        color.b = (float)(color.b) / 255.0f;
        color.a = (float)(color.a) / 255.0f;
        return color;
    }

    public static bool CheckInnerRange(int min, int max, int compareValue)
    {
        if (compareValue >= min && compareValue < max)
            return true;

        return false;
    }

    public static bool IsCheckHourOverrun(System.DateTime compareDate, int compareHour)
    {
        System.DateTime currentCheckDate = System.DateTime.Now;
        System.DateTime resetCurrentDate = new System.DateTime(compareDate.Year, compareDate.Month, compareDate.Day, compareHour, 0, 0);
        System.DateTime resetNextDate = resetCurrentDate.AddDays(1);

        if (compareDate.Hour < compareHour)
        {
            if (currentCheckDate < resetCurrentDate)
                return false;
        }
        else if (compareDate.Hour >= compareHour)
        {
            if (currentCheckDate < resetNextDate)
                return false;
        }

        return true;
    }

    // 초단위를 12:00:00형식의 스트링으로 변환
    public static string ChangeTimeStringFromSeconds(int seconds)
    {
        System.TimeSpan span = new System.TimeSpan(0, 0, seconds);
        return span.ToString();
    }

}

public class SortedIndex : IComparer<int>
{
    eOrderType orderType = eOrderType.Ascending;

    public enum eOrderType
    {
        Descending,
        Ascending,
    }

    public SortedIndex(eOrderType type)
    {
        orderType = type;
    }

    public int Compare(int x, int y)
    {
        if (orderType == eOrderType.Descending)
        {
            if (x > y)
                return -1;
            else if (x < y)
                return 1;
            else
                return 0;
        }
        else
        {
            if (x < y)
                return -1;
            else if (x > y)
                return 1;
            else
                return 0;
        }
    }
}

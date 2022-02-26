using UnityEngine;
using System.Collections;
 
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
 
    public static T GetInstance()
    {
        return instance;
    }
 
    public void SetInstance(T t)
    {
        if(instance ==null )
        {
            instance = t;
        }
    }
 
    public virtual void Init()
    {
        return;
    }
 
    public virtual void Release()
    {
        return;
    }
}
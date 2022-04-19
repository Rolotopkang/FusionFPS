using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MyPhotonNetwrokPool : IPunPrefabPool
{
    /// <summary>Contains a GameObject per prefabId, to speed up instantiation.</summary>
    public readonly Dictionary<string, GameObject> ResourceCache = new Dictionary<string, GameObject>();

    /// <summary>Returns an inactive instance of a networked GameObject, to be used by PUN.</summary>
    /// <param name="prefabId">String identifier for the networked object.</param>
    /// <param name="position">Location of the new object.</param>
    /// <param name="rotation">Rotation of the new object.</param>
    /// <returns></returns>
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        GameObject res = null;
        bool cached = this.ResourceCache.TryGetValue(prefabId, out res);
        if (!cached)
        {
            res = Resources.Load<GameObject>(prefabId);
            if (res == null)
            {
                Debug.LogError("DefaultPool failed to load \"" + prefabId + "\". Make sure it's in a \"Resources\" folder. Or use a custom IPunPrefabPool.");
            }
            else
            {
                this.ResourceCache.Add(prefabId, res);
            }
        }

        bool wasActive = res.activeSelf;
        if (wasActive) res.SetActive(false);

        GameObject instance =GameObject.Instantiate(res, position, rotation) as GameObject;

        if (wasActive) res.SetActive(true);
        return instance;
    }

    /// <summary>Simply destroys a GameObject.</summary>
    /// <param name="gameObject">The GameObject to get rid of.</param>
    public void Destroy(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    public MyPhotonNetwrokPool(string prefabId)
    {
        PreloadPrefab(prefabId);
    }
    
    /// <summary>
    /// 预加载网络预制体到预制体池
    /// </summary>
    /// <param name="prefabId">预制体位置</param>
    public void PreloadPrefab(string prefabId)
    {
        GameObject res = null;
        res = Resources.Load<GameObject>(prefabId);
        if (res == null)
        {
            Debug.LogError("DefaultPool failed to load \"" + prefabId + "\". Make sure it's in a \"Resources\" folder. Or use a custom IPunPrefabPool.");
        }
        else
        {
            this.ResourceCache.Add(prefabId, res);
            Debug.Log("网络池预载入对象："+prefabId+"完成");
        }
    }
}

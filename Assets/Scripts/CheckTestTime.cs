using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckTestTime : MonoBehaviour
{

    [SerializeField]
    private string TimeURL;

    [SerializeField]
    private long TestLimitTime;

    private long currentTime;
    
    void Start()
    {
    	// 自动获取网络时间
        StartCoroutine(GetWebTime());
    }

    #region 获取网络时间

    /// <summary>
    /// 获取当前网络时间
    /// </summary>
    /// <returns></returns>
    IEnumerator GetWebTime()
    {
        // 获取时间地址
        string url = TimeURL; // 百度 //http://www.beijing-time.org/"; // 北京时间
        Debug.Log("开始获取服务器时间... 获取地址是: " + url);

        DateTime _webNowTime = DateTime.Now;
        // 获取时间
        UnityWebRequest WebRequest = new UnityWebRequest(url);

        // 等待请求完成
        yield return WebRequest.SendWebRequest();
        
        //网页加载完成  并且下载过程中没有错误   string.IsNullOrEmpty 判断字符串是否是null 或者是" ",如果是返回true
        //WebRequest.error  下载过程中如果出现下载错误  会返回错误信息 如果下载没有完成那么将会阻塞到下载完成
        if (WebRequest.isDone && string.IsNullOrEmpty(WebRequest.error))
        {
        	// 将返回值存为字典
            Dictionary<string, string> resHeaders = WebRequest.GetResponseHeaders();
            string key = "DATE";
            string value = null;
            // 获取key为"DATE" 的 Value值
            if (resHeaders != null && resHeaders.ContainsKey(key))
            {
                resHeaders.TryGetValue(key, out value);
            }

            if (value == null)
            {
                Debug.LogError("没有获取到key为DATE对应的Value值...");
                yield break;
            }

            // 取到了value，则进行转换为本地时间
            _webNowTime = FormattingGMT(value);
            Debug.Log(value + " ，转换后的网络时间：" + _webNowTime);

            // 转换成时间戳
            TimeSpan cha = (_webNowTime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)));
            currentTime = (long) cha.TotalSeconds;
            Debug.Log("网络时间转时间戳：" + currentTime);
        }
        else
        {
            UI_Error.GetInstance().OpenErrorUI("测试时间已过，请删除包体","退出",
                UI_Error.GetInstance().Exit);
            yield break;
        }
        CheckTime();
    }


    private void CheckTime()
    {
        if (currentTime>TestLimitTime)
        {
            UI_Error.GetInstance().OpenUI_TestTimeOutWarning();
        }
    }
    
    
    /// <summary>
    /// GMT(格林威治时间)时间转成本地时间
    /// </summary>
    /// <param name="gmt">字符串形式的GMT时间</param>
    /// <returns></returns>
    private DateTime FormattingGMT(string gmt)
    {
        DateTime dt = DateTime.MinValue;
        try
        {
            string pattern = "";
            if (gmt.IndexOf("+0") != -1)
            {
                gmt = gmt.Replace("GMT", "");
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
            }

            if (gmt.ToUpper().IndexOf("GMT") != -1)
            {
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
            }

            if (pattern != "")
            {
                dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);
                dt = dt.ToLocalTime();
            }
            else
            {
                dt = Convert.ToDateTime(gmt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.LogError("时间转换错误...");
        }
        return dt;
    }
    #endregion
    
    
}

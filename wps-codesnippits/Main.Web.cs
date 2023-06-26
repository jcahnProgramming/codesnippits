using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public static partial class Main
{
    public static class Web
    {
        public static async Task<string> Post(string url, string request)
        {
            return await Post(url, request, null);
        }
        public static async Task<string> Post(string url, string request, string token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, ""))
            {
                using (UploadHandler handler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(request))
                {
                    contentType = "application/json"
                })
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        webRequest.SetRequestHeader("Authorization", "Bearer " + token);
                    }
                    webRequest.uploadHandler = handler;
                    webRequest.timeout = 15;
                    //Trace.Log(string.Format("Web Post: {0} (Token: {1}) | Data: {2}", url, token, request));
                    await webRequest.SendWebRequest();
                    while (!webRequest.isDone)
                    {
                        await Task.Delay(50);
                    }
                    if (webRequest.isNetworkError || webRequest.isHttpError)
                    {
                        //Trace.Log(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                        return null;
                    }
                    else
                    {
                        //Trace.Log(string.Format("Web Response: {0}", webRequest.downloadHandler.text));
                        return webRequest.downloadHandler.text;
                    }
                }
            }
        }
        public static async Task<string> Get(string url, string token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    webRequest.SetRequestHeader("Authorization", "Bearer " + token);
                }
                webRequest.timeout = 15;
                //Trace.Log(string.Format("Web Get: {0} (Token: {1})", url, token));
                await webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    await Task.Delay(50);
                }
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Trace.LogError(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                    return null;
                }
                else
                {
                    //Trace.Log(string.Format("Web Response: {0}", webRequest.downloadHandler.text));
                    return webRequest.downloadHandler.text;
                }
            }
        }
        public static async Task<T> Get<T>(string url, string token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    webRequest.SetRequestHeader("Authorization", "Bearer " + token);
                }
                webRequest.timeout = 15;
                //Trace.Log(string.Format("Web Get: {0} (Token: {1})", url, token));
                await webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    await Task.Delay(50);
                }
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Trace.LogError(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                    return default(T);
                }
                else
                {
                    //Trace.Log(string.Format("Web Response: {0}", webRequest.downloadHandler.text));
                    return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                }
            }
        }
        public static async Task<T> Post<T>(string url, string token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, ""))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    webRequest.SetRequestHeader("Authorization", "Bearer " + token);
                }
                webRequest.timeout = 15;
                Trace.Log(string.Format("Web Request: {0} (Token: {1})", url, token));
                await webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    await Task.Delay(50);
                }
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Trace.LogError(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                    return default(T);
                }
                else
                {
                    //Trace.Log(string.Format("Web Response: {0}", webRequest.downloadHandler.text));
                    return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                }
            }
        }
        public static async Task<T> Post<T>(string url, string request, string token)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, ""))
            {
                using (UploadHandler handler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(request))
                {
                    contentType = "application/json"
                })
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        webRequest.SetRequestHeader("Authorization", "Bearer " + token);
                    }
                    webRequest.uploadHandler = handler;
                    webRequest.timeout = 15;
                    Trace.Log(string.Format("Web Request: {0} (Token: {1}) | Data: {2}", url, token, request));
                    await webRequest.SendWebRequest();
                    while (!webRequest.isDone)
                    {
                        await Task.Delay(50);
                    }
                    if (webRequest.isNetworkError || webRequest.isHttpError)
                    {
                        Trace.Log(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                        return default(T);
                    }
                    else
                    {
                        Trace.Log(string.Format("Web Response: {0}", webRequest.downloadHandler.text));
                        return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                    }
                }
            }
        }
        public static async Task<T> Post<T>(string url, WWWForm form)
        {
            //List<IMultipartFormSection> data = new List<IMultipartFormSection> {
            //    new MultipartFormDataSection(form)
            //};
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                webRequest.timeout = 15;
                //Trace.Log(string.Format("Web Request: {0} | Data: {1}", url, form));
                await webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    await Task.Delay(50);
                }
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    //Trace.Log(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                    return default(T);
                }
                else
                {
                    //Trace.Log(string.Format("Web Response: {0}", webRequest.downloadHandler.text));
                    return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                }
            }
        }
    }
}

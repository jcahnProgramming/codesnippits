using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

public static partial class Main
{

    public static class Graphics
    {

        private static ConcurrentDictionary<string, Sprite> ThumbnailCache = new ConcurrentDictionary<string, Sprite>();

        public static IEnumerator FromCache(VideoItemView view, bool download, Action<SpriteCallback> callback)
        {
            if (ThumbnailCache.TryGetValue(view.Video.ID, out var sprite))
            {
                //Trace.Log("Thumbnails exists in cache: " + view.Video.ID);
                callback(new SpriteCallback { View = view, Sprite = sprite });
                yield break;
            }

            var path = string.Format("{0}/textures/{1}", Application.persistentDataPath, view.Video.ID);
            if (File.Exists(path))
            {
                yield return MipmapTexture(File.ReadAllBytes(path), (texture) => {
                    var sprite = SpriteFromTexture2D(texture);
                    ToCache(view.Video.ID, sprite, false);
                    callback(new SpriteCallback { View = view, Sprite = sprite });
                });
            }
            else if (download)
            {
                if (view.Video.Thumbnail.StartsWith("asset-"))
                {
                    yield return AppLoader.Instance.BlobService.GetImageBlob((data) =>
                    {
                        if (data.IsError)
                        {
                            Trace.LogError(data.ErrorMessage);
                        }
                        else
                        {
                            SpriteFromTexture2D(data.Data, view, (sprite) =>
                            {
                                ToCache(view.Video.ID, sprite.Sprite, true);
                                callback(sprite);
                            });
                        }
                    }, view.Video.Thumbnail);
                }
                else
                {
                    using (UnityWebRequest webRequest = UnityWebRequest.Get(view.Video.Thumbnail))
                    {
                        webRequest.timeout = 15;
                        yield return webRequest.SendWebRequest();
                        if (webRequest.isNetworkError || webRequest.isHttpError) //TODO: both of these bools are marked as obsolete 
                        {
                            Trace.LogError(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                        }
                        else
                        {
                            yield return MipmapTexture(webRequest.downloadHandler.data, (texture) => {
                                var sprite = SpriteFromTexture2D(texture);
                                ToCache(view.Video.ID, sprite, true);
                                callback(new SpriteCallback { View = view, Sprite = sprite });
                            });
                        }
                    }
                }
            }
        }

        public static IEnumerator Cache(VideoDataItem video)
        {
            var path = $"{Application.persistentDataPath}/textures/{video.ID}";

            if (File.Exists(path))
            {
                yield return MipmapTexture(File.ReadAllBytes(path), (texture) => {
                    var sprite = SpriteFromTexture2D(texture);
                    ToCache(video.ID, sprite, false);
                });
            }
            else
            {
                if (video.Thumbnail.StartsWith("asset-"))
                {
                    yield return AppLoader.Instance.BlobService.GetImageBlob((data) =>
                    {
                        if (data.IsError)
                        {
                            Trace.LogError("Missing asset: " + video.Name + " (" + data.ErrorMessage + ")");
                        }
                        else
                        {
                            SpriteFromTexture2D(data.Data, null, (sprite) =>
                            {
                                ToCache(video.ID, sprite.Sprite, false);
                            });
                        }
                    }, video.Thumbnail);
                }
                else
                {
                    using (UnityWebRequest webRequest = UnityWebRequest.Get(video.Thumbnail))
                    {
                        webRequest.timeout = 15;
                        yield return webRequest.SendWebRequest();
                        if (webRequest.isNetworkError || webRequest.isHttpError)
                        {
                            Trace.LogError(string.Format("Web Request Failed: {0} ({1})", webRequest.error, webRequest.downloadHandler.text));
                        }
                        else
                        {
                            yield return MipmapTexture(webRequest.downloadHandler.data, (texture) => {
                                var sprite = SpriteFromTexture2D(texture);
                                ToCache(video.ID, sprite, true);
                            });
                        }
                    }
                }
            }
        }

        public static void ToCache(string key, Sprite sprite, bool useDisk)
        {
            ThumbnailCache.TryAdd(key, sprite);

            if (useDisk)
            {
                var path = string.Format("{0}/textures/{1}", Application.persistentDataPath, key);
                //Trace.Log(path);
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                //Trace.Log("Write cache: " + key);
                File.WriteAllBytes(path, sprite.texture.EncodeToJPG());
            }
        }

        public static void SpriteFromTexture2D(Texture2D texture, VideoItemView view, Action<SpriteCallback> callback)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            callback.Invoke(new SpriteCallback { View = view, Sprite = sprite });
        }

        public static Sprite SpriteFromTexture2D(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        }

        public static IEnumerator MipmapTexture(byte[] data, Action<Texture2D> callback)
        {
            Texture2D newTexture = new Texture2D(2, 2)
            {
                anisoLevel = 4,
                mipMapBias = -0.7f,
                filterMode = FilterMode.Trilinear,
                requestedMipmapLevel = 0
            };
            yield return newTexture.LoadImage(data);
            callback(newTexture);
        }

        public static Texture2D MipmapTexture(byte[] data)
        {
            Texture2D newTexture = new Texture2D(2, 2)
            {
                anisoLevel = 4,
                mipMapBias = -0.7f,
                filterMode = FilterMode.Trilinear,
                requestedMipmapLevel = 0
            };
            newTexture.LoadImage(data);
            return newTexture;
        }

        public class SpriteCallback
        {
            public VideoItemView View { get; set; }
            public Sprite Sprite { get; set; }
        }
    }

}
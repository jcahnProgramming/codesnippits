using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using VRStandardAssets.Utils;

namespace Assets.Scripts {
    public class VideoItemView : MonoBehaviour {

        [SerializeField] public TMP_Text Title;
        [SerializeField] public TMP_Text Description;
        [SerializeField] public TMP_Text Views;
        [SerializeField] public TMP_Text Likes;
        [SerializeField] public TMP_Text Dislikes;
        [SerializeField] public Image Thumbnail;
        [SerializeField] public Image Rating;
        [SerializeField] public Button Favorite;
        [SerializeField] public Button Download;
        [SerializeField] public VideoItem Video;

        void Start() {
            Title.text = Video.Title;
            Description.text = Video.Description;
            Views.text = string.Format("{0} Views", Main.Strings.GetCompactedNumber(Video.Data.TotalViews));
            Likes.text = Video.Data.TotalLikes.ToString();
            Dislikes.text = Video.Data.TotalDislikes.ToString();
            var favoriteImage = Favorite.gameObject.FindObject("Image").GetComponent<Image>();
            favoriteImage.sprite = Video.Data.IsFavorite ? Resources.Load<Sprite>("ic_gallery_favorite_on") : Resources.Load<Sprite>("ic_gallery_favorite_off");
            var downloadImage = Download.gameObject.FindObject("Image").GetComponent<Image>();
            if (Video.Data.IsDownloaded) {
                downloadImage.sprite = Resources.Load<Sprite>("ic_gallery_download_on");
                //Download.gameObject.GetComponent<VRInteractiveItem>().IsGazeable = false;
            } else if (Events.downloadBusy) {
                downloadImage.sprite = Resources.Load<Sprite>("ic_gallery_download_blocked");
                Download.gameObject.GetComponent<VRInteractiveItem>().IsGazeable = false;
            } else {
                downloadImage.sprite = Resources.Load<Sprite>("ic_gallery_download_off");
                Download.gameObject.GetComponent<VRInteractiveItem>().IsGazeable = true;
            }
            StartCoroutine(LoadThumbnail());
            Events.CheckVideoDownload(this, Download.gameObject);
            //var downloadImage = Download.gameObject.FindObject("Image");
            //if (Video.Data.Stats.Rating > 0) {
            //    if (Video.Data.Stats.Rating > 0 && Video.Data.Stats.Rating < 1) {
            //        Rating.sprite = Resources.Load<Sprite>("rating1");
            //    } else if (Video.Data.Stats.Rating >= 1 && Video.Data.Stats.Rating < 2) {
            //        Rating.sprite = Resources.Load<Sprite>("rating2");
            //    } else if (Video.Data.Stats.Rating >= 2 && Video.Data.Stats.Rating < 3) {
            //        Rating.sprite = Resources.Load<Sprite>("rating3");
            //    } else if (Video.Data.Stats.Rating >= 3 && Video.Data.Stats.Rating < 4) {
            //        Rating.sprite = Resources.Load<Sprite>("rating4");
            //    } else if (Video.Data.Stats.Rating >= 4 && Video.Data.Stats.Rating <= 5) {
            //        Rating.sprite = Resources.Load<Sprite>("rating5");
            //    }
            //}
            //await Main.Graphics.DownloadThumbnailAsync(Video, (sprite) => OnThumbnailLoaded(sprite));
        }

        private void Awake() {
            
        }

        public void Load() {
            
        }

        public IEnumerator LoadThumbnail() {
            yield return new WaitForSeconds(0.1f);
            yield return Main.Graphics.FromCache(this, true, (sprite) => {
                this.Thumbnail.sprite = sprite.Sprite;
            });
        }
    }
}

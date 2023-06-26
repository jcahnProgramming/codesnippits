using System.Collections.Generic;
using System.Threading.Tasks;

using Assets.Scripts.Utils;

using Newtonsoft.Json;

using static Main;

namespace Assets.Scripts {
    public class YoutubeManager {

        public static YoutubeManager Instance {
            get {
                if (instance == null) {
                    instance = new YoutubeManager();
                }
                return instance;
            }
        }
        private static YoutubeManager instance;

        public YoutubeManager() {
        }

        public async Task<List<VideoDataItem>> LoadVideosAsync(string ids) {
            Trace.Log("Loading Youtube metadata: " + ids);
            string data = JsonConvert.SerializeObject(new { ids });
            return await Web.Post<List<VideoDataItem>>(string.Format("{0}/proxy/youtube", AppLoader.ProxyUrl), data, null);
        }
    }
}

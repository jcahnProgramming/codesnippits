using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Assets.Scripts;
using Assets.Scripts.Utils;
using Azure.Storage.Blobs;
using Azure.StorageServices;

using ControllerSelection;

using CSVFile;

using Microsoft.AppCenter.Unity.Analytics;
using Microsoft.AppCenter.Unity.Crashes;

using Newtonsoft.Json;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using VRStandardAssets.Utils;

using WPM;

using static Main;

public class AppLoader : MonoBehaviour
{

    public static AppLoader Instance;
    private static bool Testing = false;
    //private static bool UseCache = false;
    private static bool ForceCache = false;
    //private static int CacheDuration = 120;
    private static string APIUrl = "#";
    private static string AZBaseUrl = "https://management.azure.com";
    private static string AZUrl = "#";
    public static string ProxyUrl = "https://wildx.azurewebsites.net";
    private static string ResourceGroupName = "wpsVRApp";
    private static string MediaAccountName = "wpsvrmedia";
    private static string StorageAccountName = "wpsvr";
    private bool isOfflineMode = false;
    private bool safarisInitialized = false;

    public string AccessToken;
    public VideoData Data, TestData;
    public GameDriveData GDData;
    public List<Marker> Markers;
    public string CurrentRegion;
    public string FilterRegion;
    public List<VideoDataItem> CurrentVideos;
    public Dictionary<string, bool?> FilterSpeciesCategory;
    public Dictionary<string, bool?> FilterPrimarySpecies, CurrentPrimarySpecies;
    public Dictionary<string, bool?> FilterIUCN;
    public Dictionary<string, bool?> FilterPopulationTrend;
    public bool isJourneysActive = false;
    public bool isEncountersActive = false;
    public bool isSafarisActive = false;
    public bool loopingJourneys = false;

    public BlobService BlobService;

    private bool usingCDNCache = false;
    private bool usingGameDriveCache = false;
    private string tutorialText;
    private bool startupTutorialShown = false;

    private bool ShowGraphicDeath = false, ShowGraphicSex = false;

    public enum SortOrder
    {
        MostViewed, TopRated, Newest, Favorites, Downloads
    }
    public SortOrder Order = SortOrder.MostViewed;

    public BlobServiceClient blobServiceClient { get; private set; }

    public async void Start()
    {
        Instance = this;
        Trace.Log("VideoLoader Startup");
        //UseCache = Trace.IsDebug;
        HideMainMenu();
        UserInterface.instance.m_Splash.SetActive(true);
        if (Trace.UseOculusInput)
        {
            InitOculus();
        }

        Trace.Log("Start loading videos");

        var token = "#"; //this had been added to preserve the code w/o revealing sensitive company data.

        var StorageClient = new StorageServiceClient("https://wpsvr.blob.core.windows.net/", token);
        BlobService = StorageClient.GetBlobService();

        StartCoroutine(WaitForSplash());
        Trace.Log("Start loading videos");
        await LoadAsync();
    }

    private void InitOculus()
    {
        try
        {
            //GameObject.Find("OVRCameraRig").GetComponent<VREyeRaycaster>().enabled = false;
            //GameObject.Find("OVRCameraRig").GetComponent<MouseLook>().enabled = false;
            //GameObject.Find("OVRCameraRig").GetComponent<OVRRawRaycaster>().enabled = true;
            //GameObject.Find("OVRCameraRig").GetComponent<OVRPointerVisualizer>().enabled = true;
            Oculus.Platform.Core.AsyncInitialize();
            Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
        }
        catch (Exception e)
        {
            Debug.LogError("Platform failed to initialize due to exception: " + e.ToString());
            Debug.LogException(e);
            Application.Quit();
        }
    }

    private void EntitlementCallback(Oculus.Platform.Message msg)
    {
        if (msg.IsError)
        {
            Application.Quit();
        }
        else
        {
            Trace.Log("You are entitled to use this app.");
        }
    }

    private IEnumerator WaitForSplash()
    {
        yield return new WaitForSeconds(5f);
        UserInterface.instance.m_Splash.SetActive(false);
        //if (!startupTutorialShown) {
        //    ShowTutorial();
        //}
        //ShowMainMenu();

        //if (TestData.Videos.Count > 1)
        //{
            loopingJourneys = true;
            Journeys();
            GameManager.instance.SetActiveUIPosition(4);
        //}
        ////ShowModeEncounters();
        //else
        //{
        //    ShowMainMenu();
        //}
    }

    public void CheckConnectivity()
    {
        isOfflineMode = Application.internetReachability == NetworkReachability.NotReachable;
        Trace.Log("Internet Connection: " + Application.internetReachability.ToString() + " (Offline: " + isOfflineMode + ")");
    }

    async Task LoadAsync()
    {
        Trace.Log("Connectivity: " + Application.internetReachability.ToString());
        CheckConnectivity();
        if (isOfflineMode)
        {
            Trace.Log("No internet connection, using cache");
            Trace.UseMetadataCache = true;
            Trace.UseYoutubeCache = true;
        }
        if (UserInterface.instance.m_AllMarker)
        {
            UserInterface.instance.m_AllMarker.SetActive(false);
        }
        if (UserInterface.instance.m_SpecialMarker)
        {
            UserInterface.instance.m_SpecialMarker.SetActive(false);
        }
        UserInterface.instance.m_UI.SetActive(true);
        UserInterface.instance.m_VideoCanvas.SetActive(false);
        UserInterface.instance.m_Loading.SetActive(true);
        await GetVideoDataAsync();
        //if (m_AllMarker) {
        //    m_AllMarker.SetActive(true);
        //}
        //if (m_SpecialMarker) {
        //    m_SpecialMarker.SetActive(true);
        //}
    }

    private void UpdateFavorites()
    {
        string favoritesValue = Preferences.GetString("Favorites", "");
        if (!string.IsNullOrEmpty(favoritesValue))
        {
            VideoFavorites favorites = null;
            try
            {
                favorites = JsonConvert.DeserializeObject<VideoFavorites>(favoritesValue);
            }
            catch (Exception e)
            {
                Trace.LogError(e.ToString());
            }
            if (favorites != null && favorites.Favorites != null)
            {
                foreach (var favorite in favorites.Favorites)
                {
                    var video = Data.Videos.FirstOrDefault(_ => _.ID == favorite.ID);
                    if (video != null)
                    {
                        video.IsFavorite = favorite.IsFavorite;
                    }
                }
            }
        }
    }

    public void SafarisButterflyExperiencePOC()
    {
        GameManager.instance.SetActiveUIPosition(6);
        isSafarisActive = true;
        Trace.Log("Butterfly Pavilion Sample Safaris");
        HideMainMenu();
        UserInterface.instance.m_World.SetActive(false);
        UserInterface.instance.m_Loading.SetActive(false);

        //user interface 3 buttons for 3 different videos
        //curated for users to play
        UserInterface.instance.m_SafarisInterface.SetActive(true);

        if (!safarisInitialized)
        {
            //video1 - Safaris

            VideoDataItem v1 = new VideoDataItem();
            v1.ID = HardcodedValues.Instance.v1_videoID;
            v1.Name = HardcodedValues.Instance.v1_videoName;
            v1.Overview = HardcodedValues.Instance.v1_videoOverview;
            v1.Thumbnail = string.Format("asset-{0}/poster.png", v1.ID);
            v1.Metadata = new List<VideoAttribute>();
            var v1_path = string.Format("{0}/downloads/{1}", Application.persistentDataPath, v1.ID);
            v1.IsDownloaded = File.Exists(v1_path);

            //video2 - Safaris

            VideoDataItem v2 = new VideoDataItem();
            v2.ID = HardcodedValues.Instance.v2_videoID;
            v2.Name = HardcodedValues.Instance.v2_videoName;
            v2.Overview = HardcodedValues.Instance.v2_videoOverview;
            v2.Thumbnail = string.Format("asset-{0}/poster.png", v2.ID);
            v2.Metadata = new List<VideoAttribute>();
            var v2_path = string.Format("{0}/downloads/{1}", Application.persistentDataPath, v2.ID);
            v2.IsDownloaded = File.Exists(v2_path);

            //video3 - Safaris

            VideoDataItem v3 = new VideoDataItem();
            v3.ID = HardcodedValues.Instance.v3_videoID;
            v3.Name = HardcodedValues.Instance.v3_videoName;
            v3.Overview = HardcodedValues.Instance.v3_videoOverview;
            v3.Thumbnail = string.Format("asset-{0}/poster.png", v3.ID);
            v3.Metadata = new List<VideoAttribute>();
            var v3_path = string.Format("{0}/downloads/{1}", Application.persistentDataPath, v3.ID);
            v3.IsDownloaded = File.Exists(v3_path);

            List<VideoDataItem> safariVideos = new List<VideoDataItem>();

            safariVideos.Add(v1);
            safariVideos.Add(v2);
            safariVideos.Add(v3);

            Trace.Log(v1.ToString());

            foreach (VideoDataItem video in safariVideos)
            {
                if (isOfflineMode && !video.IsDownloaded)
                    continue;
                var item = new VideoItem
                {
                    ID = video.ID,
                    Title = video.Name,
                    Description = video.Overview,
                    Url = "",
                    Thumbnail = video.Thumbnail,
                    Data = video
                };
                var videoItem = Instantiate(UserInterface.instance.m_Template);
                videoItem.tag = "Clone";
                videoItem.GetComponent<VideoItemView>().Video = item;
                videoItem.transform.SetParent(UserInterface.instance.m_Grid_Safari.transform, false);
                videoItem.SetActive(true);
                Trace.Log("Video Item Completed: " + video.Name);
            }
            safarisInitialized = true;
        }
    }

    public void UpdateDownloads()
    {
        foreach (var video in Data.Videos)
        {
            var path = string.Format("{0}/downloads/{1}", Application.persistentDataPath, video.ID);
            video.IsDownloaded = File.Exists(path);
        }
    }

    private void LoadMarkerData()
    {
        if (Data == null)
        {
            UserInterface.instance.m_Loading.FindObject("Text").GetComponent<TMP_Text>().text = "Failed to load data";
            return;
        }
        UpdateFavorites();
        UpdateDownloads();
        Markers = GetMarkers();

        if (Markers != null)
        {
            //Trace.Log("Markers: " + Markers.Count);
            foreach (var marker in Markers)
            {
                ////Trace.Log("Add marker: " + marker.Title);
                var markerObject = Instantiate(UserInterface.instance.m_BaseMarker);
                markerObject.name = marker.Title;
                UserInterface.instance.m_Globe.calc.fromUnit = UNIT_TYPE.DecimalDegrees;
                UserInterface.instance.m_Globe.calc.fromLatDec = marker.Latitude;
                UserInterface.instance.m_Globe.calc.fromLonDec = marker.Longitude;
                if (UserInterface.instance.m_Globe.calc.Convert())
                {
                    var position = UserInterface.instance.m_Globe.calc.toSphereLocation;
                    //Trace.Log("Position: " + position);
                    //markerObject.GetComponent<Marker>().Title = marker.Title;
                    var mapItem = markerObject.GetComponent<InteractiveMapItem>();
                    mapItem.m_Region.text = marker.Title;
                    markerObject.SetActive(true);
                    UserInterface.instance.m_Globe.AddMarker(markerObject, position, 0.1f);
                }
            }
        }
        //StartCoroutine(LoadThumbnailCache());
        UserInterface.instance.m_Loading.SetActive(false);
        UserInterface.instance.m_UI.SetActive(false);
        UserInterface.instance.m_Splash.SetActive(false);
    }

    public async Task<string> GetVideoUrl(VideoDataItem video)
    {
        Trace.Log(JsonConvert.SerializeObject(video));
        AzureStreamingLocators locators = await Web.Post<AzureStreamingLocators>(string.Format(AZUrl, string.Format("assets/{0}/listStreamingLocators", video.ID)), AccessToken);
        Trace.Log(JsonConvert.SerializeObject(locators));
        if (locators != null && locators.StreamingLocators != null && locators.StreamingLocators.Count > 0)
        {
            var locator = locators.StreamingLocators.FirstOrDefault();
            AzureStreamingPaths paths = await Web.Post<AzureStreamingPaths>(string.Format(AZUrl, string.Format("streamingLocators/{0}/listPaths", locator.StreamingLocatorID)), AccessToken);
            AzureStream stream = null;
            if (paths != null && paths.Streams != null && paths.Streams.Count > 0)
            {
                Trace.Log(JsonConvert.SerializeObject(paths));
                stream = paths.Streams.FirstOrDefault(_ => _.Protocol.Equals("Dash"));
            }
            if (stream != null && stream.Paths != null && stream.Paths.Count > 0)
            {
                //return string.Format("https://wpsvrmedia-usea.streaming.media.azure.net{0}?api-version=2018-07-01", stream.Paths.FirstOrDefault());
                //return string.Format("https://premium-wpsvrmedia-usea.streaming.media.azure.net{0}?api-version=2018-07-01", stream.Paths.FirstOrDefault());
                string path = stream.Paths.Where(_ => _.Contains("csf")).FirstOrDefault();
                if (path != null)
                {
                    path = path.Replace("csf", "csf,filter=HQ");
                    return string.Format("https://premium-wpsvrmedia-usea.streaming.media.azure.net{0}?api-version=2018-07-01", path);
                    //return string.Format("https://wpsvrmedia-usea.streaming.media.azure.net{0}?api-version=2018-07-01", path);
                }
            }
        }
        return "";
    }

    public async Task<string> GetVideoDownloadUrl(VideoDataItem video)
    {
        Trace.Log(JsonConvert.SerializeObject(video));
        string request = JsonConvert.SerializeObject(new { permissions = "Read", expiryTime = XmlConvert.ToString(DateTime.UtcNow.AddDays(1), XmlDateTimeSerializationMode.RoundtripKind) });
        AzureContainerURLs urls = await Web.Post<AzureContainerURLs>(string.Format(AZUrl, string.Format("assets/{0}/listContainerSas", video.ID)), request, AccessToken);
        Trace.Log(JsonConvert.SerializeObject(urls));
        if (urls != null && urls.URLs != null && urls.URLs.Count > 0)
        {
            return urls.URLs.FirstOrDefault();
        }
        return "";
    }

    private async Task GetVideoDataAsync()
    {

        // Check for local tutorial text
        var resource = Resources.Load<TextAsset>("tutorial");
        if (resource != null)
        {
            tutorialText = resource.text;
            //if (!string.IsNullOrEmpty(tutorialText)) {
            //    startupTutorialShown = true;
            //    ShowTutorial();
            //}
        }

        Data = new VideoData();
        Data.Videos = new List<VideoDataItem>();
        TestData = new VideoData();
        TestData.Videos = new List<VideoDataItem>();

        if (string.IsNullOrEmpty(AccessToken))
        {
            Trace.Log("No access token - logging in");
            AccessToken = await Web.Get(string.Format("{0}/proxy/token", ProxyUrl), null);
            Trace.Log("Got access token: " + AccessToken);
            Analytics.TrackEvent(Events.EventAuthAzure, new Events.EventTraceProperties().Add(Events.GetProperties()).Properties);
        }

        if (string.IsNullOrEmpty(AccessToken))
        {
            Trace.Log("Failed to retrieve auth token from Azure");
            //m_Loading.FindObject("Text").GetComponent<TMP_Text>().text = "Failed to load data";
            Crashes.TrackError(new Exception("Failed to retrieve auth token from Azure"), new Events.EventTraceProperties().Add(Events.GetProperties()).Add(Events.GetOculusProperties()).Properties);
            Trace.UseMetadataCache = true;
            Trace.UseYoutubeCache = true;
            ForceCache = true;
            isOfflineMode = true;
        }
        
        if (Trace.UseMetadataCache)
        {
            // Check cache
            Trace.Log($"CDN Cache Date: {Preferences.CDNCacheDate}");
            var cacheAge = DateTime.Now - Preferences.CDNCacheDate;
            Trace.Log("CDN Cache Age: " + cacheAge);
            if (cacheAge < Trace.CacheDuration || ForceCache)
            {
                if (!string.IsNullOrEmpty(Preferences.CDNData))
                {
                    Trace.Log("Using cached CDN data");
                    Trace.Log(Preferences.CDNData);
                    Data = JsonConvert.DeserializeObject<VideoData>(Preferences.CDNData);
                    usingCDNCache = true;
                }
            }
        }

        if (Trace.UseGameDriveCache)
        {
            // Check cache
            Trace.Log($"GameDrive Cache Date: { Preferences.GameDriveCacheDate}");
            var cacheAge = DateTime.Now - Preferences.GameDriveCacheDate;
            Trace.Log("GameDrive Cache Age: " + cacheAge);
            if (cacheAge < Trace.CacheDuration || ForceCache)
            {
                if (!string.IsNullOrEmpty(Preferences.GameDriveData))
                {
                    Trace.Log("Using cached game drive data");
                    Trace.Log(Preferences.GameDriveData);
                    GDData = JsonConvert.DeserializeObject<GameDriveData>(Preferences.GameDriveData);
                    usingGameDriveCache = true;
                }
            }
        }
        
        Trace.Log($"Tutorial shown: {Preferences.TutorialShown}");
        if (!usingCDNCache)
        {
            //TODO: Move the connection string out of the code
            blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=wpsvr;AccountKey=6Enz3KLUSzJzMrDHGkFHUYnj+b6vb6BuONBzB5tgwQwHpRDo/Y/QHAopvkN6P4bUqF0shcfi7L/9oLS9PBHU+Q==;EndpointSuffix=core.windows.net");
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("metadata");

            await DownloadParseMetadataCsv(blobContainerClient);

            await DownloadParseGameDataMetadataCsv(blobContainerClient);

            //downloadtestinformation() is a method to pull data from testCSV sheet for journeys/invertebrates
            await DownloadTestInformation(blobContainerClient);
       
            Trace.Log(JsonConvert.SerializeObject(GDData));
            Preferences.GameDriveData = JsonConvert.SerializeObject(GDData);
            Preferences.GameDriveCacheDate = DateTime.Now;
        }

        await LoadVideoData();

        Analytics.TrackEvent(Events.EventVideoListLoadAzure, new Events.EventTraceProperties().Add(Events.GetProperties()).Add(Events.GetOculusProperties()).Properties);
    }

    private async Task DownloadParseMetadataCsv(BlobContainerClient blobContainerClient)
    {
        var blobMetadataClient = blobContainerClient.GetBlobClient("metadata.csv");

        var metadataDlResult = await blobMetadataClient.DownloadContentAsync();

        using (StreamReader stream = new StreamReader(metadataDlResult.Value.Content.ToStream()))
        {
            using (CSVReader reader = new CSVReader(stream, new CSVSettings { AllowNull = true, HeaderRowIncluded = true, SkipLines = 1, FieldDelimiter = ',', TextQualifier = '\"' }))
            {
                Trace.Log("Header: " + JsonConvert.SerializeObject(reader.Headers));
                foreach (var line in reader.Lines())
                {
                    VideoDataItem item = new VideoDataItem();
                    item.ID = line[0];
                    item.Name = line[1];
                    item.Overview = line[2];
                    item.Thumbnail = string.Format("asset-{0}/poster.png", item.ID);
                    item.Metadata = new List<VideoAttribute>();
                    for (int index = 3; index < reader.Headers.Length; index++)
                    {
                        item.Metadata.Add(new VideoAttribute { Name = reader.Headers[index], Value = line[index] });
                    }
                    Data.Videos.Add(item);
                }
            }
        }
    }

    private async Task DownloadTestInformation(BlobContainerClient blobContainerClient)
    {
        var blobMetadataClient = blobContainerClient.GetBlobClient("test_metadata.csv");
        var metadataDlResult = await blobMetadataClient.DownloadContentAsync();

        using (StreamReader stream = new StreamReader(metadataDlResult.Value.Content.ToStream()))
        {
            using (CSVReader reader = new CSVReader(stream, new CSVSettings { AllowNull = true, HeaderRowIncluded = true, SkipLines = 1, FieldDelimiter = ',', TextQualifier = '\"' }))
            {
                Trace.Log("Header: " + JsonConvert.SerializeObject(reader.Headers));
                foreach (var line in reader.Lines())
                {
                    VideoDataItem item = new VideoDataItem();
                    item.ID = line[0];
                    item.Name = line[1];
                    item.Overview = line[2];
                    item.Thumbnail = string.Format("asset-{0}/poster.png", item.ID);
                    item.Metadata = new List<VideoAttribute>();
                    for (int index = 3; index < reader.Headers.Length; index++)
                    {
                        item.Metadata.Add(new VideoAttribute { Name = reader.Headers[index], Value = line[index] });
                    }
                    var path = string.Format("{0}/downloads/{1}", Application.persistentDataPath, item.ID);
                    item.IsDownloaded = File.Exists(path);
                    item.Url = path;

                    TestData.Videos.Add(item);

                }
            }
        }
    }

    private async Task DownloadParseGameDataMetadataCsv(BlobContainerClient blobContainerClient)
    {
        var blobGameDriveMetadataClient = blobContainerClient.GetBlobClient("Game_Drive_Metadata.csv");
        var gameDriveMetadataDlResult = await blobGameDriveMetadataClient.DownloadContentAsync();

        GDData = new GameDriveData
        {
            GameDrives = new List<GameDrive>()
        };

        using (var stream = new StreamReader(gameDriveMetadataDlResult.Value.Content.ToStream()))
        using (var reader = new CSVReader(stream, new CSVSettings { AllowNull = true, HeaderRowIncluded = true, SkipLines = 1, FieldDelimiter = ',', TextQualifier = '\"' }))
        {
            Trace.Log($"Header: {JsonConvert.SerializeObject(reader.Headers)}");
            int index = 1;
            GameDrive gameDrive = null;
            foreach (var line in reader.Lines())
            {
                if (gameDrive == null || !gameDrive.Name.Equals(line[3]))
                {
                    gameDrive = new GameDrive
                    {
                        Name = line[3]
                    };
                    gameDrive.Items = new List<GameDriveDataItem>();
                    GDData.GameDrives.Add(gameDrive);
                }
                GameDriveDataItem item = new GameDriveDataItem();
                item.EventType = GameDriveDataItem.GetEventType(line[0]);
                item.BundleGroup = line[1];
                item.TrackingID = line[2];
                item.MediaID = line[3];
                item.MediaName = line[4];
                if (!string.IsNullOrWhiteSpace(line[5]) && !string.IsNullOrWhiteSpace(line[6]))
                {
                    //Trace.Log("Times: " + line[5] + "; " + line[6]);
                    item.StartTime = GameDriveDataItem.ParseTime(line[5]);
                    item.EndTime = GameDriveDataItem.ParseTime(line[6]);
                }
                item.ResolutionBase = line[7];
                if (!string.IsNullOrWhiteSpace(line[8]) && !string.IsNullOrWhiteSpace(line[9]) && !string.IsNullOrWhiteSpace(line[10]) &&
                    !string.IsNullOrWhiteSpace(line[11]) && !string.IsNullOrWhiteSpace(line[12]) && !string.IsNullOrWhiteSpace(line[13]))
                {
                    try
                    {
                        item.StartPositionX = float.Parse(line[8], CultureInfo.InvariantCulture);
                        item.StartPositionY = float.Parse(line[9], CultureInfo.InvariantCulture);
                        item.StartPositionZ = float.Parse(line[10], CultureInfo.InvariantCulture);
                        item.EndPositionX = float.Parse(line[11], CultureInfo.InvariantCulture);
                        item.EndPositionY = float.Parse(line[12], CultureInfo.InvariantCulture);
                        item.EndPositionZ = float.Parse(line[13], CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        Trace.LogError(string.Format("Invalid position data at line '{0}': {1}", index, e.Message));
                    }
                }
                item.Shape = GameDriveDataItem.GetShape(line[14]);
                if (!string.IsNullOrWhiteSpace(line[15]) && !string.IsNullOrWhiteSpace(line[16]))
                {
                    try
                    {
                        item.BoundsHeight = int.Parse(line[15], CultureInfo.InvariantCulture);
                        item.BoundsWidth = int.Parse(line[16], CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        Trace.LogError(string.Format("Invalid bounds data at line '{0}': {1}", index, e.Message));
                    }
                }
                if (!string.IsNullOrWhiteSpace(line[17]))
                {
                    item.IsPause = bool.Parse(line[17]);
                }
                gameDrive.Items.Add(item);
                index++;
            }
        }
        
    }

    private async Task LoadVideoData()
    {
        try
        {
            var cacheAge = DateTime.Now.Subtract(Preferences.YouTubeCacheDate);
            Trace.Log("Youtube Cache Age: " + cacheAge);
            if (cacheAge > Trace.CacheDuration)
            {
                Trace.UseYoutubeCache = false;
            }
            if (!Trace.UseYoutubeCache)
            {
                Trace.Log("Youtube data cache stale: Updating data.");
                Data = await UpdateYouTubeVideoData(Data);
                Preferences.YouTubeCacheDate = DateTime.Now;
            }
            else
            {
                List<VideoDataItem> cacheData = null;
                try
                {
                    cacheData = JsonConvert.DeserializeObject<List<VideoDataItem>>(Preferences.YouTubeData);
                }
                catch { }
                UpdateVideoDataFromYoutube(Data, cacheData);
            }
            Preferences.CDNData = JsonConvert.SerializeObject(Data);
            Preferences.CDNCacheDate = DateTime.Now;
            await LoadThumbnailCache();
        }
        catch (Exception e)
        {
            Trace.LogError(e.ToString());
            Crashes.TrackError(new Exception(string.Format("Failed to augment YouTube data: {0}", e.Message)), new Events.EventTraceProperties().Add(Events.GetProperties()).Add(Events.GetOculusProperties()).Properties);
        }
        
        UpdateFavorites();
    }

    private void ShowTutorial()
    {
        if (string.IsNullOrEmpty(tutorialText))
        {
            // Get cached tutorial?
            tutorialText = Preferences.TutorialText;
        }
        if (!string.IsNullOrEmpty(tutorialText))
        {
            Preferences.TutorialText = tutorialText;
            Events.Instance.TutorialText = tutorialText;
            if (!Preferences.TutorialShown)
            {
                Events.Instance.ShowTutorial();
            }
        }
    }

    public void SetFavorites()
    {
        VideoFavorites favorites = new VideoFavorites();
        foreach (var video in Data.Videos)
        {
            if (video.IsFavorite)
            {
                favorites.Favorites.Add(new VideoFavorite { ID = video.ID, IsFavorite = true });
            }
        }
        Preferences.SetString("Favorites", JsonConvert.SerializeObject(favorites));
    }

    private static async Task<bool> GetNfoBlobData(BlobService client, VideoDataItem item, Container container, Blob blob)
    {
        bool add = false;
        string path = string.Format("{0}/{1}", container.Name, blob.Name);
        var blobClient = await client.GetBlob((data) =>
        {
            if (data.IsError)
            {
                //Trace.Log("Error: " + JsonConvert.SerializeObject(data));
            }
            if (data != null && data.Data != null)
            {
                ////Trace.Log("XML: " + Encoding.UTF8.GetString(data.Data));
                var xml = Encoding.UTF8.GetString(data.Data);
                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (xml.StartsWith(_byteOrderMarkUtf8))
                {
                    xml = xml.Remove(0, _byteOrderMarkUtf8.Length);
                }
                //Trace.Log(xml);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlNodeList nodes = document.GetElementsByTagName("title");
                if (nodes != null && nodes.Count > 0)
                {
                    item.Name = nodes[0].InnerText;
                }
                nodes = document.GetElementsByTagName("plot");
                if (nodes != null && nodes.Count > 0)
                {
                    item.Overview = nodes[0].InnerText;
                }
                nodes = document.GetElementsByTagName("tag");
                if (nodes != null && nodes.Count > 0)
                {
                    item.Metadata = new List<VideoAttribute>();
                    for (int index = 0; index < nodes.Count; index++)
                    {
                        var node = nodes[index];
                        if (node.InnerText.IndexOf(":") > -1)
                        {
                            string name = node.InnerText.Substring(0, node.InnerText.IndexOf(":"));
                            string value = node.InnerText.Substring(node.InnerText.IndexOf(":") + 1);
                            // Cleanup
                            name = name.Replace("  ", " ").Trim();
                            value = value.Replace("  ", " ").Trim();
                            //
                            item.Metadata.Add(new VideoAttribute { Name = name, Value = value });
                        }
                        /*string[] parts = node.InnerText.Split(new string[] { ":" }, 2, StringSplitOptions.None);
                        if (parts != null && parts.Length == 2) {
                            item.Metadata.Add(new VideoAttribute { Name = parts[0].Trim(), Value = parts[1].Trim() });
                        }*/
                    }
                }
                add = true;
            }
        }, path);
        return add;
    }

    async Task<VideoData> UpdateYouTubeVideoData(VideoData data)
    {
        if (data != null && data.Videos != null && data.Videos.Count > 0)
        {
            List<string> ids = new List<string>();
            foreach (var item in data.Videos)
            {
                if (item.Metadata != null && item.Metadata.FirstOrDefault(_ => _.Name.Equals("YouTube")) != null)
                {
                    var value = item.Metadata.FirstOrDefault(_ => _.Name.Equals("YouTube")).Value.Trim().Replace("https://youtu.be/", "");
                    ids.Add(value);
                }
            }
            if (ids.Count == 0)
                return data;
            //Trace.Log("Fetching YouTube videos");
            var youtubeVideos = await YoutubeManager.Instance.LoadVideosAsync(string.Join(",", ids));
            if (youtubeVideos != null && youtubeVideos.Count > 0)
            {
                UpdateVideoDataFromYoutube(data, youtubeVideos);
                Preferences.YouTubeData = JsonConvert.SerializeObject(youtubeVideos);
                Analytics.TrackEvent(Events.EventVideoListAugmentYouTube, new Events.EventTraceProperties().Add(Events.GetProperties()).Add(Events.GetOculusProperties()).Properties);
            }
        }
        return data;
    }

    private static void UpdateVideoDataFromYoutube(VideoData data, List<VideoDataItem> youtubeVideos)
    {
        //Trace.Log(string.Format("Found {0} YouTube videos", youtubeVideos.Count));
        foreach (var youTubeVideo in youtubeVideos)
        {
            string youTubeID = "";
            foreach (var video in data.Videos)
            {
                if (video.Metadata != null)
                {
                    var youTubeAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("YouTube"));
                    if (youTubeAttribute != null && !string.IsNullOrEmpty(youTubeAttribute.Value.Trim()))
                    {
                        youTubeID = youTubeAttribute.Value.Trim().Substring(youTubeAttribute.Value.Trim().LastIndexOf("/") + 1);
                        ////Trace.Log(string.Format("Compare YouTube IDs: {0} vs {1}", youTubeID, youTubeVideo.ID));
                        if (youTubeVideo.ID.Equals(youTubeID))
                        {
                            ////Trace.Log(string.Format("Using stats from YouTube video '{0}' - {1} views", youTubeVideo.ID, youTubeVideo.TotalViews));
                            video.DateCreated = youTubeVideo.DateCreated;
                            video.TotalLikes = youTubeVideo.TotalLikes;
                            video.TotalViews = youTubeVideo.TotalViews;
                            video.TotalDislikes = youTubeVideo.TotalDislikes;
                            if (youTubeVideo.TotalLikes > 0)
                            {
                                ////Trace.Log(string.Format("Calc rating: {0} likes, {1} dislikes, {2} views", youTubeVideo.TotalLikes, youTubeVideo.TotalDislikes, youTubeVideo.TotalViews));
                                var left = youTubeVideo.TotalLikes / ((float)(youTubeVideo.TotalLikes + youTubeVideo.TotalDislikes));
                                ////Trace.Log("Left: " + left);
                                video.Rating = ((left * 100f) / 20f);
                                ////Trace.Log("Rating: " + video.Rating);
                            }
                            //Trace.Log("YouTube: " + youTubeVideo.Name + " - " + JsonConvert.SerializeObject(youTubeVideo));
                            if (!string.IsNullOrEmpty(youTubeVideo.Thumbnail))
                            {
                                video.Thumbnail = youTubeVideo.Thumbnail;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    List<Marker> GetMarkers()
    {
        var markers = new List<Marker>();
        List<string> RegionCache = new List<string>();
        if (Data != null)
        {
            if (Data.Videos != null)
            {
                if (UserInterface.instance.m_AllMarker)
                {
                    UserInterface.instance.m_AllMarker.SetActive(true);
                }
                if (UserInterface.instance.m_SpecialMarker)
                {
                    UserInterface.instance.m_SpecialMarker.SetActive(true);
                }
                foreach (var result in Data.Videos)
                {
                    ////Trace.Log(JsonConvert.SerializeObject(result.Metadata));
                    if (result.Metadata != null && result.Metadata.Count > 0)
                    {
                        var regionAttribute = result.Metadata.FirstOrDefault(_ => _.Name.Equals("Portal 1"));
                        if (regionAttribute != null)
                        {
                            //Marker marker = markers.FirstOrDefault(_ => _.Title.Equals(region, System.StringComparison.InvariantCultureIgnoreCase));
                            string[] regions = regionAttribute.Value.Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var region in regions)
                            {
                                //var country = WorldMapGlobe.instance.GetCountry(region);
                                //if (country != null) {
                                if (!RegionCache.Contains(region))
                                {
                                    // TEMP!
                                    RegionCache.Add(region);
                                    float latitude = 0, longitude = 0;
                                    if (region.Equals("Africa"))
                                    {
                                        latitude = -9.809050f;
                                        longitude = 29.134136f;
                                    }
                                    //else if (region.Equals("Asia"))
                                    //{
                                    //    latitude = 9.782303f;
                                    //    longitude = 112.278657f;
                                    //}
                                    else if (region.Equals("Europe"))
                                    {
                                        latitude = 48.1327673f;
                                        longitude = 4.1753323f;
                                    }
                                    else if (region.Equals("North America"))
                                    {
                                        latitude = 41.102244f;
                                        longitude = -106.568978f;
                                    }
                                    else if (region.Equals("Central America"))
                                    {
                                        latitude = 9.782305f;
                                        longitude = -84.156874f;
                                    }
                                    else if (region.Equals("Antarctic"))
                                    {
                                        //latitude = -57.992002f;
                                        //longitude = -57.965478f;
                                        latitude = -43.533549f;
                                        longitude = -30.435558f;
                                    }
                                    else if (region.Equals("Oceans and Rivers"))
                                    {
                                        latitude = -0.013573f; // Pacific
                                        longitude = -137.77018f; // Pacific
                                        // Add additional markers
                                        markers.Add(new Marker { Title = region, Latitude = -0.013573f, Longitude = -20.96355f, Flag = "" }); // Atlantic
                                        markers.Add(new Marker { Title = region, Latitude = -0.365133f, Longitude = 74.221990f, Flag = "" }); // Indian
                                    }
                                    Trace.Log("Adding marker: " + region);
                                    if (latitude != 0 && longitude != 0)
                                    {
                                        markers.Add(new Marker { Title = region, Latitude = latitude, Longitude = longitude, Flag = "" });
                                    }
                                }
                            }
                            //} else {
                            //    //Trace.Log(string.Format("Country '{0}' not found!", region));
                            //}
                        }
                    }
                }
            }
        }
        return markers;
    }

    public void LoadVideos()
    {
        if (string.IsNullOrEmpty(CurrentRegion))
            return;
        LoadVideos(CurrentRegion);
    }

    public void LoadVideos(string name, bool clearFilter = false)
    {
        GameManager.instance.E_videoMenuOpen = true;

        if (!UserInterface.instance.m_Grid || !UserInterface.instance.m_Template)
            return;
        if (UserInterface.instance.m_AllMarker)
        {
            UserInterface.instance.m_AllMarker.SetActive(false);
        }
        if (UserInterface.instance.m_SpecialMarker)
        {
            UserInterface.instance.m_SpecialMarker.SetActive(false);
        }
        //Trace.Log("Load Videos: " + name);
        if (clearFilter)
        {
            FilterSpeciesCategory = new Dictionary<string, bool?>();
            CurrentPrimarySpecies = new Dictionary<string, bool?>();
            FilterPrimarySpecies = new Dictionary<string, bool?>();
            FilterIUCN = new Dictionary<string, bool?>();
            FilterPopulationTrend = new Dictionary<string, bool?>();
        }
        CurrentRegion = name;
        if (CurrentRegion.Equals("AllMarker"))
        {
            UserInterface.instance.m_Title.GetComponent<TMP_Text>().text = "All Videos";
        }
        else if (CurrentRegion.Equals("SpecialMarker"))
        {
            UserInterface.instance.m_Title.GetComponent<TMP_Text>().text = "Compilations";
        }
        else if (CurrentRegion.Equals("ButterflyPav"))
        {
            UserInterface.instance.m_Title.GetComponent<TMP_Text>().text = "Safaris";
        }
        else
        {
            UserInterface.instance.m_Title.GetComponent<TMP_Text>().text = isOfflineMode ? string.Format("{0} [Offline Mode]", name) : name;
        }
        if (UserInterface.instance.m_EmptyText)
        {
            UserInterface.instance.m_EmptyText.SetActive(false);
        }
        UserInterface.instance.m_UI.SetActive(true);
        UserInterface.instance.m_VideoCanvas.SetActive(true);
        UserInterface.instance.m_Loading.SetActive(true);
        foreach (Transform child in UserInterface.instance.m_Grid.transform)
        {
            if (child.tag.Contains("Template"))
                continue;
            //Trace.Log("Destroying: " + child.name);
            Destroy(child.gameObject);
        }
        bool noneSelected = FilterPrimarySpecies.All(_ => !_.Value.HasValue || !_.Value.Value);
        Debug.Log("None selected: " + noneSelected);
        ShowGraphicDeath = bool.Parse(Preferences.GetString("GraphicDeath", "false"));
        ShowGraphicSex = bool.Parse(Preferences.GetString("GraphicSex", "false"));
        int viewCount = 0;
        if (!string.IsNullOrEmpty(CurrentRegion) && Data != null && Data.Videos != null)
        {
            //Trace.Log(JsonConvert.SerializeObject(Data.Videos));
            Analytics.TrackEvent(string.Format("{0}_{1}", Events.EventVideoPortal, CurrentRegion), new Events.EventTraceProperties().Add(Events.GetProperties()).Add(Events.GetOculusProperties()).Properties);
            CurrentVideos = new List<VideoDataItem>();
            foreach (var video in Data.Videos)
            {
                bool add = CheckVideoFilterAdd(video, false, noneSelected);
                if (add)
                {
                    CurrentVideos.Add(video);
                }
            }
            if (UserInterface.instance.m_EmptyText)
            {
                UserInterface.instance.m_EmptyText.SetActive(false);
            }
            if (Order == SortOrder.MostViewed)
            {
                CurrentVideos = CurrentVideos.OrderByDescending(_ => _.TotalViews).ToList();
            }
            else if (Order == SortOrder.TopRated)
            {
                CurrentVideos = CurrentVideos.OrderByDescending(_ => _.TotalLikes).ToList();
            }
            else if (Order == SortOrder.Newest)
            {
                CurrentVideos = CurrentVideos.OrderByDescending(_ => _.DateCreated).ToList();
            }
            else if (Order == SortOrder.Favorites)
            {
                CurrentVideos = CurrentVideos.Where(_ => _.IsFavorite).ToList();
            }
            else if (Order == SortOrder.Downloads)
            {
                CurrentVideos = CurrentVideos.Where(_ => _.IsDownloaded).ToList();
            }
            foreach (var video in CurrentVideos)
            {
                if (isOfflineMode && !video.IsDownloaded)
                    continue;
                var item = new VideoItem
                {
                    ID = video.ID,
                    Title = video.Name,
                    Description = video.Overview,
                    Url = "",
                    Thumbnail = video.Thumbnail,
                    Data = video
                };
                var videoItem = Instantiate(UserInterface.instance.m_Template);
                videoItem.tag = "Clone";
                videoItem.GetComponent<VideoItemView>().Video = item;
                videoItem.transform.SetParent(UserInterface.instance.m_Grid.transform, false);
                videoItem.SetActive(true);
                viewCount++;
            }
            try
            {
                LoadFilterData(CurrentVideos);
            }
            catch (Exception e)
            {
                Trace.LogError(e.ToString());
            }
            if (viewCount == 0)
            {
                if (UserInterface.instance.m_EmptyText)
                {
                    var text = UserInterface.instance.m_EmptyText.GetComponent<TMP_Text>();
                    if (isOfflineMode)
                    {
                        text.text = "You're in offline mode and have no downloaded videos.\nPlease go online to view video.";
                    }
                    else
                    {
                        text.text = "No videos for the specified filter.\nPlease select fewer filter options.";
                    }
                    UserInterface.instance.m_EmptyText.SetActive(true);
                }
            }
        }
        else
        {
            Trace.Log("Region is blank or Video data is missing.");
        }
        if (UserInterface.instance.m_LeftArrow != null && UserInterface.instance.m_RightArrow != null)
        {
            if (viewCount <= 6)
            {
                //Trace.Log("Hide Arrows");
                UserInterface.instance.m_LeftArrow.SetActive(false);
                UserInterface.instance.m_RightArrow.SetActive(false);
            }
            else
            {
                //Trace.Log("Show Arrows");
                UserInterface.instance.m_LeftArrow.SetActive(true);
                UserInterface.instance.m_RightArrow.SetActive(true);
            }
        }
        if (UserInterface.instance.m_ScrollPanel)
        {
            UserInterface.instance.m_ScrollPanel.horizontalNormalizedPosition = 0;
        }
        //StartCoroutine(LoadThumbnails());
        UserInterface.instance.m_Loading.SetActive(false);
    }

    private bool CheckVideoFilterAdd(VideoDataItem video, bool skipPrimarySpecies = false, bool noneSelected = false)
    {
        bool add = false;
        if (video.Metadata != null && video.Metadata.Count > 0)
        {
            var liveAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Live"));
            if (liveAttribute == null || liveAttribute.Value.Trim().ToUpper().Equals("N"))
                return false;
            var graphicAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Graphic"));
            if (graphicAttribute != null)
            {
                if (!ShowGraphicDeath && graphicAttribute.Value.Trim().ToLower().Contains("death"))
                {
                    return false;
                }
                if (!ShowGraphicSex && graphicAttribute.Value.Trim().ToLower().Contains("sex"))
                {
                    return false;
                }
            }
            var region1Attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Portal 1"));
            var region2Attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Portal 2"));
            if (CurrentRegion.Equals("AllMarker") ||
                (region1Attribute != null && region1Attribute.Value.Trim().Contains(CurrentRegion)) ||
                (region2Attribute != null && region2Attribute.Value.Trim().Contains(CurrentRegion)))
            {
                // Default add
                add = true;
                if (Events.Instance.IsFilterActive && !skipPrimarySpecies)
                {
                    add = FilterPrimarySpecies != null && FilterPrimarySpecies.Count > 0 && FilterPrimarySpecies.Any(_ => _.Value.HasValue && _.Value.Value);
                }
                // Check filters (to remove "add" via filter)
                var speciesAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Colloquial Species Name"));
                if (add && !skipPrimarySpecies && FilterPrimarySpecies != null && FilterPrimarySpecies.Count > 0)
                {
                    //if (FilterPrimarySpecies.Any(_ => _.Value.HasValue && _.Value.Value)) {
                    if (noneSelected)
                    {
                        //if (speciesAttribute == null || !FilterPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim()) ||
                        //    (speciesAttribute != null && FilterPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim()) &&
                        //    (!FilterPrimarySpecies[speciesAttribute.Value.Trim()].HasValue || FilterPrimarySpecies[speciesAttribute.Value.Trim()].Value == false))) {
                        //    add = false;
                        //}
                        add = speciesAttribute != null && FilterPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim());
                    }
                    else
                    {
                        if (speciesAttribute == null || !FilterPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim()) ||
                            (speciesAttribute != null && FilterPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim()) &&
                            (!FilterPrimarySpecies[speciesAttribute.Value.Trim()].HasValue || FilterPrimarySpecies[speciesAttribute.Value.Trim()].Value == false)))
                        {
                            add = false;
                        }
                    }
                    //}
                }
                if (add && FilterIUCN != null && FilterIUCN.Count > 0)
                {
                    if (FilterIUCN.Any(_ => _.Value.HasValue && _.Value.Value))
                    {
                        var iucnAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("IUCN"));
                        //Debug.Log(speciesAttribute.Value + " IUCN - Attr: " + JsonConvert.SerializeObject(iucnAttribute) + "Val: " + JsonConvert.SerializeObject(FilterIUCN[iucnAttribute.Value]));
                        if (iucnAttribute != null && FilterIUCN.ContainsKey(iucnAttribute.Value.Trim()) &&
                            (!FilterIUCN[iucnAttribute.Value.Trim()].HasValue || FilterIUCN[iucnAttribute.Value.Trim()].Value == false))
                        {
                            add = false;
                        }
                    }
                }
                if (add)
                {
                    //Debug.Log("Add after IUCN!");
                }
                if (add && FilterPopulationTrend != null && FilterPopulationTrend.Count > 0)
                {
                    if (FilterPopulationTrend.Any(_ => _.Value.HasValue && _.Value.Value))
                    {
                        var populationAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Population Trend"));
                        if (populationAttribute != null && FilterPopulationTrend.ContainsKey(populationAttribute.Value.Trim()) &&
                            (!FilterPopulationTrend[populationAttribute.Value.Trim()].HasValue || FilterPopulationTrend[populationAttribute.Value.Trim()].Value == false))
                        {
                            add = false;
                        }
                    }
                }
            }
            else if (CurrentRegion.Equals("SpecialMarker"))
            {
                var compilationAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Compilations"));
                if (compilationAttribute != null && compilationAttribute.Value.Trim().Equals("Y"))
                {
                    add = true;
                }
            }
        }
        return add;
    }

    public void UpdateVideoGalleryThumbnailHoverState()
    {
        foreach (Transform child in UserInterface.instance.m_Grid.transform)
        {
            if (child.tag.Contains("Template"))
                continue;
            var details = child.gameObject.FindObject("VideoItemDetails");
            details.SetActive(false);
        }
    }

    public void UpdateVideosDownloadStatus()
    {
        if (!UserInterface.instance.m_Grid)
            return;
        try
        {
            foreach (Transform child in UserInterface.instance.m_Grid.transform)
            {
                if (child.tag.Contains("Template"))
                    continue;
                var video = child.GetComponent<VideoItemView>();
                var downloadImage = video.Download.gameObject.FindObject("Image").GetComponent<Image>();
                if (Events.downloadBusy)
                {
                    if (video.Video.Data.ID != Events.activeDownload.Video.Data.ID)
                    {
                        if (!video.Video.Data.IsDownloaded)
                        {
                            downloadImage.sprite = Resources.Load<Sprite>("ic_gallery_download_blocked");
                        }
                        video.Download.GetComponent<VRInteractiveItem>().IsGazeable = false;
                    }
                }
                else
                {
                    downloadImage.sprite = video.Video.Data.IsDownloaded ? Resources.Load<Sprite>("ic_gallery_download_on") : Resources.Load<Sprite>("ic_gallery_download_off");
                    video.Download.GetComponent<VRInteractiveItem>().IsGazeable = !video.Video.Data.IsDownloaded;
                }
            }
        }
        catch (Exception e)
        {
            Trace.LogError(e.ToString());
        }
    }

    IEnumerator LoadThumbnailCache()
    {
        Trace.Log("Start: Loading thumbnail cache");
        foreach (var video in Data.Videos)
        {
            yield return Main.Graphics.Cache(video);
        }
        Trace.Log("End: Loading thumbnail cache");
        //m_Loading.SetActive(false);
    }

    IEnumerator LoadThumbnails()
    {
        foreach (Transform child in UserInterface.instance.m_Grid.transform)
        {
            if (child.tag.Contains("Template"))
                continue;
            var view = child.GetComponent<VideoItemView>();
            if (view)
            {
                yield return Main.Graphics.FromCache(view, true, (item) =>
                {
                    try
                    {
                        view.Thumbnail.sprite = item.Sprite;
                    }
                    catch (Exception e)
                    {
                        // ???
                        Trace.LogError(e.ToString());
                        Trace.Log(view.Thumbnail.ToString());
                    }
                });
            }
        }
    }

    IEnumerator LoadThumbnail(Transform item)
    {
        var view = item.GetComponent<VideoItemView>();
        if (view)
        {
            yield return Main.Graphics.FromCache(view, true, (callback) =>
            {
                try
                {
                    callback.View.Thumbnail.sprite = callback.Sprite;
                }
                catch (Exception e)
                {
                    // ???
                    Trace.LogError(e.ToString());
                }
            });
        }
    }

    public void LoadFilterData()
    {
        FilterRegion = null;
        LoadVideos(CurrentRegion, true);
    }

    public void SetFilterOn()
    {
        if (UserInterface.instance.m_FilterButton)
        {
            var button = UserInterface.instance.m_FilterButton.GetComponent<Button>();
            var text = UserInterface.instance.m_FilterButton.FindObject("Text").GetComponent<TMP_Text>();
            var image = button.GetComponent<Image>();
            image.color = new Color32(0, 157, 255, 255);
            text.text = "Active Filter";
        }
    }

    public void LoadFilterData(List<VideoDataItem> videos)
    {
        if (videos == null || videos.Count == 0)
            return;
        if (!UserInterface.instance.m_FilterPanel)
            return;
        Trace.Log("Load filter: check region - " + CurrentRegion + " = " + FilterRegion);
        if (CurrentRegion == FilterRegion)
            return;
        if (UserInterface.instance.m_FilterButton)
        {
            var button = UserInterface.instance.m_FilterButton.GetComponent<Button>();
            var text = UserInterface.instance.m_FilterButton.FindObject("Text").GetComponent<TMP_Text>();
            var image = button.GetComponent<Image>();
            image.color = new Color32(255, 255, 255, 255);
            text.text = "Filter";
        }
        Trace.Log("Loading filter data for region: " + CurrentRegion);
        FilterRegion = CurrentRegion;

        // Load category filters
        LoadFilterDataCategory(videos, "SpeciesCategoryFilterContent", "SpeciesCategoryFilterTemplate", "Species Category", ref FilterSpeciesCategory, true, true);
        LoadFilterPrimarySpeciesDataCategory(videos, "PrimarySpeciesFilterContent", "PrimarySpeciesFilterTemplate", "Colloquial Species Name", ref FilterPrimarySpecies, true, true);
        LoadIUCNFilterDataCategory(videos);
        LoadFilterDataCategory(videos, "PopulationTrendFilterContent", "PopulationTrendFilterTemplate", "Population Trend", ref FilterPopulationTrend, false, true);

        // Store current species for checking/unchecking filters
        CurrentPrimarySpecies = FilterPrimarySpecies;
        Debug.Log(JsonConvert.SerializeObject(CurrentPrimarySpecies));
    }

    public void LoadIUCNFilterDataCategory(List<VideoDataItem> videos)
    {
        var list = GetIUCNFilterAttributeItems(videos);
        LoadFilterDataCategory(videos, "IUCNFilterContent", "IUCNFilterTemplate", list, ref FilterIUCN, true);
    }

    private List<string> GetIUCNFilterAttributeItems(List<VideoDataItem> videos)
    {
        Dictionary<string, bool> list = new Dictionary<string, bool> {
            { "Extinct (EX)", true },
            { "Extinct In The Wild (EW)", true },
            { "Critically Endangered (CR)", true },
            { "Endangered (EN)", true },
            { "Vulnerable (VU)", true },
            { "Near Threatened (NT)", true },
            { "Least Concern (LC)", true },
            { "Data Deficient (DD)", true },
            { "Not Evaluated (NE)", true }
        };
        foreach (var video in videos)
        {
            if (video.Metadata != null && video.Metadata.Count > 0)
            {
                var attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("IUCN"));
                if (attribute != null && list.ContainsKey(attribute.Value.Trim()))
                {
                    list[attribute.Value.Trim()] = true;
                }
            }
        }
        return list.Where(_ => _.Value).Select(_ => _.Key).ToList();
    }

    public void LoadFilterDataCategory(List<VideoDataItem> videos, string gridName, string templateName, string metadata, ref Dictionary<string, bool?> filter, bool sort, bool on)
    {
        var list = GetFilterAttributeItems(videos, metadata);
        if (sort)
        {
            list.Sort();
        }
        LoadFilterDataCategory(videos, gridName, templateName, list, ref filter, on);
    }

    public void LoadFilterPrimarySpeciesDataCategory(List<VideoDataItem> videos, string gridName, string templateName, string metadata, ref Dictionary<string, bool?> filter, bool sort, bool on)
    {
        var list = GetFilterAttributePrimarySpeciesItems(videos, metadata);
        if (sort)
        {
            list.Sort();
        }
        LoadFilterDataCategory(videos, gridName, templateName, list, ref filter, on);
    }

    public void LoadFilterDataCategory(List<VideoDataItem> videos, string gridName, string templateName, List<string> items, ref Dictionary<string, bool?> filter, bool on)
    {
        if (videos == null || videos.Count == 0 || string.IsNullOrEmpty(gridName) || string.IsNullOrEmpty(templateName) || items == null)
            return;
        // Get filter grid
        var grid = UserInterface.instance.m_FilterPanel.FindObject(gridName).GetComponent<GridLayoutGroup>();
        foreach (Transform child in grid.transform)
        {
            if (child.tag.Contains("Template"))
                continue;
            Destroy(child.gameObject);
        }
        var template = UserInterface.instance.m_FilterPanel.FindObject(templateName);
        template.SetActive(false);
        filter = new Dictionary<string, bool?>();
        foreach (var item in items)
        {
            filter.Add(item.Trim(), null);
        }
        foreach (var filterKey in filter.Keys)
        {
            var filterItem = Instantiate(template);
            var item = filterItem.GetComponent<Toggle>();
            var label = filterItem.FindObject("Label").GetComponent<TMP_Text>();
            item.isOn = on;
            label.text = filterKey.Trim();
            filterItem.name = filterKey.Trim();
            filterItem.tag = "Clone";
            filterItem.transform.SetParent(grid.transform, false);
            filterItem.SetActive(true);
        }
    }

    private List<string> GetFilterAttributeItems(List<VideoDataItem> videos, string metadata)
    {
        List<string> list = new List<string>();
        foreach (var video in videos)
        {
            if (video.Metadata != null && video.Metadata.Count > 0)
            {
                var attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals(metadata));
                if (attribute != null && !string.IsNullOrEmpty(attribute.Value.Trim()) && !list.Contains(attribute.Value.Trim()))
                {
                    list.Add(attribute.Value.Trim());
                }
            }
        }
        return list;
    }

    private List<string> GetFilterAttributePrimarySpeciesItems(List<VideoDataItem> videos, string metadata)
    {
        List<string> list = new List<string>();
        foreach (var video in videos)
        {
            if (video.Metadata != null && video.Metadata.Count > 0)
            {
                var attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals(metadata));
                //Debug.Log("GetFilterAttributePrimarySpeciesItems: " + metadata + " - " + attribute.Value.Trim());
                bool add = CheckVideoFilterAdd(video, true);
                if (add && attribute != null && !string.IsNullOrEmpty(attribute.Value.Trim()) && !list.Contains(attribute.Value.Trim()))
                {
                    list.Add(attribute.Value.Trim());
                }
            }
        }
        return list;
    }

    public void UpdateSpeciesCategoryFilterDataCategory(bool clear, bool deselectAll = false)
    {
        Trace.Log("UpdateSpeciesCategoryFilterDataCategory");
        if (Data.Videos == null || Data.Videos.Count == 0)
            return;
        // Get species category filter grid
        var categories = new List<string>();
        var categoryGrid = UserInterface.instance.m_FilterPanel.FindObject("SpeciesCategoryFilterContent").GetComponent<GridLayoutGroup>();
        foreach (Transform child in categoryGrid.transform)
        {
            if (child.tag.Contains("Template"))
                continue;
            var item = child.gameObject.GetComponent<Toggle>();
            var label = child.gameObject.FindObject("Label").GetComponent<TMP_Text>();
            if (item.isOn || clear)
            {
                categories.Add(label.text);
            }
        }
        // Check ignore
        //if (categories.Count == 0)
        //    return;
        // Get primary species filter grid
        var grid = UserInterface.instance.m_FilterPanel.FindObject("PrimarySpeciesFilterContent").GetComponent<GridLayoutGroup>();
        foreach (Transform child in grid.transform)
        {
            if (child.tag.Contains("Template"))
                continue;
            //var item = child.gameObject.GetComponent<Toggle>();
            //var label = child.gameObject.FindObject("Label").GetComponent<TMP_Text>();
            ////if (item.isOn) {
            //    FilterPrimarySpecies[label.text] = item.isOn;
            ////}
            Destroy(child.gameObject);
        }
        var template = UserInterface.instance.m_FilterPanel.FindObject("PrimarySpeciesFilterTemplate");
        template.SetActive(false);
        FilterPrimarySpecies = new Dictionary<string, bool?>();
        foreach (var video in Data.Videos)
        {
            if (video.Metadata != null && video.Metadata.Count > 0)
            {
                var region1Attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Portal 1"));
                var region2Attribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Portal 2"));
                var categoryAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Species Category"));
                var speciesAttribute = video.Metadata.FirstOrDefault(_ => _.Name.Equals("Colloquial Species Name"));
                if (CurrentRegion.Equals("AllMarker") || (region1Attribute != null && region1Attribute.Value.Trim().Contains(CurrentRegion)) ||
                    (region2Attribute != null && region2Attribute.Value.Trim().Contains(CurrentRegion)))
                {
                    if (speciesAttribute != null && !string.IsNullOrEmpty(speciesAttribute.Value.Trim()) &&
                        !FilterPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim()) &&
                        categoryAttribute != null && categories.Contains(categoryAttribute.Value.Trim()))
                    {
                        // Add stub
                        bool add = CheckVideoFilterAdd(video, true);
                        if (add)
                        {
                            bool? isOn = null;
                            //bool existing = CurrentPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim());
                            //bool? active = null;
                            if (CurrentPrimarySpecies.ContainsKey(speciesAttribute.Value.Trim()))
                            {
                                isOn = CurrentPrimarySpecies[speciesAttribute.Value.Trim()];
                            }
                            //if (existing && active == null) {
                            //    isOn = true;
                            //}
                            if (isOn == null)
                            {
                                isOn = true;
                            }
                            if (deselectAll)
                            {
                                isOn = false;
                            }
                            //Debug.Log("Existing/Active: " + speciesAttribute.Value + " - " + isOn);
                            //FilterPrimarySpecies.Add(speciesAttribute.Value.Trim(), existing && !deselectAll ? true : (bool?)null);
                            FilterPrimarySpecies.Add(speciesAttribute.Value.Trim(), isOn);
                        }
                    }
                }
            }
        }
        var keysList = FilterPrimarySpecies.Keys.ToList();
        keysList.Sort();
        foreach (var key in keysList)
        {
            var filterItem = Instantiate(template);
            var label = filterItem.FindObject("Label").GetComponent<TMP_Text>();
            label.text = key;
            filterItem.name = key;
            filterItem.tag = "Clone";
            filterItem.transform.SetParent(grid.transform, false);
            filterItem.SetActive(true);
            var item = filterItem.gameObject.GetComponent<Toggle>();
            item.isOn = FilterPrimarySpecies[key].HasValue && FilterPrimarySpecies[key].Value;
        }
        Debug.Log("Keys List: " + JsonConvert.SerializeObject(keysList));
    }
}

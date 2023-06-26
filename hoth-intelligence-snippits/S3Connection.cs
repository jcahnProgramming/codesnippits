//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the AWS Mobile SDK For Unity 
// Sample Application License Agreement (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located 
// in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace AWSSDK.Examples
{
    public class S3Connection : MonoBehaviour
    {
        public GameManager gm;
        public AWSConnectionVariables aws;
        public UserInterface ui;
        public Utility utility;

        public string IdentityPoolId = "";
        public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;
        private RegionEndpoint _CognitoIdentityRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
        }
        public string S3Region = RegionEndpoint.USEast1.SystemName;
        private RegionEndpoint _S3Region
        {
            get { return RegionEndpoint.GetBySystemName(S3Region); }
        }
        public string S3BucketName = null;
        public string SampleFileName = null;
        //public Text ResultText = null;

        void Start()
        {
            UnityInitializer.AttachToGameObject(this.gameObject);
        }

        #region private members

        private IAmazonS3 _s3Client;
        private AWSCredentials _credentials;

        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
                return _credentials;
            }
        }

        private IAmazonS3 Client
        {
            get
            {
                if (_s3Client == null)
                {
                    _s3Client = new AmazonS3Client(Credentials, _S3Region);
                }
                //test comment
                return _s3Client;
            }
        }

        #endregion

        #region Get Bucket List
        /// <summary>
        /// Example method to Demostrate GetBucketList
        /// </summary>
        public void GetBucketList()
        {
            //ResultText.text = "Fetching all the Buckets";
            Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
            {
                //ResultText.text += "\n";
                if (responseObject.Exception == null)
                {
                    //ResultText.text += "Got Response \nPrinting now \n";
                    responseObject.Response.Buckets.ForEach((s3b) =>
                    {
                        //ResultText.text += string.Format("bucket = {0}, created date = {1} \n", s3b.BucketName, s3b.CreationDate);
                    });
                }
                else
                {
                    //ResultText.text += "Got Exception \n";
                }
            });
        }

        #endregion

        /// <summary>
        /// Get Objects from S3 Bucket
        /// </summary>
        public void GetObjects(string username)
        {
            //if we're loggin in again we need to clear out the drop down list of contents and the previously selected download.
            aws.filelist.Clear();
            ui.dropdownForDownloadList.options.Clear();
            ui.selectedDropdownItemLabel.text = "";

            Debug.Log("inside get objects");
            utility.LoggingFromOtherScripts("username1: " + username);

#if UNITY_EDITOR
            utility.LoggingFromOtherScripts("#if test editor");
#elif UNITY_IOS
           gm.LoggingFromOtherScripts("#if test ios");
#endif

            var request = new ListObjectsV2Request()
            {
                BucketName = S3BucketName
                //Prefix = (username)
            };

            Client.ListObjectsV2Async(request, (responseObject) =>
            {
                utility.LoggingFromOtherScripts("Client.ListObjectsAsync");
                if (responseObject.Exception == null)
                {
                    utility.LoggingFromOtherScripts("inside responseObject.Exception == null ");
                    utility.LoggingFromOtherScripts("items returned count: " + responseObject.Response.S3Objects.Count().ToString());

                    utility.LoggingFromOtherScripts(GenericToDataString.ObjectDumper.Dump(responseObject));

                    responseObject.Response.S3Objects.ForEach((o) =>
                    {
                        utility.LoggingFromOtherScripts("responseObject.Response.S3Objects.ForEach ");

                        GetObject(o.Key);
                    });
                }
                else
                {
                    utility.LoggingFromOtherScripts("inside responseObject.Exception != null ERROR");
                }
            });
            utility.LoggingFromOtherScripts("User gained access via Username and Password to S3 Bucket");
        }

        /// <summary>
        /// Get Object from S3 Bucket
        /// </summary>
        private void GetObject(string userAndFileName)
        {
            string fileName = "";
            string user = "";

            fileName = userAndFileName.Substring(userAndFileName.LastIndexOf('/') + 1);
            user = userAndFileName.Substring(0, userAndFileName.LastIndexOf('/'));

            utility.LoggingFromOtherScripts("userAndFileName: [" + userAndFileName + "] fileName: [" + fileName + "] user: [" + user + "]");
            // fix equality make case insensitive
            if (ui.username == user)
            {
#if UNITY_EDITOR
                string streamingAssetsAndUserPath = Path.Combine(Application.streamingAssetsPath, user);
#elif UNITY_IOS
                string streamingAssetsAndUserPath = Path.Combine(Application.persistentDataPath, user);
#endif
                utility.LoggingFromOtherScripts("streamingAssetsAndUserPath: [" + streamingAssetsAndUserPath + "]");

                System.IO.Directory.CreateDirectory(streamingAssetsAndUserPath);

                Client.GetObjectAsync(S3BucketName, userAndFileName, (responseObj) =>
                {
                    using (var response = responseObj.Response)
                    {
                        if (responseObj.Response.ResponseStream != null)
                        {
                            string userAndFileName = response.Key;

#if UNITY_EDITOR
                            string streamingAssetsAndUserAndFileNamePath = Path.Combine(Application.streamingAssetsPath, userAndFileName);
#elif UNITY_IOS
                            string streamingAssetsAndUserAndFileNamePath = Path.Combine(Application.persistentDataPath, userAndFileName);
#endif

                            utility.LoggingFromOtherScripts("streamingAssetsAndUserAndFileNamePath: in Client.GetObjectAsync [" + streamingAssetsAndUserAndFileNamePath + "]");

                            using (var fs = System.IO.File.Create(streamingAssetsAndUserAndFileNamePath))
                            {
                                utility.LoggingFromOtherScripts("in System.IO.File.Create");

                                int readByte = 0;
                                bool foundnewlineflag = false;
                                bool writetoend = false;
                                bool firstbyteread = true;
                                while (readByte != -1)
                                {
                                    readByte = response.ResponseStream.ReadByte();

                                    if (writetoend == true)
                                    {
                                        fs.WriteByte(Convert.ToByte(readByte));
                                    }
                                    else
                                    {
                                        // 0xA is \n
                                        if (readByte == 0xA)
                                        {
                                            foundnewlineflag = true;
                                        }
                                        // 0x76 is v
                                        else if ((readByte == 0x76) && (foundnewlineflag || firstbyteread))
                                        {
                                            fs.WriteByte(Convert.ToByte(readByte));
                                            writetoend = true;
                                        }
                                        else
                                        {
                                            foundnewlineflag = false;
                                            firstbyteread = false;
                                        }
                                    }
                                }
                                fs.Flush();
                            }
                        }
                    }
                });

                utility.LoggingFromOtherScripts("Adding to dropdown list");
                aws.filelist.Add(userAndFileName);
                Dropdown.OptionData newDropDownItem = new Dropdown.OptionData();
                newDropDownItem.text = fileName;
                ui.dropdownForDownloadList.options.Add(newDropDownItem);
                utility.LoggingFromOtherScripts("Dropdown List Ready to Populate with S3 Text");
            }
        }

        /// <summary>
        /// Post Object to S3 Bucket. 
        /// </summary>
        public void PostObject()
        {
            //ResultText.text = "Retrieving the file";

            string fileName = GetFileHelper();
             
            var stream = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            //ResultText.text += "\nCreating request object";
            var request = new PostObjectRequest()
            {
                Bucket = S3BucketName,
                Key = fileName,
                InputStream = stream,
                CannedACL = S3CannedACL.Private
            };
            utility.LoggingFromOtherScripts("Objects Posted to DropdownList");

            //ResultText.text += "\nMaking HTTP post call";

            Client.PostObjectAsync(request, (responseObj) =>
            {
                if (responseObj.Exception == null)
                {
                    //ResultText.text += string.Format("\nobject {0} posted to bucket {1}", responseObj.Request.Key, responseObj.Request.Bucket);
                }
                else
                {
                    //ResultText.text += "\nException while posting the result object";
                    //ResultText.text += string.Format("\n receieved error {0}", responseObj.Response.HttpStatusCode.ToString());
                }
            });
        }

        /// <summary>
        /// Delete Objects in S3 Bucket
        /// </summary>
        public void DeleteObject()
        {
            //ResultText.text = string.Format("deleting {0} from bucket {1}", SampleFileName, S3BucketName);
            List<KeyVersion> objects = new List<KeyVersion>();
            objects.Add(new KeyVersion()
            {
                Key = SampleFileName
            });

            var request = new DeleteObjectsRequest()
            {
                BucketName = S3BucketName,
                Objects = objects
            };

            Client.DeleteObjectsAsync(request, (responseObj) =>
            {
                //ResultText.text += "\n";
                if (responseObj.Exception == null)
                {
                    //ResultText.text += "Got Response \n \n";

                    //ResultText.text += string.Format("deleted objects \n");

                    responseObj.Response.DeletedObjects.ForEach((dObj) =>
                    {
                        //ResultText.text += dObj.Key;
                    });
                }
                else
                {
                    //ResultText.text += "Got Exception \n";
                }
            });
        }


        #region helper methods

        private string GetFileHelper()
        {
            var fileName = SampleFileName;

            if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName))
            {
                var streamReader = File.CreateText(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName);
                streamReader.WriteLine("This is a sample s3 file uploaded from unity s3 sample");
                streamReader.Close();
            }
            return fileName;
        }

        private string GetPostPolicy(string bucketName, string key, string contentType)
        {
            bucketName = bucketName.Trim();

            key = key.Trim();
            // uploadFileName cannot start with /
            if (!string.IsNullOrEmpty(key) && key[0] == '/')
            {
                throw new ArgumentException("uploadFileName cannot start with / ");
            }

            contentType = contentType.Trim();

            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException("bucketName cannot be null or empty. It's required to build post policy");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("uploadFileName cannot be null or empty. It's required to build post policy");
            }
            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType cannot be null or empty. It's required to build post policy");
            }

            string policyString = null;
            int position = key.LastIndexOf('/');
            if (position == -1)
            {
                policyString = "{\"expiration\": \"" + DateTime.UtcNow.AddHours(24).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" +
                    bucketName + "\"},[\"starts-with\", \"$key\", \"" + "\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", " + "\"" + contentType + "\"" + "]]}";
            }
            else
            {
                policyString = "{\"expiration\": \"" + DateTime.UtcNow.AddHours(24).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" +
                    bucketName + "\"},[\"starts-with\", \"$key\", \"" + key.Substring(0, position) + "/\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", " + "\"" + contentType + "\"" + "]]}";
            }

            return policyString;
        }

    }

        #endregion
}

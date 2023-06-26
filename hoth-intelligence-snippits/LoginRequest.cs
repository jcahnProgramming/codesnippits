using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.S3;
using Amazon.CognitoIdentity;
using UnityEngine.UI;
using Amazon.S3.Model;
using AWSSDK;

public class LoginRequest : MonoBehaviour
{
    public InputField username;
    public InputField password;

    [Header("C# References")]
    public GameManager gm;
    public Utility utility;
    public AWSConnectionVariables aws;
    public ChangeView changeView;
    public UserInterface ui;
    public FaceController fc;
    public MeshFilterSwap mfs;
    //public CognitoAWSCredentials(string accountId, string identityPoolId, string unAuthRoleArn, string authRoleArn, RegionEndpoint region);


    // Start is called before the first frame update
    void Start()
    {
        gm = new GameManager();
    }

    public void LoginAttempt(string token)
    {
        utility.LoggingFromOtherScripts("User Credentials Accessing CognitoAWSCredentials System on US-East 1 DataServer");
        CognitoAWSCredentials credentials = new CognitoAWSCredentials( 
            token,
            "us-east-1:2b2c5357-6906-4cd6-9139-100edfd43757",    // Cognito Identity Pool ID
            "arn:aws:iam::934149855634:role/Cognito_hothxriosUnauth_Role",
            "arn:aws:iam::934149855634:role/Cognito_hothxriosAuth_Role",
             RegionEndpoint.USEast1
        // Region
        );


        AmazonS3Client S3Client = new AmazonS3Client(credentials);
        utility.LoggingFromOtherScripts("New S3 Client Opened Successfully...");

        /*        AmazonS3 S3Client = AmazonS3ClientBuilder
                        .standard()
                        .withCredentials(new AWSStaticCredentialsProvider(credentials))
                        .withForceGlobalBucketAccessEnabled(true)
                        .build();*/

        string ResultText;

        // ResultText is a label used for displaying status information
        ResultText = "Fetching all the Buckets";
        utility.LoggingFromOtherScripts(ResultText);
        S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            ResultText += "\n";
            if (responseObject.Exception == null)
            {
                ResultText += "Got Response \nPrinting now \n";
                utility.LoggingFromOtherScripts(ResultText);
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    ResultText += string.Format("bucket = {0}, created date = {1} \n",
                    s3b.BucketName, s3b.CreationDate);
                    utility.LoggingFromOtherScripts(ResultText);
                });
            }
            else
            {
                ResultText += "Got Exception \n";
                utility.LoggingFromOtherScripts(ResultText);
            }
        });
        //var request = new ListObjectsRequest()
        //{
        //    BucketName = s3BucketName
        //};

    }

/*    public void GetAWSCredentials()
    {
        IAmazonS3 s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);

        credentials.GetIdentityIdAsync(delegate (AmazonCognitoIdentityResult<string> result) {
            if (result.Exception != null)
            {
                //Exception!
                Debug.Log("credentials.GetIdentityIdAsync error");
            }
            string identityId = result.Response;
        });

        LoginAttempt(credentials);
    }
*/
    // Update is called once per frame
    void Update()
    {
        
    }
}

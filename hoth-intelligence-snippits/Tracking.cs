using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [Header("C# References")]
    public GameManager gm;
    public Utility utility;
    public AWSConnectionVariables aws;
    public ChangeView changeView;
    public UserInterface ui;
    public FaceController fc;
    public MeshFilterSwap mfs;

    [Header("FaceMask Resources")]
    public GameObject FaceMask;
    public MeshFilter brainMesh;
    public MeshFilter ventriclesMesh;
    public MeshFilter skullMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FaceMaskTrackingSetup(bool value)
    {
        if (value)
        {
            if (utility.isLoggingEnabled)
            {
                utility.LoggingFromOtherScripts("Tracking System Initializing...");

                Instantiate(FaceMask, new Vector3(0, 0, 0), Quaternion.identity);

                if (brainMesh == null)
                {
                    mfs.ResetDefaultMeshes();
                    utility.LoggingFromOtherScripts("Brain Mesh Missing - Resetting to Default Mesh...");
                }
                else if (ventriclesMesh == null)
                {
                    mfs.ResetDefaultMeshes();
                    utility.LoggingFromOtherScripts("Ventricles Mesh Missing - Resetting to Default Mesh...");
                }
                else if (skullMesh == null)
                {
                    mfs.ResetDefaultMeshes();
                    utility.LoggingFromOtherScripts("Skull Mesh Missing - Resetting to Default Mesh...");
                }
                else
                {
                    fc.ChangeMeshFilter(brainMesh, ventriclesMesh, skullMesh);
                }
            }
        }
        else
        {
            if (utility.isLoggingEnabled)
            {
                if (FaceMask.activeInHierarchy)
                {
                    Destroy(FaceMask);
                    utility.LoggingFromOtherScripts("Face Mask in Hierarchy has been Successfully Destroyed.");
                }
                utility.LoggingFromOtherScripts("Body Tracking has been stopped...");
            }
        }
    }

    void CustomFaceMaskTrackingSetup(bool value, int selectedObj)
    {
        if (value)
        {
            if (utility.isLoggingEnabled)
            {
                utility.LoggingFromOtherScripts("Tracking System Initializing...");

                Instantiate(FaceMask, new Vector3(0, 0, 0), Quaternion.identity);

                if (brainMesh != null)
                {
                    mfs.GetBrainObj(selectedObj);
                    utility.LoggingFromOtherScripts("Brain Mesh - Successfully Loaded into Face Mask...");
                }
                else if (ventriclesMesh != null)
                {
                    mfs.GetVentriclesObj(selectedObj);
                    utility.LoggingFromOtherScripts("Ventricles Mesh - Successfully Loaded into Face Mask...");
                }
                else if (skullMesh != null)
                {
                    mfs.GetSkullObj(selectedObj);
                    utility.LoggingFromOtherScripts("Skull Mesh - Successfully Loaded into Face Mask...");
                }
                else
                {
                    mfs.ResetDefaultMeshes();
                    fc.ChangeMeshFilter(brainMesh, ventriclesMesh, skullMesh);
                }
            }
        }
        else
        {
            if (utility.isLoggingEnabled)
            {
                if (FaceMask.activeInHierarchy)
                {
                    Destroy(FaceMask);
                    utility.LoggingFromOtherScripts("Face Mask in Hierarchy has been Successfully Destroyed.");
                }
                utility.LoggingFromOtherScripts("Body Tracking has been stopped...");
            }
        }
    }

    public void isBodyTracking(bool value, int selectedObj)
    {
        CustomFaceMaskTrackingSetup(value, selectedObj);
    }

    public void isOBJTracking(bool value, int selectedObj)
    {
        CustomFaceMaskTrackingSetup(value, selectedObj);
    }

    public void isFaceTracking(bool value)
    {
        FaceMaskTrackingSetup(value);
        mfs.ResetDefaultMeshes();
    }
}

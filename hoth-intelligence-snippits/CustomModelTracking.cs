using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomModelTracking : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button brainBtn;
    public Button ventriclesBtn;
    public Button skullBtn;

    [Header("3D ModelInfo")]
    public GameObject brain;
    public GameObject ventricles;
    public GameObject skull;

    [HideInInspector]
    public MeshFilter downloadedMesh;



    private bool brainActive = true;
    private bool skullActive = true;
    private bool ventriclesActive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void SwapMeshFilter(MeshFilter downloadedMesh)
    {
        //this method swaps the Mesh Filter on the Brain .obj file for an external one. The external one is downloaded in the MainMenu Scene
        //It comes from the S3 Amazon AWS Bucket.
        brain.GetComponent<MeshFilter>().mesh = downloadedMesh.mesh;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BrainToggle()
    {
        brainActive = !brainActive;

        brain.SetActive(brainActive);

        brainBtn.GetComponentInChildren<Text>().text = $"Brain {(brain.activeSelf ? "On" : "Off")}";
    }

    public void VentriclesToggle()
    {
        ventriclesActive = !ventriclesActive;

        ventricles.SetActive(ventriclesActive);

        ventriclesBtn.GetComponentInChildren<Text>().text = $"Ventricles {(ventricles.activeSelf ? "On" : "Off")}";
    }

    public void SkullToggle()
    {
        skullActive = !skullActive;
        skull.SetActive(skullActive);

        skullBtn.GetComponentInChildren<Text>().text = $"Skull {(skull.activeSelf ? "On" : "Off")}";
    }
}

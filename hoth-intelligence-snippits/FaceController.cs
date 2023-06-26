using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]

public class FaceController : MonoBehaviour
{
    [SerializeField]
    public FaceMaterial[] meshes;

    [SerializeField]
    private Button swapDepthMaskBtn;

    [Header("UI Elements")]
    private int swapCounter = 0;

    [Header("Depth Mask Objects")]
    private GameObject depthMask;

    [Header("3d Model GameObjects")]
    private GameObject brain;
    private GameObject ventricles;
    private GameObject skull;

    private ARFaceManager ARFaceManager;
    public GameManager gm = new GameManager();
    public MeshFilterSwap mfs = new MeshFilterSwap();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        ARFaceManager = GetComponent<ARFaceManager>();
        swapDepthMaskBtn.onClick.AddListener(SwapDepthMasks);
    }

    void FindGameObjectReferences()
    {
        brain = GameObject.FindGameObjectWithTag("brain");
        ventricles = GameObject.FindGameObjectWithTag("ventricles");
        skull = GameObject.FindGameObjectWithTag("skull");
    }

    public void ChangeMeshFilter(MeshFilter brainMesh, MeshFilter ventriclesMesh, MeshFilter skullMesh)
    {
        FindGameObjectReferences();
        brain.GetComponent<MeshFilter>().mesh = brainMesh.mesh;
        ventricles.GetComponent<MeshFilter>().mesh = ventriclesMesh.mesh;
        skull.GetComponent<MeshFilter>().mesh = skullMesh.mesh;

    }
    public bool CheckIfNullReference()
    {
        if (brain.gameObject == null)
        {
            //send message to user letting them know
            return true;
        }
        else if (ventricles.gameObject == null)
        {
            //send message to user letting them know
            return true;
        }
        else if (skull.gameObject == null)
        {
            //send message to user letting them know
            return true;
        }
        else
        {
            //error
            return false;
        }
    }

    void SwapDepthMasks()
    {
        depthMask = GameObject.FindGameObjectWithTag("depthMask");
        FindGameObjectReferences();
        if (CheckIfNullReference())
        {
            //if this comes back true we need to set the gameObjects equal to their default counterparts in Meshfilterswap.cs
            brain.GetComponent<MeshFilter>().mesh = mfs.defaultBrain.mesh;
            ventricles.GetComponent<MeshFilter>().mesh = mfs.defaultVentricles.mesh;
            skull.GetComponent<MeshFilter>().mesh = mfs.defaultSkull.mesh;
        }
        swapCounter = swapCounter == meshes.Length - 1 ? 0 : swapCounter + 1;

        depthMask.GetComponent<MeshFilter>().sharedMesh = meshes[swapCounter].mesh.sharedMesh;
        swapDepthMaskBtn.GetComponentInChildren<Text>().text = $"Face Mesh ({meshes[swapCounter].Name})";
    }
}



[System.Serializable]

public class FaceMaterial
{
    public MeshFilter mesh;
    public string Name;
}

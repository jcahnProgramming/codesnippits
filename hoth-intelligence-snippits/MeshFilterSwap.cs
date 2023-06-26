using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFilterSwap : MonoBehaviour
{
    public MeshFilter[] brainObjs;
    public MeshFilter[] ventriclesObjs;
    public MeshFilter[] skullObjs;

    public MeshFilter defaultBrain;
    public MeshFilter defaultVentricles;
    public MeshFilter defaultSkull;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SwapBrain(int value)
    {
        defaultBrain.mesh = brainObjs[value].mesh;
    }

    void SwapVentricles(int value)
    {
        defaultVentricles.mesh = ventriclesObjs[value].mesh;
    }

    void SwapSkull(int value)
    {
        defaultSkull.mesh = skullObjs[value].mesh;
    }

    public void GetBrainObj(int value)
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("brainObj");

        tmp.GetComponent<MeshFilter>().mesh = brainObjs[value].mesh;
    }

    public void GetVentriclesObj(int value)
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("ventriclesObj");
        tmp.GetComponent<MeshFilter>().mesh = ventriclesObjs[value].mesh;
    }

    public void GetSkullObj(int value)
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("skullObj");
        tmp.GetComponent<MeshFilter>().mesh = skullObjs[value].mesh;
    }

    void SetUpCustomModel(int value)
    {
        GetBrainObj(value);
        GetVentriclesObj(value);
        GetSkullObj(value);
    }

    public void ResetDefaultMeshes()
    {
        GameObject tmpBrain = GameObject.FindGameObjectWithTag("brainObj");
        GameObject tmpVentricles = GameObject.FindGameObjectWithTag("ventriclesObj");
        GameObject tmpSkull = GameObject.FindGameObjectWithTag("skullObj");

        tmpBrain.GetComponent<MeshFilter>().mesh = defaultBrain.mesh;
        tmpVentricles.GetComponent<MeshFilter>().mesh = defaultVentricles.mesh;
        tmpSkull.GetComponent<MeshFilter>().mesh = defaultSkull.mesh;
    }



}

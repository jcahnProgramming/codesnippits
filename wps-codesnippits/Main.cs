using UnityEngine;

public static partial class Main {

    public static GameObject FindObject(this GameObject parent, string name) {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs) {
            //Trace.Log(t.name + " == " + name);
            if (t.name == name) {
                return t.gameObject;
            }
        }
        return null;
    }
}

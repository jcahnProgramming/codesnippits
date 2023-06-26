using UnityEngine;
using VRStandardAssets.Utils;
using WPM;

namespace Assets.Scripts {
    public abstract class InteractiveItem : MonoBehaviour {
        //[SerializeField] protected VRInteractiveItem interactiveItem;
        //[SerializeField] protected Renderer m_Renderer;
        private VRInteractiveItem interactiveItem;
        protected Vector3 _hitPosition;

        public void Start() {
            
        }

        public void Awake() {
        }


        protected void OnEnable() {
            interactiveItem = GetComponent<VRInteractiveItem>();
            interactiveItem.OnOver += HandleOver;
            interactiveItem.OnMove += HandleMove;
            interactiveItem.OnGazed += HandleGazed;
            interactiveItem.OnOut += HandleOut;
            interactiveItem.OnClick += HandleClick;
            interactiveItem.OnDoubleClick += HandleDoubleClick;
        }


        protected void OnDisable() {
            interactiveItem.OnOver -= HandleOver;
            interactiveItem.OnMove -= HandleMove;
            interactiveItem.OnGazed -= HandleGazed;
            interactiveItem.OnOut -= HandleOut;
            interactiveItem.OnClick -= HandleClick;
            interactiveItem.OnDoubleClick -= HandleDoubleClick;
        }

        protected abstract void HandleOver(RaycastHit hit, GameObject item);
        protected abstract void HandleMove(RaycastHit hit, GameObject item);
        protected abstract void HandleGazed(RaycastHit hit, GameObject item);
        protected abstract void HandleOut(GameObject item);
        protected abstract void HandleClick();
        protected abstract void HandleDoubleClick();

        public void Update() {
            
        }
    }
}
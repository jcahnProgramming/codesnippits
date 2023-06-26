using RenderHeads.Media.AVProVideo;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

using WPM;

namespace Assets.Scripts {
    public class InteractiveSeekSliderItem : InteractiveItem {

        [SerializeField] private RectTransform slider;
        [SerializeField] private MediaPlayer mediaPlayer;

        public new void Awake() {
        }

        protected override void HandleOver(RaycastHit hit, GameObject item) {
            _hitPosition = hit.point;
        }

        protected override void HandleMove(RaycastHit hit, GameObject item) {
        }

        protected override void HandleOut(GameObject item) {
            _hitPosition = Vector3.zero;
        }

        protected override void HandleGazed(RaycastHit hit, GameObject item) {
            _hitPosition = hit.point;
            if (slider && mediaPlayer) {
                float pos = (hit.point.x + 37.3f) / 74.6f;
                float seekTime = mediaPlayer.Info.GetDurationMs() * pos;
                mediaPlayer.Control.Seek(seekTime);
            }
        }

        protected override void HandleClick() {

        }

        protected override void HandleDoubleClick() {

        }

        public new void Update() {
            
        }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Unitilities;
using Unitilities.Camera;
using UnityEngine;
using Zenject;

namespace Entity
{
    /// <summary>
    /// 摄像机范围设置器
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class CameraBoundSetter : MonoBehaviour, IHasExtraData
    {
        public Bounds2D bounds;

        public Dictionary<string, object> ExtraData
        {
            get => new Dictionary<string, object> {{$"CameraBoundSetter.{nameof(bounds)}", bounds}};
            set
            {
                if(value.TryGetValue($"CameraBoundSetter.{nameof(bounds)}", out var obj))
                {
                    if (obj is JObject jObj)
                    {
                        bounds = jObj.ToObject<Bounds2D>();
                    }
                    else
                    {
                        SetBoundsAsBoxCollider();
                    }
                }
            }
        }


        [Inject] private SimpleCam2D _cam;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<MainCharacter>())
            {
                _cam.cameraBounds = bounds;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            bounds.DrawGizmos();
        }
#endif
        [ContextMenu("Set Bounds as Box Collider")]
        public void SetBoundsAsBoxCollider()
        {
            var b = GetComponent<BoxCollider2D>();
            if (!b) return;
            bounds.Center = transform.TransformPoint(b.offset);
            bounds.Size = b.size;
        }

    }

}

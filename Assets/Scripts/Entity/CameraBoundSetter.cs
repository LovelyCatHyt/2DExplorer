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
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraBoundSetter : MonoBehaviour, IHasExtraData
    {
        public Bounds2D bounds;

        public Dictionary<string, object> ExtraData
        {
            get
            {
                var box = GetComponent<BoxCollider2D>();
                JObject boxData = new JObject();
                boxData["offset"] = JToken.FromObject(box.offset);
                boxData["size"] = JToken.FromObject(box.size);
                return new Dictionary<string, object>
                {
                    {$"CameraBoundSetter.{nameof(bounds)}", bounds},
                    {"CameraBoundSetter.BoxCollider2D", boxData}
                };
            }
            set
            {
                if (value.TryGetValue("CameraBoundSetter.BoxCollider2D", out var box))
                {
                    if(box is JObject b)
                    {
                        var collider = GetComponent<BoxCollider2D>();
                        collider.offset = b["offset"]?.ToObject<Vector2>() ?? Vector2.zero;
                        collider.size = b["size"]?.ToObject<Vector2>() ?? Vector2.one;
                    }
                }
                if(value.TryGetValue($"CameraBoundSetter.{nameof(bounds)}", out var obj))
                {
                    // TODO: set box collider
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
            bounds.Center = transform.TransformPoint(b.offset);
            bounds.Size = b.size;
        }

    }

}

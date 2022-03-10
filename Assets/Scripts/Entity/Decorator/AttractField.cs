using Unitilities;
using UnityEngine;

namespace Entity.Decorator
{
    /// <summary>
    /// "引力"场, 可以让进入 Trigger 的物体受吸引力, 引力随距离变化的过程可以用曲线来描述
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class AttractField : MonoBehaviour
    {
        [Min(0)] public float maxDistance;
        public ParticleSystem.MinMaxCurve forceCurve = 50;

        private void OnTriggerStay2D(Collider2D other)
        {
            var rigid = other.GetComponent<Rigidbody2D>();
            if (rigid)
            {
                var dir = transform.Position2D() - other.transform.Position2D();
                var magnitude = dir.magnitude;
                var force = dir.normalized * forceCurve.Evaluate(magnitude / maxDistance);
                rigid.AddForce(force);
            }
        }
    }

}

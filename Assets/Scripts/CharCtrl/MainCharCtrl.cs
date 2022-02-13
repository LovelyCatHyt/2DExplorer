using System;
using UnityEngine;

namespace CharCtrl
{
    /// <summary>
    /// ½ÇÉ«¿ØÖÆ
    /// </summary>
    public class MainCharCtrl : MonoBehaviour
    {
        [Min(0)] public float speed;

        private Rigidbody2D _rigid;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (input.x != 0 || input.y != 0)
            {
                input *= speed / input.magnitude;
            }

            _rigid.velocity = input;
        }
    }
}

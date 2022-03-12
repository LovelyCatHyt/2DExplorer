using System;
using UnityEngine;

namespace UserCtrl
{
    /// <summary>
    /// 角色控制
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
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
            var tempSpeed = input.magnitude;
            if (tempSpeed > speed)
            {
                input *= speed / tempSpeed;
            }

            _rigid.velocity = input;
        }
    }
}

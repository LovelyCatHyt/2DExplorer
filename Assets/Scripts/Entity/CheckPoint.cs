using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 检查点
    /// </summary>
    public class CheckPoint : MonoBehaviour
    {
        public MainCharacter connectedChar;
        public int index;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var character = other.gameObject.GetComponent<MainCharacter>();
            if (!character) return;
            if (character.currentCheckPoint.index < index)
            {
                character.currentCheckPoint = this;
                connectedChar = character;
            }

            if (character.currentCheckPoint == this)
            {
                // Debug.Log($"Character {character} enter the connected checkpoint.");
            }
        }
    }

}

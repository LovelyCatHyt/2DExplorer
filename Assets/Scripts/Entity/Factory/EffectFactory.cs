using System;
using System.Collections;
using System.Collections.Generic;
using Unitilities;
using Unitilities.Serialization;
using UnityEngine;

namespace Entity.Factory
{
    /// <summary>
    /// 特效工厂
    /// </summary>
    public class EffectFactory : MonoBehaviour
    {
        public List<SerializableKeyValuePair<string, GameObjectPool>> effectPoolList;

        private Dictionary<string, GameObjectPool> _effectPoolDict;

        private void Awake()
        {
            _effectPoolDict = UnityDictConverter.ConvertToDict(effectPoolList);
        }

        public void CreateEffect(string effectType, Vector2 position)
        {
            if (_effectPoolDict[effectType].Pop(out var go))
            {
                go.transform.SetPosition2D(position);
            }
            else
            {
                throw new NullReferenceException($"BulletPool of {effectType} is empty!");
            }
        }
    }

}

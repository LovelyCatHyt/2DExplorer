using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileDataIO
{
    /// <summary>
    /// ��ͼ���ݽӿ�
    /// </summary>
    public interface ITileDataFormat
    {
        /// <summary>
        /// �������ݵ���ͼ��ָ����
        /// </summary>
        /// <param name="map">Ҫ�����ĵ�ͼĿ��</param>
        /// <param name="origin">������ԭ��</param>
        /// <param name="tileLUT">��Ƭ��, ������������Ƭʵ��</param>
        public void ExportToMap(Tilemap map, Vector3Int origin, Dictionary<string, TileBase> tileLUT);

        /// <summary>
        /// �ӵ�ͼ��ָ��λ�õ���
        /// </summary>
        /// <param name="map"></param>
        /// <param name="region"></param>
        public void ImportFromMap(Tilemap map, BoundsInt region);
        
    }

}
using System.Collections;
using System.Collections.Generic;
using Game;
using Unitilities;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Ui
{
    /// <summary>
    /// 退出游戏按钮
    /// </summary>
    public class QuitGameButton : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private GameInstance _game;    // in case I need it
        
        public void OnPointerClick(PointerEventData eventData)
        {
            QuitGame.Quit();
        }
    }

}

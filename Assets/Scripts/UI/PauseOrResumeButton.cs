using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Ui
{
    public class PauseOrResumeButton : MonoBehaviour, IPointerClickHandler
    {

        [Inject] private GameInstance _game;
        [SerializeField] private Text _text;
        
        public void SwitchState(bool isPaused)
        {
            _text.text = isPaused ? "▶" : "||";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_game.State == GameState.InGame)
            {
                _game.Pause();
                SwitchState(true);
            }
            else
            {
                _game.Resume();
                SwitchState(false);
            }
        }
    }
}

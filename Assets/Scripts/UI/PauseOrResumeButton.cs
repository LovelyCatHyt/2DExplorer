using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui
{
    public class PauseOrResumeButton : MonoBehaviour
    {

        [Inject] private GameInstance _game;
        [SerializeField] private Text _text;


        public void OnClick()
        {
            if(_game.State == GameState.InGame)
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

        public void SwitchState(bool isPaused)
        {
            _text.text = isPaused ? "▶" : "||";
        }
    }
}

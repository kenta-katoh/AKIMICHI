using Akimichi.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class ResultDataScene : MonoBehaviour
    {
        [SerializeField]
        private ResultDataContents contents1st = null;

        [SerializeField]
        private ResultDataContents contents2nd = null;

        [SerializeField]
        private ResultDataContents contents3rd = null;

        [SerializeField]
        private ResultDataContents contents4th = null;

        private void Start()
        {
            TransitionManager.Instance().Open();

            foreach (var item in Enum.GetValues(typeof(GameConst.PlayerIndex)))
            {
                GameConst.PlayerIndex playerIndex = (GameConst.PlayerIndex)item;
                PlayerData data = ResultDataManager.Instance().GetPlayerData(playerIndex);
                switch(playerIndex)
                {
                    case GameConst.PlayerIndex.First:
                        contents1st.SetData(playerIndex, data);
                        break;
                    case GameConst.PlayerIndex.Second:
                        contents2nd.SetData(playerIndex, data);
                        break;
                    case GameConst.PlayerIndex.Third:
                        contents3rd.SetData(playerIndex, data);
                        break;
                    case GameConst.PlayerIndex.Fourth:
                        contents4th.SetData(playerIndex, data);
                        break;
                }
            }
            ResultDataManager.Instance().Dispose();

            AudioManager.Instance().PlaySE(SoundConst.RESULT.Data);
        }

        public void TransLobby()
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            AudioManager.Instance().FadeSE();
            TransitionManager.Instance().Transition(SceneConst.Lobby);
        }

        public void TransHome()
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            AudioManager.Instance().FadeSE();
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}

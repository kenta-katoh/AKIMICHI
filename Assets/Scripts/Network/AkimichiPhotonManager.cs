using Akimichi.Game;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class AkimichiPhotonManager : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// マスターサーバーへの接続が成功した時に呼ばれるコールバック
        /// </summary>
        public override void OnConnectedToMaster()
        {
            Debug.Log("マスターサーバーに接続しました");
            NetworkManager.Instance().onConnectedToMaster?.Invoke();
        }

        /// <summary>
        /// Photonのサーバーから切断された時に呼ばれるコールバック
        /// </summary>
        /// <param name="cause"></param>
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
            NetworkManager.Instance().onDisconnected?.Invoke();
        }

        /// <summary>
        /// ロビー接続時コールバック
        /// </summary>
        public override void OnJoinedLobby()
        {
            Debug.Log("ロビーに参加しました");
            NetworkManager.Instance().onJoinedLobby?.Invoke();
        }

        /// <summary>
        /// ロビー接続解除時コールバック
        /// </summary>
        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            Debug.Log("ロビーから退出しました");
            NetworkManager.Instance().onLeaveLobby?.Invoke();
        }

        /// <summary>
        /// ルームの作成が成功した時に呼ばれるコールバック
        /// </summary>
        public override void OnCreatedRoom()
        {
            Debug.Log("ルームの作成に成功しました");
            NetworkManager.Instance().onCreateRoom?.Invoke();
        }

        /// <summary>
        /// ルームの作成が失敗した時に呼ばれるコールバック
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log($"ルームの作成に失敗しました: {message}");
            NetworkManager.Instance().onCreateRoomFailed?.Invoke();
        }

        /// <summary>
        /// ルームへの参加が成功した時に呼ばれるコールバック
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("ルームへ参加しました");
            NetworkManager.Instance().onJoinedRoom?.Invoke();
        }

        /// <summary>
        /// ルーム名を指定したルームへの参加が失敗した時に呼ばれるコールバック
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"ルームへの参加に失敗しました: {message}");
            NetworkManager.Instance().onJoinedRoomFailed?.Invoke();
        }

        /// <summary>
        /// ルームから退出した時に呼ばれるコールバック
        /// </summary>
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            Debug.Log("ルームから退出しました");
            NetworkManager.Instance().onLeaveRoom?.Invoke();
        }

        /// <summary>
        /// ロビー接続時にルーム情報に更新があった場合
        /// </summary>
        /// <param name="roomList"></param>
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            Debug.Log("ルーム情報が更新されました");
            NetworkManager.Instance().onRoomListUpdate?.Invoke(roomList);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("プレイヤーが入室しました");
            NetworkManager.Instance().onPlayerEnteredRoom?.Invoke(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log("プレイヤーが退室しました");
            NetworkManager.Instance().onPlayerLeftRoom?.Invoke(otherPlayer);
        }

        internal void SendEvent(EventConst.Event subtractWeight, object datas)
        {
            throw new NotImplementedException();
        }
    }
}

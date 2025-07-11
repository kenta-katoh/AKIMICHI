using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace Akimichi.Game
{
    public class EventWindow : MonoBehaviour
    {
        [SerializeField]
        private AnimeController animeController = null;

        [SerializeField]
        private TextMeshProUGUI message = null;

        [SerializeField]
        private Image image = null;

        [SerializeField]
        private GameObject input = null;

        [SerializeField]
        private GameObject inputArrow = null;

        private EventDataBase eventData = null;
        private List<string> fonts = new List<string>();
        private bool isFontLoad = false;
        private bool isInputWait = false;
        private int waitFrame = 2;
        private EventConst.EventMessageType currentMessageType = EventConst.EventMessageType.Main;

        /// <summary>
        /// イベント開始
        /// </summary>
        /// <param name="eventData"></param>
        public void StartEvent(EventDataBase data)
        {
            this.isInputWait = false;
            this.inputArrow.SetActive(false);
            this.input.SetActive(false);
            this.image.enabled = true;
            this.message.text = "";
            this.eventData = data;
            this.currentMessageType = EventConst.EventMessageType.Main;

            string msg = this.eventData.Message(this.currentMessageType);
            this.fonts.Clear();
            this.fonts.AddRange(msg.Select(x => x.ToString()).ToList());

            this.animeController.PlayAnime("IsOpen", true, "Open", () =>
            {
                this.isFontLoad = true;
                this.waitFrame = EventConst.FontWaitFrame;
            });
        }

        private void Update()
        {
            if(this.isFontLoad)
            {
                if(this.waitFrame == 0)
                {
                    string font = this.fonts[0];
                    // 末尾文字を判定
                    if (font != EventConst.MessageLastChar)
                    {
                        this.message.text += font;
                        this.fonts.RemoveAt(0);
                        this.waitFrame = EventConst.FontWaitFrame;
                    }
                    else
                    {
                        // 入力待機状態へ
                        this.isFontLoad = false;
                        this.isInputWait = true;
                        this.input.SetActive(true);
                        this.inputArrow.SetActive(true);
                        if (this.currentMessageType == EventConst.EventMessageType.Main && 
                            this.eventData.IsMainMessageLast && 
                            this.eventData.IsSelectEvent)
                        {
                            // 選択イベント
                            this.animeController.PlayAnime("IsSelect", true, "Select", null);
                            this.isInputWait = false;
                            this.input.SetActive(false);
                        }
                    }
                }
                else
                {
                    this.waitFrame--;
                }
            }
        }

        /// <summary>
        /// タップ入力
        /// </summary>
        public void Input()
        {
            if (this.isInputWait)
            {
                this.isInputWait = false;
                AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
                switch (this.currentMessageType)
                {
                    case EventConst.EventMessageType.Main:
                        if (!this.eventData.IsMainMessageLast)
                        {
                            // メインメッセージがまだ終わってないので続きを取得
                            ReplayMessage();
                        }
                        else
                        {
                            this.input.SetActive(false);
                            // メインメッセージが終わったので次の思考
                            if (!this.eventData.IsSelectEvent)
                            {
                                // 選択イベントではなく読み物だったのでクローズ
                                this.animeController.PlayAnime("IsOpen", false, "Close", () =>
                                {
                                    this.isFontLoad = false;
                                    this.image.enabled = false;
                                });
                            }
                        }
                        break;
                    case EventConst.EventMessageType.Yes:
                        if (!this.eventData.IsYesMessageLast)
                        {
                            // メッセージがまだ終わってないので続きを取得
                            ReplayMessage();
                        }
                        else
                        {
                            this.input.SetActive(false);
                            this.animeController.PlayAnime("IsOpen", false, "Close", () =>
                            {
                                this.isFontLoad = false;
                                this.image.enabled = false;
                                this.eventData.OnFinished();
                                this.eventData = null;
                            });
                        }
                        break;
                    case EventConst.EventMessageType.No:
                        if (!this.eventData.IsNoMessageLast)
                        {
                            // メッセージがまだ終わってないので続きを取得
                            ReplayMessage();
                        }
                        else
                        {
                            this.input.SetActive(false);
                            this.animeController.PlayAnime("IsOpen", false, "Close", () =>
                            {
                                this.isFontLoad = false;
                                this.image.enabled = false;
                                this.eventData.OnFinished();
                                this.eventData = null;
                            });
                        }
                        break;
                }
            }
        }

        public void YesInput()
        {
            this.eventData.YesAction();
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            this.animeController.PlayAnime("IsSelect", false, "Result", () => 
            {
                this.currentMessageType = EventConst.EventMessageType.Yes;
                ReplayMessage();
            });
        }

        public void NoInput()
        {
            this.eventData.NoAction();
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            this.animeController.PlayAnime("IsSelect", false, "Result", () =>
            {
                this.currentMessageType = EventConst.EventMessageType.No;
                ReplayMessage();
            });
        }

        private void ReplayMessage()
        {
            this.message.text = "";
            string msg = this.eventData.Message(this.currentMessageType);
            this.fonts.Clear();
            this.fonts.AddRange(msg.Select(x => x.ToString()).ToList());
            this.isFontLoad = true;
            this.waitFrame = EventConst.FontWaitFrame;
            this.input.SetActive(false);
            this.inputArrow.SetActive(false);
            this.isInputWait = false;
        }
    }
}

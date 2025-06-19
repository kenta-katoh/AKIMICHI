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

        private EventDataBase eventData = null;
        private List<string> fonts = new List<string>();
        private bool isFontLoad = false;
        private bool isInputWait = false;
        private int waitFrame = 2;

        /// <summary>
        /// イベント開始
        /// </summary>
        /// <param name="eventData"></param>
        public void StartEvent(EventDataBase data)
        {
            this.isInputWait = false;
            this.input.SetActive(false);
            this.image.enabled = true;
            this.message.text = "";
            this.eventData = data;

            string msg = this.eventData.Message(EventConst.EventMessageType.Main);
            this.fonts.Clear();
            this.fonts.AddRange(msg.Select(x => x.ToString()).ToList());

            this.animeController.PlayAnime("IsOpen", true, "Open", () =>
            {
                this.input.SetActive(true);
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
                    if (font != EventConst.MessageLastChar)
                    {
                        this.message.text += font;
                        this.fonts.RemoveAt(0);
                        this.waitFrame = EventConst.FontWaitFrame;
                    }
                    else
                    {
                        this.isFontLoad = false;
                        this.isInputWait = true;
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
            }
        }
    }
}

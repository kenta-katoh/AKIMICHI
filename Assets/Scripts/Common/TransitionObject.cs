using System;
using UnityEngine;

namespace Akimichi
{
    public class TransitionObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject image = null;

        [SerializeField]
        private AnimeController animeController = null;

        [SerializeField]
        private RectTransform left = null;

        [SerializeField]
        private RectTransform right = null;

        private Action onClose = null;
        private Action onOpen = null;

        private void Awake()
        {
            this.image.SetActive(false);
        }

        public void Close(Action action)
        {
            this.image.SetActive(true);
            this.onClose = null;
            this.onClose = action;

            Vector2 size = ((RectTransform)gameObject.transform).sizeDelta;
            this.left.sizeDelta = new Vector2 ((size.x * 0.5f) * 1.2f, size.y * 1.2f);
            this.right.sizeDelta = new Vector2((size.x * 0.5f) * 1.2f, size.y * 1.2f);

            this.animeController.PlayAnime("Close", true, "Close", () =>
            {
                this.animeController.SetBool("Close", false);
                this.onClose?.Invoke();
            });

            AudioManager.Instance().PlaySE(SoundConst.SE.Trans);
        }

        public void Open(Action action)
        {
            this.image.SetActive(true);
            this.onOpen = null;
            this.onOpen = action;

            Vector2 size = ((RectTransform)gameObject.transform).sizeDelta;
            this.left.sizeDelta = new Vector2((size.x * 0.5f) * 1.2f, size.y * 1.2f);
            this.right.sizeDelta = new Vector2((size.x * 0.5f) * 1.2f, size.y * 1.2f);

            this.animeController.PlayAnime("Open", true, "Open", () =>
            {
                this.animeController.SetBool("Open", false);
                this.image.SetActive(false);
                this.onOpen?.Invoke();
            });

            AudioManager.Instance().PlaySE(SoundConst.SE.Trans);
        }
    }
}

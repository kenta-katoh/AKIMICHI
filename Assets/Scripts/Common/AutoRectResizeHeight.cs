using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoRectResizeHeight : UIBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float ratio = 0.0f;

    [SerializeField]
    private RectTransform parentRect = null;

    [SerializeField]
    private RectTransform rect1 = null;

    [SerializeField]
    private RectTransform rect2 = null;

    protected override void Awake()
    {
        this.rect1.anchorMin = new Vector2(0, 1);
        this.rect1.anchorMax = new Vector2(1, 1);
        this.rect1.pivot = new Vector2(0.5f, 1);

        this.rect2.anchorMin = new Vector2(0, 0);
        this.rect2.anchorMax = new Vector2(1, 0);
        this.rect2.pivot = new Vector2(0.5f, 0);
    }

    /// <summary>
    /// アタッチされているオブジェクトのRectTransformが変更された時によばれる
    /// </summary>
    override protected void OnRectTransformDimensionsChange()
    {
        float height = Mathf.Abs(this.parentRect.rect.height);
        float leftHeight = height * this.ratio;
        float rightHeight = height - leftHeight;

        this.rect1.offsetMin = new Vector2(0, -leftHeight);
        this.rect1.offsetMax = new Vector2(0, 0);
        this.rect2.offsetMin = new Vector2(0, 0);
        this.rect2.offsetMax = new Vector2(0, rightHeight);
    }
}

using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class FadingObject : MonoBehaviour
{
    public bool HasFaded { get; private set; }
    [SerializeField] private Color fadeColor = new Color(255, 255, 255, 40);
    [SerializeField] private SpriteRenderer sr;
    private Color originalColor;
    
    private Tween tween;
    private void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Fade()
    {
        ChangeColor(fadeColor);
        HasFaded = true;
    }

    public void Unfade()
    {
        ChangeColor(originalColor);
        HasFaded = false;
    }

    private void ChangeColor(Color c)
    {
        if (tween != null) tween.Kill(true);
        tween = sr.DOColor(c, .3f);
    }
}

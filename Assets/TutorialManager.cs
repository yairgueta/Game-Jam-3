using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform tutorialsImages;
    [SerializeField] private Button nextButton, prevButton;
    [SerializeField] private Button startButton;
    [SerializeField] private float fadeInDuration, fadeOutDuration;
    [SerializeField] private Ease fadeInEase, fadeOutEase;
    private Image[] tutsImages;
    private int currentIndex;
    private Tween fadeAnimation;
    
    void Start()
    {
        nextButton.onClick.AddListener(() => Scroll(true));
        prevButton.onClick.AddListener(() => Scroll(false));
        tutsImages = tutorialsImages.GetComponentsInChildren<Image>();
        
        if (tutsImages == null || tutsImages.Length == 0) Debug.LogError("No Tutorial Images were found! (put them in the second child)");
        foreach (var image in tutsImages) image.color = new Color(255, 255, 255, 0);
        
        tutsImages[currentIndex].color = new Color(255, 255, 255, 255);
        startButton.gameObject.SetActive(currentIndex == tutsImages.Length - 1);
        prevButton.interactable = false;

        startButton.interactable = false;
        GameManager.Instance.ONFinishLoading.Register(startButton.gameObject, o => startButton.interactable = true);
        startButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            GameManager.Instance.StartGame();
        });
    }

    private Image CurrentImage => tutsImages[currentIndex];
    
    void Scroll(bool isRight)
    {
        var oldImage = CurrentImage;
        currentIndex += isRight ? 1 : -1;
        AnimateTransition(oldImage, CurrentImage);
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < tutsImages.Length - 1;
        
    }
    
    private void AnimateTransition(Image oldImage, Image newImage)
    {
        fadeAnimation?.Kill(true);
        prevButton.interactable = nextButton.interactable = false;
        fadeAnimation = DOTween.Sequence()
            .Append(oldImage.DOFade(0f, fadeOutDuration).SetEase(fadeOutEase))
            .Append(newImage.DOFade(1, fadeInDuration).SetEase(fadeInEase))
            .AppendCallback(() => startButton.gameObject.SetActive(currentIndex == tutsImages.Length - 1));
    }
}

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MenuNextPrev : MonoBehaviour
{
    [Header("Menu Setup")]
    [SerializeField] private GameObject menu;

    [SerializeField] private RectTransform menuRectTransform;
    [SerializeField] private CanvasGroup menuCanvasGroup;

    public enum AnimateToDirection
    {
        Top, Bottom, Left, Right
    }

    [Header("Animation Setup")]
    [SerializeField] private AnimateToDirection openDirection = AnimateToDirection.Top;
    [SerializeField] private AnimateToDirection closeDirection = AnimateToDirection.Bottom;

    [SerializeField] private Vector2 distanceToAnimate = new Vector2(x:100, y:100);
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[Range(0, 1f)][SerializeField] private float animationDuration = 0.5f;

    private bool isOpen;
    private Vector2 initialPosition;
    private Vector2 currentPosition;

    private Vector2 upOffset;
    private Vector2 downOffset;
    private Vector2 leftOffset;
    private Vector2 rightOffset;

    private Coroutine animateMenuCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = menu.transform.position;

		InitializedOffsetPositions();
    }

    private void InitializedOffsetPositions()
    {
        upOffset = new Vector2(0, distanceToAnimate.y);
        downOffset = new Vector2(0, -distanceToAnimate.y);

        rightOffset = new Vector2(distanceToAnimate.x, 0);
        leftOffset = new Vector2(-distanceToAnimate.x, 0);
    }

    [ContextMenu("Toggle On Off")]
    public void ToggleOnOff()
    {
        if (isOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    [ContextMenu("Open Menu")]
    public void OpenMenu()
    {
        if (isOpen)
            return;

        isOpen = true;
		//OnOpenMenu?.Invoke();

        if (animateMenuCoroutine != null)
            StopCoroutine(animateMenuCoroutine);

        animateMenuCoroutine = StartCoroutine(AnimateMenu(true));
    }

    [ContextMenu("Close Menu")]
    public void CloseMenu()
    {
        if (!isOpen) 
            return;

        isOpen = false;
        //OnCloseWindow?.Invoke();

        animateMenuCoroutine = StartCoroutine(AnimateMenu(false));
    }

    private Vector2 GetOffset(AnimateToDirection direction)
    {
        switch (direction)
        {
            case AnimateToDirection.Top:
                return upOffset;
            case AnimateToDirection.Bottom:
                return downOffset;
            case AnimateToDirection.Left:
                return leftOffset;
            case AnimateToDirection.Right:
                return rightOffset;
            default:
                return Vector3.zero;
        }
    }

    private IEnumerator AnimateMenu(bool open)
    {
        if (open)
            menu.gameObject.SetActive(true);

        currentPosition = menu.transform.position;

        float elapsedTime = 0;

        Vector2 targetPostion = currentPosition;

        if (open)
            targetPostion = currentPosition * GetOffset(openDirection);
        else 
            targetPostion = currentPosition * GetOffset(closeDirection);

        while (elapsedTime < animationDuration)
        {
            float evaluationAtTime = easingCurve.Evaluate(elapsedTime / animationDuration);

            menu.transform.position = Vector2.Lerp(currentPosition, targetPostion, evaluationAtTime);

            menuCanvasGroup.alpha = open 
                ? Mathf.Lerp(0f, 1f, evaluationAtTime)
                : Mathf.Lerp(1f, 0f, evaluationAtTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        menu.transform.position = targetPostion;

        menuCanvasGroup.alpha = open ? 1 : 0;
        menuCanvasGroup.interactable = open;
        menuCanvasGroup.blocksRaycasts = open;

        if (!open)
        {
            menu.gameObject.SetActive(false);
            menu.transform.position = initialPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (menu != null)
        {
            menuRectTransform = menu.GetComponent<RectTransform>();
            menuCanvasGroup = menu.GetComponent<CanvasGroup>();
        }

        distanceToAnimate.x = Mathf.Max(0, distanceToAnimate.x);
        distanceToAnimate.y = Mathf.Max(0, distanceToAnimate.y);

        
    }
}

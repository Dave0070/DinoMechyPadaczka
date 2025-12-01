using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleFactor = 1.2f;       // Powiêkszenie
    public float scaleSpeed = 10f;         // Szybkoœæ animacji powiêkszenia
    public float spacingSize = 50f;        // Dodatkowy odstêp po lewej i prawej stronie

    private Vector3 originalScale;
    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    private Coroutine scaleCoroutine;
    private static ButtonHoverScale currentlyEnlargedButton = null;

    private LayoutElement leftSpacer;
    private LayoutElement rightSpacer;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
            layoutElement = gameObject.AddComponent<LayoutElement>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentlyEnlargedButton == this)
            return;

        // Cofnij poprzedni guzik
        if (currentlyEnlargedButton != null)
        {
            currentlyEnlargedButton.ShrinkImmediately();
            currentlyEnlargedButton.RemoveSpacers();
            currentlyEnlargedButton.ResetLayoutIgnore();
            currentlyEnlargedButton = null;
        }

        currentlyEnlargedButton = this;

        layoutElement.ignoreLayout = true; // Zignoruj uk³ad, aby nie przesuwaæ powiêkszonego przycisku

        AddSpacers(); // Dodaj spacery wokó³ przycisku
        StartScaleCoroutine(originalScale * scaleFactor); // Animuj powiêkszenie
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentlyEnlargedButton == this)
        {
            StartScaleCoroutine(originalScale); // Przywróæ oryginaln¹ skalê
            RemoveSpacers();                    // Usuñ spacery
            ResetLayoutIgnore();                // Przywróæ normalne dzia³anie uk³adu
            currentlyEnlargedButton = null;
        }
    }

    private void StartScaleCoroutine(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    private void ShrinkImmediately()
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        rectTransform.localScale = originalScale;
    }

    private IEnumerator ScaleRoutine(Vector3 targetScale)
    {
        float t = 0f;
        Vector3 startScale = rectTransform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    private void AddSpacers()
    {
        Transform parent = rectTransform.parent;

        // Lewy spacer
        GameObject left = new GameObject("LeftSpacer", typeof(RectTransform), typeof(LayoutElement));
        left.transform.SetParent(parent, false);
        leftSpacer = left.GetComponent<LayoutElement>();
        leftSpacer.preferredWidth = 0;
        leftSpacer.flexibleWidth = 0;

        // Prawy spacer
        GameObject right = new GameObject("RightSpacer", typeof(RectTransform), typeof(LayoutElement));
        right.transform.SetParent(parent, false);
        rightSpacer = right.GetComponent<LayoutElement>();
        rightSpacer.preferredWidth = 0;
        rightSpacer.flexibleWidth = 0;

        int index = rectTransform.GetSiblingIndex();

        // Ustawienie pozycji spacerów
        left.transform.SetSiblingIndex(index);      // Lewy spacer
        rectTransform.SetSiblingIndex(index + 1);  // Przyciski nadal w tym samym miejscu
        right.transform.SetSiblingIndex(index + 2); // Prawy spacer

        // Odpychanie przycisków po lewej i prawej stronie powiêkszonego przycisku
        LayoutElement[] layoutElements = parent.GetComponentsInChildren<LayoutElement>();
        foreach (var layout in layoutElements)
        {
            if (layout != this.layoutElement) // Pomijamy powiêkszony przycisk
            {
                if (layout.transform.GetSiblingIndex() > index)
                {
                    // Przyciski po prawej stronie odpychamy w prawo
                    StartCoroutine(AnimateSpacerWidth(rightSpacer, spacingSize));
                }
                else
                {
                    // Przyciski po lewej stronie odpychamy w lewo
                    StartCoroutine(AnimateSpacerWidth(leftSpacer, -spacingSize));
                }
            }
        }

        // Dodatkowe odpychanie przycisków po lewej stronie
        foreach (var layout in layoutElements)
        {
            if (layout != this.layoutElement && layout.transform.GetSiblingIndex() < index)
            {
                // Przyciski po lewej stronie odpychamy w lewo
                StartCoroutine(AnimateSpacerWidth(leftSpacer, -spacingSize));
            }
        }
    }

    private void RemoveSpacers()
    {
        if (leftSpacer != null)
        {
            Destroy(leftSpacer.gameObject);
            leftSpacer = null;
        }

        if (rightSpacer != null)
        {
            Destroy(rightSpacer.gameObject);
            rightSpacer = null;
        }
    }

    private void ResetLayoutIgnore()
    {
        if (layoutElement != null)
            layoutElement.ignoreLayout = false;
    }

    private IEnumerator AnimateSpacerWidth(LayoutElement spacer, float targetWidth)
    {
        float t = 0f;
        float start = spacer.preferredWidth;

        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            spacer.preferredWidth = Mathf.Lerp(start, targetWidth, t);
            yield return null;
        }

        spacer.preferredWidth = targetWidth;
    }
}

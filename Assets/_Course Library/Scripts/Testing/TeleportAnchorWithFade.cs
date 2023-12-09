using System.Collections;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportAnchorWithFade : TeleportationAnchor
{
    private FadeCanvas fadeCanvas = null;

    //private bool m_isHover;
    protected override void Awake()
    {
        base.Awake();
        fadeCanvas = FindObjectOfType<FadeCanvas>();
    }
  /*  protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        m_isHover = true;
    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {

        base.OnHoverExited(args);
        m_isHover = false;
    }*/
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
 
        base.OnSelectEntered(args);

        if (teleportTrigger == TeleportTrigger.OnSelectEntered)
            StartCoroutine(FadeSequence(base.OnSelectEntered, args));
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {

        if (teleportTrigger == TeleportTrigger.OnSelectExited)
        {
            StartCoroutine(FadeSequence(base.OnSelectExited, args));
        }
    }
    /*
    private IEnumerator DelayBeforeExit(SelectExitEventArgs args)
    {
        yield return new WaitForEndOfFrame();
        if ( m_isHover)
        {
            m_isHover = false;
            StartCoroutine(FadeSequence(base.OnSelectExited, args));
        }
    }*/

    protected override void OnActivated(ActivateEventArgs args)
    {

        if (teleportTrigger == TeleportTrigger.OnActivated)
            StartCoroutine(FadeSequence(base.OnActivated, args));
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {

        if (teleportTrigger == TeleportTrigger.OnDeactivated)
            StartCoroutine(FadeSequence(base.OnDeactivated, args));
    }

    private IEnumerator FadeSequence<T>(UnityAction<T> action, T args)
        where T : BaseInteractionEventArgs
    {
        fadeCanvas.QuickFadeIn();

        yield return fadeCanvas.CurrentRoutine;
        action.Invoke(args);

        fadeCanvas.QuickFadeOut();
    }
}


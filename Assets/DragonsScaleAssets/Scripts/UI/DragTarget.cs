using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class DragTarget: VisualElement
{
    public string TargetTag = "item";

    [CanBeNull] private DraggableObject _holdObject;

    public new class UxmlFactory : UxmlFactory<DragTarget, UxmlTraits>
    {
    }

    public Action<DragTarget> DropRequest;

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        private UxmlStringAttributeDescription _mTargetTag = new() { name = "target-tag" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((DragTarget)ve).TargetTag = _mTargetTag.GetValueFromBag(bag, cc);
        }
    }

    public DragTarget()
    {
        RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    public Action<DragTarget> HeldObjectChange;

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (_holdObject != null)
        {
            RemoveHold();
            HeldObjectChange?.Invoke(this);
        }
        DropRequest?.Invoke(this);
    }

    public void Hold(DraggableObject draggableObject)
    {
        RemoveHold();
        _holdObject = draggableObject;
        HeldObjectChange?.Invoke(this);

        VisualElement previewImageElement = GetPreviewImageElement();
        if (previewImageElement != null)
        {
            Debug.Log("Adding preview element");
            StyleBackground backgroundImage = draggableObject.GetPreviewElement()?.style?.backgroundImage ?? null;
            previewImageElement.style.backgroundImage = backgroundImage;
        }
        else
        {
            Debug.LogWarning("DragTarget does not have an image container named \"image\"");
        }
    }

    [CanBeNull]
    private VisualElement GetPreviewImageElement()
    {
        return this.Q<VisualElement>("image");
    }

    public void RemoveHold()
    {
        VisualElement previewImage = GetPreviewImageElement();
        if (previewImage == null)
        {
            Debug.LogWarning($"DragTarget {name} does not have an image container named \"image\"");
            return;
        }

        _holdObject = null;
        previewImage.style.backgroundImage = null;
    }

    public DraggableObject GetHeldDraggable()
    {
        return _holdObject;
    }
}

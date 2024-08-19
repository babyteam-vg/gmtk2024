using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class DragTarget: VisualElement
{
    public string TargetTag = "item";

    [CanBeNull] private DraggableObject _holdObject = null;

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

    public void SetDraggableView(DraggableObject draggableObject)
    {
        Add(draggableObject);
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (_holdObject != null)
        {
            RemoveHold();
        }
        DropRequest?.Invoke(this);
    }

    public void Hold(DraggableObject draggableObject)
    {
        RemoveHold();
        _holdObject = draggableObject;

        VisualElement previewImageElement = GetPreviewImageElement();
        if (previewImageElement != null)
        {
            StyleBackground? previewImage =  draggableObject.GetPreviewImage();
            if (previewImage == null)
            {
                Debug.LogWarning("DraggableObject does not have a preview image");
                return;
            }
            previewImageElement.style.backgroundImage = (StyleBackground)previewImage;
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

    public DraggableObject GetHoldDraggable()
    {
        return _holdObject;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// An element in the UI with an inner element that will be dragged when clicked if registered by a DragController. 
/// </summary>
public class DraggableObject : VisualElement
{
    [CanBeNull] private string CompatibleTargetTags = null;

    public new class UxmlFactory : UxmlFactory<DraggableObject, UxmlTraits>
    {
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        private UxmlStringAttributeDescription _mCompatibleTargetTag = new() { name = "compatible-target-tag" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((DraggableObject)ve).CompatibleTargetTags = _mCompatibleTargetTag.GetValueFromBag(bag, cc);
        }
    }

    public Action<DraggableObject> DragRequest;
    private VisualElement _preview;

    public DraggableObject()
    {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    public void OnMouseDown(MouseDownEvent evt)
    {
        DragRequest?.Invoke(this);
    }

    [CanBeNull]
    public VisualElement GetPreviewElement()
    {
        return _preview;
    }
    
    public VisualElement ResetPreview()
    {
        Add(_preview);
        return _preview;
    }

    public void SetPreview(VisualElement element)
    {
        Clear();
        _preview = element;
        ResetPreview();
    }

    public void SetCompatibleTargetTags(string compatibleTargetTag)
    {
        CompatibleTargetTags = compatibleTargetTag;
    }

    public bool IsCompatibleWith(string objTargetTag)
    {
        // left as a function knowing the current simplicity to expand the logic later if needed
        return CompatibleTargetTags == objTargetTag;
    }

    /// <summary>
    /// Gets the image that will be used to show this object is being used in a drag target.
    /// Currently it will pick the background image of a child element with the name "image"
    /// in the preview.
    /// </summary>
    /// <returns>
    /// The image that will be used to show this object is being used in a drag target.
    /// </returns>
    public StyleBackground? GetPreviewImage()
    {
        VisualElement previewImage = _preview.Q<VisualElement>("image");
        return previewImage?.style?.backgroundImage;
    }
}
using System;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class DraggableObject : VisualElement
{
    public new class UxmlFactory : UxmlFactory<DraggableObject>
    {
    }

    public Action DragRequest { get; set; }

    public DraggableObject()
    {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    public void OnMouseDown(MouseDownEvent evt)
    {
        DragRequest?.Invoke();
    }

    public VisualElement GetPreviewElement()
    {
        VisualElement preview = this.Q<VisualElement>("preview");
        if (preview != null) return preview;

        preview = new VisualElement
        {
            name = "preview",
            style =
            {
                backgroundColor = new Color(0, 0, 0, 140)
            }
        };

        Add(preview);
        return preview;
    }
}
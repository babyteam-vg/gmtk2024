using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragController : MonoBehaviour
{
    [SerializeField] public int cursorSize = 92;

    private UIDocument _document;
    public DraggableObject CurrentDraggedObject { get; private set; }

    public List<string> targetIds = new()
    {
        "CraftTL",
        "CraftTC",
        "CraftTR",

        "CraftML",
        "CraftMC",
        "CraftMR",

        "CraftBL",
        "CraftBC",
        "CraftBR",
    };

    private readonly List<DraggableObject> _draggableObjects = new();

    private VisualElement _cursorElement;

    public void Start()
    {
        _document = GetComponent<UIDocument>();

        SetUpCursorElement();

        _document.rootVisualElement.RegisterCallback<MouseUpEvent>(_ => StopDrag());
        _document.rootVisualElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    private void OnDraggableClick(DraggableObject draggableObject)
    {
        
    }

    private void SetUpCursorElement()
    {
        _cursorElement = new VisualElement
        {
            style =
            {
                position = Position.Absolute,
                left = 0,
                top = 0,
                justifyContent = Justify.Center,
                alignItems = Align.Stretch,
                width = cursorSize,
                height = cursorSize
            }
        };

        _document.rootVisualElement.Add(_cursorElement);
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        _cursorElement.style.left = evt.localMousePosition.x;
        _cursorElement.style.top = evt.localMousePosition.y;
    }

    public bool AddItemAt(string containerId)
    {
        VisualElement container = _document.rootVisualElement.Q<VisualElement>(containerId);
        if (container == null)
        {
            return false;
        }

        DraggableObject draggableObject = new();

        _draggableObjects.Add(draggableObject);
        container.Add(draggableObject);

        draggableObject.DragRequest += () =>
        {
            StartDrag(draggableObject);
        };

        return true;
    }

    public void StartDrag(DraggableObject draggableObject)
    {
        CurrentDraggedObject = draggableObject;
        _cursorElement.Add(
            draggableObject.GetPreviewElement()
        );
    }

    public void StopDrag()
    {
        _cursorElement.Clear();
        CurrentDraggedObject = null;
    }
}
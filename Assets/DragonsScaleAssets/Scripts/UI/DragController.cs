using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DragController : MonoBehaviour
{
    [SerializeField] public int cursorSize = 136;
    [SerializeField] public Vector2 cursorOffset = new(0, 0);

    [HideInInspector] public UIDocument document;
    public DraggableObject CurrentDraggedObject { get; private set; }

    private readonly List<DraggableObject> _registeredDraggables = new();
    private List<DragTarget> _dragTargets = new();

    private VisualElement _cursorElement;

    public void Start()
    {
        document = GetComponent<UIDocument>();

        SetUpCursorElement();

        document.rootVisualElement.RegisterCallback<MouseUpEvent>(_ => StopDrag());
        document.rootVisualElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);

        _dragTargets = document.rootVisualElement.Query<DragTarget>().ToList();

        foreach (DragTarget target in _dragTargets)
        {
            target.DropRequest += OnDropRequest;
        }
    }

    /// <summary>
    /// Handles when a drag target receives a drop request.
    /// </summary>
    /// <param name="target"></param>
    private void OnDropRequest(DragTarget target)
    {
       
        if (CurrentDraggedObject == null)
        {
            return;
        }

        if (!CurrentDraggedObject.IsCompatibleWith(target.TargetTag))
        {
            return;
        }
        target.Hold(CurrentDraggedObject);
        StopDrag();
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
                height = cursorSize,
            },
            focusable = false,
            pickingMode = PickingMode.Ignore,
        };

        document.rootVisualElement.Add(_cursorElement);
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        _cursorElement.style.left = evt.localMousePosition.x + cursorOffset.x;
        _cursorElement.style.top = evt.localMousePosition.y + cursorOffset.y;
    }

    /// <summary>
    /// Adds a draggable object to the specified container. This draggable object will be tracked by the drag controller
    /// </summary>
    /// <param name="draggableObject">
    /// The draggable object to add
    /// </param>
    /// <returns></returns>
    public bool RegisterElement(DraggableObject draggableObject)
    {
        _registeredDraggables.Add(draggableObject);

        if (CurrentDraggedObject != null)
        {
            return false;
        }

        draggableObject.focusable = true;
        draggableObject.DragRequest += StartDrag;

        return true;
    }

    /// <summary>
    /// Stops the preview drag using the StopDrag method, then starts a drag picking up the preview element defined by
    /// the draggable object.
    /// </summary>
    /// <param name="draggableObject">
    /// The draggable object to start dragging
    /// </param>
    public void StartDrag(DraggableObject draggableObject)
    {
        StopDrag();
        CurrentDraggedObject = draggableObject;

        VisualElement preview = draggableObject.GetPreviewElement();
        if (preview != null)
        {
            preview.pickingMode = PickingMode.Ignore;
        }

        _cursorElement.Add(preview);
    }

    public void StopDrag()
    {
        VisualElement preview = CurrentDraggedObject?.ResetPreview();
        if (preview != null)
        {
            preview.pickingMode = PickingMode.Position;
        }

        CurrentDraggedObject = null;
    }

    private void OnDestroy()
    {
        foreach (DragTarget target in _dragTargets)
        {
            target.DropRequest -= OnDropRequest;
        }

        foreach (DraggableObject draggableObject in _registeredDraggables)
        {
            draggableObject.DragRequest -= StartDrag;
        }
    }
}
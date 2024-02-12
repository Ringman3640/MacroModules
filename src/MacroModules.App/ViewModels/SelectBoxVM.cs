using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace MacroModules.App.ViewModels;

public partial class SelectBoxVM : MouseAwareVM
{
    [ObservableProperty]
    private Rect _selectBoxRegion;

    [ObservableProperty]
    private Visibility _selectBoxVisibility = Visibility.Hidden;

    public List<BoardElementVM> SelectedElementsList
    {
        get { return selectedElements.ToList(); }
    }

    public SelectBoxVM(ModuleBoardVM moduleBoard)
    {
        this.moduleBoard = moduleBoard;
    }

    public void LockSelectBoxPivotToMouse()
    {
        selectBoxPivot = MousePosition;
    }

    public void StartSelectBox()
    {
        SelectBoxVisibility = Visibility.Visible;
        MoveSelectBoxWithMouse();
    }

    public void MoveSelectBoxWithMouse()
    {
        SelectBoxRegion = new(MousePosition, selectBoxPivot);
    }

    public void ConfirmSelectBox()
    {
        foreach (var element in moduleBoard.Elements)
        {
            double absolutePosX = (element.VisualPosition.X * moduleBoard.BoardScale) + moduleBoard.BoardPosition.X;
            double absolutePosY = (element.VisualPosition.Y * moduleBoard.BoardScale) + moduleBoard.BoardPosition.Y;
            double scaledWidth = element.Dimensions.Width * moduleBoard.BoardScale;
            double scaledHeight = element.Dimensions.Height * moduleBoard.BoardScale;

            Rect elementBounds = new(absolutePosX, absolutePosY, scaledWidth, scaledHeight);
            if (elementBounds.IntersectsWith(SelectBoxRegion))
            {
                selectedElements.Add(element);
            }
        }

        SelectBoxVisibility = Visibility.Hidden;
    }

    public void Select(BoardElementVM element)
    {
        selectedElements.Add(element);
    }

    public void Unselect(BoardElementVM element)
    {
        selectedElements.Remove(element);
    }

    public void UnselectAll()
    {
        selectedElements.Clear();
    }

    public bool IsSelected(BoardElementVM element)
    {
        return selectedElements.Contains(element);
    }

    public void LockSelectedToMouse()
    {
        foreach (var element in selectedElements)
        {
            element.LockMouseOffset();
        }
    }

    public void MoveSelectedWithMouse()
    {
        foreach (var element in selectedElements)
        {
            element.MoveWithMouse();
        }
    }

    public void SetActualPositionOfSelected()
    {
        foreach (var element in selectedElements)
        {
            element.ActualPosition = element.VisualPosition;
        }
    }

    private ModuleBoardVM moduleBoard;
    private HashSet<BoardElementVM> selectedElements = new();
    private Point selectBoxPivot;
}

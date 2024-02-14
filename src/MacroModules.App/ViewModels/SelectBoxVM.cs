using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace MacroModules.App.ViewModels;

public partial class SelectBoxVM : MouseAwareVM
{
    [ObservableProperty]
    private Rect _selectBoxRegion;

    [ObservableProperty]
    private Visibility _selectBoxVisibility = Visibility.Hidden;

    [ObservableProperty]
    private ObservableCollection<BoardElementVM> _selectedElements = new();

    public List<BoardElementVM> SelectedElementsList
    {
        get { return SelectedElements.ToList(); }
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
                SelectedElements.Add(element);
            }
        }

        SelectBoxVisibility = Visibility.Hidden;
    }

    public void Select(BoardElementVM element)
    {
        SelectedElements.Add(element);
    }

    public void Unselect(BoardElementVM element)
    {
        SelectedElements.Remove(element);
    }

    public void UnselectAll()
    {
        SelectedElements.Clear();
    }

    public bool IsSelected(BoardElementVM element)
    {
        return SelectedElements.Contains(element);
    }

    public void LockSelectedToMouse()
    {
        foreach (var element in SelectedElements)
        {
            element.LockMouseOffset();
        }
    }

    public void MoveSelectedWithMouse()
    {
        foreach (var element in SelectedElements)
        {
            element.MoveWithMouse();
        }
    }

    public void SetActualPositionOfSelected()
    {
        foreach (var element in SelectedElements)
        {
            element.ActualPosition = element.VisualPosition;
        }
    }

    private ModuleBoardVM moduleBoard;
    private Point selectBoxPivot;
}

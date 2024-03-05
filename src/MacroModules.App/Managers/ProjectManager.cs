using CommunityToolkit.Mvvm.Messaging;
using MacroModules.App.Messages;
using MacroModules.App.ViewModels;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.BoardElements;
using MacroModules.Model.Modules;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace MacroModules.App.Managers;

public class ProjectManager
{
    public static string DefaultProjectName { get; } = "Untitled Macro";

    public WorkspaceVM Workspace { get; private set; }

    public string ProjectName { get; set; } = DefaultProjectName;

    public string? ProjectFilePath { get; set; } = null;

    public bool SaveNeeded { get; set; } = true;

    public ProjectManager(WorkspaceVM workspace)
    {
        Workspace = workspace;

        WeakReferenceMessenger.Default.Register<CommitAddedMessage>(this, (recipient, message) =>
        {
            SaveNeeded = true;
        });
    }

    public bool Save()
    {
        if (ProjectFilePath == null)
        {
            return PerformSaveAs();
        }
        return SaveProject();
    }

    public bool PerformSaveAs()
    {
        SaveFileDialog saveDialog = new();
        saveDialog.FileName = ProjectName;
        saveDialog.DefaultExt = ".mmod";
        saveDialog.Filter = "MacroModules project files (.mmod)|*.mmod";

        if (saveDialog.ShowDialog() != true)
        {
            return false;
        }

        ProjectFilePath = saveDialog.FileName;
        SaveNeeded = true;
        return SaveProject();
    }

    public void OpenNew()
    {
        if (SaveNeeded)
        {
            switch (ShowUnsavedChangesMessage())
            {
                case MessageBoxResult.Yes:
                    Save();
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    return;
            }
        }

        ClearCurrentWorkspace();
        ClearCurrentProject();
    }

    public bool PerformOpenExisting()
    {
        if (SaveNeeded)
        {
            switch (ShowUnsavedChangesMessage())
            {
                case MessageBoxResult.Yes:
                    Save();
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    return false;
            }
        }

        OpenFileDialog openDialog = new();
        openDialog.Filter = "MacroModules project files (.mmod)|*.mmod";

        if (openDialog.ShowDialog() != true)
        {
            return false;
        }

        ProjectFilePath = openDialog.FileName;
        return LoadProject();
    }

    private void ClearCurrentWorkspace()
    {
        Workspace.Executor.Terminate();
        Workspace.PropertiesPanel.DisplayedModule = null;
        Workspace.CommitManager.ClearCommits();
        Workspace.ModuleBoard.Reset();
    }

    private void ClearCurrentProject()
    {
        ProjectName = DefaultProjectName;
        ProjectFilePath = null;
        SaveNeeded = false;
    }

    private bool SaveProject()
    {
        if (ProjectFilePath == null)
        {
            return false;
        }
        if (!SaveNeeded)
        {
            return true;
        }

        List<BoardElement> elements = [];
        foreach (var element in Workspace.ModuleBoard.Elements)
        {
            elements.Add(element.ElementData);
        }
        ProjectData data = new(elements, ProjectName);

        FileStream? projectFile = null;
        try
        {
            projectFile = new(ProjectFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            projectFile.SetLength(0);
            JsonSerializer.Serialize(projectFile, data);
        }
        catch
        {
            return false;
        }
        finally
        {
            projectFile?.Close();
        }

        SaveNeeded = false;
        return true;
    }

    private bool LoadProject()
    {
        if (ProjectFilePath == null)
        {
            return false;
        }

        FileStream? projectFile = null;
        ProjectData? data = null;
        try
        {
            projectFile = new(ProjectFilePath, FileMode.Open, FileAccess.Read);
            data = JsonSerializer.Deserialize<ProjectData>(projectFile)!;
        }
        catch
        {
            return false;
        }
        finally
        {
            projectFile?.Close();
        }

        ProjectName = data.Projectname;
        List<BoardElementVM> elementVMs = [];
        foreach (var elementData in data.Elements)
        {
            if (elementData is Module module)
            {
                ModuleVM moduleVM = ModuleVMFactory.Create(module);
                moduleVM.SetStartingPosition(moduleVM.ActualPosition);
                elementVMs.Add(moduleVM);
            }
            else
            {
                // TODO: create label element VM
            }
        }

        ClearCurrentWorkspace();
        Workspace.ModuleBoard.AddElements(elementVMs);
        Workspace.CommitManager.ClearCommits();
        SaveNeeded = false;
        return true;
    }

    private MessageBoxResult ShowUnsavedChangesMessage()
    {
        return MessageBox.Show(
            messageBoxText: "The current project has unsaved changes. Would you like to save them?",
            caption: "Unsaved Changes",
            button: MessageBoxButton.YesNoCancel,
            icon: MessageBoxImage.Warning);
    }
}

using MacroModules.App.ViewModels;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.BoardElements;
using MacroModules.Model.Modules;
using System.IO;
using System.Text.Json;

namespace MacroModules.App.Managers;

public class ProjectManager
{
    public WorkspaceVM Workspace { get; private set; }

    public string ProjectName { get; set; } = "Untitled";

    public string? ProjectFilePath { get; set; } = null;

    public bool SaveNeeded { get; set; } = true;

    public ProjectManager(WorkspaceVM workspace)
    {
        Workspace = workspace;
    }

    public bool Save()
    {
        if (ProjectFilePath == null)
        {
            return false;
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
            projectFile = new(ProjectFilePath, FileMode.Create, FileAccess.Write);
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

    public bool Load()
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

        Workspace.ModuleBoard.AddElements(elementVMs);
        Workspace.CommitManager.ClearCommits();
        return true;
    }
}

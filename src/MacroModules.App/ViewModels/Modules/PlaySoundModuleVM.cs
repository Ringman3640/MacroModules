using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class PlaySoundModuleVM : ModuleVM
{
    public string SoundFile
    {
        get { return castedModuleData.SoundFile; }
        set
        {
            if (castedModuleData.SoundFile != value)
            {
                FullCommitPropertyChange(castedModuleData.SoundFile, value);
                castedModuleData.SoundFile = value;
                OnPropertyChanged();
            }
        }
    }

    public double Volume
    {
        get { return castedModuleData.Volume; }
        set
        {
            if (castedModuleData.Volume != value)
            {
                FullCommitPropertyChange(castedModuleData.Volume, value);
                castedModuleData.Volume = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.PlaySound;

    public override string ElementTitle { get; } = "Play Sound";

    public PlaySoundModuleVM() : base()
    {
        castedModuleData = (PlaySoundModule)ModuleData;
    }

    public PlaySoundModuleVM(PlaySoundModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly PlaySoundModule castedModuleData;
}

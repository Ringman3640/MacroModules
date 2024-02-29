using System.Windows;

namespace MacroModules.AppControls.PropertyEditors
{
    public partial class BoolPropertyEditor : BasePropertyEditor
    {
        public bool BoolProperty
        {
            get { return (bool)GetValue(BoolPropertyProperty); }
            set { SetValue(BoolPropertyProperty, value); }
        }
        public static readonly DependencyProperty BoolPropertyProperty = DependencyProperty.Register(
            name: nameof(BoolProperty),
            propertyType: typeof(bool),
            ownerType: typeof(BoolPropertyEditor),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: false,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsSet
        {
            get { return BoolProperty; }
            set
            {
                if (BoolProperty != value)
                {
                    BoolProperty = value;
                    OnPropertyChanged();
                }
            }
        }

        public BoolPropertyEditor() : base()
        {
            InitializeComponent();
        }
    }
}

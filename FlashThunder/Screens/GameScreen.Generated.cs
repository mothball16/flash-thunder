//Code for GameScreen
using GumRuntime;
using MonoGameGum.GueDeriving;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace FlashThunder.Screens;
partial class GameScreen : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("GameScreen");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new GameScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(GameScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("GameScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime UnitCount { get; protected set; }
    public TextRuntime TurnOrder { get; protected set; }
    public ContainerRuntime UnitInformation { get; protected set; }
    public TextRuntime UnitName { get; protected set; }
    public ContainerRuntime HealthDisplay { get; protected set; }
    public ColoredRectangleRuntime BaseBar { get; protected set; }
    public ColoredRectangleRuntime HealthBar { get; protected set; }
    public TextRuntime HealthText { get; protected set; }

    public GameScreen(InteractiveGue visual) : base(visual) { }
    public GameScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        UnitCount = this.Visual?.GetGraphicalUiElementByName("UnitCount") as TextRuntime;
        TurnOrder = this.Visual?.GetGraphicalUiElementByName("TurnOrder") as TextRuntime;
        UnitInformation = this.Visual?.GetGraphicalUiElementByName("UnitInformation") as ContainerRuntime;
        UnitName = this.Visual?.GetGraphicalUiElementByName("UnitName") as TextRuntime;
        HealthDisplay = this.Visual?.GetGraphicalUiElementByName("HealthDisplay") as ContainerRuntime;
        BaseBar = this.Visual?.GetGraphicalUiElementByName("BaseBar") as ColoredRectangleRuntime;
        HealthBar = this.Visual?.GetGraphicalUiElementByName("HealthBar") as ColoredRectangleRuntime;
        HealthText = this.Visual?.GetGraphicalUiElementByName("HealthText") as TextRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}

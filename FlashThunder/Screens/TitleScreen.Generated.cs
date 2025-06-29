//Code for TitleScreen
using GumRuntime;
using MonoGameGum.GueDeriving;
using FlashThunder.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace FlashThunder.Screens;
partial class TitleScreen : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("TitleScreen");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new TitleScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(TitleScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("TitleScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime TitleText { get; protected set; }
    public StackPanel LeftPanel { get; protected set; }
    public ButtonStandard PlayButton { get; protected set; }
    public ButtonStandard ShopButton { get; protected set; }
    public SpriteRuntime CautionLineTop { get; protected set; }
    public SpriteRuntime CautionLineBottom { get; protected set; }
    public ContainerRuntime Border { get; protected set; }

    public TitleScreen(InteractiveGue visual) : base(visual) { }
    public TitleScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        TitleText = this.Visual?.GetGraphicalUiElementByName("TitleText") as TextRuntime;
        LeftPanel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.GetFrameworkElementByName<StackPanel>(this.Visual,"LeftPanel");
        PlayButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.GetFrameworkElementByName<ButtonStandard>(this.Visual,"PlayButton");
        ShopButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.GetFrameworkElementByName<ButtonStandard>(this.Visual,"ShopButton");
        CautionLineTop = this.Visual?.GetGraphicalUiElementByName("CautionLineTop") as SpriteRuntime;
        CautionLineBottom = this.Visual?.GetGraphicalUiElementByName("CautionLineBottom") as SpriteRuntime;
        Border = this.Visual?.GetGraphicalUiElementByName("Border") as ContainerRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}

//Code for AbilityLabelComponent (Container)
using GumRuntime;
using MonoGameGum.GueDeriving;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace FlashThunder.Components;
partial class AbilityLabelComponent : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("AbilityLabelComponent");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new AbilityLabelComponent(visual);
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(AbilityLabelComponent)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("AbilityLabelComponent", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public SpriteRuntime Icon { get; protected set; }
    public SpriteRuntime Selected { get; protected set; }
    public SpriteRuntime Charge { get; protected set; }
    public TextRuntime HotkeyText { get; protected set; }

    public AbilityLabelComponent(InteractiveGue visual) : base(visual) { }
    public AbilityLabelComponent()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Icon = this.Visual?.GetGraphicalUiElementByName("Icon") as SpriteRuntime;
        Selected = this.Visual?.GetGraphicalUiElementByName("Selected") as SpriteRuntime;
        Charge = this.Visual?.GetGraphicalUiElementByName("Charge") as SpriteRuntime;
        HotkeyText = this.Visual?.GetGraphicalUiElementByName("HotkeyText") as TextRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}

//Code for PopupComponent1 (Container)
using GumRuntime;
using MonoGameGum.GueDeriving;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace FlashThunder.Components;
partial class PopupComponent1 : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("PopupComponent1");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new PopupComponent1(visual);
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(PopupComponent1)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("PopupComponent1", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ColoredRectangleRuntime LeftBar { get; protected set; }
    public TextRuntime Message { get; protected set; }

    public PopupComponent1(InteractiveGue visual) : base(visual) { }
    public PopupComponent1()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        LeftBar = this.Visual?.GetGraphicalUiElementByName("LeftBar") as ColoredRectangleRuntime;
        Message = this.Visual?.GetGraphicalUiElementByName("Message") as TextRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}

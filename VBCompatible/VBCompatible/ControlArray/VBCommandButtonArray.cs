using System;
using System.ComponentModel;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBCommandButton))]
    public class VBCommandButtonArray : VBControllArray<VBCommandButton>
    {
        public VBCommandButtonArray() { }

        public VBCommandButtonArray(IContainer Container) : base(Container) { }
    }
}

using System.ComponentModel;

namespace VBCompatible
{
    internal class SR
    {
        public class VBCustomAttribute : CategoryAttribute
        {
            public VBCustomAttribute() : base("カスタム") { }
        }

        public class VB60Attribute : CategoryAttribute
        {
            public VB60Attribute() : base("VB6.0互換") { }
        }

        public class AnmationAttribute : CategoryAttribute
        {
            public AnmationAttribute() : base("Anmation") { }
        }
    }
}

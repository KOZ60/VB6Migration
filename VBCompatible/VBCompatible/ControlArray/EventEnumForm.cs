namespace VBCompatible.ControlArray
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.ComponentModel;
    using System.Windows.Forms;

    public partial class EventEnumForm : VBForm
    {
        readonly List<Assembly> Assemblies = new List<Assembly>();
        readonly HashSet<string> controlEvents = new HashSet<string>();
        public EventEnumForm() {
            InitializeComponent();
            Assemblies.Add(Assembly.GetAssembly(typeof(Control)));
            Assemblies.Add(Assembly.GetAssembly(typeof(VBTextBox)));
            cboAssembly.Items.AddRange(Assemblies.OrderBy(i => i.FullName).ToArray());
            foreach (EventInfo info in typeof(Control).GetEvents()) {
                controlEvents.Add(info.Name);
            }
        }

        public void AddAssembly(Assembly asm) {
            if (!Assemblies.Contains(asm)) {
                Assemblies.Add(asm);
                cboAssembly.Items.Clear();
                cboAssembly.Items.AddRange(Assemblies.OrderBy(i => i.FullName).ToArray());
                cboControl.Items.Clear();
            }
        }

        private void cboAssembly_SelectionChangeCommitted(object sender, EventArgs e) {
            cboControl.Items.Clear();
            Assembly assembly = cboAssembly.SelectedItem as Assembly;
            if (assembly != null) {
                var lst = new List<Type>();
                foreach (Type type in assembly.GetTypes()) {
                    if (type == typeof(Control) || 
                        ( type.IsSubclassOf(typeof(Control)) && !type.IsSubclassOf(typeof(Form)))) {
                        lst.Add(type);
                    }
                }
                cboControl.Items.AddRange(lst.OrderBy(i => i.FullName).ToArray());
            }
        }

        private void cboControl_SelectionChangeCommitted(object sender, EventArgs e) {
            Type type = cboControl.SelectedItem as Type;
            if (type != null) {
                var sb = new StringBuilder();
                List<EventInfo> lst = new List<EventInfo>();
                lst.AddRange(EnumEvents(type));
                lst = lst.OrderBy((i) => i.Name).ToList();
                txtEvents.Text = CreateControlArray(type, lst);
            }
        }

        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private List<EventInfo> EnumEvents(Type type) {
            List<EventInfo> events = new List<EventInfo>();
            events.AddRange(type.GetEvents(flags));
            if (type.BaseType != null && type.BaseType != typeof(Control)) {
                events.AddRange(EnumEvents(type.BaseType));
            }
            return events;
        }

        private string CreateControlArray(Type type, List<EventInfo> eventList) {
            var s0 = type.Name;
            var s1 = new StringBuilder();
            var s2 = new StringBuilder();
            var s3 = new StringBuilder();
            var s4 = new StringBuilder();
            foreach (var item in eventList) {
                s1.AppendLine("            o.{0} += On{0};", item.Name);
                s2.AppendLine("            o.{0} -= On{0};", item.Name);
                s3.AppendLine("        private {1} On{0} => new {1}((s, e) => {0}?.Invoke(s, e));",
                                                    item.Name, item.EventHandlerType.Name);
                var strNew = string.Empty;
                if (controlEvents.Contains(item.Name)) {
                    strNew = "new ";
                }
                s4.AppendLine("        public {2}event {1} {0};",
                                                    item.Name, item.EventHandlerType.Name, strNew);
            }
            var result = template.Replace("%0%", s0);
            result = result.Replace("%1%", s1.ToString().TrimEnd('\r', '\n'));
            result = result.Replace("%2%", s2.ToString().TrimEnd('\r', '\n'));
            result = result.Replace("%3%", s3.ToString().TrimEnd('\r', '\n'));
            result = result.Replace("%4%", s4.ToString().TrimEnd('\r', '\n'));
            return result;
        }

        const string template = @"    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty(""Index"", typeof(%0%))]
    public class %0%Array : VBControllArray<%0%> {

        public %0%Array() { }

        public %0%Array(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(%0% o) {
%1%
        }

        protected override void HookDownEvents(%0% o) {
%2%
        }

%3%

%4%
    }";
    }
}

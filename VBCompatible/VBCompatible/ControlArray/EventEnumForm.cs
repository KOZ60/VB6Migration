using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace VBCompatible.ControlArray
{
    public partial class EventEnumForm : VBForm
    {
        readonly List<Assembly> Assemblies = new List<Assembly>();
        readonly HashSet<Tuple<string, string>> ControlEvents = new HashSet<Tuple<string, string>>();

        public EventEnumForm() {
            InitializeComponent();
            Assemblies.Add(Assembly.GetAssembly(typeof(Control)));
            Assemblies.Add(Assembly.GetAssembly(typeof(VBTextBox)));
            cboAssembly.Items.AddRange(Assemblies.OrderBy(i => i.FullName).ToArray());
            foreach (EventInfo info in typeof(Control).GetEvents()) {
                var item = new Tuple<string, string>(info.EventHandlerType.ToString(), info.Name);
                ControlEvents.Add(item);
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
                lst.AddRange(type.GetEvents().OrderBy(i => i.Name).ToArray());
                foreach (EventInfo info in lst) {
                    var item = new Tuple<string, string>(info.EventHandlerType.ToString(), info.Name);
                    if (type == typeof(Control) || !ControlEvents.Contains(item)) {
                        sb.Append(info.EventHandlerType.ToString());
                        sb.Append('\t');
                        sb.AppendLine(info.Name);
                    }
                }
                txtEvents.Text = sb.ToString();
            }
        }

    }
}

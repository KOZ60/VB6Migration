using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    public abstract class BaseControlArray : Component, ISupportInitialize
    {
        protected Hashtable indices;
        protected Hashtable controls;
        protected Hashtable controlAddedAtDesignTime;
        protected IContainer components;
        protected bool fIsEndInitCalled;

        protected BaseControlArray() {
            indices = new Hashtable();
            controls = new Hashtable();
            controlAddedAtDesignTime = new Hashtable();
        }

        protected BaseControlArray(IContainer Container) : this() {
            Container.Add(this);
            components = Container;
        }

        protected bool BaseCanExtend(object target) {
            bool flag;
            Information.Err().Clear();
            flag = (!this.indices.ContainsKey(RuntimeHelpers.GetObjectValue(target)) ? false : true);
            return flag;
        }

        protected int BaseGetIndex(object ctl) {
            int num = 0;
            RuntimeHelpers.GetObjectValue(this.indices[RuntimeHelpers.GetObjectValue(ctl)]);
            if (this.indices.ContainsKey(RuntimeHelpers.GetObjectValue(ctl))) {
                num = Conversions.ToInteger(this.indices[RuntimeHelpers.GetObjectValue(ctl)]);
            } else {
                try {
                    ISite site = this.Site;
                    if (site != null && site.DesignMode) {
                        Information.Err().Clear();
                        num = -1;
                        return num;
                    }
                } catch (Exception exception) {
                    ProjectData.SetProjectError(exception);
                    ProjectData.ClearProjectError();
                }
                VB6Errors.RaiseError(343, VB6Errors.ERR_CArrObjectNotArray);
            }
            return num;
        }

        protected object BaseGetItem(int Index) {
            object obj;
            object objectValue = RuntimeHelpers.GetObjectValue(this.controls[Index]);
            if (objectValue != null) {
                obj = objectValue;
            } else {
                VB6Errors.RaiseError(340, string.Concat(VB6Errors.ERR_CArrIllegalIndex1, Index));
                obj = null;
            }
            return obj;
        }

        protected void BaseResetIndex(object o) { }

        protected void BaseSetIndex(object ctl, int Index, bool fIsDynamic = false) {
            if (Index < 0) {
                VB6Errors.RaiseError(341, VB6Errors.ERR_CArrCantAlloc);
            }
            if (!this.CanCallSetIndex(fIsDynamic)) {
                VB6Errors.RaiseError(5, VB6Errors.ERR_IllegalFuncCall);
            }
            if (this.controls[Index] != null) {
                VB6Errors.RaiseError(360, string.Concat(VB6Errors.CArrDesign_ObjectAlreadyLoaded1, Index));
            }
            if (this.indices[RuntimeHelpers.GetObjectValue(ctl)] != null) {
                this.controls.Remove(RuntimeHelpers.GetObjectValue(this.indices[RuntimeHelpers.GetObjectValue(ctl)]));
                this.indices.Remove(RuntimeHelpers.GetObjectValue(ctl));
            }
            this.indices[RuntimeHelpers.GetObjectValue(ctl)] = Index;
            this.controls[Index] = RuntimeHelpers.GetObjectValue(ctl);
            if (!fIsDynamic) {
                this.controlAddedAtDesignTime[RuntimeHelpers.GetObjectValue(ctl)] = true;
            }
            this.HookUpControlEvents(RuntimeHelpers.GetObjectValue(ctl));
            TypeDescriptor.Refresh(RuntimeHelpers.GetObjectValue(ctl));
        }

        protected bool BaseShouldSerializeIndex(object o) {
            return this.indices.ContainsKey(RuntimeHelpers.GetObjectValue(o));
        }

        public void BeginInit() {
        }

        private bool CanCallSetIndex(bool fIsDynamic) {
            bool flag;
            flag = (fIsDynamic || !this.fIsEndInitCalled ? true : false);
            return flag;
        }

        private object CloneControl(object ctl, int NewIndex) {
            bool flag = ctl is AxHost;
            object objectValue = RuntimeHelpers.GetObjectValue(Activator.CreateInstance(ctl.GetType()));
            AxHost axHost = null;
            if (flag) {
                axHost = (AxHost)objectValue;
                axHost.BeginInit();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                binaryFormatter.Serialize(memoryStream, ((AxHost)ctl).OcxState);
                memoryStream.Position = (long)0;
                axHost.OcxState = (AxHost.State)binaryFormatter.Deserialize(memoryStream);
            }
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(RuntimeHelpers.GetObjectValue(ctl));
            int count = checked(properties.Count - 1);
            for (int i = 0; i <= count; i = checked(i + 1)) {
                PropertyDescriptor item = properties[i];
                if (item.SerializationVisibility == DesignerSerializationVisibility.Visible) {
                    bool flag1 = true;
                    try {
                        if (!item.ShouldSerializeValue(RuntimeHelpers.GetObjectValue(ctl))) {
                            flag1 = false;
                        }
                    } catch (Exception exception) {
                        ProjectData.SetProjectError(exception);
                        flag1 = false;
                        Information.Err().Clear();
                        ProjectData.ClearProjectError();
                    }
                    if (flag1) {
                        string name = item.Name;
                        if (Operators.CompareString(name, "TabIndex", false) == 0) {
                            flag1 = false;
                        } else if (Operators.CompareString(name, "Index", false) == 0) {
                            flag1 = false;
                        } else if (Operators.CompareString(name, "MdiList", false) == 0 || Operators.CompareString(name, "intcut", false) == 0) {
                            if (ctl is MenuItem) {
                                flag1 = false;
                            }
                        }
                    }
                    if (flag1) {
                        try {
                            object obj = RuntimeHelpers.GetObjectValue(item.GetValue(RuntimeHelpers.GetObjectValue(ctl)));
                            item.SetValue(RuntimeHelpers.GetObjectValue(objectValue), RuntimeHelpers.GetObjectValue(obj));
                        } catch (Exception exception1) {
                            ProjectData.SetProjectError(exception1);
                            ProjectData.ClearProjectError();
                        }
                        if (ctl is RadioButton && Operators.CompareString(item.Name, "Checked", false) == 0) {
                            ((RadioButton)ctl).Checked = false;
                        }
                    }
                }
            }
            try {
                if (!(ctl is MenuItem)) {
                    Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), "Visible", CallType.Set, new object[] { false });
                }
            } catch (Exception exception2) {
                ProjectData.SetProjectError(exception2);
                ProjectData.ClearProjectError();
            }
            Information.Err().Clear();
            if (!(ctl is MenuItem)) {
                try {
                    Versioned.CallByName(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(Versioned.CallByName((Control)Versioned.CallByName(RuntimeHelpers.GetObjectValue(ctl), "Parent", CallType.Get, new object[0]), "Controls", CallType.Get, new object[0]))), "Add", CallType.Method, new object[] { objectValue });
                } catch (Exception exception4) {
                    ProjectData.SetProjectError(exception4);
                    try {
                        if (objectValue is IComponent) {
                            this.components.Add((IComponent)objectValue);
                        }
                    } catch (Exception exception3) {
                        ProjectData.SetProjectError(exception3);
                        ProjectData.ClearProjectError();
                    }
                    Information.Err().Clear();
                    ProjectData.ClearProjectError();
                }
            } else {
                this.InsertNewMenuItem((MenuItem)objectValue, NewIndex);
            }
            if (objectValue is IButtonControl) {
                Form form = ((Control)objectValue).FindForm();
                if (form != null) {
                    if (form.AcceptButton == ctl) {
                        form.AcceptButton = (IButtonControl)objectValue;
                    }
                    if (form.CancelButton == ctl) {
                        form.CancelButton = (IButtonControl)objectValue;
                    }
                }
            }
            if (objectValue is Control) {
                try {
                    ToolTip toolTip = (ToolTip)Versioned.CallByName(((Control)objectValue).FindForm(), "ToolTip1", CallType.Get, new object[0]);
                    string str = toolTip.GetToolTip((Control)ctl);
                    if (Operators.CompareString(str, "", false) != 0) {
                        toolTip.SetToolTip((Control)objectValue, str);
                    }
                } catch (Exception exception5) {
                    ProjectData.SetProjectError(exception5);
                    Information.Err().Clear();
                    ProjectData.ClearProjectError();
                }
            }
            if (flag) {
                axHost.EndInit();
            }
            return objectValue;
        }

        public int Count() {
            return controls.Count;
        }

        [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
        protected override void Dispose(bool disposing) {
            this.components = null;
            base.Dispose(disposing);
        }

        public void EndInit() {
            this.fIsEndInitCalled = true;
        }

        private void FindIndexForNewMenuArrayItem(int nArrayIndexNew, ref Menu mnuParent, ref int nMenuIndexToInsert) {
            int num = 0;
            DictionaryEntry dictionaryEntry;
            try {
                num = this.UBound();
            } catch (Exception exception) {
                ProjectData.SetProjectError(exception);
                VB6Errors.RaiseError(5, VB6Errors.CArr_NoControlToClone);
                ProjectData.ClearProjectError();
            }
            MenuItem objectValue = (MenuItem)RuntimeHelpers.GetObjectValue(this.controls[num]);
            mnuParent = objectValue.Parent;
            if (mnuParent == null) {
                VB6Errors.RaiseError(5, string.Concat( VB6Errors.Misc_NotInMenu1, " Load"));
            }
            if (nArrayIndexNew > num) {
                nMenuIndexToInsert = checked(objectValue.Index + 1);
                return;
            }
            int num1 = 32767;
            IDictionaryEnumerator enumerator = this.indices.GetEnumerator();
            while (enumerator.MoveNext()) {
                object current = enumerator.Current;
                if (current != null) {
                    dictionaryEntry = (DictionaryEntry)current;
                } else {
                    dictionaryEntry = new DictionaryEntry();
                }
                int num2 = Conversions.ToInteger(dictionaryEntry.Value);
                if (num2 <= nArrayIndexNew || num2 >= num1) {
                    continue;
                }
                num1 = num2;
            }
            object obj = RuntimeHelpers.GetObjectValue(this.controls[num1]);
            nMenuIndexToInsert = ((MenuItem)obj).Index;
        }

        protected abstract Type GetControlInstanceType();

        public IEnumerator GetEnumerator() {
            return this.controls.Values.GetEnumerator();
        }

        private object GetLowestCtl() {
            object item;
            try {
                int num = this.LBound();
                item = this.controls[num];
            } catch (Exception exception) {
                ProjectData.SetProjectError(exception);
                Information.Err().Clear();
                item = null;
                ProjectData.ClearProjectError();
            }
            return item;
        }

        protected abstract void HookUpControlEvents(object o);

        private void InsertNewMenuItem(MenuItem mnuNew, int nArrayIndexNew) {
            int num = 0;
            Menu menu = null;
            this.FindIndexForNewMenuArrayItem(nArrayIndexNew, ref menu, ref num);
            menu.MenuItems.Add(num, mnuNew);
        }

        public int LBound() {
            DictionaryEntry dictionaryEntry;
            DictionaryEntry dictionaryEntry1;
            int num = 32767;
            if (this.indices.Count == 0) {
                VB6Errors.RaiseError(9, VB6Errors.OutOfBounds_CArr_NoCtlsInArray);
            }
            IDictionaryEnumerator enumerator = this.indices.GetEnumerator();
            while (enumerator.MoveNext()) {
                object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                if (objectValue != null) {
                    dictionaryEntry1 = (DictionaryEntry)objectValue;
                } else {
                    dictionaryEntry = new DictionaryEntry();
                    dictionaryEntry1 = dictionaryEntry;
                }
                dictionaryEntry = dictionaryEntry1;
                int num1 = Conversions.ToInteger(dictionaryEntry.Value);
                if (num1 >= num) {
                    continue;
                }
                num = num1;
            }
            return num;
        }

        public void Load(int Index) {
            object objectValue = null;
            if (Index < 0) {
                VB6Errors.RaiseError(341, VB6Errors.ERR_CArrCantAlloc);
            }
            if (this.controls[Index] != null) {
                VB6Errors.RaiseError(360, string.Concat(VB6Errors.ERR_CArrObjectAlreadyLoaded1,Index));
            }
            object obj = RuntimeHelpers.GetObjectValue(this.GetLowestCtl());
            if (obj != null) {
                objectValue = RuntimeHelpers.GetObjectValue(this.CloneControl(RuntimeHelpers.GetObjectValue(obj), Index));
            } else {
                VB6Errors.RaiseError(5, VB6Errors.CArr_NoControlToClone);
            }
            if (objectValue == null) {
                VB6Errors.RaiseError(7, VB6Errors.CArr_UnableToClone);
                return;
            }
            this.BaseSetIndex(RuntimeHelpers.GetObjectValue(objectValue), Index, true);
        }

        private void RemoveMenuItem(MenuItem mnu) {
            Menu parent = mnu.Parent;
            if (parent != null) {
                parent.MenuItems.Remove(mnu);
            }
        }

        public int UBound() {
            DictionaryEntry dictionaryEntry;
            DictionaryEntry dictionaryEntry1;
            int num = -1;
            if (this.indices.Count == 0) {
                VB6Errors.RaiseError(9, VB6Errors.OutOfBounds_CArr_NoCtlsInArray);
            }
            IDictionaryEnumerator enumerator = this.indices.GetEnumerator();
            while (enumerator.MoveNext()) {
                object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                if (objectValue != null) {
                    dictionaryEntry1 = (DictionaryEntry)objectValue;
                } else {
                    dictionaryEntry = new DictionaryEntry();
                    dictionaryEntry1 = dictionaryEntry;
                }
                dictionaryEntry = dictionaryEntry1;
                int num1 = Conversions.ToInteger(dictionaryEntry.Value);
                if (num1 <= num) {
                    continue;
                }
                num = num1;
            }
            return num;
        }

        public void Unload(int Index) {
            if (Index < 0) {
                VB6Errors.RaiseError(341, VB6Errors.ERR_CArrCantAlloc);
            }
            object objectValue = RuntimeHelpers.GetObjectValue(this.controls[Index]);
            if (objectValue == null) {
                VB6Errors.RaiseError(340, string.Concat(VB6Errors.ERR_CArrIllegalIndex1, Index));
            }
            if (Conversions.ToBoolean(this.controlAddedAtDesignTime[RuntimeHelpers.GetObjectValue(objectValue)])) {
                VB6Errors.RaiseError(362, VB6Errors.ERR_CArrLdStaticControl);
            }
            this.controls.Remove(Index);
            this.indices.Remove(RuntimeHelpers.GetObjectValue(objectValue));
            this.controlAddedAtDesignTime.Remove(RuntimeHelpers.GetObjectValue(objectValue));
            if (!(objectValue is MenuItem)) {
                try {
                    Versioned.CallByName(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(Versioned.CallByName((Control)Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), "Parent", CallType.Get, new object[0]), "Controls", CallType.Get, new object[0]))), "Remove", CallType.Method, new object[] { objectValue });
                } catch (Exception exception1) {
                    ProjectData.SetProjectError(exception1);
                    try {
                        if (objectValue is IComponent) {
                            this.components.Remove((IComponent)objectValue);
                        }
                    } catch (Exception exception) {
                        ProjectData.SetProjectError(exception);
                        ProjectData.ClearProjectError();
                    }
                    ProjectData.ClearProjectError();
                }
            } else {
                this.RemoveMenuItem((MenuItem)objectValue);
            }
            try {
                Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), "Dispose", CallType.Method, new object[0]);
            } catch (Exception exception2) {
                ProjectData.SetProjectError(exception2);
                ProjectData.ClearProjectError();
            }
            Information.Err().Clear();
        }
    }
}

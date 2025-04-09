using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MSWinsockLib
{

#pragma warning disable 1591


    /// <summary>
    /// コンポーネントの配列をサポートするクラスです。
    /// </summary>
    public abstract class BaseComponentArray : Component, ISupportInitialize
    {
        protected Hashtable indices;

        protected Hashtable items;

        protected Hashtable itemAddedAtDesignTime;

        protected IContainer components;

        protected bool fIsEndInitCalled;

        protected BaseComponentArray()
        {
            this.indices = new Hashtable();
            this.items = new Hashtable();
            this.itemAddedAtDesignTime = new Hashtable();
        }

        protected BaseComponentArray(IContainer Container)
            : this()
        {
            Container.Add(this);
            this.components = Container;
        }

        protected bool BaseCanExtend(object target)
        {
            Information.Err().Clear();
            if (this.indices.ContainsKey(RuntimeHelpers.GetObjectValue(target)))
            {
                return true;
            }
            return false;
        }

        protected short BaseGetIndex(object ctl)
        {
            short num = 0;
            RuntimeHelpers.GetObjectValue(this.indices[RuntimeHelpers.GetObjectValue(ctl)]);
            if (this.indices.ContainsKey(RuntimeHelpers.GetObjectValue(ctl)))
            {
                return Conversions.ToShort(this.indices[RuntimeHelpers.GetObjectValue(ctl)]);
            }
            try
            {
                ISite site = this.Site;
                if (site != null && site.DesignMode)
                {
                    Information.Err().Clear();
                    num = -1;
                    return num;
                }
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                ProjectData.ClearProjectError();
            }
            NativeMethods.VBRaiseError(343, "配列のコンポーネントではありません。");
            return num;
        }

        protected object BaseGetItem(short Index)
        {
            object objectValue = RuntimeHelpers.GetObjectValue(this.items[Index]);
            if (objectValue != null)
            {
                return objectValue;
            }
            NativeMethods.VBRaiseError(340, string.Format("インデックス ({0}) が不正です。)", Conversions.ToString((int)Index)));
            return null;
        }

        protected void BaseResetIndex(object o)
        {
        }

        protected void BaseSetIndex(object ctl, short Index, bool fIsDynamic = false)
        {
            if (Index < 0)
            {
                NativeMethods.VBRaiseError(341, string.Format("インデックスが不正です。Index = {0}", Conversions.ToString((int)Index)));
            }
            if (!this.CanCallSetIndex(fIsDynamic))
            {
                NativeMethods.VBRaiseError(5, "現在の状態ではインデックスがセットできません。");
            }
            if (this.items[Index] != null)
            {
                NativeMethods.VBRaiseError(360, string.Format("すでに配列の中に存在します。Index = {0} )", Conversions.ToString((int)Index)));
            }
            if (this.indices[RuntimeHelpers.GetObjectValue(ctl)] != null)
            {
                this.items.Remove(RuntimeHelpers.GetObjectValue(this.indices[RuntimeHelpers.GetObjectValue(ctl)]));
                this.indices.Remove(RuntimeHelpers.GetObjectValue(ctl));
            }
            this.indices[RuntimeHelpers.GetObjectValue(ctl)] = Index;
            this.items[Index] = RuntimeHelpers.GetObjectValue(ctl);
            if (!fIsDynamic)
            {
                this.itemAddedAtDesignTime[RuntimeHelpers.GetObjectValue(ctl)] = true;
            }
            this.HookUpControlEvents(RuntimeHelpers.GetObjectValue(ctl));
            TypeDescriptor.Refresh(RuntimeHelpers.GetObjectValue(ctl));
        }

        protected bool BaseShouldSerializeIndex(object o)
        {
            return this.indices.ContainsKey(RuntimeHelpers.GetObjectValue(o));
        }

        private bool CanCallSetIndex(bool fIsDynamic)
        {
            if (!fIsDynamic && this.fIsEndInitCalled)
            {
                return false;
            }
            return true;
        }

        private object CloneComponent(object ctl, short NewIndex)
        {
            Type type = ctl.GetType();
            object obj = RuntimeHelpers.GetObjectValue(Activator.CreateInstance(type));
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(RuntimeHelpers.GetObjectValue(ctl));
            int count = checked(properties.Count - 1);
            for (int i = 0; i <= count; i++)
            {
                PropertyDescriptor item = properties[i];
                if (item.SerializationVisibility == DesignerSerializationVisibility.Visible)
                {
                    bool flag1 = true;
                    try
                    {
                        if (!item.ShouldSerializeValue(RuntimeHelpers.GetObjectValue(ctl)))
                        {
                            flag1 = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        ProjectData.SetProjectError(exception);
                        flag1 = false;
                        Information.Err().Clear();
                        ProjectData.ClearProjectError();
                    }
                    if (flag1)
                    {
                        string name = item.Name;
                        if (Operators.CompareString(name, "TabIndex", false) == 0)
                        {
                            flag1 = false;
                        }
                        else if (Operators.CompareString(name, "Index", false) == 0)
                        {
                            flag1 = false;
                        }
                        else if ((Operators.CompareString(name, "MdiList", false) == 0 || Operators.CompareString(name, "Shortcut", false) == 0) && ctl is MenuItem)
                        {
                            flag1 = false;
                        }
                    }
                    if (flag1)
                    {
                        try
                        {
                            object objectValue1 = RuntimeHelpers.GetObjectValue(item.GetValue(RuntimeHelpers.GetObjectValue(ctl)));
                            item.SetValue(RuntimeHelpers.GetObjectValue(obj), RuntimeHelpers.GetObjectValue(objectValue1));
                        }
                        catch (Exception exception1)
                        {
                            ProjectData.SetProjectError(exception1);
                            ProjectData.ClearProjectError();
                        }
                        if (ctl is RadioButton && Operators.CompareString(item.Name, "Checked", false) == 0)
                        {
                            ((RadioButton)ctl).Checked = false;
                        }
                    }
                }
            }
            Information.Err().Clear();
            return obj;
        }

        public short Count()
        {
            return checked((short)this.items.Count);
        }

        [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
        protected override void Dispose(bool disposing)
        {
            this.components = null;
            base.Dispose(disposing);
        }

        private void FindIndexForNewMenuArrayItem(int nArrayIndexNew, ref Menu mnuParent, ref int nMenuIndexToInsert)
        {
            short num = 0;
            DictionaryEntry dictionaryEntry = new DictionaryEntry();
            DictionaryEntry dictionaryEntry1;
            try
            {
                num = this.UBound();
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                NativeMethods.VBRaiseError(5, "");
                ProjectData.ClearProjectError();
            }
            object objectValue = RuntimeHelpers.GetObjectValue(this.items[num]);
            MenuItem menuItem = (MenuItem)objectValue;
            mnuParent = menuItem.Parent;
            if (mnuParent == null)
            {
                NativeMethods.VBRaiseError(5, null);
            }
            if (nArrayIndexNew <= num)
            {
                short num1 = 32767;
                IDictionaryEnumerator enumerator = this.indices.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current != null)
                    {
                        dictionaryEntry1 = (DictionaryEntry)current;
                    }
                    else
                    {
                        dictionaryEntry1 = dictionaryEntry;
                    }
                    short num2 = Conversions.ToShort(dictionaryEntry1.Value);
                    if (num2 <= nArrayIndexNew || num2 >= num1)
                    {
                        continue;
                    }
                    num1 = num2;
                }
                object obj = RuntimeHelpers.GetObjectValue(this.items[num1]);
                nMenuIndexToInsert = ((MenuItem)obj).Index;
            }
            else
            {
                nMenuIndexToInsert = checked(menuItem.Index + 1);
            }
        }

        protected abstract Type GetControlInstanceType();

        public IEnumerator GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        private object GetLowestCtl()
        {
            object item;
            try
            {
                short num = this.LBound();
                item = this.items[num];
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                Information.Err().Clear();
                item = null;
                ProjectData.ClearProjectError();
            }
            return item;
        }

        protected abstract void HookUpControlEvents(object o);

        private void InsertNewMenuItem(MenuItem mnuNew, short nArrayIndexNew)
        {
            int num = 0;
            Menu menu = null;
            this.FindIndexForNewMenuArrayItem(nArrayIndexNew, ref menu, ref num);
            menu.MenuItems.Add(num, mnuNew);
        }

        public short LBound()
        {
            DictionaryEntry dictionaryEntry = new DictionaryEntry();
            DictionaryEntry dictionaryEntry1;
            short num = 32767;
            if (this.indices.Count == 0)
            {
                NativeMethods.VBRaiseError(9, "");
            }
            IDictionaryEnumerator enumerator = this.indices.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                if (objectValue != null)
                {
                    dictionaryEntry1 = (DictionaryEntry)objectValue;
                }
                else
                {
                    dictionaryEntry1 = dictionaryEntry;
                }
                short num1 = Conversions.ToShort(dictionaryEntry1.Value);
                if (num1 >= num)
                {
                    continue;
                }
                num = num1;
            }
            return num;
        }

        public void Load(short Index)
        {
            object objectValue = null;
            if (Index < 0)
            {
                NativeMethods.VBRaiseError(341, "");
            }
            if (this.items[Index] != null)
            {
                NativeMethods.VBRaiseError(360, "");
            }
            object obj = RuntimeHelpers.GetObjectValue(this.GetLowestCtl());
            if (obj != null)
            {
                objectValue = RuntimeHelpers.GetObjectValue(this.CloneComponent(RuntimeHelpers.GetObjectValue(obj), Index));
            }
            else
            {
                NativeMethods.VBRaiseError(5, "");
            }
            if (objectValue == null)
            {
                NativeMethods.VBRaiseError(7, "");
                return;
            }
            this.BaseSetIndex(RuntimeHelpers.GetObjectValue(objectValue), Index, true);
        }

        private void RemoveMenuItem(MenuItem mnu)
        {
            Menu parent = mnu.Parent;
            if (parent != null)
            {
                parent.MenuItems.Remove(mnu);
            }
        }

        public short UBound()
        {
            DictionaryEntry dictionaryEntry = new DictionaryEntry();
            DictionaryEntry dictionaryEntry1;
            short num = -1;
            if (this.indices.Count == 0)
            {
                NativeMethods.VBRaiseError(9, "");
            }
            IDictionaryEnumerator enumerator = this.indices.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                if (objectValue != null)
                {
                    dictionaryEntry1 = (DictionaryEntry)objectValue;
                }
                else
                {
                    dictionaryEntry1 = dictionaryEntry;
                }
                short num1 = Conversions.ToShort(dictionaryEntry1.Value);
                if (num1 <= num)
                {
                    continue;
                }
                num = num1;
            }
            return num;
        }

        public void Unload(short Index)
        {
            if (Index < 0)
            {
                NativeMethods.VBRaiseError(341, "");
            }
            object objectValue = RuntimeHelpers.GetObjectValue(this.items[Index]);
            if (objectValue == null)
            {
                NativeMethods.VBRaiseError(340, "");
            }
            if (Conversions.ToBoolean(this.itemAddedAtDesignTime[RuntimeHelpers.GetObjectValue(objectValue)]))
            {
                NativeMethods.VBRaiseError(362, "");
            }
            this.items.Remove(Index);
            this.indices.Remove(RuntimeHelpers.GetObjectValue(objectValue));
            this.itemAddedAtDesignTime.Remove(RuntimeHelpers.GetObjectValue(objectValue));
            if (!(objectValue is MenuItem))
            {
                try
                {
                    Control control = (Control)Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), "Parent", CallType.Get, new object[0]);
                    object obj = RuntimeHelpers.GetObjectValue(Versioned.CallByName(control, "Controls", CallType.Get, new object[0]));
                    object objectValue1 = RuntimeHelpers.GetObjectValue(obj);
                    object[] objArray = new object[] { RuntimeHelpers.GetObjectValue(objectValue) };
                    Versioned.CallByName(objectValue1, "Remove", CallType.Method, objArray);
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    try
                    {
                        if (objectValue is IComponent)
                        {
                            this.components.Remove((IComponent)objectValue);
                        }
                    }
                    catch (Exception exception)
                    {
                        ProjectData.SetProjectError(exception);
                        ProjectData.ClearProjectError();
                    }
                    ProjectData.ClearProjectError();
                }
            }
            else
            {
                this.RemoveMenuItem((MenuItem)objectValue);
            }
            try
            {
                Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), "Dispose", CallType.Method, new object[0]);
            }
            catch (Exception exception2)
            {
                ProjectData.SetProjectError(exception2);
                ProjectData.ClearProjectError();
            }
            Information.Err().Clear();
        }


        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            this.fIsEndInitCalled = true;
        }
    }
}
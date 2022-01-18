using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Runtime.CompilerServices;

namespace VBCompatible.ControlArray
{
    internal static class VB6Errors
    {
        internal const string EXC_WebItemNameNotOptional = "WebItemNameNotOptional";

        internal const string EXC_WebItemAssociatedWebClassNotOptional = "WebItemAssociatedWebClassNotOptional";

        internal const string EXC_WebItemCouldNotLoadEmbeddedResource = "WebItemCouldNotLoadEmbeddedResource";

        internal const string EXC_WebItemCouldNotLoadTemplateFile = "WebItemCouldNotLoadTemplateFile";

        internal const string EXC_WebItemUnexpectedErrorReadingTemplateFile = "WebItemUnexpectedErrorReadingTemplateFile";

        internal const string EXC_WebItemTooManyNestedTags = "WebItemTooManyNestedTags";

        internal const string EXC_WebItemClosingTagNotFound = "WebItemClosingTagNotFound";

        internal const string EXC_WebItemNoTemplateSpecified = "WebItemNoTemplateSpecified";

        internal const string EXC_WebClassNextItemCannotBeCurrentWebItem = "WebClassNextItemCannotBeCurrentWebItem";

        internal const string EXC_WebClassUserWebClassNameNotOptional = "WebClassUserWebClassNameNotOptional";

        internal const string EXC_WebClassWebClassFileNameNotOptional = "WebClassWebClassFileNameNotOptional";

        internal const string EXC_WebClassContainingClassNotOptional = "WebClassContainingClassNotOptional";

        internal const string EXC_WebClassStartNotFound = "WebClassStartNotFound";

        internal const string EXC_WebClassNextItemRespondNotFound = "WebClassNextItemRespondNotFound";

        internal const string EXC_WebClassCouldNotFindEvent = "WebClassCouldNotFindEvent";

        internal const string EXC_WebClassWebItemNotValid = "WebClassWebItemNotValid";

        internal const string ERR_IllegalFuncCall = "ERR_IllegalFuncCall";

        internal const string ERR_OutOfMemory = "ERR_OutOfMemory";

        internal const string ERR_OutOfBounds = "ERR_OutOfBounds";

        internal const string ERR_TypeMismatch = "ERR_TypeMismatch";

        internal const string ERR_DevUnavailable = "ERR_DevUnavailable";

        internal const string ERR_FileNotFound = "ERR_FileNotFound";

        internal const string ERR_FileNotFound1 = "ERR_FileNotFound1";

        internal const string ERR_PathNotFound = "ERR_PathNotFound";

        internal const string ERR_PathNotFound1 = "ERR_PathNotFound1";

        internal const string ERR_CArrIllegalIndex1 = "ERR_CArrIllegalIndex1";

        internal const string ERR_CArrObjectNotArray = "ERR_CArrObjectNotArray";

        internal const string ERR_CArrCantAlloc = "ERR_CArrCantAlloc";

        internal const string ERR_CArrObjectAlreadyLoaded1 = "ERR_CArrObjectAlreadyLoaded1";

        internal const string ERR_CArrLdStaticControl = "ERR_CArrLdStaticControl";

        internal const string ERR_PropIllegalValue = "ERR_PropIllegalValue";

        internal const string ERR_InvalidPropertyArrayIndex = "ERR_InvalidPropertyArrayIndex";

        internal const string ERR_InvalidPictureType = "ERR_InvalidPictureType";

        internal const string ERR_DevUnavailable1 = "ERR_DevUnavailable1";

        internal const string Argument_InvalidValue1 = "Argument_InvalidValue1";

        internal const string Argument_IncorrectControlType1 = "Argument_IncorrectControlType1";

        internal const string Argument_FormatUnsupportedType1 = "Argument_FormatUnsupportedType1";

        internal const string ListBoxItem_EntryTypeNotSupported1 = "ListBoxItem_EntryTypeNotSupported1";

        internal const string CArrDesign_ObjectAlreadyLoaded1 = "CArrDesign_ObjectAlreadyLoaded1";

        internal const string CArr_UnableToClone = "CArr_UnableToClone";

        internal const string CArr_NoControlToClone = "CArr_NoControlToClone";

        internal const string OutOfBounds_CArr_NoCtlsInArray = "OutOfBounds_CArr_NoCtlsInArray";

        internal const string Misc_SetUpTwipsPerPixel = "Misc_SetUpTwipsPerPixel";

        internal const string Misc_NotInMenu1 = "Misc_NotInMenu1";

        internal const string LoadRes_NotFound1 = "LoadRes_NotFound1";

        internal const string LoadRes_NotSupportedLoadResData = "LoadRes_NotSupportedLoadResData";

        internal const string AXUnknownImage = "AXUnknownImage";

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void RaiseError(int nError, string strMessage) {
            if (strMessage == null || Operators.CompareString(strMessage, "", false) == 0) {
                strMessage = Conversion.ErrorToString(nError);
            }
            Information.Err().Raise(nError, "VBCompatible.dll", strMessage, null, null);
        }
    }
}

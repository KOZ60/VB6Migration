namespace VBCompatible
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    internal static class NativeMethods
    {
        public const int ERROR_SUCCESS = 0;
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // Window メッセージ
        public const int
        WSF_VISIBLE = 0x0001,
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DELETEITEM = 0x002D,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WA_INACTIVE = 0,
        WA_ACTIVE = 1,
        WA_CLICKACTIVE = 2,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUIT = 0x0012,
        WM_QUERYOPEN = 0x0013,
        WM_ERASEBKGND = 0x0014,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_ENDSESSION = 0x0016,
        WM_SHOWWINDOW = 0x0018,
        WM_WININICHANGE = 0x001A,
        WM_SETTINGCHANGE = 0x001A,
        WM_DEVMODECHANGE = 0x001B,
        WM_ACTIVATEAPP = 0x001C,
        WM_FONTCHANGE = 0x001D,
        WM_TIMECHANGE = 0x001E,
        WM_CANCELMODE = 0x001F,
        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_CHILDACTIVATE = 0x0022,
        WM_QUEUESYNC = 0x0023,
        WM_GETMINMAXINFO = 0x0024,
        WM_PAINTICON = 0x0026,
        WM_ICONERASEBKGND = 0x0027,
        WM_NEXTDLGCTL = 0x0028,
        WM_SPOOLERSTATUS = 0x002A,
        WM_DRAWITEM = 0x002B,
        WM_MEASUREITEM = 0x002C,
        WM_VKEYTOITEM = 0x002E,
        WM_CHARTOITEM = 0x002F,
        WM_SETFONT = 0x0030,
        WM_GETFONT = 0x0031,
        WM_SETHOTKEY = 0x0032,
        WM_GETHOTKEY = 0x0033,
        WM_QUERYDRAGICON = 0x0037,
        WM_COMPAREITEM = 0x0039,
        WM_GETOBJECT = 0x003D,
        WM_COMPACTING = 0x0041,
        WM_COMMNOTIFY = 0x0044,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_POWER = 0x0048,
        WM_COPYDATA = 0x004A,
        WM_CANCELJOURNAL = 0x004B,
        WM_NOTIFY = 0x004E,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_TCARD = 0x0052,
        WM_HELP = 0x0053,
        WM_USERCHANGED = 0x0054,
        WM_NOTIFYFORMAT = 0x0055,
        WM_CONTEXTMENU = 0x007B,
        WM_STYLECHANGING = 0x007C,
        WM_STYLECHANGED = 0x007D,
        WM_DISPLAYCHANGE = 0x007E,
        WM_GETICON = 0x007F,
        WM_SETICON = 0x0080,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,
        WM_NCHITTEST = 0x0084,
        WM_NCPAINT = 0x0085,
        WM_NCACTIVATE = 0x0086,
        WM_GETDLGCODE = 0x0087,
        WM_SYNCPAINT = 0x0088,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCMOUSELEAVE = 0x02A2,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_NCXBUTTONDBLCLK = 0x00AD,
        WM_KEYFIRST = 0x0100,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_CTLCOLOR = 0x0019,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSCHAR = 0x0106,
        WM_SYSDEADCHAR = 0x0107,
        WM_KEYLAST = 0x0108,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_KEYLAST = 0x010F,
        WM_INITDIALOG = 0x0110,
        WM_COMMAND = 0x0111,
        WM_SYSCOMMAND = 0x0112,
        WM_TIMER = 0x0113,
        WM_HSCROLL = 0x0114,
        WM_VSCROLL = 0x0115,
        WM_INITMENU = 0x0116,
        WM_INITMENUPOPUP = 0x0117,
        WM_MENUSELECT = 0x011F,
        WM_MENUCHAR = 0x0120,
        WM_ENTERIDLE = 0x0121,
        WM_UNINITMENUPOPUP = 0x0125,
        WM_CHANGEUISTATE = 0x0127,
        WM_UPDATEUISTATE = 0x0128,
        WM_QUERYUISTATE = 0x0129,
        WM_CTLCOLORMSGBOX = 0x0132,
        WM_CTLCOLOREDIT = 0x0133,
        WM_CTLCOLORLISTBOX = 0x0134,
        WM_CTLCOLORBTN = 0x0135,
        WM_CTLCOLORDLG = 0x0136,
        WM_CTLCOLORSCROLLBAR = 0x0137,
        WM_CTLCOLORSTATIC = 0x0138,
        WM_MOUSEFIRST = 0x0200,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSELAST = 0x020A;

        public const int
        SC_SIZE = 0xF000,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_CLOSE = 0xF060,
        SC_KEYMENU = 0xF100,
        SC_RESTORE = 0xF120,
        SC_MOVE = 0xF010,
        SC_CONTEXTHELP = 0xF180;

        public const int WHEEL_DELTA = 120,
        WM_PARENTNOTIFY = 0x0210,
        WM_ENTERMENULOOP = 0x0211,
        WM_EXITMENULOOP = 0x0212,
        WM_NEXTMENU = 0x0213,
        WM_SIZING = 0x0214,
        WM_CAPTURECHANGED = 0x0215,
        WM_MOVING = 0x0216,
        WM_POWERBROADCAST = 0x0218,
        WM_DEVICECHANGE = 0x0219,
        WM_IME_SETCONTEXT = 0x0281,
        WM_IME_NOTIFY = 0x0282,
        WM_IME_CONTROL = 0x0283,
        WM_IME_COMPOSITIONFULL = 0x0284,
        WM_IME_SELECT = 0x0285,
        WM_IME_CHAR = 0x0286,
        WM_IME_KEYDOWN = 0x0290,
        WM_IME_KEYUP = 0x0291,
        WM_MDICREATE = 0x0220,
        WM_MDIDESTROY = 0x0221,
        WM_MDIACTIVATE = 0x0222,
        WM_MDIRESTORE = 0x0223,
        WM_MDINEXT = 0x0224,
        WM_MDIMAXIMIZE = 0x0225,
        WM_MDITILE = 0x0226,
        WM_MDICASCADE = 0x0227,
        WM_MDIICONARRANGE = 0x0228,
        WM_MDIGETACTIVE = 0x0229,
        WM_MDISETMENU = 0x0230,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_DROPFILES = 0x0233,
        WM_MDIREFRESHMENU = 0x0234,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSELEAVE = 0x02A3,
        WM_CUT = 0x0300,
        WM_COPY = 0x0301,
        WM_PASTE = 0x0302,
        WM_CLEAR = 0x0303,
        WM_UNDO = 0x0304,
        WM_RENDERFORMAT = 0x0305,
        WM_RENDERALLFORMATS = 0x0306,
        WM_DESTROYCLIPBOARD = 0x0307,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_PAINTCLIPBOARD = 0x0309,
        WM_VSCROLLCLIPBOARD = 0x030A,
        WM_SIZECLIPBOARD = 0x030B,
        WM_ASKCBFORMATNAME = 0x030C,
        WM_CHANGECBCHAIN = 0x030D,
        WM_HSCROLLCLIPBOARD = 0x030E,
        WM_QUERYNEWPALETTE = 0x030F,
        WM_PALETTEISCHANGING = 0x0310,
        WM_PALETTECHANGED = 0x0311,
        WM_HOTKEY = 0x0312,
        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
        WM_THEMECHANGED = 0x031A,
        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,
        WM_APP = unchecked((int)0x8000),
        WM_USER = 0x0400,
        WM_REFLECT = WM_USER + 0x1C00,
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = unchecked((int)0x80000000),
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WPF_SETMINPOSITION = 0x0001,
        WM_CHOOSEFONT_GETLOGFONT = (0x0400 + 1),
        SS_LEFT = 0x00000000,
        SS_CENTER = 0x00000001,
        SS_RIGHT = 0x00000002,
        SS_OWNERDRAW = 0x0000000D,
        SS_NOPREFIX = 0x00000080,
        SS_SUNKEN = 0x00001000;

        public const int
         CS_VREDRAW = 0x0001,
         CS_HREDRAW = 0x0002,
         CS_DBLCLKS = 0x0008,
         CS_OWNDC = 0x0020,
         CS_CLASSDC = 0x0040,
         CS_PARENTDC = 0x0080,
         CS_NOCLOSE = 0x0200,
         CS_SAVEBITS = 0x0800,
         CS_BYTEALIGNCLIENT = 0x1000,
         CS_BYTEALIGNWINDOW = 0x2000,
         CS_GLOBALCLASS = 0x4000,
         CS_IME = 0x00010000,
         CS_DROPSHADOW = 0x00020000,
         CW_USEDEFAULT = (unchecked((int)0x80000000));

        public const int
        DCX_WINDOW = 0x00000001,
        DCX_CACHE = 0x00000002,
        DCX_NORESETATTRS = 0x00000004,
        DCX_CLIPCHILDREN = 0x00000008,
        DCX_CLIPSIBLINGS = 0x00000010,
        DCX_PARENTCLIP = 0x00000020,
        DCX_EXCLUDERGN = 0x00000040,
        DCX_INTERSECTRGN = 0x00000080,
        DCX_EXCLUDEUPDATE = 0x00000100,
        DCX_INTERSECTUPDATE = 0x00000200,
        DCX_LOCKWINDOWUPDATE = 0x00000400,
        DCX_USESTYLE = 0x00010000,
        DCX_VALIDATE = 0x00200000;

        // EditBox メッセージ
        public const int
        ES_LEFT = 0x0000,
        ES_CENTER = 0x0001,
        ES_RIGHT = 0x0002,
        ES_ALIGNMASK = 0x0003,
        ES_MULTILINE = 0x0004,
        ES_UPPERCASE = 0x0008,
        ES_LOWERCASE = 0x0010,
        ES_AUTOVSCROLL = 0x0040,
        ES_AUTOHSCROLL = 0x0080,
        ES_NOHIDESEL = 0x0100,
        ES_READONLY = 0x0800,
        ES_PASSWORD = 0x0020,
        EN_CHANGE = 0x0300,
        EN_UPDATE = 0x0400,
        EN_HSCROLL = 0x0601,
        EN_VSCROLL = 0x0602,
        EN_ALIGN_LTR_EC = 0x0700,
        EN_ALIGN_RTL_EC = 0x0701,
        EC_LEFTMARGIN = 0x0001,
        EC_RIGHTMARGIN = 0x0002,
        EM_GETSEL = 0x00B0,
        EM_SETSEL = 0x00B1,
        EM_GETRECT = 0x00B2,
        EM_SETRECT = 0x00B3,
        EM_SETRECTNP = 0x00B4,
        EM_SCROLL = 0x00B5,
        EM_SCROLLCARET = 0x00B7,
        EM_GETMODIFY = 0x00B8,
        EM_SETMODIFY = 0x00B9,
        EM_GETLINECOUNT = 0x00BA,
        EM_REPLACESEL = 0x00C2,
        EM_GETLINE = 0x00C4,
        EM_LIMITTEXT = 0x00C5,
        EM_CANUNDO = 0x00C6,
        EM_UNDO = 0x00C7,
        EM_SETPASSWORDCHAR = 0x00CC,
        EM_GETPASSWORDCHAR = 0x00D2,
        EM_EMPTYUNDOBUFFER = 0x00CD,
        EM_LINELENGTH = 0x00C1,
        EM_SETREADONLY = 0x00CF,
        EM_SETMARGINS = 0x00D3,
        EM_GETMARGINS = 0x00D4,
        EM_POSFROMCHAR = 0x00D6,
        EM_CHARFROMPOS = 0x00D7,
        EM_LINEFROMCHAR = 0x00C9,
        EM_GETFIRSTVISIBLELINE = 0x00CE,
        EM_LINEINDEX = 0x00BB;

        public const int
        LB_ERR = (-1),
        LB_ERRSPACE = (-2),
        LBN_SELCHANGE = 1,
        LBN_DBLCLK = 2,
        LB_ADDSTRING = 0x0180,
        LB_INSERTSTRING = 0x0181,
        LB_DELETESTRING = 0x0182,
        LB_RESETCONTENT = 0x0184,
        LB_SETSEL = 0x0185,
        LB_SETCURSEL = 0x0186,
        LB_GETSEL = 0x0187,
        LB_GETCARETINDEX = 0x019F,
        LB_GETCURSEL = 0x0188,
        LB_GETTEXT = 0x0189,
        LB_GETTEXTLEN = 0x018A,
        LB_GETTOPINDEX = 0x018E,
        LB_FINDSTRING = 0x018F,
        LB_GETSELCOUNT = 0x0190,
        LB_GETSELITEMS = 0x0191,
        LB_SETTABSTOPS = 0x0192,
        LB_SETHORIZONTALEXTENT = 0x0194,
        LB_SETCOLUMNWIDTH = 0x0195,
        LB_SETTOPINDEX = 0x0197,
        LB_GETITEMRECT = 0x0198,
        LB_SETITEMHEIGHT = 0x01A0,
        LB_GETITEMHEIGHT = 0x01A1,
        LB_FINDSTRINGEXACT = 0x01A2,
        LB_ITEMFROMPOINT = 0x01A9,
        LB_SETLOCALE = 0x01A5;

        // Button

        public const int
        BS_PUSHBUTTON = 0x00000000,
        BS_DEFPUSHBUTTON = 0x00000001,
        BS_CHECKBOX = 0x00000002,
        BS_AUTOCHECKBOX = 0x00000003,
        BS_RADIOBUTTON = 0x00000004,
        BS_3STATE = 0x00000005,
        BS_AUTO3STATE = 0x00000006,
        BS_GROUPBOX = 0x00000007,
        BS_USERBUTTON = 0x00000008,
        BS_AUTORADIOBUTTON = 0x00000009,
        BS_OWNERDRAW = 0x0000000B;
        //
        // ListBox
        //
        public const int
        LBS_NOTIFY = 0x0001,
        LBS_MULTIPLESEL = 0x0008,
        LBS_OWNERDRAWFIXED = 0x0010,
        LBS_OWNERDRAWVARIABLE = 0x0020,
        LBS_HASSTRINGS = 0x0040,
        LBS_USETABSTOPS = 0x0080,
        LBS_NOINTEGRALHEIGHT = 0x0100,
        LBS_MULTICOLUMN = 0x0200,
        LBS_WANTKEYBOARDINPUT = 0x0400,
        LBS_EXTENDEDSEL = 0x0800,
        LBS_DISABLENOSCROLL = 0x1000,
        LBS_NOSEL = 0x4000,
        LOCK_WRITE = 0x1,
        LOCK_EXCLUSIVE = 0x2,
        LOCK_ONLYONCE = 0x4,
        LV_VIEW_TILE = 0x0004,
        LVBKIF_SOURCE_NONE = 0x0000,
        LVBKIF_SOURCE_URL = 0x0002,
        LVBKIF_STYLE_NORMAL = 0x0000,
        LVBKIF_STYLE_TILE = 0x0010,
        LVS_ICON = 0x0000,
        LVS_REPORT = 0x0001,
        LVS_SMALLICON = 0x0002,
        LVS_LIST = 0x0003,
        LVS_TYPEMASK = 0x0003,
        LVS_SINGLESEL = 0x0004,
        LVS_SHOWSELALWAYS = 0x0008,
        LVS_SORTASCENDING = 0x0010,
        LVS_SORTDESCENDING = 0x0020,
        LVS_SHAREIMAGELISTS = 0x0040,
        LVS_NOLABELWRAP = 0x0080,
        LVS_AUTOARRANGE = 0x0100,
        LVS_EDITLABELS = 0x0200,
        LVS_OWNERDATA = 0x1000,
        LVS_NOSCROLL = 0x2000,
        LVS_TYPESTYLEMASK = 0xfc00,
        LVS_ALIGNTOP = 0x0000,
        LVS_ALIGNLEFT = 0x0800,
        LVS_ALIGNMASK = 0x0c00,
        LVS_OWNERDRAWFIXED = 0x0400,
        LVS_NOCOLUMNHEADER = 0x4000,
        LVS_NOSORTHEADER = unchecked((int)0x8000),
        LVSCW_AUTOSIZE = -1,
        LVSCW_AUTOSIZE_USEHEADER = -2,
        LVM_REDRAWITEMS = (0x1000 + 21),
        LVM_SCROLL = (0x1000 + 20),
        LVM_SETBKCOLOR = (0x1000 + 1),
        LVM_SETBKIMAGEA = (0x1000 + 68),
        LVM_SETBKIMAGEW = (0x1000 + 138),
        LVM_SETCALLBACKMASK = (0x1000 + 11),
        LVM_GETCALLBACKMASK = (0x1000 + 10),
        LVM_GETCOLUMNORDERARRAY = (0x1000 + 59),
        LVM_GETITEMCOUNT = (0x1000 + 4),
        LVM_SETCOLUMNORDERARRAY = (0x1000 + 58),
        LVM_SETINFOTIP = (0x1000 + 173),
        LVSIL_NORMAL = 0,
        LVSIL_SMALL = 1,
        LVSIL_STATE = 2,
        LVM_SETIMAGELIST = (0x1000 + 3),
        LVM_SETSELECTIONMARK = (0x1000 + 67),
        LVM_SETTOOLTIPS = (0x1000 + 74),
        LVIF_TEXT = 0x0001,
        LVIF_IMAGE = 0x0002,
        LVIF_INDENT = 0x0010,
        LVIF_PARAM = 0x0004,
        LVIF_STATE = 0x0008,
        LVIF_GROUPID = 0x0100,
        LVIF_COLUMNS = 0x0200,
        LVIS_FOCUSED = 0x0001,
        LVIS_SELECTED = 0x0002,
        LVIS_CUT = 0x0004,
        LVIS_DROPHILITED = 0x0008,
        LVIS_OVERLAYMASK = 0x0F00,
        LVIS_STATEIMAGEMASK = 0xF000,
        LVM_GETITEMA = (0x1000 + 5),
        LVM_GETITEMW = (0x1000 + 75),
        LVM_SETITEMA = (0x1000 + 6),
        LVM_SETITEMW = (0x1000 + 76),
        LVM_SETITEMPOSITION32 = (0x01000 + 49),
        LVM_INSERTITEMA = (0x1000 + 7),
        LVM_INSERTITEMW = (0x1000 + 77),
        LVM_DELETEITEM = (0x1000 + 8),
        LVM_DELETECOLUMN = (0x1000 + 28),
        LVM_DELETEALLITEMS = (0x1000 + 9),
        LVM_UPDATE = (0x1000 + 42),
        LVM_GETNEXTITEM = (0x1000 + 12),
        LVFI_PARAM = 0x0001,
        LVFI_NEARESTXY = 0x0040,
        LVFI_PARTIAL = 0x0008,
        LVFI_STRING = 0x0002,
        LVM_FINDITEMA = (0x1000 + 13),
        LVM_FINDITEMW = (0x1000 + 83),
        LVIR_BOUNDS = 0,
        LVIR_ICON = 1,
        LVIR_LABEL = 2,
        LVIR_SELECTBOUNDS = 3,
        LVM_GETITEMPOSITION = (0x1000 + 16),
        LVM_GETITEMRECT = (0x1000 + 14),
        LVM_GETSUBITEMRECT = (0x1000 + 56),
        LVM_GETSTRINGWIDTHA = (0x1000 + 17),
        LVM_GETSTRINGWIDTHW = (0x1000 + 87),
        LVHT_NOWHERE = 0x0001,
        LVHT_ONITEMICON = 0x0002,
        LVHT_ONITEMLABEL = 0x0004,
        LVHT_ABOVE = 0x0008,
        LVHT_BELOW = 0x0010,
        LVHT_RIGHT = 0x0020,
        LVHT_LEFT = 0x0040,
        LVHT_ONITEM = (0x0002 | 0x0004 | 0x0008),
        LVHT_ONITEMSTATEICON = 0x0008,
        LVM_SUBITEMHITTEST = (0x1000 + 57),
        LVM_HITTEST = (0x1000 + 18),
        LVM_ENSUREVISIBLE = (0x1000 + 19),
        LVA_DEFAULT = 0x0000,
        LVA_ALIGNLEFT = 0x0001,
        LVA_ALIGNTOP = 0x0002,
        LVA_SNAPTOGRID = 0x0005,
        LVM_ARRANGE = (0x1000 + 22),
        LVM_EDITLABELA = (0x1000 + 23),
        LVM_EDITLABELW = (0x1000 + 118),
        LVCDI_ITEM = 0x0000,
        LVCDI_GROUP = 0x00000001,
        LVCF_FMT = 0x0001,
        LVCF_WIDTH = 0x0002,
        LVCF_TEXT = 0x0004,
        LVCF_SUBITEM = 0x0008,
        LVCF_IMAGE = 0x0010,
        LVCF_ORDER = 0x0020,
        LVCFMT_IMAGE = 0x0800,
        LVGA_HEADER_LEFT = 0x00000001,
        LVGA_HEADER_CENTER = 0x00000002,
        LVGA_HEADER_RIGHT = 0x00000004,
        LVGA_FOOTER_LEFT = 0x00000008,
        LVGA_FOOTER_CENTER = 0x00000010,
        LVGA_FOOTER_RIGHT = 0x00000020,
        LVGF_NONE = 0x00000000,
        LVGF_HEADER = 0x00000001,
        LVGF_FOOTER = 0x00000002,
        LVGF_STATE = 0x00000004,
        LVGF_ALIGN = 0x00000008,
        LVGF_GROUPID = 0x00000010,
        LVGS_NORMAL = 0x00000000,
        LVGS_COLLAPSED = 0x00000001,
        LVGS_HIDDEN = 0x00000002,
        LVIM_AFTER = 0x00000001,
        LVTVIF_FIXEDSIZE = 0x00000003,
        LVTVIM_TILESIZE = 0x00000001,
        LVTVIM_COLUMNS = 0x00000002,
        LVS_EX_GRIDLINES = 0x00000001,
        LVS_EX_CHECKBOXES = 0x00000004,
        LVS_EX_TRACKSELECT = 0x00000008,
        LVS_EX_HEADERDRAGDROP = 0x00000010,
        LVS_EX_FULLROWSELECT = 0x00000020,
        LVS_EX_ONECLICKACTIVATE = 0x00000040,
        LVS_EX_TWOCLICKACTIVATE = 0x00000080,
        LVS_EX_INFOTIP = 0x00000400,
        LVS_EX_UNDERLINEHOT = 0x00000800,
        LVS_EX_DOUBLEBUFFER = 0x00010000,
        LVN_ITEMCHANGING = ((0 - 100) - 0),
        LVN_ITEMCHANGED = ((0 - 100) - 1),
        LVN_BEGINLABELEDITA = ((0 - 100) - 5),
        LVN_BEGINLABELEDITW = ((0 - 100) - 75),
        LVN_ENDLABELEDITA = ((0 - 100) - 6),
        LVN_ENDLABELEDITW = ((0 - 100) - 76),
        LVN_COLUMNCLICK = ((0 - 100) - 8),
        LVN_BEGINDRAG = ((0 - 100) - 9),
        LVN_BEGINRDRAG = ((0 - 100) - 11),
        LVN_ODFINDITEMA = ((0 - 100) - 52),
        LVN_ODFINDITEMW = ((0 - 100) - 79),
        LVN_ITEMACTIVATE = ((0 - 100) - 14),
        LVN_GETDISPINFOA = ((0 - 100) - 50),
        LVN_GETDISPINFOW = ((0 - 100) - 77),
        LVN_ODCACHEHINT = ((0 - 100) - 13),
        LVN_ODSTATECHANGED = ((0 - 100) - 15),
        LVN_SETDISPINFOA = ((0 - 100) - 51),
        LVN_SETDISPINFOW = ((0 - 100) - 78),
        LVN_GETINFOTIPA = ((0 - 100) - 57),
        LVN_GETINFOTIPW = ((0 - 100) - 58),
        LVN_KEYDOWN = ((0 - 100) - 55),
        LWA_COLORKEY = 0x00000001,
        LWA_ALPHA = 0x00000002;

        public const int
        LVM_FIRST = 0x1000,
        LVM_ENABLEGROUPVIEW = (LVM_FIRST + 157),
        LVM_MOVEITEMTOGROUP = (LVM_FIRST + 154),
        LVM_GETCOLUMNA = (LVM_FIRST + 25),
        LVM_GETCOLUMNW = (LVM_FIRST + 95),
        LVM_SETCOLUMNA = (LVM_FIRST + 26),
        LVM_SETCOLUMNW = (LVM_FIRST + 96),
        LVM_INSERTCOLUMNA = (LVM_FIRST + 27),
        LVM_INSERTCOLUMNW = (LVM_FIRST + 97),
        LVM_INSERTGROUP = (LVM_FIRST + 145),
        LVM_REMOVEGROUP = (LVM_FIRST + 150),
        LVM_INSERTMARKHITTEST = (LVM_FIRST + 168),
        LVM_REMOVEALLGROUPS = (LVM_FIRST + 160),
        LVM_GETCOLUMNWIDTH = (LVM_FIRST + 29),
        LVM_SETCOLUMNWIDTH = (LVM_FIRST + 30),
        LVM_SETINSERTMARK = (LVM_FIRST + 166),
        LVM_GETHEADER = (LVM_FIRST + 31),
        LVM_SETTEXTCOLOR = (LVM_FIRST + 36),
        LVM_SETTEXTBKCOLOR = (LVM_FIRST + 38),
        LVM_GETTOPINDEX = (LVM_FIRST + 39),
        LVM_SETITEMPOSITION = (LVM_FIRST + 15),
        LVM_SETITEMSTATE = (LVM_FIRST + 43),
        LVM_GETITEMSTATE = (LVM_FIRST + 44),
        LVM_GETITEMTEXTA = (LVM_FIRST + 45),
        LVM_GETITEMTEXTW = (LVM_FIRST + 115),
        LVM_GETHOTITEM = (LVM_FIRST + 61),
        LVM_SETITEMTEXTA = (LVM_FIRST + 46),
        LVM_SETITEMTEXTW = (LVM_FIRST + 116),
        LVM_SETITEMCOUNT = (LVM_FIRST + 47),
        LVM_SORTITEMS = (LVM_FIRST + 48),
        LVM_GETSELECTEDCOUNT = (LVM_FIRST + 50),
        LVM_GETISEARCHSTRINGA = (LVM_FIRST + 52),
        LVM_GETISEARCHSTRINGW = (LVM_FIRST + 117),
        LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54),
        LVM_SETVIEW = (LVM_FIRST + 142),
        LVM_GETGROUPINFO = (LVM_FIRST + 149),
        LVM_SETGROUPINFO = (LVM_FIRST + 147),
        LVM_HASGROUP = (LVM_FIRST + 161),
        LVM_SETTILEVIEWINFO = (LVM_FIRST + 162),
        LVM_GETTILEVIEWINFO = (LVM_FIRST + 163),
        LVM_GETINSERTMARK = (LVM_FIRST + 167),
        LVM_GETINSERTMARKRECT = (LVM_FIRST + 169),
        LVM_SETINSERTMARKCOLOR = (LVM_FIRST + 170),
        LVM_GETINSERTMARKCOLOR = (LVM_FIRST + 171),
        LVM_ISGROUPVIEWENABLED = (LVM_FIRST + 175);

        public const int
        MDITILE_VERTICAL = 0x0000,
        MDITILE_HORIZONTAL = 0x0001;

        public const int
        CB_ERR = (-1),
        CBN_SELCHANGE = 1,
        CBN_DBLCLK = 2,
        CBN_EDITCHANGE = 5,
        CBN_EDITUPDATE = 6,
        CBN_DROPDOWN = 7,
        CBN_CLOSEUP = 8,
        CBN_SELENDOK = 9,
        CBS_SIMPLE = 0x0001,
        CBS_DROPDOWN = 0x0002,
        CBS_DROPDOWNLIST = 0x0003,
        CBS_OWNERDRAWFIXED = 0x0010,
        CBS_OWNERDRAWVARIABLE = 0x0020,
        CBS_AUTOHSCROLL = 0x0040,
        CBS_HASSTRINGS = 0x0200,
        CBS_NOINTEGRALHEIGHT = 0x0400,
        CB_GETEDITSEL = 0x0140,
        CB_LIMITTEXT = 0x0141,
        CB_SETEDITSEL = 0x0142,
        CB_ADDSTRING = 0x0143,
        CB_DELETESTRING = 0x0144,
        CB_GETCURSEL = 0x0147,
        CB_GETLBTEXT = 0x0148,
        CB_GETLBTEXTLEN = 0x0149,
        CB_INSERTSTRING = 0x014A,
        CB_RESETCONTENT = 0x014B,
        CB_FINDSTRING = 0x014C,
        CB_SETCURSEL = 0x014E,
        CB_SHOWDROPDOWN = 0x014F,
        CB_GETITEMDATA = 0x0150,
        CB_SETITEMHEIGHT = 0x0153,
        CB_GETITEMHEIGHT = 0x0154,
        CB_GETDROPPEDSTATE = 0x0157,
        CB_FINDSTRINGEXACT = 0x0158,
        CB_GETDROPPEDWIDTH = 0x015F,
        CB_SETDROPPEDWIDTH = 0x0160;

        public const int
        GWL_WNDPROC = (-4),
        GWL_HWNDPARENT = (-8),
        GWL_STYLE = (-16),
        GWL_EXSTYLE = (-20),
        GWL_ID = (-12),
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5;

        public const int
        SB_LINEUP = 0,
        SB_LINELEFT = 0,
        SB_LINEDOWN = 1,
        SB_LINERIGHT = 1,
        SB_PAGEUP = 2,
        SB_PAGELEFT = 2,
        SB_PAGEDOWN = 3,
        SB_PAGERIGHT = 3,
        SB_THUMBPOSITION = 4,
        SB_THUMBTRACK = 5,
        SB_TOP = 6,
        SB_LEFT = 6,
        SB_BOTTOM = 7,
        SB_RIGHT = 7,
        SB_ENDSCROLL = 8;

        // ID for dwIndex of ImmGetProperty
        public const int
        IGP_GETIMEVERSION = (-4),
        IGP_PROPERTY = 0x00000004,
        IGP_CONVERSION = 0x00000008,
        IGP_SENTENCE = 0x0000000c,
        IGP_UI = 0x00000010,
        IGP_SETCOMPSTR = 0x00000014,
        IGP_SELECT = 0x00000018;

        // ImmSetCompositionString Capability bits
        public const int
        SCS_CAP_COMPSTR = 0x00000001,
        SCS_CAP_MAKEREAD = 0x00000002,
        SCS_CAP_SETRECONVERTSTRING = 0x00000004;

        // TabControl Message
        public const int
          TCM_FIRST = 0x1300,
          TCM_GETIMAGELIST = TCM_FIRST + 2,  // イメージリストのハンドルを取得
          TCM_SETIMAGELIST = TCM_FIRST + 3,  // イメージリストの割り当て 
          TCM_GETITEMCOUNT = TCM_FIRST + 4,  // タブ項目の数の取得
          TCM_DELETEITEM = TCM_FIRST + 8,  // タブ項目の削除
          TCM_DELETEALLITEMS = TCM_FIRST + 9,  // 全ての項目を削除
          TCM_GETITEMRECT = TCM_FIRST + 10, // 特定タブ項目の境界矩形の取得
          TCM_GETCURSEL = TCM_FIRST + 11, // 現在選択されているタブ項目の取得
          TCM_SETCURSEL = TCM_FIRST + 12, // タブ項目の選択
          TCM_HITTEST = TCM_FIRST + 13, // 特定の座標点にタブ項目があるかテスト
          TCM_SETITEMEXTRA = TCM_FIRST + 14, // タブ項目に関連づけるデータのバイト数を設定
          TCM_ADJUSTRECT = TCM_FIRST + 40, // ウィンドウ領域と表示領域の相互変換
          TCM_SETITEMSIZE = TCM_FIRST + 41, // タブの幅と高さを設定
          TCM_REMOVEIMAGE = TCM_FIRST + 42, // イメージリストから特定のイメージを削除
          TCM_SETPADDING = TCM_FIRST + 43, // タブのアイコンとラベルの間に割り当てる領域の設定
          TCM_GETROWCOUNT = TCM_FIRST + 44, // タブの行数の取得
          TCM_GETTOOLTIPS = TCM_FIRST + 45, // ツールヒントのハンドルの取得
          TCM_SETTOOLTIPS = TCM_FIRST + 46, // ツールヒントの割り当て
          TCM_GETCURFOCUS = TCM_FIRST + 47, // フォーカスのあるタブ項目の取得
          TCM_SETCURFOCUS = TCM_FIRST + 48, // 特定のタブ項目にフォーカスを設定
          TCM_GETITEM = TCM_FIRST + 5,  // タブ項目の情報の取得
          TCM_SETITEM = TCM_FIRST + 6,  // タブ項目の情報の設定
          TCM_INSERTITEM = TCM_FIRST + 7,  // タブ項目の挿入
          TCM_SETMINTABWIDTH = TCM_FIRST + 49, // タブつまみの最小幅を設定
          TCM_DESELECTALL = TCM_FIRST + 50, // TCS_BUTTONS スタイル時に選択項目なしにする
          TCM_HIGHLIGHTITEM = TCM_FIRST + 51, // 特定タブ項目をハイライト表示する
          TCM_SETEXTENDEDSTYLE = TCM_FIRST + 52, // 拡張スタイルを設定する
          TCM_GETEXTENDEDSTYLE = TCM_FIRST + 53; // 拡張スタイルを取得する

        // TabControl Style
        public const int
        TCS_TABS = 0x0000,
        TCS_SCROLLOPPOSITE = 0x001,
        TCS_BOTTOM = 0x0002,
        TCS_RIGHT = 0x0002,
        TCS_FLATBUTTONS = 0x0008,
        TCS_HOTTRACK = 0x0040,
        TCS_VERTICAL = 0x0080,
        TCS_BUTTONS = 0x0100,
        TCS_SINGLELINE = 0x0000,
        TCS_MULTISELECT = 0x004,
        TCS_MULTILINE = 0x0200,
        TCS_RIGHTJUSTIFY = 0x0000,
        TCS_FIXEDWIDTH = 0x0400,
        TCS_RAGGEDRIGHT = 0x0800,
        TCS_FOCUSONBUTTONDOWN = 0x1000,
        TCS_OWNERDRAWFIXED = 0x2000,
        TCS_TOOLTIPS = 0x4000,
        TCS_FOCUSNEVER = 0x8000;

        // TabControl ExStyle
        public const int
            TCS_EX_FLATSEPARATORS = 0x0001,
            TCS_EX_REGISTERDROP = 0x0002;

        public const int
        UIS_SET = 1,
        UIS_CLEAR = 2,
        UIS_INITIALIZE = 3,
        UISF_HIDEFOCUS = 0x1,
        UISF_HIDEACCEL = 0x2,
        UISF_ACTIVE = 0x4,
        USERCLASSTYPE_FULL = 1,
        USERCLASSTYPE_SHORT = 2,
        USERCLASSTYPE_APPNAME = 3,
        UOI_FLAGS = 1;

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseCapture();

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public extern static IntPtr SendMessage(IntPtr hwndRef, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public extern static IntPtr SendMessage(IntPtr hwndRef, int wMsg, out int wParam, out int lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static IntPtr SendMessage(IntPtr hwndRef, int wMsg, IntPtr wParam,
                                [MarshalAs(UnmanagedType.LPTStr)] string lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public extern static IntPtr SendMessage(IntPtr hwndRef, int wMsg, IntPtr wParam, TV_HITTESTINFO lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnableWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool fEnable);

        [StructLayout(LayoutKind.Sequential)]
        public class ComboBoxInfo
        {
            public int Size;
            public RECT RectItem;
            public RECT RectButton;
            public int ButtonState;
            public IntPtr ComboBoxHandle;
            public IntPtr EditBoxHandle;
            public IntPtr ListBoxHandle;
            public ComboBoxInfo() {
                this.Size = Marshal.SizeOf(this);
            }
        }

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ComboBoxInfo cbinfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom) {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int Width {
                get {
                    return Right - Left;
                }
            }

            public int Height {
                get {
                    return Bottom - Top;
                }
            }

            public Rectangle Rectangle {
                get {
                    return Rectangle.FromLTRB(Left, Top, Right, Bottom);
                }
            }

            public Point Location {
                get {
                    return new Point(Left, Top);
                }
            }

            public Size Size {
                get {
                    return new Size(Width, Height);
                }
            }

            public bool Contains(int x, int y) {
                if (x < this.Left || x > this.Right) {
                    return false;
                }
                if (y < this.Top || y > this.Bottom) {
                    return false;
                }
                return true;
            }

            public static implicit operator Rectangle(RECT r) {
                return r.Rectangle;
            }

            public static implicit operator RECT(Rectangle r) {
                return new RECT(r);
            }

            public bool IsEmpty {
                get {
                    return (Left == 0) && (Right == 0) && (Top == 0) && (Bottom == 0);
                }
            }

        }
        [DllImport(ExternDll.User32)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [Flags()]
        public enum RDW : uint
        {
            RDW_INVALIDATE = 0x0001,
            RDW_INTERNALPAINT = 0x0002,
            RDW_ERASE = 0x0004,

            RDW_VALIDATE = 0x0008,
            RDW_NOINTERNALPAINT = 0x0010,
            RDW_NOERASE = 0x0020,

            RDW_NOCHILDREN = 0x0040,
            RDW_ALLCHILDREN = 0x0080,

            RDW_UPDATENOW = 0x0100,
            RDW_ERASENOW = 0x0200,

            RDW_FRAME = 0x0400,
            RDW_NOFRAME = 0x0800,
        }

        [DllImport(ExternDll.User32)]
        public static extern int RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);

        public const int SPI_GETWORKAREA = 48;
        public const int SPI_SETDROPSHADOW = 0x1025;
        public const int SPIF_SENDWININICHANGE = 0x2;

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, ref RECT lpvParam, int fuWinIni);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, int fdwOptions);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y) {
                this.X = x;
                this.Y = y;
            }

            public POINT(Point p) {
                this.X = p.X;
                this.Y = p.Y;
            }

            public Point Location {
                get {
                    return new Point(this.X, this.Y);
                }
            }
        }

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport(ExternDll.User32)]
        public static extern int LockWindowUpdate(IntPtr hWndLock);

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hwnd, ref RECT lpRect);

        // API ではないが、ここに記述

        public static uint MAKEDWORD(short hiWord, short loWord) {
            return ((uint)((ushort)hiWord) << 16) | (ushort)loWord;
        }

        public static uint MAKEDWORD(int hiWord, int loWord) {
            return ((uint)((uint)hiWord) << 16) | (uint)loWord;
        }

        public static long MAKELONG(int a, int b) {
            return ((uint)a & 0xffff) | (((uint)b) << 16);
        }

        public static IntPtr MAKEWPARAM(int l, int h) {
            return new IntPtr(MAKELONG(l, h));
        }

        public static int HIWORD(int n) {
            return (n >> 16) & 0xffff;
        }

        public static int HIWORD(IntPtr n) {
            return HIWORD(unchecked((int)(long)n));
        }

        public static int LOWORD(int n) {
            return n & 0xffff;
        }

        public static int LOWORD(IntPtr n) {
            return LOWORD(unchecked((int)(long)n));
        }

        public static int SignedHIWORD(IntPtr n) {
            return SignedHIWORD(unchecked((int)(long)n));
        }
        public static int SignedLOWORD(IntPtr n) {
            return SignedLOWORD(unchecked((int)(long)n));
        }

        public static int SignedHIWORD(int n) {
            int i = (int)(short)((n >> 16) & 0xffff);
            return i;
        }

        public static int SignedLOWORD(int n) {
            int i = (int)(short)(n & 0xFFFF);
            return i;
        }

        // ウインドウスタイルをセット
        public static int SetWindowStyle(IntPtr hwnd, int style) {
            int newStyle = GetWindowLong(hwnd, GWL_STYLE);
            newStyle = newStyle | style;
            return SetWindowLong(hwnd, GWL_STYLE, newStyle);
        }

        // ウインドウスタイルをリセット
        public static int ResetWindowStyle(IntPtr hwnd, int style) {
            int newStyle = GetWindowLong(hwnd, GWL_STYLE);
            newStyle = newStyle | ~style;
            return SetWindowLong(hwnd, GWL_STYLE, newStyle);
        }

        // ウインドウのスタイルを取得
        public static int GetWindowStyle(IntPtr hwnd, int mask) {
            int style = GetWindowLong(hwnd, GWL_STYLE);
            style = style & mask;
            return style;
        }

        [DllImport(ExternDll.Imm32)]
        public static extern int ImmGetProperty(IntPtr hKL, int fdwIndex);

        [DllImport(ExternDll.User32)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_DRAWFRAME = 0x0020;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOREPOSITION = 0x0200;
        public const int SWP_NOSENDCHANGING = 0x0400;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_STATECHANGED = 0x8000;

        public static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

        [DllImport(ExternDll.User32)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport(ExternDll.User32)]
        public static extern int GetSystemMetrics(int nIndex);
        //----------------------------------------------------------------------------
        //   ﾂﾘｰﾋﾞｭｰ操作用API
        //----------------------------------------------------------------------------
        public const int TRANSPARENT = 1,
        OPAQUE = 2,
        TME_HOVER = 0x00000001,
        TME_LEAVE = 0x00000002,
        TPM_LEFTBUTTON = 0x0000,
        TPM_RIGHTBUTTON = 0x0002,
        TPM_LEFTALIGN = 0x0000,
        TPM_RIGHTALIGN = 0x0008,
        TPM_VERTICAL = 0x0040,
        TV_FIRST = 0x1100,
        TBSTATE_CHECKED = 0x01,
        TBSTATE_ENABLED = 0x04,
        TBSTATE_HIDDEN = 0x08,
        TBSTATE_INDETERMINATE = 0x10,
        TBSTYLE_BUTTON = 0x00,
        TBSTYLE_SEP = 0x01,
        TBSTYLE_CHECK = 0x02,
        TBSTYLE_DROPDOWN = 0x08,
        TBSTYLE_TOOLTIPS = 0x0100,
        TBSTYLE_FLAT = 0x0800,
        TBSTYLE_LIST = 0x1000,
        TBSTYLE_EX_DRAWDDARROWS = 0x00000001,
        TB_ENABLEBUTTON = (0x0400 + 1),
        TB_ISBUTTONCHECKED = (0x0400 + 10),
        TB_ISBUTTONINDETERMINATE = (0x0400 + 13),
        TB_ADDBUTTONSA = (0x0400 + 20),
        TB_ADDBUTTONSW = (0x0400 + 68),
        TB_INSERTBUTTONA = (0x0400 + 21),
        TB_INSERTBUTTONW = (0x0400 + 67),
        TB_DELETEBUTTON = (0x0400 + 22),
        TB_GETBUTTON = (0x0400 + 23),
        TB_SAVERESTOREA = (0x0400 + 26),
        TB_SAVERESTOREW = (0x0400 + 76),
        TB_ADDSTRINGA = (0x0400 + 28),
        TB_ADDSTRINGW = (0x0400 + 77),
        TB_BUTTONSTRUCTSIZE = (0x0400 + 30),
        TB_SETBUTTONSIZE = (0x0400 + 31),
        TB_AUTOSIZE = (0x0400 + 33),
        TB_GETROWS = (0x0400 + 40),
        TB_GETBUTTONTEXTA = (0x0400 + 45),
        TB_GETBUTTONTEXTW = (0x0400 + 75),
        TB_SETIMAGELIST = (0x0400 + 48),
        TB_GETRECT = (0x0400 + 51),
        TB_GETBUTTONSIZE = (0x0400 + 58),
        TB_GETBUTTONINFOW = (0x0400 + 63),
        TB_SETBUTTONINFOW = (0x0400 + 64),
        TB_GETBUTTONINFOA = (0x0400 + 65),
        TB_SETBUTTONINFOA = (0x0400 + 66),
        TB_MAPACCELERATORA = (0x0400 + 78),
        TB_SETEXTENDEDSTYLE = (0x0400 + 84),
        TB_MAPACCELERATORW = (0x0400 + 90),
        TB_GETTOOLTIPS = (0x0400 + 35),
        TB_SETTOOLTIPS = (0x0400 + 36),
        TBIF_IMAGE = 0x00000001,
        TBIF_TEXT = 0x00000002,
        TBIF_STATE = 0x00000004,
        TBIF_STYLE = 0x00000008,
        TBIF_COMMAND = 0x00000020,
        TBIF_SIZE = 0x00000040,
        TBN_GETBUTTONINFOA = ((0 - 700) - 0),
        TBN_GETBUTTONINFOW = ((0 - 700) - 20),
        TBN_QUERYINSERT = ((0 - 700) - 6),
        TBN_DROPDOWN = ((0 - 700) - 10),
        TBN_HOTITEMCHANGE = ((0 - 700) - 13),
        TBN_GETDISPINFOA = ((0 - 700) - 16),
        TBN_GETDISPINFOW = ((0 - 700) - 17),
        TBN_GETINFOTIPA = ((0 - 700) - 18),
        TBN_GETINFOTIPW = ((0 - 700) - 19),
        TTS_ALWAYSTIP = 0x01,
        TTS_NOPREFIX = 0x02,
        TTS_NOANIMATE = 0x10,
        TTS_NOFADE = 0x20,
        TTS_BALLOON = 0x40,
            //TTI_NONE                =0,
            //TTI_INFO                =1, 
            TTI_WARNING = 2,
            //TTI_ERROR               =3, 
            TTF_IDISHWND = 0x0001,
        TTF_RTLREADING = 0x0004,
        TTF_TRACK = 0x0020,
        TTF_CENTERTIP = 0x0002,
        TTF_SUBCLASS = 0x0010,
        TTF_TRANSPARENT = 0x0100,
        TTF_ABSOLUTE = 0x0080,
        TTDT_AUTOMATIC = 0,
        TTDT_RESHOW = 1,
        TTDT_AUTOPOP = 2,
        TTDT_INITIAL = 3,
        TTM_TRACKACTIVATE = (0x0400 + 17),
        TTM_TRACKPOSITION = (0x0400 + 18),
        TTM_ACTIVATE = (0x0400 + 1),
        TTM_POP = (0x0400 + 28),
        TTM_ADJUSTRECT = (0x400 + 31),
        TTM_SETDELAYTIME = (0x0400 + 3),
        TTM_SETTITLEA = (WM_USER + 32),  // wParam = TTI_*, lParam = char* szTitle 
            TTM_SETTITLEW = (WM_USER + 33), // wParam = TTI_*, lParam = wchar* szTitle 
            TTM_ADDTOOLA = (0x0400 + 4),
        TTM_ADDTOOLW = (0x0400 + 50),
        TTM_DELTOOLA = (0x0400 + 5),
        TTM_DELTOOLW = (0x0400 + 51),
        TTM_NEWTOOLRECTA = (0x0400 + 6),
        TTM_NEWTOOLRECTW = (0x0400 + 52),
        TTM_RELAYEVENT = (0x0400 + 7),
        TTM_GETTIPBKCOLOR = (0x0400 + 22),
        TTM_SETTIPBKCOLOR = (0x0400 + 19),
        TTM_SETTIPTEXTCOLOR = (0x0400 + 20),
        TTM_GETTIPTEXTCOLOR = (0x0400 + 23),
        TTM_GETTOOLINFOA = (0x0400 + 8),
        TTM_GETTOOLINFOW = (0x0400 + 53),
        TTM_SETTOOLINFOA = (0x0400 + 9),
        TTM_SETTOOLINFOW = (0x0400 + 54),
        TTM_HITTESTA = (0x0400 + 10),
        TTM_HITTESTW = (0x0400 + 55),
        TTM_GETTEXTA = (0x0400 + 11),
        TTM_GETTEXTW = (0x0400 + 56),
        TTM_UPDATE = (0x0400 + 29),
        TTM_UPDATETIPTEXTA = (0x0400 + 12),
        TTM_UPDATETIPTEXTW = (0x0400 + 57),
        TTM_ENUMTOOLSA = (0x0400 + 14),
        TTM_ENUMTOOLSW = (0x0400 + 58),
        TTM_GETCURRENTTOOLA = (0x0400 + 15),
        TTM_GETCURRENTTOOLW = (0x0400 + 59),
        TTM_WINDOWFROMPOINT = (0x0400 + 16),
        TTM_GETDELAYTIME = (0x0400 + 21),
        TTM_SETMAXTIPWIDTH = (0x0400 + 24),
        TTN_GETDISPINFOA = ((0 - 520) - 0),
        TTN_GETDISPINFOW = ((0 - 520) - 10),
        TTN_SHOW = ((0 - 520) - 1),
        TTN_POP = ((0 - 520) - 2),
        TTN_NEEDTEXTA = ((0 - 520) - 0),
        TTN_NEEDTEXTW = ((0 - 520) - 10),
        TBS_AUTOTICKS = 0x0001,
        TBS_VERT = 0x0002,
        TBS_TOP = 0x0004,
        TBS_BOTTOM = 0x0000,
        TBS_BOTH = 0x0008,
        TBS_NOTICKS = 0x0010,
        TBM_GETPOS = (0x0400),
        TBM_SETTIC = (0x0400 + 4),
        TBM_SETPOS = (0x0400 + 5),
        TBM_SETRANGE = (0x0400 + 6),
        TBM_SETRANGEMIN = (0x0400 + 7),
        TBM_SETRANGEMAX = (0x0400 + 8),
        TBM_SETTICFREQ = (0x0400 + 20),
        TBM_SETPAGESIZE = (0x0400 + 21),
        TBM_SETLINESIZE = (0x0400 + 23),
        TB_LINEUP = 0,
        TB_LINEDOWN = 1,
        TB_PAGEUP = 2,
        TB_PAGEDOWN = 3,
        TB_THUMBPOSITION = 4,
        TB_THUMBTRACK = 5,
        TB_TOP = 6,
        TB_BOTTOM = 7,
        TB_ENDTRACK = 8,
        TVS_HASBUTTONS = 0x0001,
        TVS_HASLINES = 0x0002,
        TVS_LINESATROOT = 0x0004,
        TVS_EDITLABELS = 0x0008,
        TVS_SHOWSELALWAYS = 0x0020,
        TVS_RTLREADING = 0x0040,
        TVS_CHECKBOXES = 0x0100,
        TVS_TRACKSELECT = 0x0200,
        TVS_FULLROWSELECT = 0x1000,
        TVS_NONEVENHEIGHT = 0x4000,
        TVS_INFOTIP = 0x0800,
        TVS_NOTOOLTIPS = 0x0080,
        TVIF_TEXT = 0x0001,
        TVIF_IMAGE = 0x0002,
        TVIF_PARAM = 0x0004,
        TVIF_STATE = 0x0008,
        TVIF_HANDLE = 0x0010,
        TVIF_SELECTEDIMAGE = 0x0020,
        TVIS_SELECTED = 0x0002,
        TVIS_EXPANDED = 0x0020,
        TVIS_EXPANDEDONCE = 0x0040,
        TVIS_STATEIMAGEMASK = 0xF000,
        TVI_ROOT = (unchecked((int)0xFFFF0000)),
        TVI_FIRST = (unchecked((int)0xFFFF0001)),
        TVM_INSERTITEMA = (0x1100 + 0),
        TVM_INSERTITEMW = (0x1100 + 50),
        TVM_DELETEITEM = (0x1100 + 1),
        TVM_EXPAND = (0x1100 + 2),
        TVE_COLLAPSE = 0x0001,
        TVE_EXPAND = 0x0002,
        TVM_GETITEMRECT = (0x1100 + 4),
        TVM_GETINDENT = (0x1100 + 6),
        TVM_SETINDENT = (0x1100 + 7),
        TVM_GETIMAGELIST = (0x1100 + 8),
        TVM_SETIMAGELIST = (0x1100 + 9),
        TVM_GETNEXTITEM = (0x1100 + 10),
        TVGN_NEXT = 0x0001,
        TVGN_PREVIOUS = 0x0002,
        TVGN_PARENT = 0x0003,
        TVGN_FIRSTVISIBLE = 0x0005,
        TVGN_NEXTVISIBLE = 0x0006,
        TVGN_PREVIOUSVISIBLE = 0x0007,
        TVGN_DROPHILITE = 0x0008,
        TVGN_CARET = 0x0009,
        TVM_SELECTITEM = (0x1100 + 11),
        TVM_GETITEMA = (0x1100 + 12),
        TVM_GETITEMW = (0x1100 + 62),
        TVM_SETITEMA = (0x1100 + 13),
        TVM_SETITEMW = (0x1100 + 63),
        TVM_EDITLABELA = (0x1100 + 14),
        TVM_EDITLABELW = (0x1100 + 65),
        TVM_GETEDITCONTROL = (0x1100 + 15),
        TVM_GETVISIBLECOUNT = (0x1100 + 16),
        TVM_HITTEST = (0x1100 + 17),
        TVM_ENSUREVISIBLE = (0x1100 + 20),
        TVM_ENDEDITLABELNOW = (0x1100 + 22),
        TVM_GETISEARCHSTRINGA = (0x1100 + 23),
        TVM_GETISEARCHSTRINGW = (0x1100 + 64),
        TVM_SETITEMHEIGHT = (0x1100 + 27),
        TVM_GETITEMHEIGHT = (0x1100 + 28),
        TVN_SELCHANGINGA = ((0 - 400) - 1),
        TVN_SELCHANGINGW = ((0 - 400) - 50),
        TVN_GETINFOTIPA = ((0 - 400) - 13),
        TVN_GETINFOTIPW = ((0 - 400) - 14),
        TVN_SELCHANGEDA = ((0 - 400) - 2),
        TVN_SELCHANGEDW = ((0 - 400) - 51),
        TVC_UNKNOWN = 0x0000,
        TVC_BYMOUSE = 0x0001,
        TVC_BYKEYBOARD = 0x0002,
        TVN_GETDISPINFOA = ((0 - 400) - 3),
        TVN_GETDISPINFOW = ((0 - 400) - 52),
        TVN_SETDISPINFOA = ((0 - 400) - 4),
        TVN_SETDISPINFOW = ((0 - 400) - 53),
        TVN_ITEMEXPANDINGA = ((0 - 400) - 5),
        TVN_ITEMEXPANDINGW = ((0 - 400) - 54),
        TVN_ITEMEXPANDEDA = ((0 - 400) - 6),
        TVN_ITEMEXPANDEDW = ((0 - 400) - 55),
        TVN_BEGINDRAGA = ((0 - 400) - 7),
        TVN_BEGINDRAGW = ((0 - 400) - 56),
        TVN_BEGINRDRAGA = ((0 - 400) - 8),
        TVN_BEGINRDRAGW = ((0 - 400) - 57),
        TVN_BEGINLABELEDITA = ((0 - 400) - 10),
        TVN_BEGINLABELEDITW = ((0 - 400) - 59),
        TVN_ENDLABELEDITA = ((0 - 400) - 11),
        TVN_ENDLABELEDITW = ((0 - 400) - 60),

        TCIF_TEXT = 0x0001,
        TCIF_IMAGE = 0x0002,
        TCN_SELCHANGE = ((0 - 550) - 1),
        TCN_SELCHANGING = ((0 - 550) - 2),
        TBSTYLE_WRAPPABLE = 0x0200,
        TVM_SETBKCOLOR = (TV_FIRST + 29),
        TVM_SETTEXTCOLOR = (TV_FIRST + 30),
        TYMED_NULL = 0,
        TVM_GETLINECOLOR = (TV_FIRST + 41),
        TVM_SETLINECOLOR = (TV_FIRST + 40),
        TVM_SETTOOLTIPS = (TV_FIRST + 24),
        TVSIL_STATE = 2,
        TVM_SORTCHILDRENCB = (TV_FIRST + 21),
        TVM_CREATEDRAGIMAGE = TV_FIRST + 18,
        TMPF_FIXED_PITCH = 0x01;

        public const int TVIS_OVERLAYMASK = 0xf00;
        public const int
         TVHT_NOWHERE = 0x0001,
         TVHT_ONITEMICON = 0x0002,
         TVHT_ONITEMLABEL = 0x0004,
         TVHT_ONITEMINDENT = 0x0008,
         TVHT_ONITEMBUTTON = 0x0010,
         TVHT_ONITEMRIGHT = 0x0020,
         TVHT_ONITEMSTATEICON = 0x0040,
         TVHT_ABOVE = 0x0100,
         TVHT_BELOW = 0x0200,
         TVHT_TORIGHT = 0x0400,
         TVHT_TOLEFT = 0x0800,
         TVHT_ONITEM = TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON
         ;
        //----------------------------------------------------------------------------
        //   ﾋﾞｯﾄﾏｯﾌﾟ操作用API
        //----------------------------------------------------------------------------
        public const int SRCCOPY = 0xcc0020;
        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

        //----------------------------------------------------------------------------
        //   ｲﾒｰｼﾞﾘｽﾄ操作用API
        //----------------------------------------------------------------------------
        public const int ILC_COLOR16 = 0x10;
        public const int ILC_MASK = 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct INITCOMMONCONTROLSEX
        {
            public int dwSize;
            public int dwICC;
        }

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto)]
        public static extern IntPtr ImageList_Create(int cx, int cy, int Flags, int cInitial, int cGrow);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto)]
        public static extern int ImageList_Add(IntPtr himl, IntPtr hbmImage, IntPtr hbmMask);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto)]
        public static extern bool ImageList_SetOverlayImage(IntPtr himl, int iImage, int iOverlay);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWndRef, int Msg, IntPtr wParam, ref TVITEM lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWndRef, int Msg, IntPtr wParam, ref RECT lParam);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern bool ImageList_DragShowNolock(bool bShow);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern bool ImageList_Draw(IntPtr himl, int i, IntPtr hdcDst, int x, int y, int fStyle);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int Flags);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern int ImageList_ReplaceIcon(IntPtr himl, int i, IntPtr hIcon);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern bool ImageList_SetDragCursorImage(IntPtr himl, int iDrag, int dxHotspot, int dyHotspot);

        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern int InitCommonControlsEx(ref INITCOMMONCONTROLSEX lpInitCtrls);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
            //public int iIntegral;
        }

        public const int
            LOGPIXELSX = 88,
            LOGPIXELSY = 90;

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport(ExternDll.Gdi32)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        [StructLayout(LayoutKind.Sequential)]
        public struct DRAWITEMSTRUCT
        {
            public int CtlType;
            public int CtlID;
            public int itemID;
            public int itemAction;
            public int itemState;
            public IntPtr hwndItem;
            public IntPtr hDC;
            public RECT rcItem;
            public IntPtr itemData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LV_ITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            internal int lParam;
            internal int iIndent;
        }

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hwndRef, int Msg, IntPtr wParam, ref LV_ITEM lParam);

        public static IntPtr ListView_SetItemState(IntPtr hwndRef, int index, int data, int mask) {
            LV_ITEM item = default(LV_ITEM);
            item.stateMask = mask;
            item.state = data;
            return SendMessage(hwndRef, LVM_SETITEMSTATE, new IntPtr(index), ref item);
        }

        public static int
        LVNI_ALL = 0x0000,
        LVNI_FOCUSED = 0x0001,
        LVNI_SELECTED = 0x0002,
        LVNI_CUT = 0x0004,
        LVNI_DROPHILITED = 0x0008,
        LVNI_ABOVE = 0x0100,
        LVNI_BELOW = 0x0200,
        LVNI_TOLEFT = 0x0400,
        LVNI_TORIGHT = 0x0800;

        public static int ListView_GetNextItem(IntPtr hwndRef, int index, int flags) {
            IntPtr LParam = new IntPtr(MAKEDWORD(flags, 0));
            IntPtr result = SendMessage(hwndRef, LVM_GETNEXTITEM, new IntPtr(index), LParam);
            return (int)result;
        }

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetFocus();

        [DllImport(ExternDll.User32)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        public const int
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_TAB = 0x09,
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_CAPITAL = 0x14,
        VK_KANA = 0x15,
        VK_ESCAPE = 0x1B,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        VK_INSERT = 0x002D,
        VK_DELETE = 0x002E,
        VK_D = 0x44,
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C;

        [DllImport(ExternDll.User32)]
        public static extern short GetKeyState(int nVirtKey);

        // Drag & Drop
        [DllImport(ExternDll.Comctl32, CharSet = CharSet.Unicode)]
        public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int iTrack, int dxHotspot, int dyHotspot);

        [DllImport(ExternDll.Comctl32)]
        public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

        [DllImport(ExternDll.Comctl32)]
        public static extern bool ImageList_DragLeave(IntPtr hwndLock);

        [DllImport(ExternDll.Comctl32)]
        public static extern bool ImageList_DragMove(int x, int y);

        [DllImport(ExternDll.Comctl32)]
        public static extern void ImageList_EndDrag();

        [DllImport(ExternDll.Comctl32)]
        public static extern bool ImageList_Destroy(IntPtr himl);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(ExternDll.User32)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);
        [DllImport(ExternDll.User32)]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport(ExternDll.User32)]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        public const int
            CP_932 = 932,
            WC_DISCARDNS = 0x00000010,
            WC_SEPCHARS = 0x00000020,
            WC_DEFAULTCHAR = 0x00000040,
            WC_ERR_INVALID_CHARS = 0x00000080,
            WC_COMPOSITECHECK = 0x00000200,
            WC_NO_BEST_FIT_CHARS = 0x00000400;

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WideCharToMultiByte(
                    int CodePage,
                    int dwFlags,
                    string lpWideCharStr,
                    int cchWideChar,
                    StringBuilder lpMultibyteStr,
                    int cchMultibyte,
                    ref char lpDefaultChar,
                    out bool lpUsedDefaultChar);

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WideCharToMultiByte(
                    int CodePage,
                    int dwFlags,
                    ref char lpWideChar,
                    int cchWideChar,
                    byte[] lpMultibyteStr,
                    int cchMultibyte,
                    ref char lpDefaultChar,
                    out bool lpUsedDefaultChar);

        public const int
            LCMAP_LOWERCASE = 0x00000100,  // lower case letters
            LCMAP_UPPERCASE = 0x00000200,  // upper case letters
            LCMAP_SORTKEY = 0x00000400,  // WC sort key (normalize)
            LCMAP_BYTEREV = 0x00000800,  // byte reversal
            LCMAP_HIRAGANA = 0x00100000,  // map katakana to hiragana
            LCMAP_KATAKANA = 0x00200000,  // map hiragana to katakana
            LCMAP_HALFWIDTH = 0x00400000,  // map double byte to single byte
            LCMAP_FULLWIDTH = 0x00800000,  // map single byte to double byte
            LCMAP_LINGUISTIC_CASING = 0x01000000,  // use linguistic rules for casing
            LCMAP_SIMPLIFIED_CHINESE = 0x02000000,  // map traditional chinese to simplified chinese
            LCMAP_TRADITIONAL_CHINESE = 0x04000000;  // map simplified chinese to traditional chinese


        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int LCMapString(
                                int Locale,
                                int dwMapFlags,
                                [MarshalAs(UnmanagedType.LPWStr)] string lpSrcStr,
                                int cchSrc,
                                IntPtr lpDestStr,
                                int cchDest);

        [DllImport(ExternDll.Kernel32)]
        public static extern uint GetTickCount();

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetMenu(IntPtr hWnd);
        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport(ExternDll.User32)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO
        {
            public int cbSize;
            public int fMask;
            public int fType;
            public int fState;
            public int wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public IntPtr dwTypeData;
            public int cch;
            public IntPtr hbmpItem;
        }

        // Values for the fMask parameter
        public const int MIIM_BITMAP = 0x00000080;
        public const int MIIM_CHECKMARKS = 0x00000008;
        public const int MIIM_DATA = 0x00000020;
        public const int MIIM_FTYPE = 0x00000100;
        public const int MIIM_ID = 0x00000002;
        public const int MIIM_STATE = 0x00000001;
        public const int MIIM_STRING = 0x00000040;
        public const int MIIM_SUBMENU = 0x00000004;
        public const int MIIM_TYPE = 0x00000010;

        public const int HBMMENU_CALLBACK = -1;
        public const int HBMMENU_MBAR_CLOSE = 5;
        public const int HBMMENU_MBAR_CLOSE_D = 6;
        public const int HBMMENU_MBAR_MINIMIZE = 3;
        public const int HBMMENU_MBAR_MINIMIZE_D = 7;
        public const int HBMMENU_MBAR_RESTORE = 2;
        public const int HBMMENU_POPUP_CLOSE = 8;
        public const int HBMMENU_POPUP_MAXIMIZE = 10;
        public const int HBMMENU_POPUP_MINIMIZE = 11;
        public const int HBMMENU_POPUP_RESTORE = 9;
        public const int HBMMENU_SYSTEM = 1;

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport(ExternDll.User32)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto)]
        public static extern int GetWindowsDirectory(StringBuilder buffer, int length);

        public const int NIM_ADD = 0x00000000,
        NIM_MODIFY = 0x00000001,
        NIM_DELETE = 0x00000002,
        NIF_MESSAGE = 0x00000001,
        NIM_SETVERSION = 0x00000004,
        NIF_ICON = 0x00000002,
        NIF_INFO = 0x00000010,
        NIF_TIP = 0x00000004,
        NIIF_NONE = 0x00000000,
        NIIF_INFO = 0x00000001,
        NIIF_WARNING = 0x00000002,
        NIIF_ERROR = 0x00000003,
        NIN_BALLOONSHOW = (WM_USER + 2),
        NIN_BALLOONHIDE = (WM_USER + 3),
        NIN_BALLOONTIMEOUT = (WM_USER + 4),
        NIN_BALLOONUSERCLICK = (WM_USER + 5),
        NFR_ANSI = 1,
        NFR_UNICODE = 2,
        NM_CLICK = ((0 - 0) - 2),
        NM_DBLCLK = ((0 - 0) - 3),
        NM_RCLICK = ((0 - 0) - 5),
        NM_RDBLCLK = ((0 - 0) - 6),
        NM_CUSTOMDRAW = ((0 - 0) - 12),
        NM_RELEASEDCAPTURE = ((0 - 0) - 16),
        NONANTIALIASED_QUALITY = 3;

        public const int
        CDDS_PREPAINT = 0x00000001,
        CDDS_POSTPAINT = 0x00000002,
        CDDS_ITEM = 0x00010000,
        CDDS_SUBITEM = 0x00020000,
        CDDS_ITEMPREPAINT = (0x00010000 | 0x00000001),
        CDDS_ITEMPOSTPAINT = (0x00010000 | 0x00000002)
        ;

        public const int
        CDRF_DODEFAULT = 0x00000000,
        CDRF_NEWFONT = 0x00000002,
        CDRF_SKIPDEFAULT = 0x00000004,
        CDRF_NOTIFYPOSTPAINT = 0x00000010,
        CDRF_NOTIFYITEMDRAW = 0x00000020,
        CDRF_NOTIFYSUBITEMDRAW = CDRF_NOTIFYITEMDRAW
        ;


        [StructLayout(LayoutKind.Sequential)]
        public struct NMTVCUSTOMDRAW
        {
            public NMCUSTOMDRAW nmcd;
            public int clrText;
            public int clrTextBk;
            public int iLevel;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMCUSTOMDRAW
        {
            public NMHDR nmcd;
            public int dwDrawStage;
            public IntPtr hdc;
            public RECT rc;
            public IntPtr dwItemSpec;
            public int uItemState;
            public IntPtr lItemlParam;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom; //This is declared as UINT_PTR in winuser.h 
            public int code;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class TV_HITTESTINFO
        {
            public int pt_x;
            public int pt_y;
            public int flags = 0;
            public IntPtr hItem = IntPtr.Zero;
        }

        public static IntPtr TreeView_CreateDragImage(TreeView view, TreeNode node) {
            return SendMessage(view.Handle,
                               TVM_CREATEDRAGIMAGE,
                               IntPtr.Zero,
                               node.Handle);
        }

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr GetSysColorBrush(int nIndex);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SetBkColor(IntPtr hdc, int crColor);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern uint SetTextColor(IntPtr hdc, int crColor);

        [DllImport(ExternDll.Gdi32)]
        public static extern int SetBkMode(IntPtr hdc, int iBkMode);

        [DllImport(ExternDll.Gdi32)]
        public static extern int GetBkMode(IntPtr hdc);

        public enum StockObjects
        {
            WHITE_BRUSH = 0,
            LTGRAY_BRUSH = 1,
            GRAY_BRUSH = 2,
            DKGRAY_BRUSH = 3,
            BLACK_BRUSH = 4,
            NULL_BRUSH = 5,
            HOLLOW_BRUSH = NULL_BRUSH,
            WHITE_PEN = 6,
            BLACK_PEN = 7,
            NULL_PEN = 8,
            OEM_FIXED_FONT = 10,
            ANSI_FIXED_FONT = 11,
            ANSI_VAR_FONT = 12,
            SYSTEM_FONT = 13,
            DEVICE_DEFAULT_FONT = 14,
            DEFAULT_PALETTE = 15,
            SYSTEM_FIXED_FONT = 16,
            DEFAULT_GUI_FONT = 17,
            DC_BRUSH = 18,
            DC_PEN = 19,
        }

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr GetStockObject(StockObjects fnObject);

        public const int
            SW_HIDE = 0,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11;

        [DllImport(ExternDll.User32)]
        public static extern IntPtr ShowWindow(IntPtr hwnd, int cmdShow);

        public struct HitTestValues
        {
            public const int HTERROR = -2;
            public const int HTTRANSPARENT = -1;
            public const int HTNOWHERE = 0;
            public const int HTCLIENT = 1;
            public const int HTCAPTION = 2;
            public const int HTSYSMENU = 3;
            public const int HTGROWBOX = 4;
            public const int HTMENU = 5;
            public const int HTHSCROLL = 6;
            public const int HTVSCROLL = 7;
            public const int HTMINBUTTON = 8;
            public const int HTMAXBUTTON = 9;
            public const int HTLEFT = 10;
            public const int HTRIGHT = 11;
            public const int HTTOP = 12;
            public const int HTTOPLEFT = 13;
            public const int HTTOPRIGHT = 14;
            public const int HTBOTTOM = 15;
            public const int HTBOTTOMLEFT = 16;
            public const int HTBOTTOMRIGHT = 17;
            public const int HTBORDER = 18;
            public const int HTOBJECT = 19;
            public const int HTCLOSE = 20;
            public const int HTHELP = 21;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "BeginPaint", CharSet = CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "EndPaint", CharSet = CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class WINDOWPLACEMENT
        {
            public int Length;
            public int Flags;
            public int ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;

            public WINDOWPLACEMENT() {
                this.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            }
        }

        [DllImport(ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, WINDOWPLACEMENT lpwndpl);

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;

            public override string ToString() {
                return "(WINDOWPOS) hwnd=0x" + Convert.ToString((long)hwnd, 16)
                + " hwndInsertAfter=0x" + Convert.ToString((long)hwndInsertAfter, 16)
                + " x=" + Convert.ToString(x, 10)
                + " y=" + Convert.ToString(y, 10)
                + " cx=" + Convert.ToString(cx, 10)
                + " cy=" + Convert.ToString(cy, 10)
                + " flags=0x" + Convert.ToString(flags, 16);
            }
        }

        public enum SBOrientation : int
        {
            SB_HORZ = 0x0,
            SB_VERT = 0x1,
            SB_CTL = 0x2,
            SB_BOTH = 0x3
        }

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar);


        [StructLayout(LayoutKind.Sequential)]
        public class ScrollBarInfo
        {
            public int cbSize;
            public RECT rcScrollBar;
            public int dxyLineButton;
            public int xyThumbTop;
            public int xyThumbBottom;
            public int reserved;
            public int rgstate0;
            public int rgstate1;
            public int rgstate2;
            public int rgstate3;
            public int rgstate4;
            public int rgstate5;

            public ScrollBarInfo() {
                this.cbSize = Marshal.SizeOf(this);
            }
        }

        public const uint OBJID_HSCROLL = 0xFFFFFFFA;
        public const uint OBJID_VSCROLL = 0xFFFFFFFB;
        public const uint OBJID_CLIENT = 0xFFFFFFFC;

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool GetScrollBarInfo(IntPtr hWnd, uint idObject, ScrollBarInfo psbi);


        [StructLayout(LayoutKind.Sequential)]
        public class ScrollInfo
        {
            public int cbSize = Marshal.SizeOf(typeof(ScrollInfo));
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;

            public ScrollInfo() {
            }

            public ScrollInfo(int mask, int min, int max, int page, int pos) {
                fMask = mask;
                nMin = min;
                nMax = max;
                nPage = page;
                nPos = pos;
            }
        }

        public const int
        WSB_PROP_CYVSCROLL = 0x1,
        WSB_PROP_CXHSCROLL = 0x2,
        WSB_PROP_CYHSCROLL = 0x4,
        WSB_PROP_CXVSCROLL = 0x8,
        WSB_PROP_CXHTHUMB = 0x10,
        WSB_PROP_CYVTHUMB = 0x20,
        WSB_PROP_VBKGCOLOR = 0x40,
        WSB_PROP_HBKGCOLOR = 0x80,
        WSB_PROP_VSTYLE = 0x100,
        WSB_PROP_HSTYLE = 0x200,
        WSB_PROP_WINSTYLE = 0x400,
        WSB_PROP_PALETTE = 0x800,
        WSB_PROP_MASK = 0xfff;

        public const int
        FSB_FLAT_MODE = 2,
        FSB_ENCARTA_MODE = 1,
        FSB_REGULAR_MODE = 0;

        public const int
        SB_BOTH = 3,
        SB_CTL = 2,
        SB_HORZ = 0,
        SB_VERT = 1,

        SIF_RANGE = 0x1,
        SIF_PAGE = 0x2,
        SIF_POS = 0x4,
        SIF_DISABLENOSCROLL = 0x8,
        SIF_TRACKPOS = 0x10,
        SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_EnableScrollBar(IntPtr hwnd, int wSBFlags, int wArrows);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_GetScrollInfo(IntPtr hwnd, int code, ScrollInfo lpsi);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern int FlatSB_GetScrollPos(IntPtr hwnd, int code);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_GetScrollProp(IntPtr hwnd, int propIndex, ref int pValue);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_GetScrollPropPtr(IntPtr hwnd, int propIndex, ref int pValue);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_GetScrollRange(IntPtr hwnd, int code, ref int lpMinPos, ref int lpMaxPos);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern int FlatSB_SetScrollInfo(IntPtr hwnd, int code, ScrollInfo lpsi, bool fRedraw);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern int FlatSB_SetScrollPos(IntPtr hwnd, int code, int nPos, bool fRedraw);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_SetScrollProp(IntPtr hwnd, int index, ref int newValue, bool fRedraw);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern int FlatSB_SetScrollRange(IntPtr hwnd, int code, int nMinPos, int nMaxPos, bool fRedraw);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern bool FlatSB_ShowScrollBar(IntPtr hwnd, int code, bool fShow);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern int InitializeFlatSB(IntPtr hwnd);

        [DllImport(ExternDll.Comctl32, SetLastError = true)]
        public static extern int UninitializeFlatSB(IntPtr hwnd);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr SelectPalette(IntPtr hdc, IntPtr hpal, int bForceBackground);

        [DllImport(ExternDll.Gdi32)]
        public static extern int RealizePalette(IntPtr hDC);

        [DllImport(ExternDll.Gdi32)]
        public static extern int SetPixel(IntPtr hdc, int X, int Y, int crColor);

        [DllImport(ExternDll.Gdi32)]
        public static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);


        //typedef struct tagDATETIMEPICKERINFO {
        //  DWORD cbSize;
        //  RECT  rcCheck;
        //  DWORD stateCheck;
        //  RECT  rcButton;
        //  DWORD stateButton;
        //  HWND  hwndEdit;
        //  HWND  hwndUD;
        //  HWND  hwndDropDown;
        //} DATETIMEPICKERINFO, *LPDATETIMEPICKERINFO;

        [StructLayout(LayoutKind.Sequential)]
        public class DateTimePickerInfo
        {
            public int cbSize;
            public RECT rcCheck;
            public int stateCheck;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndEdit;
            public IntPtr hwndUD;
            public IntPtr hwndDropDown;
            public DateTimePickerInfo() {
                this.cbSize = Marshal.SizeOf(this);
            }
        }

        public const int
            DTM_FIRST = 0x1000,
            DTM_CLOSEMONTHCAL = DTM_FIRST + 13,
            DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14,
            DTM_GETIDEALSIZE = DTM_FIRST + 15,
            DTM_GETMCCOLOR = DTM_FIRST + 7,
            DTM_GETMCFONT = DTM_FIRST + 10,
            DTM_GETMCSTYLE = DTM_FIRST + 12,
            DTM_GETMONTHCAL = DTM_FIRST + 8,
            DTM_GETRANGE = DTM_FIRST + 3,
            DTM_GETSYSTEMTIME = DTM_FIRST + 1,
            DTM_SETFORMAT = DTM_FIRST + 50,
            DTM_SETMCCOLOR = DTM_FIRST + 6,
            DTM_SETMCFONT = DTM_FIRST + 9,
            DTM_SETMCSTYLE = DTM_FIRST + 11,
            DTM_SETRANGE = DTM_FIRST + 4,
            DTM_SETSYSTEMTIME = DTM_FIRST + 2;

        public const int
            MCSC_BACKGROUND = 0,
            MCSC_TEXT = 1,
            MCSC_TITLEBK = 2,
            MCSC_TITLETEXT = 3,
            MCSC_MONTHBK = 4,
            MCSC_TRAILINGTEXT = 5;

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public extern static IntPtr SendMessage(IntPtr hwndRef, int wMsg, IntPtr wParam, DateTimePickerInfo lParam);

        // WM_PRINT
        public const int
            PRF_CHECKVISIBLE = 0x00000001,
            PRF_NONCLIENT = 0x00000002,
            PRF_CLIENT = 0x00000004,
            PRF_ERASEBKGND = 0x00000008,
            PRF_CHILDREN = 0x00000010,
            PRF_OWNED = 0x00000020;

        [StructLayout(LayoutKind.Sequential)]
        public class COMRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public COMRECT() {
            }

            public COMRECT(System.Drawing.Rectangle r) {
                this.left = r.X;
                this.top = r.Y;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }


            public COMRECT(int left, int top, int right, int bottom) {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /* Unused
            public RECT ToRECT() {
                return new RECT(left, top, right, bottom);
            }
            */

            public static COMRECT FromXYWH(int x, int y, int width, int height) {
                return new COMRECT(x, y, x + width, y + height);
            }

            public override string ToString() {
                return "Left = " + left + " Top " + top + " Right = " + right + " Bottom = " + bottom;
            }
        }

        // GDI+ で生成するハーフトーンの品質を向上
        public static IntPtr SetUpPalette(IntPtr dc, bool force, bool realizePalette) {
            IntPtr halftonePalette = Graphics.GetHalftonePalette();
            IntPtr result = SelectPalette(dc, halftonePalette, (force ? 0 : 1));
            if (result != IntPtr.Zero && realizePalette) {
                RealizePalette(dc);
            }
            return result;
        }

        // WM_PRINT を送って hdc に描画してもらう
        public static void PaintDC(Control con, IntPtr hdc) {
            SendMessage(con.Handle, WM_PRINT, hdc,
                    (IntPtr)(PRF_CHILDREN | PRF_CLIENT | PRF_ERASEBKGND));
        }

        [DllImport(ExternDll.User32)]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0;
            public RECT rgrc1;
            public RECT rgrc2;
            public WINDOWPOS lppos;
        }

        public const int
            AW_NONE = 0,
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000;


        [DllImport(ExternDll.User32)]
        public static extern bool AnimateWindow(IntPtr hWnd, int dwTimeMSec, int dwFlags);

        public const int MAX_PATH = 260;

        [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetTempFileName(string lpPathName, string lpPrefixString, int uUnique, [Out] StringBuilder lpTempFileName);

        public static string GetTempFileName(string pathName, string prefix) {
            StringBuilder sb = new StringBuilder(MAX_PATH);
            if (GetTempFileName(pathName, prefix, 0, sb) == 0) {
                return string.Empty;
            }
            return sb.ToString();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public IntPtr hwnd;
            public int wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCTEX
        {
            public MOUSEHOOKSTRUCT mouseHookStruct;
            public int mouseData;
        }

        // フックタイプ
        public const int
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14;

        // フックコード
        public const int
        HC_ACTION = 0,
        HC_GETNEXT = 1,
        HC_SKIP = 2,
        HC_NOREMOVE = 3,
        HC_NOREM = HC_NOREMOVE,
        HC_SYSMODALON = 4,
        HC_SYSMODALOFF = 5;

        // CBTProc フックコード
        public const int
        HCBT_MOVESIZE = 0,
        HCBT_MINMAX = 1,
        HCBT_QS = 2,
        HCBT_CREATEWND = 3,
        HCBT_DESTROYWND = 4,
        HCBT_ACTIVATE = 5,
        HCBT_CLICKSKIPPED = 6,
        HCBT_KEYSKIPPED = 7,
        HCBT_SYSCOMMAND = 8,
        HCBT_SETFOCUS = 9;

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, uint threadId);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.Kernel32)]
        public static extern uint GetCurrentThreadId();

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public int message;
            public IntPtr hwnd;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CBT_CREATEWND
        {
            // CREATESTRUCT*
            public IntPtr lpcs;
            public IntPtr hwndInsertAfter;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CREATESTRUCT
        {
            public IntPtr lpCreateParams;
            public IntPtr hInstance;
            public IntPtr hMenu;
            public IntPtr hwndParent;
            public int cy;
            public int cx;
            public int y;
            public int x;
            public int style;
            public IntPtr lpszName;
            public IntPtr lpszClass;
            public int dwExStyle;
        }

        #region EnumWindows

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private static bool EnumWindowCallback(IntPtr handle, IntPtr pointer) {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = (List<IntPtr>)gch.Target;
            list.Add(handle);
            return true;
        }

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumThreadWindows(uint dwThreadId, EnumWindowsProc lpfn, IntPtr lParam);

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lparam);

        public static List<IntPtr> GetChildWindows(Form prantForm) {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try {
                EnumWindowsProc childProc = new EnumWindowsProc(EnumWindowCallback);
                EnumChildWindows(prantForm.Handle, childProc, GCHandle.ToIntPtr(listHandle));
            } finally {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static List<IntPtr> GetThreadWindows() {
            List<IntPtr> lst = GetThreadWindows(GetCurrentThreadId());
            return GetThreadWindows(GetCurrentThreadId());
        }

        public static List<IntPtr> GetThreadWindows(uint dwThreadId) {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try {
                EnumWindowsProc childProc = new EnumWindowsProc(EnumWindowCallback);
                EnumThreadWindows(dwThreadId, childProc, GCHandle.ToIntPtr(listHandle));
            } finally {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        private class OwnedArgument
        {
            public List<IntPtr> OwnedList;
            public IntPtr OwnerHandle;
        }

        /// <summary>
        /// 指定したウインドウをオーナーとするウインドウを列挙する
        /// </summary>
        public static List<IntPtr> GetOwnedWindows(IntPtr ownerHandle) {
            OwnedArgument owned = new OwnedArgument();
            owned.OwnedList = new List<IntPtr>();
            owned.OwnerHandle = ownerHandle;
            GCHandle listHandle = GCHandle.Alloc(owned);
            try {
                EnumWindowsProc childProc = new EnumWindowsProc(EnumOwnedCallback);
                EnumWindows(childProc, GCHandle.ToIntPtr(listHandle));
            } finally {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return owned.OwnedList;
        }

        /// <summary>
        /// GetOwnedWindows の CallBack
        /// </summary>
        private static bool EnumOwnedCallback(IntPtr handle, IntPtr pointer) {
            // オーナーウインドウを取得
            IntPtr hwndOwner = GetWindow(handle, GW_OWNER);

            // ポインタから OwnedArgument を取得
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            OwnedArgument owned = (OwnedArgument)gch.Target;

            // オーナーハンドルが等しいもののみ追加
            if (hwndOwner == owned.OwnerHandle) {
                owned.OwnedList.Add(handle);
            }
            return true;
        }

        #endregion

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport(ExternDll.User32, EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport(ExternDll.User32, EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex) {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(ExternDll.User32)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        public const int
            HTBORDER = 18,	    //	サイズ変更される境界線を持たないウィンドウ境界内にあります
            HTBOTTOM = 15,	    //	ウィンドウ境界の下端の線上にあります
            HTBOTTOMLEFT = 16,	//	ウィンドウ境界の左下隅にあります
            HTBOTTOMRIGHT = 17,	//	ウィンドウ境界の右下隅にあります
            HTCAPTION = 2,	    //	タイトル バーの領域内にあります
            HTCLIENT = 1,	    //	クライアント領域内にあります
            HTCLOSE = 20,	    //	In a Close button.
            HTERROR = -2,	    //	スクリーンの背景上またはウィンドウの区切り線上にあります
            HTGROWBOX = 4,	    //	サイズ ボックス内にあります
            HTHELP = 21,	    //	In a Help button.
            HTHSCROLL = 6,	    //	水平スクロール バー内にあります
            HTLEFT = 10,	    //	ウィンドウ境界の左端の線上にあります
            HTMAXBUTTON = 9,	//	最大表示ボタン内にあります
            HTMENU = 5,	        //	In a menu.
            HTMINBUTTON = 8,	//	アイコン化ボタン内にあります
            HTNOWHERE = 0,	    //	スクリーンの背景かウィンドウの区切り線上にあります
            HTREDUCE = 8,	    //	アイコン化ボタン内にあります
            HTRIGHT = 11,	    //	ウィンドウ境界の右端の線上にあります
            HTSIZE = 4,	        //	サイズ ボックス内にあります (HTGROWBOX と同じです)
            HTSYSMENU = 3,	    //	コントロール メニュー内、または子ウィンドウのクローズ ボタン内にあります
            HTTOP = 12,	        //	ウィンドウ境界の上端の線上にあります
            HTTOPLEFT = 13,	    //	ウィンドウ境界の左上隅にあります
            HTTOPRIGHT = 14,	//	ウィンドウ境界の右上隅にあります
            HTTRANSPARENT = -1,	//	現在ほかのウィンドウに覆い隠されているウィンドウ内にあります
            HTVSCROLL = 7,	    //	垂直スクロール バー内にあります
            HTZOOM = 9;	        //	最大表示ボタン内にあります

        #region PrevInstance

        public const int
        STANDARD_RIGHTS_REQUIRED = 0x000F0000,
        STANDARD_RIGHTS_READ = 0x00020000,
        TOKEN_ASSIGN_PRIMARY = 0x0001,
        TOKEN_DUPLICATE = 0x0002,
        TOKEN_IMPERSONATE = 0x0004,
        TOKEN_QUERY = 0x0008,
        TOKEN_QUERY_SOURCE = 0x0010,
        TOKEN_ADJUST_PRIVILEGES = 0x0020,
        TOKEN_ADJUST_GROUPS = 0x0040,
        TOKEN_ADJUST_DEFAULT = 0x0080,
        TOKEN_ADJUST_SESSIONID = 0x0100,
        PROCESS_QUERY_INFORMATION = 0x0400,
        TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY),
        TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
        TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
        TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
        TOKEN_ADJUST_SESSIONID);

        [DllImport(ExternDll.Kernel32)]
        public static extern IntPtr OpenProcess(
             uint processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport(ExternDll.Advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenProcessToken(
            IntPtr processHandle,
            uint desiredAcces,
            out IntPtr tokenHandle
        );

        public static SafeProcessTokenHandle OpenProcessToken(IntPtr processHandle, uint desiredAcces) {
            IntPtr hToken;
            if (OpenProcessToken(processHandle, desiredAcces, out hToken)) {
                return new SafeProcessTokenHandle(hToken);
            } else {
                throw new Win32Exception();
            }
        }

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        [DllImport(ExternDll.Advapi32, SetLastError = true)]
        private static extern bool GetTokenInformation(
            IntPtr TokenHandle,
            TOKEN_INFORMATION_CLASS TokenInformationClass,
            IntPtr TokenInformation,
            int TokenInformationLength,
            out int ReturnLength);

        private static int GetTokenInformationSize(
            IntPtr tokenHandle,
            TOKEN_INFORMATION_CLASS TokenInformationClass) {
            int returnSize;
            GetTokenInformation(
                tokenHandle,
                TokenInformationClass,
                IntPtr.Zero,
                0,
                out returnSize
                );
            return returnSize;
        }

        public static SafeCoTaskMemHandle GetTokenInformation(
            IntPtr TokenHandle,
            TOKEN_INFORMATION_CLASS TokenInformationClass
            ) {
            int size = GetTokenInformationSize(TokenHandle, TokenInformationClass);
            if (size > 0) {
                var handle = new SafeCoTaskMemHandle(size);
                if (GetTokenInformation(
                    TokenHandle,
                    TokenInformationClass,
                    (IntPtr)handle,
                    size,
                    out size
                    )) {
                    return handle;
                } else {
                    throw new Win32Exception();
                }
            } else {
                return null;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_USER
        {
            public SID_AND_ATTRIBUTES User;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public int Attributes;
        }

        public enum SID_NAME_USE
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        [DllImport(ExternDll.Advapi32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LookupAccountSid(
            string lpSystemName,
            IntPtr Sid,
            StringBuilder lpName,
            ref int cchName,
            StringBuilder ReferencedDomainName,
            ref int cchReferencedDomainName,
            out SID_NAME_USE peUse);

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetCurrentProcessId();

        #endregion

        [DllImport(ExternDll.User32)]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool WinHelp(IntPtr hWndMain, string lpszHelp, int nCommand, int dwData);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool WinHelp(IntPtr hWndMain, string lpszHelp, int nCommand, string dwData);

        public const int GCS_COMPREADSTR = 0x0001;
        public const int GCS_COMPSTR = 0x0008;
        public const int GCS_RESULTREADSTR = 0x0200;

        [DllImport(ExternDll.Imm32, SetLastError = true)]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Unicode)]
        public static extern int ImmGetCompositionString(IntPtr hIMC, int dwIndex, StringBuilder lpBuf, int dwBufLen);

        [DllImport(ExternDll.Imm32)]
        public static extern bool ImmGetOpenStatus(IntPtr hIMC);

        [DllImport(ExternDll.Imm32)]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string lpString);


        [DllImport(ExternDll.Gdi32)]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest,
            int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc,
            int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
            TernaryRasterOperations dwRop);

        [DllImport(ExternDll.Gdi32)]
        public static extern bool SetStretchBltMode(IntPtr hdc, StretchMode iStretchMode);

        public enum StretchMode
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }

        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020,    /* dest = source*/
            SRCPAINT = 0x00EE0086,  /* dest = source OR dest*/
            SRCAND = 0x008800C6, /* dest = source AND dest*/
            SRCINVERT = 0x00660046, /* dest = source XOR dest*/
            SRCERASE = 0x00440328, /* dest = source AND (NOT dest )*/
            NOTSRCCOPY = 0x00330008, /* dest = (NOT source)*/
            NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
            MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)*/
            MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest*/
            PATCOPY = 0x00F00021, /* dest = pattern*/
            PATPAINT = 0x00FB0A09, /* dest = DPSnoo*/
            PATINVERT = 0x005A0049, /* dest = pattern XOR dest*/
            DSTINVERT = 0x00550009, /* dest = (NOT dest)*/
            BLACKNESS = 0x00000042, /* dest = BLACK*/
            WHITENESS = 0x00FF0062, /* dest = WHITE*/
        };

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        public static extern IntPtr GlobalAlloc(int uFlags, IntPtr dwBytes);

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        public static extern int GlobalSize(IntPtr hMem);

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        public static extern IntPtr GlobalReAlloc(IntPtr hMem, IntPtr dwBytes, int Flags);

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport(ExternDll.Kernel32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport(ExternDll.Kernel32, EntryPoint = "RtlMoveMemory", SetLastError = false, CharSet = CharSet.Unicode)]
        public static extern void MoveMemory(IntPtr dest, StringBuilder src, int size);

        public const int
            GMEM_FIXED = 0x0000,
            GMEM_MOVEABLE = 0x0002,
            GMEM_ZEROINIT = 0x0040,
            GPTR = GMEM_ZEROINIT,
            GHND = GMEM_MOVEABLE | GMEM_ZEROINIT;

        // クリップボード操作キーコード
        public const int CTRL_A = 01; // Ctrl + A
        public const int CTRL_C = 03; // Ctrl + C
        public const int CTRL_V = 22; // Ctrl + V
        public const int CTRL_X = 24; // Ctrl + X
        public const int CTRL_Z = 26; // Ctrl + Z

        public const int
            CF_TEXT = 1,
            CF_BITMAP = 2,
            CF_METAFILEPICT = 3,
            CF_SYLK = 4,
            CF_DIF = 5,
            CF_TIFF = 6,
            CF_OEMTEXT = 7,
            CF_DIB = 8,
            CF_PALETTE = 9,
            CF_PENDATA = 10,
            CF_RIFF = 11,
            CF_WAVE = 12,
            CF_UNICODETEXT = 13,
            CF_ENHMETAFILE = 14,
            CF_HDROP = 15,
            CF_LOCALE = 16,
            CF_MAX = 17,
            CF_OWNERDISPLAY = 0x80,
            CF_DSPTEXT = 0x81,
            CF_DSPBITMAP = 0x82,
            CF_DSPMETAFILEPICT = 0x83,
            CF_DSPENHMETAFILE = 0x8E;

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool CloseClipboard();

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport(ExternDll.User32)]
        public static extern bool EmptyClipboard();

        public static bool SetClipboardText(string value) {
            // Global メモリを確保して文字列をコピー
            var builder = new StringBuilder(value);
            IntPtr hMem = GlobalAlloc(GHND, (IntPtr)(value.Length * 2 + 2));
            if (hMem == IntPtr.Zero) {
                throw new Win32Exception();
            }
            IntPtr ptr = GlobalLock(hMem);
            if (ptr == IntPtr.Zero) {
                throw new Win32Exception();
            }
            MoveMemory(ptr, builder, value.Length * 2);
            GlobalUnlock(hMem);

            // クリップボード操作
            bool result = false;
            if (OpenClipboard(IntPtr.Zero)) {
                EmptyClipboard();
                if (SetClipboardData(CF_UNICODETEXT, hMem) != IntPtr.Zero) {
                    result = true;
                } else {
                    // SetClipboardData が失敗したら解放
                    // 成功した場合はクリップボードが保持するので解放してはいけない
                    GlobalFree(hMem);
                }
                CloseClipboard();
            }
            return result;
        }

        #region SwitchToThisWindow がサポートされているか判定

        [DllImport(ExternDll.User32, SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static bool _SwitchToThisWindowChecked = false;

        private static bool _IsSupportSwitchToThisWindow = false;

        private static bool IsSupportSwitchToThisWindow {
            get {
                if (!_SwitchToThisWindowChecked) {
                    IntPtr hModule = LoadLibrary("user32.dll");
                    if (hModule != IntPtr.Zero) {
                        IntPtr addr = GetProcAddress(hModule, "SwitchToThisWindow");
                        FreeLibrary(hModule);
                        _IsSupportSwitchToThisWindow = (addr != IntPtr.Zero);
                    } else {
                        _IsSupportSwitchToThisWindow = false;
                    }
                    _SwitchToThisWindowChecked = true;
                }
                return _IsSupportSwitchToThisWindow;
            }
        }

        #endregion

        #region 指定したウインドウを前面に表示


        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWNORMAL = 1;

        [DllImport(ExternDll.User32)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport(ExternDll.User32, EntryPoint = "SystemParametersInfo", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfoGet(uint action, uint param, ref uint vparam, uint init);

        [DllImport(ExternDll.User32, EntryPoint = "SystemParametersInfo", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfoSet(uint action, uint param, uint vparam, uint init);

        private const uint SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000;
        private const uint SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001;
        private const uint SPIF_UPDATEINIFILE = 0x01;
        private const uint SPIF_SENDCHANGE = 0x02;

        public static void SetActiveWindow(IntPtr hWnd) {

            // IntPtr.Zero が渡されたらスレッドの ActiveWindow を対象とする
            if (hWnd == IntPtr.Zero) {
                hWnd = GetActiveWindow();
            }

            //ウィンドウが最小化されている場合は元に戻す
            if (IsIconic(hWnd)) {
                ShowWindowAsync(hWnd, SW_RESTORE);
            }

            //フォアグラウンドウィンドウのハンドルを取得
            IntPtr forehWnd = GetForegroundWindow();
            if (forehWnd == hWnd) {
                return;
            }

            // SwitchToThisWindow がサポートされているなら実行して終了(これがいちばん確実)
            if (IsSupportSwitchToThisWindow) {
                SwitchToThisWindow(hWnd, true);
                return;
            }

            //フォアグラウンドのスレッドIDを取得
            uint foreThread = GetWindowThreadProcessId(forehWnd, IntPtr.Zero);
            //自分のスレッドIDを収得
            uint thisThread = GetCurrentThreadId();

            uint timeout = 200000;
            if (foreThread != thisThread) {
                //ForegroundLockTimeoutの現在の設定を取得
                SystemParametersInfoGet(SPI_GETFOREGROUNDLOCKTIMEOUT, 0, ref timeout, 0);

                //ForegroundLockTimeoutの値を0にする
                SystemParametersInfoSet(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, 0, 0);

                //入力処理機構にアタッチする
                AttachThreadInput(thisThread, foreThread, true);
            }

            //ウィンドウをフォアグラウンドにする処理
            SetForegroundWindow(hWnd);
            SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW | SWP_ASYNCWINDOWPOS);
            BringWindowToTop(hWnd);
            ShowWindowAsync(hWnd, SW_SHOW);
            SetFocus(hWnd);

            if (foreThread != thisThread) {
                //ForegroundLockTimeoutの値を元に戻す
                SystemParametersInfoSet(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, timeout, 0);

                //デタッチ
                AttachThreadInput(thisThread, foreThread, false);
            }
        }

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetActiveWindow();

        #endregion


        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(ExternDll.Psapi, SetLastError = true)]
        public static extern bool EnumProcessModules(IntPtr hProcess, IntPtr lphModule, uint cb, [MarshalAs(UnmanagedType.U4)] out uint lpcbNeeded);

        [DllImport(ExternDll.Psapi, CharSet = CharSet.Auto)]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

        public const int OFN_READONLY = 0x00000001,
        OFN_OVERWRITEPROMPT = 0x00000002,
        OFN_HIDEREADONLY = 0x00000004,
        OFN_NOCHANGEDIR = 0x00000008,
        OFN_SHOWHELP = 0x00000010,
        OFN_ENABLEHOOK = 0x00000020,
        OFN_NOVALIDATE = 0x00000100,
        OFN_ALLOWMULTISELECT = 0x00000200,
        OFN_PATHMUSTEXIST = 0x00000800,
        OFN_FILEMUSTEXIST = 0x00001000,
        OFN_CREATEPROMPT = 0x00002000,
        OFN_EXPLORER = 0x00080000,
        OFN_NODEREFERENCELINKS = 0x00100000,
        OFN_ENABLESIZING = 0x00800000,
        OFN_USESHELLITEM = 0x01000000,
        OLEIVERB_PRIMARY = 0,
        OLEIVERB_SHOW = -1,
        OLEIVERB_HIDE = -3,
        OLEIVERB_UIACTIVATE = -4,
        OLEIVERB_INPLACEACTIVATE = -5,
        OLEIVERB_DISCARDUNDOSTATE = -6,
        OLEIVERB_PROPERTIES = -7,
        OLE_E_INVALIDRECT = unchecked((int)0x8004000D),
        OLE_E_NOCONNECTION = unchecked((int)0x80040004),
        OLE_E_PROMPTSAVECANCELLED = unchecked((int)0x8004000C),
        OLEMISC_RECOMPOSEONRESIZE = 0x00000001,
        OLEMISC_INSIDEOUT = 0x00000080,
        OLEMISC_ACTIVATEWHENVISIBLE = 0x0000100,
        OLEMISC_ACTSLIKEBUTTON = 0x00001000,
        OLEMISC_SETCLIENTSITEFIRST = 0x00020000,
        OBJ_PEN = 1,
        OBJ_BRUSH = 2,
        OBJ_DC = 3,
        OBJ_METADC = 4,
        OBJ_PAL = 5,
        OBJ_FONT = 6,
        OBJ_BITMAP = 7,
        OBJ_REGION = 8,
        OBJ_METAFILE = 9,
        OBJ_MEMDC = 10,
        OBJ_EXTPEN = 11,
        OBJ_ENHMETADC = 12,
        ODS_CHECKED = 0x0008,
        ODS_COMBOBOXEDIT = 0x1000,
        ODS_DEFAULT = 0x0020,
        ODS_DISABLED = 0x0004,
        ODS_FOCUS = 0x0010,
        ODS_GRAYED = 0x0002,
        ODS_HOTLIGHT = 0x0040,
        ODS_INACTIVE = 0x0080,
        ODS_NOACCEL = 0x0100,
        ODS_NOFOCUSRECT = 0x0200,
        ODS_SELECTED = 0x0001,
        OLECLOSE_SAVEIFDIRTY = 0,
        OLECLOSE_PROMPTSAVE = 2;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTL
        {
            public Int32 x;
            public Int32 y;
        }

        [DllImport(ExternDll.Winspool, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags fuFlags,
            int uTimeout,
            out UIntPtr lpdwResult);

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public const int PRINTER_ACCESS_ADMINISTER = 0x0004;
        public const int PRINTER_ACCESS_USE = 0x0008;
        public const int PRINTER_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;

        [StructLayout(LayoutKind.Sequential)]
        public struct PRINTER_DEFAULTS
        {
            public IntPtr pDatatype;
            public IntPtr pDevMode;
            public int DesiredAccess;
        }

        [DllImport(ExternDll.Winspool, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, ref PRINTER_DEFAULTS pDefault);

        [DllImport(ExternDll.Winspool, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport(ExternDll.Winspool, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetPrinter(
            IntPtr hPrinter,
            int level,
            IntPtr pPrinter,
            int cbBuf,
            out int pcbNeeded);

        [StructLayout(LayoutKind.Sequential)]
        public struct PRINTER_INFO_2
        {
            public IntPtr pServerName;
            public IntPtr pPrinterName;
            public IntPtr pShareName;
            public IntPtr pPortName;
            public IntPtr pDriverName;
            public IntPtr pComment;
            public IntPtr pLocation;
            public IntPtr pDevMode;
            public IntPtr pSepFile;
            public IntPtr pPrintProcessor;
            public IntPtr pDatatype;
            public IntPtr pParameters;
            public IntPtr pSecurityDescriptor;
            public int Attributes; // See note below!
            public int Priority;
            public int DefaultPriority;
            public int StartTime;
            public int UntilTime;
            public int Status;
            public int cJobs;
            public int AveragePPM;
        }

        [DllImport(ExternDll.Winspool, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint SetPrinter(
            IntPtr hPrinter,
            int level,
            IntPtr pPrinter,
            int command
        );

        [DllImport(ExternDll.Comdlg32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ChooseColor(CHOOSECOLOR cc);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class CHOOSECOLOR
        {
            public int lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public int rgbResult;
            public IntPtr lpCustColors;
            public int Flags;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;

            public CHOOSECOLOR() {
                this.lStructSize = Marshal.SizeOf(this);
                this.lCustData = IntPtr.Zero;
                this.lpfnHook = IntPtr.Zero;
                this.lpTemplateName = null;
            }
        }

        [DllImport(ExternDll.Msctf, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr SetInputScope(IntPtr hwnd, int inputScope);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        public enum CombineRgnStyles : int
        {
            RGN_AND = 1,
            RGN_OR = 2,
            RGN_XOR = 3,
            RGN_DIFF = 4,
            RGN_COPY = 5,
            RGN_MIN = RGN_AND,
            RGN_MAX = RGN_COPY
        }

        [DllImport(ExternDll.Gdi32)]
        public static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, CombineRgnStyles fnCombineMode);

        [DllImport(ExternDll.Gdi32)]
        public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.Shlwapi)]
        public static extern IntPtr PathCombine(StringBuilder lpszDest, string lpszDir, string lpszFile);

        [DllImport(ExternDll.Shlwapi)]
        public static extern bool PathRelativePathTo(StringBuilder lpszDest, string lpszFrom, FileAttributes dwAttrFrom, string pszTo, FileAttributes dwAttrTo);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr WindowFromPoint(POINT p);

        public const int
            GA_PARENT = 1,
            GA_ROOT = 2,
            GA_ROOTOWNER = 3;

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetAncestor(IntPtr handle, int flags);


        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hwnd, string text, string caption, int type);

        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public const int WTS_CURRENT_SESSION = -1;

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime,
            WTSLogonTime,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames,
            WTSClientInfo,
            WTSSessionInfo,
            WTSSessionInfoEx,
            WTSConfigInfo,
            WTSValidationInfo,
            WTSSessionAddressV4,
            WTSIsRemoteSession
        }

        [DllImport(ExternDll.Wtsapi32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool WTSQuerySessionInformation(
                                                    IntPtr hServer,
                                                    int SessionId,
                                                    WTS_INFO_CLASS WTSInfoClass,
                                                    out IntPtr ppBuffer,
                                                    out int pBytesReturned);

        [DllImport(ExternDll.Wtsapi32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void WTSFreeMemory(IntPtr pMemory);

        public const int CLIENTNAME_LENGTH = 20;
        public const int DOMAIN_LENGTH = 17;
        public const int USERNAME_LENGTH = 20;
        public const int CLIENTADDRESS_LENGTH = 30;

        public enum AddressFamily
        {
            AF_UNSPEC = 0,
            AF_UNIX = 1,
            AF_INET = 2,
            AF_AX25 = 3,
            AF_IPX = 4,
            AF_APPLETALK = 5,
            AF_NETROM = 6,
            AF_BRIDGE = 7,
            AF_AAL5 = 8,
            AF_X25 = 9,
            AF_INET6 = 10,
            AF_MAX = 12,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WTSCLIENT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CLIENTNAME_LENGTH + 1)]
            public string ClientName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DOMAIN_LENGTH + 1)]
            public string Domain;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = USERNAME_LENGTH + 1)]
            public string UserName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 1)]
            public string WorkDirectory;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 1)]
            public string InitialProgram;
            public byte EncryptionLevel;
            public AddressFamily ClientAddressFamily;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CLIENTADDRESS_LENGTH + 1)]
            public ushort[] ClientAddress;
            public ushort HRes;
            public ushort VRes;
            public ushort ColorDepth;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 1)]
            public string ClientDirectory;
            public uint ClientBuildNumber;
            public uint ClientHardwareId;
            public ushort ClientProductId;
            public ushort OutBufCountHost;
            public ushort OutBufCountClient;
            public ushort OutBufLength;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 1)]
            public string DeviceId;
        }

        public static T GetTerminalServerInformation<T>(WTS_INFO_CLASS info) {
            IntPtr pBuffer = IntPtr.Zero;
            int nSize;
            T result = default(T);
            if (WTSQuerySessionInformation(
                                WTS_CURRENT_SERVER_HANDLE,
                                WTS_CURRENT_SESSION,
                                info,
                                out pBuffer,
                                out nSize)) {

                result = (T)Marshal.PtrToStructure(pBuffer, typeof(T));
                WTSFreeMemory(pBuffer);
            }
            return result;
        }

        public const int SM_REMOTESESSION = 0x1000;

        [DllImport(ExternDll.Uxtheme, CharSet = CharSet.Auto)]
        public static extern IntPtr SetThemeAppProperties(int value);

        [DllImport(ExternDll.Uxtheme, CharSet = CharSet.Auto)]
        public static extern int GetThemeAppProperties();

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto)]
        public static extern int SearchPath(string lpPath, string fileName, string lpExtension, int nBufferLength, StringBuilder buffer, out IntPtr lpFilePart);

        public static string SearchPath(string fileName) {
            IntPtr filePart;
            var buffer = new StringBuilder(MAX_PATH + 1);
            int result = SearchPath(null, fileName, null, MAX_PATH, buffer, out filePart);
            if (result > 0) {
                return buffer.ToString();
            }
            string pathExt = Environment.GetEnvironmentVariable("PATHEXT");
            foreach (string ext in pathExt.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
                result = SearchPath(null, fileName, ext, MAX_PATH, buffer, out filePart);
                if (result > 0) {
                    return buffer.ToString();
                }
            }
            return string.Empty;
        }

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int ProcessIdToSessionId(int dwProcessId, out int pSessionId);

        public static bool IsWindows10 {
            get {
                return WindowsVersion.Major == 10;
            }
        }

        private static Version _WindowsVersion;

        public static Version WindowsVersion {
            get {
                if (_WindowsVersion == null) {
                    try {
                        var fullPath = SearchPath("kernel32.dll");
                        var fi = FileVersionInfo.GetVersionInfo(fullPath);
                        _WindowsVersion = new Version(fi.ProductVersion);
                    } catch {
                        _WindowsVersion = Environment.Version;
                    }
                }
                return _WindowsVersion;
            }
        }

        [DllImport(ExternDll.Kernel32)]
        public static extern void ExitThread(int exitCode);

        [DllImport(ExternDll.Kernel32)]
        public static extern void ExitProcess(int exitCode);

        [StructLayout(LayoutKind.Sequential)]
        internal struct LUID
        {
            public int LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class TokenPrivileges
        {
            public int PrivilegeCount = 1;
            public LUID Luid;
            public int Attributes = 0;
        }

        [DllImport(ExternDll.Advapi32, CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true, BestFitMapping = false)]
        public static extern bool LookupPrivilegeValue([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpName, out LUID lpLuid);

        [DllImport(ExternDll.Advapi32, CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(
            IntPtr TokenHandle,
            bool DisableAllPrivileges,
            TokenPrivileges NewState,
            int BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength
        );

        public const int
            SE_PRIVILEGE_DISABLED = 0x00000000,
            SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001,
            SE_PRIVILEGE_ENABLED = 0x00000002,
            SE_PRIVILEGE_REMOVE = 0x00000004;

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public int Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }

        [DllImport(ExternDll.Advapi32, SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeName(
                            string lpSystemName,
                            ref LUID lpLuid,
                            StringBuilder lpName,
                            ref int cchName);

        [DllImport(ExternDll.Advapi32, SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeDisplayName(
            string lpSystemName,
            string lpName,
            StringBuilder lpDisplayName,
            ref int cchDisplayName,
            ref int lpLanguageId
        );

        [DllImport(ExternDll.Comdlg32, EntryPoint = "PrintDlg", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PrintDlg_32([In, Out] PRINTDLG_32 lppd);

        [DllImport(ExternDll.Comdlg32, EntryPoint = "PrintDlg", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PrintDlg_64([In, Out] PRINTDLG_64 lppd);

        public static bool PrintDlg([In, Out] PRINTDLG lppd) {
            if (IntPtr.Size == 4) {
                PRINTDLG_32 lppd_32 = lppd as PRINTDLG_32;
                if (lppd_32 == null) {
                    throw new System.NullReferenceException("PRINTDLG data is null");
                }
                return PrintDlg_32(lppd_32);
            } else {
                PRINTDLG_64 lppd_64 = lppd as PRINTDLG_64;
                if (lppd_64 == null) {
                    throw new System.NullReferenceException("PRINTDLG data is null");
                }
                return PrintDlg_64(lppd_64);
            }
        }

        internal static PRINTDLG CreatePRINTDLG() {
            PRINTDLG data = null;
            if (IntPtr.Size == 4) {
                data = new PRINTDLG_32();
            } else {
                data = new PRINTDLG_64();
            }
            data.lStructSize = Marshal.SizeOf(data);
            data.hwndOwner = IntPtr.Zero;
            data.hDevMode = IntPtr.Zero;
            data.hDevNames = IntPtr.Zero;
            data.Flags = 0;
            data.hDC = IntPtr.Zero;
            data.nFromPage = 1;
            data.nToPage = 1;
            data.nMinPage = 0;
            data.nMaxPage = 9999;
            data.nCopies = 1;
            data.hInstance = IntPtr.Zero;
            data.lCustData = IntPtr.Zero;
            data.lpfnPrintHook = IntPtr.Zero;
            data.lpfnSetupHook = IntPtr.Zero;
            data.lpPrintTemplateName = null;
            data.lpSetupTemplateName = null;
            data.hPrintTemplate = IntPtr.Zero;
            data.hSetupTemplate = IntPtr.Zero;
            return data;
        }

        public interface PRINTDLG : IDisposable
        {
            int lStructSize { get; set; }

            IntPtr hwndOwner { get; set; }
            IntPtr hDevMode { get; set; }
            IntPtr hDevNames { get; set; }
            IntPtr hDC { get; set; }

            int Flags { get; set; }

            short nFromPage { get; set; }
            short nToPage { get; set; }
            short nMinPage { get; set; }
            short nMaxPage { get; set; }
            short nCopies { get; set; }

            IntPtr hInstance { get; set; }
            IntPtr lCustData { get; set; }

            IntPtr lpfnPrintHook { get; set; }
            IntPtr lpfnSetupHook { get; set; }

            string lpPrintTemplateName { get; set; }
            string lpSetupTemplateName { get; set; }

            IntPtr hPrintTemplate { get; set; }
            IntPtr hSetupTemplate { get; set; }
        }


        // Any change in PRINTDLG_32, should also be in PRINTDLG and PRINTDLG_64
        // x86 requires EXPLICIT packing of 1.
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        public class PRINTDLG_32 : PRINTDLG
        {
            int m_lStructSize;

            IntPtr m_hwndOwner;
            IntPtr m_hDevMode;
            IntPtr m_hDevNames;
            IntPtr m_hDC;

            int m_Flags;

            short m_nFromPage;
            short m_nToPage;
            short m_nMinPage;
            short m_nMaxPage;
            short m_nCopies;

            IntPtr m_hInstance;
            IntPtr m_lCustData;

            IntPtr m_lpfnPrintHook;
            IntPtr m_lpfnSetupHook;

            string m_lpPrintTemplateName;
            string m_lpSetupTemplateName;

            IntPtr m_hPrintTemplate;
            IntPtr m_hSetupTemplate;

            public int lStructSize { get { return m_lStructSize; } set { m_lStructSize = value; } }

            public IntPtr hwndOwner { get { return m_hwndOwner; } set { m_hwndOwner = value; } }
            public IntPtr hDevMode { get { return m_hDevMode; } set { m_hDevMode = value; } }
            public IntPtr hDevNames { get { return m_hDevNames; } set { m_hDevNames = value; } }
            public IntPtr hDC { get { return m_hDC; } set { m_hDC = value; } }

            public int Flags { get { return m_Flags; } set { m_Flags = value; } }

            public short nFromPage { get { return m_nFromPage; } set { m_nFromPage = value; } }
            public short nToPage { get { return m_nToPage; } set { m_nToPage = value; } }
            public short nMinPage { get { return m_nMinPage; } set { m_nMinPage = value; } }
            public short nMaxPage { get { return m_nMaxPage; } set { m_nMaxPage = value; } }
            public short nCopies { get { return m_nCopies; } set { m_nCopies = value; } }

            public IntPtr hInstance { get { return m_hInstance; } set { m_hInstance = value; } }
            public IntPtr lCustData { get { return m_lCustData; } set { m_lCustData = value; } }

            public IntPtr lpfnPrintHook { get { return m_lpfnPrintHook; } set { m_lpfnPrintHook = value; } }
            public IntPtr lpfnSetupHook { get { return m_lpfnSetupHook; } set { m_lpfnSetupHook = value; } }

            public string lpPrintTemplateName { get { return m_lpPrintTemplateName; } set { m_lpPrintTemplateName = value; } }
            public string lpSetupTemplateName { get { return m_lpSetupTemplateName; } set { m_lpSetupTemplateName = value; } }

            public IntPtr hPrintTemplate { get { return m_hPrintTemplate; } set { m_hPrintTemplate = value; } }
            public IntPtr hSetupTemplate { get { return m_hSetupTemplate; } set { m_hSetupTemplate = value; } }

            public void Dispose() {
                GlobalFree(hDevMode);
                GlobalFree(hDevNames);
            }
        }

        // Any change in PRINTDLG_64, should also be in PRINTDLG_32 and PRINTDLG
        // x64 does not require EXPLICIT packing of 1.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class PRINTDLG_64 : PRINTDLG
        {
            int m_lStructSize;

            IntPtr m_hwndOwner;
            IntPtr m_hDevMode;
            IntPtr m_hDevNames;
            IntPtr m_hDC;

            int m_Flags;

            short m_nFromPage;
            short m_nToPage;
            short m_nMinPage;
            short m_nMaxPage;
            short m_nCopies;

            IntPtr m_hInstance;
            IntPtr m_lCustData;

            IntPtr m_lpfnPrintHook;
            IntPtr m_lpfnSetupHook;

            string m_lpPrintTemplateName;
            string m_lpSetupTemplateName;

            IntPtr m_hPrintTemplate;
            IntPtr m_hSetupTemplate;

            public int lStructSize { get { return m_lStructSize; } set { m_lStructSize = value; } }

            public IntPtr hwndOwner { get { return m_hwndOwner; } set { m_hwndOwner = value; } }
            public IntPtr hDevMode { get { return m_hDevMode; } set { m_hDevMode = value; } }
            public IntPtr hDevNames { get { return m_hDevNames; } set { m_hDevNames = value; } }
            public IntPtr hDC { get { return m_hDC; } set { m_hDC = value; } }

            public int Flags { get { return m_Flags; } set { m_Flags = value; } }

            public short nFromPage { get { return m_nFromPage; } set { m_nFromPage = value; } }
            public short nToPage { get { return m_nToPage; } set { m_nToPage = value; } }
            public short nMinPage { get { return m_nMinPage; } set { m_nMinPage = value; } }
            public short nMaxPage { get { return m_nMaxPage; } set { m_nMaxPage = value; } }
            public short nCopies { get { return m_nCopies; } set { m_nCopies = value; } }

            public IntPtr hInstance { get { return m_hInstance; } set { m_hInstance = value; } }
            public IntPtr lCustData { get { return m_lCustData; } set { m_lCustData = value; } }

            public IntPtr lpfnPrintHook { get { return m_lpfnPrintHook; } set { m_lpfnPrintHook = value; } }
            public IntPtr lpfnSetupHook { get { return m_lpfnSetupHook; } set { m_lpfnSetupHook = value; } }

            public string lpPrintTemplateName { get { return m_lpPrintTemplateName; } set { m_lpPrintTemplateName = value; } }
            public string lpSetupTemplateName { get { return m_lpSetupTemplateName; } set { m_lpSetupTemplateName = value; } }

            public IntPtr hPrintTemplate { get { return m_hPrintTemplate; } set { m_hPrintTemplate = value; } }
            public IntPtr hSetupTemplate { get { return m_hSetupTemplate; } set { m_hSetupTemplate = value; } }

            public void Dispose() {
                GlobalFree(hDevMode);
                GlobalFree(hDevNames);
            }
        }

        public enum FileFuncFlags : uint
        {
            FO_MOVE = 0x1,
            FO_COPY = 0x2,
            FO_DELETE = 0x3,
            FO_RENAME = 0x4
        }

        [Flags]
        public enum FILEOP_FLAGS : ushort
        {
            FOF_MULTIDESTFILES = 0x1,
            FOF_CONFIRMMOUSE = 0x2,
            FOF_SILENT = 0x4,
            FOF_RENAMEONCOLLISION = 0x8,
            FOF_NOCONFIRMATION = 0x10,
            FOF_WANTMAPPINGHANDLE = 0x20,
            FOF_ALLOWUNDO = 0x40,
            FOF_FILESONLY = 0x80,
            FOF_SIMPLEPROGRESS = 0x100,
            FOF_NOCONFIRMMKDIR = 0x200,
            FOF_NOERRORUI = 0x400,
            FOF_NOCOPYSECURITYATTRIBS = 0x800,
            FOF_NORECURSION = 0x1000,
            FOF_NO_CONNECTED_ELEMENTS = 0x2000,
            FOF_WANTNUKEWARNING = 0x4000,
            FOF_NORECURSEREPARSE = 0x8000
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FileFuncFlags wFunc;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pTo;
            public FILEOP_FLAGS fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszProgressTitle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct SHFILEOPSTRUCT64
        {
            public IntPtr hwnd;
            public FileFuncFlags wFunc;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pTo;
            public FILEOP_FLAGS fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszProgressTitle;
        }

        [DllImport(ExternDll.Shell32, CharSet = CharSet.Auto, EntryPoint = "SHFileOperation")]
        static extern int SHFileOperation32(ref SHFILEOPSTRUCT lpFileOp);

        [DllImport(ExternDll.Shell32, CharSet = CharSet.Auto, EntryPoint = "SHFileOperation")]
        static extern int SHFileOperation64(ref SHFILEOPSTRUCT64 lpFileOp);

        public static int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp) {
            if (IntPtr.Size == 4) {
                return SHFileOperation32(ref lpFileOp);
            } else {
                var lpFileOp64 = new SHFILEOPSTRUCT64 {
                    hwnd = lpFileOp.hwnd,
                    wFunc = lpFileOp.wFunc,
                    pFrom = lpFileOp.pFrom,
                    pTo = lpFileOp.pTo,
                    fFlags = lpFileOp.fFlags,
                    fAnyOperationsAborted = lpFileOp.fAnyOperationsAborted,
                    hNameMappings = lpFileOp.hNameMappings,
                    lpszProgressTitle = lpFileOp.lpszProgressTitle
                };

                int result = SHFileOperation64(ref lpFileOp64);

                lpFileOp.fAnyOperationsAborted = lpFileOp64.fAnyOperationsAborted;
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TEXTMETRICW
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public ushort tmFirstChar;
            public ushort tmLastChar;
            public ushort tmDefaultChar;
            public ushort tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }

        [DllImport(ExternDll.Gdi32)]
        public static extern bool GetTextMetricsW(IntPtr hdc, out TEXTMETRICW lptm);

        [DllImport(ExternDll.User32)]
        public static extern bool CreateCaret(IntPtr hwnd, SafeHandle hbitmap, int width, int height);

        [DllImport(ExternDll.User32)]
        public static extern int HideCaret(IntPtr hwnd);

        [DllImport(ExternDll.User32)]
        public static extern int ShowCaret(IntPtr hwnd);

        [DllImport(ExternDll.User32)]
        public static extern bool DestroyCaret();

        [DllImport(ExternDll.Gdi32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

        [DllImport(ExternDll.Gdi32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart, StringBuilder lpString, int cbString);

        internal class SafeCoTaskMemHandle : SafeHandleZeroIsInvalid
        {
            public SafeCoTaskMemHandle(int size) : base(Marshal.AllocCoTaskMem(size)) { }

            protected override bool ReleaseHandleCore() {
                Marshal.FreeCoTaskMem(handle);
                return true;
            }
        }

        internal abstract class SafeHandleZeroIsInvalid : SafeHandle
        {
            protected SafeHandleZeroIsInvalid(IntPtr handle) : base(handle, true) { }

            public sealed override bool IsInvalid {
                get {
                    return handle == IntPtr.Zero;
                }
            }

            protected sealed override bool ReleaseHandle() {
                bool result = false;
                if (!IsInvalid) {
                    result = ReleaseHandleCore();
                    SetHandle(IntPtr.Zero);
                }
                return result;
            }

            protected abstract bool ReleaseHandleCore();

            public static implicit operator IntPtr(SafeHandleZeroIsInvalid value) {
                return value.handle;
            }
        }

        internal class SafeProcessTokenHandle : SafeHandleZeroIsInvalid
        {
            public SafeProcessTokenHandle(IntPtr hToken) : base(hToken) { }

            protected override bool ReleaseHandleCore() {
                return CloseHandle(handle);
            }
        }

        public const int VAR_FORMAT_NOSUBSTITUTE = 0x20;
        public const int VAR_CALENDAR_HIJRI = 0x8;

        [StructLayout(LayoutKind.Sequential)]
        public struct VARIANT
        {
            public short vt;
            public short reserved1;
            public short reserved2;
            public short reserved3;
            public IntPtr data1;
            public IntPtr data2;
        }

        [DllImport(ExternDll.Oleaut32, ExactSpelling = true)]
        public static extern int VariantClear(IntPtr pObject);

        [DllImport(ExternDll.Oleaut32, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int VarFormat(IntPtr pvariant,
                                         [MarshalAs(UnmanagedType.VBByRefStr)] ref string sfmt,
                                         int dow, int woy, int dwFlags,
                                         [MarshalAs(UnmanagedType.BStr)] out string sb);

        [DllImport(ExternDll.Oleaut32, ExactSpelling = true)]
        public static extern int VariantInit(IntPtr pObject);

        internal static class ExternDll
        {
            public const string Activeds = "activeds.dll";
            public const string Advapi32 = "advapi32.dll";
            public const string Clr = "clr.dll";
            public const string Comctl32 = "comctl32.dll";
            public const string Comdlg32 = "comdlg32.dll";
            public const string Crypt32 = "crypt32.dll";
            public const string Fxassert = "Fxassert.dll";
            public const string Gdi32 = "gdi32.dll";
            public const string Gdiplus = "gdiplus.dll";
            public const string Hhctrl = "hhctrl.ocx";
            public const string Imm32 = "imm32.dll";
            public const string Kernel32 = "kernel32.dll";
            public const string Loadperf = "Loadperf.dll";
            public const string Mqrt = "mqrt.dll";
            public const string Mscoree = "mscoree.dll";
            public const string Msi = "msi.dll";
            public const string Ntdll = "ntdll.dll";
            public const string Ole32 = "ole32.dll";
            public const string Oleacc = "oleacc.dll";
            public const string Oleaut32 = "oleaut32.dll";
            public const string Olepro32 = "olepro32.dll";
            public const string PerfCounter = "perfcounter.dll";
            public const string Powrprof = "Powrprof.dll";
            public const string Psapi = "psapi.dll";
            public const string Shell32 = "shell32.dll";
            public const string Shfolder = "shfolder.dll";
            public const string Shlwapi = "shlwapi.dll";
            public const string User32 = "user32.dll";
            public const string Uxtheme = "uxtheme.dll";
            public const string Version = "version.dll";
            public const string Vsassert = "vsassert.dll";
            public const string WinMM = "winmm.dll";
            public const string Winspool = "winspool.drv";
            public const string Wtsapi32 = "wtsapi32.dll";
            public const string Msctf = "Msctf.dll";
        }
    }
}

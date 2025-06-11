// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.RtfApi
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal class RtfApi
{
  public const int EM_FORMATRANGE = 1081;
  public const int WM_USER = 1024 /*0x0400*/;
  public const int EM_GETCHARFORMAT = 1082;
  public const int EM_SETCHARFORMAT = 1092;
  public const int EM_SETEVENTMASK = 1073;
  public const int EM_GETPARAFORMAT = 1085;
  public const int EM_SETPARAFORMAT = 1095;
  public const int EM_SETTYPOGRAPHYOPTIONS = 1226;
  public const int WM_SETREDRAW = 11;
  public const int TO_ADVANCEDTYPOGRAPHY = 1;
  public const int SCF_SELECTION = 1;
  public const int SCF_WORD = 2;
  public const int SCF_ALL = 4;
  public const int LF_FACESIZE = 32 /*0x20*/;
  public const uint CFM_BOLD = 1;
  public const uint CFM_ITALIC = 2;
  public const uint CFM_UNDERLINE = 4;
  public const uint CFM_STRIKEOUT = 8;
  public const uint CFM_PROTECTED = 16 /*0x10*/;
  public const uint CFM_LINK = 32 /*0x20*/;
  public const uint CFM_SIZE = 2147483648 /*0x80000000*/;
  public const uint CFM_COLOR = 1073741824 /*0x40000000*/;
  public const uint CFM_FACE = 536870912 /*0x20000000*/;
  public const uint CFM_OFFSET = 268435456 /*0x10000000*/;
  public const uint CFM_CHARSET = 134217728 /*0x08000000*/;
  public const uint CFM_SUBSCRIPT = 196608 /*0x030000*/;
  public const uint CFM_SUPERSCRIPT = 196608 /*0x030000*/;
  public const uint CFE_BOLD = 1;
  public const uint CFE_ITALIC = 2;
  public const uint CFE_UNDERLINE = 4;
  public const uint CFE_STRIKEOUT = 8;
  public const uint CFE_PROTECTED = 16 /*0x10*/;
  public const uint CFE_LINK = 32 /*0x20*/;
  public const uint CFE_AUTOCOLOR = 1073741824 /*0x40000000*/;
  public const uint CFE_SUBSCRIPT = 65536 /*0x010000*/;
  public const uint CFE_SUPERSCRIPT = 131072 /*0x020000*/;
  public const byte CFU_UNDERLINENONE = 0;
  public const byte CFU_UNDERLINE = 1;
  public const byte CFU_UNDERLINEWORD = 2;
  public const byte CFU_UNDERLINEDOUBLE = 3;
  public const byte CFU_UNDERLINEDOTTED = 4;
  public const byte CFU_UNDERLINEDASH = 5;
  public const byte CFU_UNDERLINEDASHDOT = 6;
  public const byte CFU_UNDERLINEDASHDOTDOT = 7;
  public const byte CFU_UNDERLINEWAVE = 8;
  public const byte CFU_UNDERLINETHICK = 9;
  public const byte CFU_UNDERLINEHAIRLINE = 10;
  public const int CFM_SMALLCAPS = 64 /*0x40*/;
  public const int CFM_ALLCAPS = 128 /*0x80*/;
  public const int CFM_HIDDEN = 256 /*0x0100*/;
  public const int CFM_OUTLINE = 512 /*0x0200*/;
  public const int CFM_SHADOW = 1024 /*0x0400*/;
  public const int CFM_EMBOSS = 2048 /*0x0800*/;
  public const int CFM_IMPRINT = 4096 /*0x1000*/;
  public const int CFM_DISABLED = 8192 /*0x2000*/;
  public const int CFM_REVISED = 16384 /*0x4000*/;
  public const int CFM_BACKCOLOR = 67108864 /*0x04000000*/;
  public const int CFM_LCID = 33554432 /*0x02000000*/;
  public const int CFM_UNDERLINETYPE = 8388608 /*0x800000*/;
  public const int CFM_WEIGHT = 4194304 /*0x400000*/;
  public const int CFM_SPACING = 2097152 /*0x200000*/;
  public const int CFM_KERNING = 1048576 /*0x100000*/;
  public const int CFM_STYLE = 524288 /*0x080000*/;
  public const int CFM_ANIMATION = 262144 /*0x040000*/;
  public const int CFM_REVAUTHOR = 32768 /*0x8000*/;
  public const short FW_DONTCARE = 0;
  public const short FW_THIN = 100;
  public const short FW_EXTRALIGHT = 200;
  public const short FW_LIGHT = 300;
  public const short FW_NORMAL = 400;
  public const short FW_MEDIUM = 500;
  public const short FW_SEMIBOLD = 600;
  public const short FW_BOLD = 700;
  public const short FW_EXTRABOLD = 800;
  public const short FW_HEAVY = 900;
  public const short FW_ULTRALIGHT = 200;
  public const short FW_REGULAR = 400;
  public const short FW_DEMIBOLD = 600;
  public const short FW_ULTRABOLD = 800;
  public const short FW_BLACK = 900;
  public const uint PFM_STARTINDENT = 1;
  public const uint PFM_RIGHTINDENT = 2;
  public const uint PFM_OFFSET = 4;
  public const uint PFM_ALIGNMENT = 8;
  public const uint PFM_TABSTOPS = 16 /*0x10*/;
  public const uint PFM_NUMBERING = 32 /*0x20*/;
  public const uint PFM_OFFSETINDENT = 2147483648 /*0x80000000*/;
  public const ushort PFN_BULLET = 1;
  public const ushort PFA_LEFT = 1;
  public const ushort PFA_RIGHT = 2;
  public const ushort PFA_CENTER = 3;
  public const ushort PFA_JUSTIFY = 4;

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT lp);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref CHARFORMAT lp);
}

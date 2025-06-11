// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.XlsIOConfig
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

[ToolboxBitmap(typeof (XlsIOConfig), "ToolBoxIcons.XlsIO.bmp")]
public class XlsIOConfig : Component
{
  public XlsIOConfig()
  {
    int num = ExcelEngine.IsSecurityGranted ? 1 : 0;
    ExcelEngine.ValidateLicense();
  }

  public string Copyright => "Syncfusion, Inc. 2001 - 2004";
}

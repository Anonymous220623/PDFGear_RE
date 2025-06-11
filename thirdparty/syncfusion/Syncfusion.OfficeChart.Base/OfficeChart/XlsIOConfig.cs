// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.XlsIOConfig
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

[ToolboxBitmap(typeof (XlsIOConfig), "ToolBoxIcons.XlsIO.bmp")]
internal class XlsIOConfig : Component
{
  public XlsIOConfig()
  {
    if (!ExcelEngine.IsSecurityGranted)
      return;
    ExcelEngine.ValidateLicense();
  }

  public string Copyright => "Syncfusion, Inc. 2001 - 2004";
}

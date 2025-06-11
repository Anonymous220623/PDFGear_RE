// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants.ChartPageSetupConstants
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants;

internal class ChartPageSetupConstants : IPageSetupConstantsProvider
{
  public string PageMarginsTag => "pageMargins";

  public string LeftMargin => "l";

  public string RightMargin => "r";

  public string TopMargin => "t";

  public string BottomMargin => "b";

  public string HeaderMargin => "header";

  public string FooterMargin => "footer";

  public string Namespace => "http://schemas.openxmlformats.org/drawingml/2006/chart";
}

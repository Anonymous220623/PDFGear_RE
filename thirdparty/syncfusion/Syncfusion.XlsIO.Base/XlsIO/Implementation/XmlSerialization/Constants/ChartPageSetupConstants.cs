// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Constants.ChartPageSetupConstants
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;

public class ChartPageSetupConstants : IPageSetupConstantsProvider
{
  private bool m_isChartEx;

  public string PageMarginsTag => "pageMargins";

  public string LeftMargin => "l";

  public string RightMargin => "r";

  public string TopMargin => "t";

  public string BottomMargin => "b";

  public string HeaderMargin => "header";

  public string FooterMargin => "footer";

  public string Namespace
  {
    get
    {
      return this.m_isChartEx ? "http://schemas.microsoft.com/office/drawing/2014/chartex" : "http://schemas.openxmlformats.org/drawingml/2006/chart";
    }
  }

  internal ChartPageSetupConstants(bool value) => this.m_isChartEx = value;

  public ChartPageSetupConstants()
  {
  }
}

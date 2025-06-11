// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.FitToCellArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
public class FitToCellArgument : MarkerArgument
{
  private const int DEF_PRIORITY = 4;
  private const string DEF_MARKER_VALUE = "fittocell";
  private bool m_isFitToCell;

  public override int Priority => 4;

  public override bool IsApplyable => true;

  public bool IsFitToCell => this.m_isFitToCell;

  public override MarkerArgument TryParse(string strArgument)
  {
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    if (!(strArgument.ToLower() == "fittocell"))
      return (MarkerArgument) null;
    this.m_isFitToCell = true;
    return (MarkerArgument) this.Clone();
  }
}

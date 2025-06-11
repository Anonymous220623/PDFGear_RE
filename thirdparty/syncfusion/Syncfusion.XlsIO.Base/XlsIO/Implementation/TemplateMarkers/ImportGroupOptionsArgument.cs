// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.ImportGroupOptionsArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
internal class ImportGroupOptionsArgument : MarkerArgument
{
  private const int DEF_PRIORITY = 8;
  private const string DEF_EXPAND_GROUP_MARKER_VALUE = "expandgroup";
  private const string DEF_COLLAPSE_GROUP_MARKER_VALUE = "collapsegroup";
  private bool m_isCollapse;
  private bool m_isExpand;

  internal ImportGroupOptionsArgument()
  {
  }

  public override int Priority => 8;

  public override bool IsApplyable => true;

  public bool IsCollapse => this.m_isCollapse;

  public bool IsExpand => this.m_isExpand;

  public override MarkerArgument TryParse(string strArgument)
  {
    this.m_isExpand = false;
    this.m_isCollapse = false;
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    if (strArgument.ToLower() == "expandgroup")
    {
      this.m_isExpand = true;
    }
    else
    {
      if (!(strArgument.ToLower() == "collapsegroup"))
        return (MarkerArgument) null;
      this.m_isCollapse = true;
    }
    return (MarkerArgument) this.Clone();
  }
}

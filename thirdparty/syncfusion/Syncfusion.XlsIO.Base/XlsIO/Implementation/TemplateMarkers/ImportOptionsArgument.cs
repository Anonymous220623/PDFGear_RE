// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.ImportOptionsArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
internal class ImportOptionsArgument : MarkerArgument
{
  private const int DEF_PRIORITY = 7;
  private const string DEF_DEFAULT_MARKER_VALUE = "default";
  private const string DEF_MERGE_MARKER_VALUE = "merge";
  private const string DEF_REPEAT_MARKER_VALUE = "repeat";
  private bool m_isMerge;
  private bool m_isRepeat;

  internal ImportOptionsArgument()
  {
  }

  public override int Priority => 7;

  public override bool IsApplyable => true;

  public bool IsMerge => this.m_isMerge;

  public bool IsRepeat => this.m_isRepeat;

  public override MarkerArgument TryParse(string strArgument)
  {
    this.m_isMerge = false;
    this.m_isRepeat = false;
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    if (strArgument.ToLower() == "merge")
    {
      this.m_isMerge = true;
    }
    else
    {
      if (!(strArgument.ToLower() == "repeat"))
        return (MarkerArgument) null;
      this.m_isRepeat = true;
    }
    return (MarkerArgument) this.Clone();
  }
}

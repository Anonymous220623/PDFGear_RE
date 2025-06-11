// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.HiddenGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class HiddenGroup
{
  public HiddenGroup()
  {
  }

  public HiddenGroup(int from, int to, int level, string groupName)
    : this(from, to, level, groupName, string.Empty)
  {
  }

  public HiddenGroup(int from, int to, int level, string groupName, string totalHeader)
  {
    this.From = from;
    this.To = to;
    this.Level = level;
    this.GroupName = groupName;
    this.ItemTotalHeader = totalHeader;
  }

  public int From { get; set; }

  public int To { get; set; }

  public int Level { get; set; }

  public string GroupName { get; set; }

  public string ItemTotalHeader { get; set; }

  public override string ToString() => $"{this.From}-{this.To}";

  public HiddenGroup Clone(HiddenGroup ParentGroup)
  {
    if (this.From < 0)
      return (HiddenGroup) null;
    return new HiddenGroup()
    {
      From = ParentGroup.From,
      To = ParentGroup.To,
      GroupName = ParentGroup.GroupName,
      Level = ParentGroup.Level
    };
  }
}

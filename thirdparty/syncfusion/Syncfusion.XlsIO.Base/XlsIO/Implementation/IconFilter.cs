// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.IconFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class IconFilter : IFilter
{
  private ExcelIconSetType m_iconSetType;
  private int m_iconId = int.MinValue;

  public ExcelFilterType FilterType => ExcelFilterType.IconFilter;

  public ExcelIconSetType IconSetType
  {
    get => this.m_iconSetType;
    internal set => this.m_iconSetType = value;
  }

  public int IconId
  {
    get => this.m_iconId;
    internal set => this.m_iconId = value;
  }

  public IconFilter Clone() => (IconFilter) this.MemberwiseClone();
}

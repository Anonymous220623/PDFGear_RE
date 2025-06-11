// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IconConditionValue
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public class IconConditionValue : ConditionValue, IIconConditionValue, IConditionValue
{
  private ExcelIconSetType m_iconSet;
  private int m_index;

  public ExcelIconSetType IconSet
  {
    get => this.m_iconSet;
    set => this.m_iconSet = value;
  }

  public int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public IconConditionValue(
    ExcelIconSetType icon,
    int index,
    ConditionValueType type,
    string value)
    : base(type, value)
  {
    this.m_iconSet = icon;
    this.m_index = index;
  }

  public IconConditionValue(ExcelIconSetType icon, int index)
  {
    this.m_iconSet = icon;
    this.m_index = index;
  }
}

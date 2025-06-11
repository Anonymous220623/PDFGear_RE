// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.ODFStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class ODFStyle : DefaultStyle, INamedObject
{
  private string m_name;
  private string m_dataStyleName;
  private uint m_defaultOutlineLevel;
  private string m_displayName;
  private uint m_listLevel;
  private string m_listStyleName;
  private string m_masterPageName;
  private string m_nextStyleName;
  private string m_parentStyleName;
  private string m_percentageDataStyleName;
  private bool m_isInlineStyle;
  private bool m_hasParent;
  internal byte styleFlags;
  internal bool isDefault;

  public new string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string DataStyleName
  {
    get => this.m_dataStyleName;
    set => this.m_dataStyleName = value;
  }

  internal uint DefaultOutlineLevel
  {
    get => this.m_defaultOutlineLevel;
    set => this.m_defaultOutlineLevel = value;
  }

  internal string DisplayName
  {
    get => this.m_displayName;
    set => this.m_displayName = value;
  }

  internal string ListStyleName
  {
    get => this.m_listStyleName;
    set => this.m_listStyleName = value;
  }

  internal string MasterPageName
  {
    get => this.m_masterPageName;
    set => this.m_masterPageName = value;
  }

  internal string NextStyleName
  {
    get => this.m_nextStyleName;
    set => this.m_nextStyleName = value;
  }

  internal string ParentStyleName
  {
    get => this.m_parentStyleName;
    set => this.m_parentStyleName = value;
  }

  internal string PercentageDataStyleName
  {
    get => this.m_percentageDataStyleName;
    set => this.m_percentageDataStyleName = value;
  }

  internal uint ListLevel
  {
    get => this.m_listLevel;
    set => this.m_listLevel = value;
  }

  internal bool IsInlineSTyle
  {
    get => this.m_isInlineStyle;
    set => this.m_isInlineStyle = value;
  }

  internal bool HasParent
  {
    get => this.m_hasParent;
    set => this.m_hasParent = value;
  }

  internal bool HasKey(int propertyKey, int flagname)
  {
    return (flagname & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  public override bool Equals(object obj)
  {
    ODFStyle odfStyle = obj as ODFStyle;
    if (odfStyle == null)
      return false;
    if (this.ParagraphProperties != null && odfStyle.ParagraphProperties != null && this.HasKey(1, (int) this.StylePropFlag) && odfStyle.HasKey(1, (int) odfStyle.StylePropFlag))
    {
      bool flag = this.ParagraphProperties.Equals((object) odfStyle.ParagraphProperties);
      if (!flag)
        return flag;
    }
    if (this.TableCellProperties != null && odfStyle.TableCellProperties != null && this.HasKey(2, (int) this.StylePropFlag) && odfStyle.HasKey(2, (int) odfStyle.StylePropFlag))
    {
      bool flag = this.TableCellProperties.Equals((object) odfStyle.TableCellProperties);
      if (!flag)
        return flag;
    }
    bool flag1 = this.Textproperties != null && odfStyle.Textproperties != null && this.HasKey(6, (int) this.StylePropFlag) && odfStyle.HasKey(6, (int) odfStyle.StylePropFlag);
    if (flag1)
    {
      this.Textproperties.Equals((object) odfStyle.Textproperties);
      if (!flag1)
        return flag1;
    }
    bool flag2 = this.TableColumnProperties != null && odfStyle.TableColumnProperties != null && this.HasKey(3, (int) this.StylePropFlag) && odfStyle.HasKey(3, (int) odfStyle.StylePropFlag);
    if (flag2)
    {
      this.TableColumnProperties.Equals((object) odfStyle.TableColumnProperties);
      if (!flag2)
        return flag2;
    }
    bool flag3 = this.TableRowProperties != null && odfStyle.TableRowProperties != null && this.HasKey(5, (int) this.StylePropFlag) && odfStyle.HasKey(5, (int) odfStyle.StylePropFlag);
    if (flag3)
    {
      flag3 = this.TableRowProperties.Equals((object) odfStyle.TableRowProperties);
      if (!flag3)
        return flag3;
    }
    return flag3;
  }

  internal void Close() => this.Dispose();
}

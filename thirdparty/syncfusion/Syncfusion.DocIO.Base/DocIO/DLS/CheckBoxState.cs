// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CheckBoxState
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class CheckBoxState
{
  private string m_Font;
  private string m_Value;
  private ContentControlProperties m_contentControlProperties;

  internal ContentControlProperties ContentControlProperties
  {
    get => this.m_contentControlProperties;
    set => this.m_contentControlProperties = value;
  }

  public string Font
  {
    get => this.m_Font;
    set
    {
      this.m_Font = value;
      if (this.ContentControlProperties == null || this.ContentControlProperties.Document.IsOpening || this.ContentControlProperties.Document.IsCloning)
        return;
      this.ContentControlProperties.ChangeCheckboxState(this.ContentControlProperties.IsChecked);
    }
  }

  public string Value
  {
    get => this.m_Value;
    set
    {
      this.m_Value = value;
      if (this.ContentControlProperties == null || this.ContentControlProperties.Document.IsOpening || this.ContentControlProperties.Document.IsCloning)
        return;
      this.ContentControlProperties.ChangeCheckboxState(this.ContentControlProperties.IsChecked);
    }
  }
}

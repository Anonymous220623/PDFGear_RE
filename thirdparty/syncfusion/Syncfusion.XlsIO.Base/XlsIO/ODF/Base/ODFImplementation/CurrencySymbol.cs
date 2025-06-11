// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.CurrencySymbol
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class CurrencySymbol : LanguageStyle
{
  private string m_data;

  internal string Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  public override bool Equals(object obj) => this.m_data.Equals((obj as CurrencySymbol).Data);
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.Month
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class Month : DateBase
{
  private string m_possesiveForm;
  private string m_textual;

  internal string PossesiveForm
  {
    get => this.m_possesiveForm;
    set => this.m_possesiveForm = value;
  }

  internal string Textual
  {
    get => this.m_textual;
    set => this.m_textual = value;
  }
}

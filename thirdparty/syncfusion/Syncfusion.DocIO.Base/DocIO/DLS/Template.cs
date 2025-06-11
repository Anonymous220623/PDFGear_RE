// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Template
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Template
{
  private SttbfAssoc m_assocStrings;

  public string Path
  {
    get => this.m_assocStrings.AttachedTemplate;
    set => this.m_assocStrings.AttachedTemplate = value;
  }

  internal Template(SttbfAssoc assocStrings) => this.m_assocStrings = assocStrings;
}

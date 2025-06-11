// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.EquationField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;

#nullable disable
namespace Syncfusion.Layouting;

internal class EquationField
{
  private WField m_eqFieldEntity;
  private LayoutedEQFields m_layouttedEQField;

  internal WField EQFieldEntity
  {
    get => this.m_eqFieldEntity;
    set => this.m_eqFieldEntity = value;
  }

  internal LayoutedEQFields LayouttedEQField
  {
    get => this.m_layouttedEQField;
    set => this.m_layouttedEQField = value;
  }
}

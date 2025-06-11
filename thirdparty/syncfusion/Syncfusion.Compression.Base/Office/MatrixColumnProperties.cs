// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.MatrixColumnProperties
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class MatrixColumnProperties : OwnerHolder
{
  private MathHorizontalAlignment m_alignment;
  private int m_count;

  internal int Count
  {
    get => this.m_count;
    set => this.m_count = value;
  }

  internal MathHorizontalAlignment Alignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  internal MatrixColumnProperties(IOfficeMathEntity owner)
    : base(owner)
  {
  }

  internal MatrixColumnProperties Clone(IOfficeMathEntity owner)
  {
    MatrixColumnProperties columnProperties = (MatrixColumnProperties) this.MemberwiseClone();
    columnProperties.SetOwner(owner);
    return columnProperties;
  }

  internal override void Close() => base.Close();
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.CameraTool
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

internal class CameraTool
{
  private int m_shapeId;
  private string m_localName;
  private string m_cellRange;

  internal int ShapeID
  {
    get => this.m_shapeId;
    set => this.m_shapeId = value;
  }

  internal string LocalName
  {
    get => this.m_localName;
    set => this.m_localName = value;
  }

  internal string CellRange
  {
    get => this.m_cellRange;
    set => this.m_cellRange = value;
  }

  internal CameraTool Clone(object parent) => (CameraTool) this.MemberwiseClone();
}

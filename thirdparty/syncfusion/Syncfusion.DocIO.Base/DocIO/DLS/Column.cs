// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Column
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Column(IWordDocument doc) : FormatBase(doc, (Entity) null)
{
  internal const int WidthKey = 1;
  internal const int SpaceKey = 2;

  public float Width
  {
    get => (float) this.GetPropertyValue(1);
    set => this.SetPropertyValue(1, (object) value);
  }

  public float Space
  {
    get => (float) this.GetPropertyValue(2);
    set => this.SetPropertyValue(2, (object) value);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("Width", this.Width);
    writer.WriteValue("Spacing", this.Space);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.Width = reader.ReadFloat("Width");
    this.Space = reader.ReadFloat("Spacing");
  }

  internal Column Clone() => (Column) this.CloneImpl();

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
      case 2:
        return (object) 0.0f;
      default:
        throw new ArgumentException("key has invalid value");
    }
  }

  internal bool Compare(Column column)
  {
    return this.Compare(1, (FormatBase) column) && this.Compare(2, (FormatBase) column);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value)
  {
    this[propKey] = value;
    this.OnStateChange((object) this);
  }
}

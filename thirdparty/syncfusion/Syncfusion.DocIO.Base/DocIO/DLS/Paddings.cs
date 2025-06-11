// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Paddings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Paddings : FormatBase
{
  public const int LeftKey = 1;
  public const int TopKey = 2;
  public const int BottomKey = 3;
  public const int RightKey = 4;

  public float Left
  {
    get => (float) this[1];
    set => this[1] = (object) value;
  }

  public float Top
  {
    get => (float) this[2];
    set => this[2] = (object) value;
  }

  public float Right
  {
    get => (float) this[4];
    set => this[4] = (object) value;
  }

  public float Bottom
  {
    get => (float) this[3];
    set => this[3] = (object) value;
  }

  public float All
  {
    set => this.Left = this.Right = this.Top = this.Bottom = value;
  }

  internal bool IsEmpty
  {
    get
    {
      return (double) this.Left == 0.0 && (double) this.Right == 0.0 && (double) this.Top == 0.0 && (double) this.Bottom == 0.0;
    }
  }

  internal Paddings(FormatBase parent, int baseKey)
    : base(parent, baseKey)
  {
  }

  internal Paddings()
  {
  }

  internal void UpdatePaddings(Paddings padding)
  {
    if (padding.IsDefault)
      return;
    this.Left = padding.Left;
    this.Right = padding.Right;
    this.Top = padding.Top;
    this.Bottom = padding.Bottom;
  }

  internal void ImportPaddings(Paddings basePaddings)
  {
    if (basePaddings.HasKey(1))
      this.Left = basePaddings.Left;
    if (basePaddings.HasKey(4))
      this.Right = basePaddings.Right;
    if (basePaddings.HasKey(2))
      this.Top = basePaddings.Top;
    if (!basePaddings.HasKey(3))
      return;
    this.Bottom = basePaddings.Bottom;
  }

  internal bool Compare(Paddings paddings)
  {
    return this.Compare(this.GetBaseKey(1), (FormatBase) paddings) && this.Compare(this.GetBaseKey(4), (FormatBase) paddings) && this.Compare(this.GetBaseKey(2), (FormatBase) paddings) && this.Compare(this.GetBaseKey(3), (FormatBase) paddings);
  }

  internal Paddings Clone()
  {
    return new Paddings()
    {
      Left = this.Left,
      Right = this.Right,
      Top = this.Top,
      Bottom = this.Bottom
    };
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
        return (object) 0.0f;
      case 2:
        return (object) 0.0f;
      case 3:
        return (object) 0.0f;
      case 4:
        return (object) 0.0f;
      default:
        throw new ArgumentException("key has invalid value");
    }
  }

  protected override void InitXDLSHolder()
  {
    if (!this.IsDefault)
      return;
    this.XDLSHolder.SkipMe = true;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(1))
      writer.WriteValue("Left", this.Left);
    if (this.HasKey(4))
      writer.WriteValue("Right", this.Right);
    if (this.HasKey(3))
      writer.WriteValue("Bottom", this.Bottom);
    if (!this.HasKey(2))
      return;
    writer.WriteValue("Top", this.Top);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Left"))
      this.Left = reader.ReadFloat("Left");
    if (reader.HasAttribute("Right"))
      this.Right = reader.ReadFloat("Right");
    if (reader.HasAttribute("Bottom"))
      this.Bottom = reader.ReadFloat("Bottom");
    if (!reader.HasAttribute("Top"))
      return;
    this.Top = reader.ReadFloat("Top");
  }

  protected override void OnChange(FormatBase format, int propertyKey)
  {
    base.OnChange(format, propertyKey);
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.Values
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class Values
{
  private bool? boolValue;
  private ColorValues color;
  private float? floatValue;
  private int? intValue;
  private string stringValue;

  internal bool? Bool
  {
    get => this.boolValue;
    set => this.boolValue = value;
  }

  internal ColorValues Color
  {
    get => this.color;
    set => this.color = value;
  }

  internal float? Float
  {
    get => this.floatValue;
    set => this.floatValue = value;
  }

  internal int? Int
  {
    get => this.intValue;
    set => this.intValue = value;
  }

  internal string String
  {
    get => this.stringValue;
    set => this.stringValue = value;
  }

  internal Values Clone()
  {
    Values values = (Values) this.MemberwiseClone();
    if (this.color != null)
      values.color = this.color.Clone();
    return values;
  }
}

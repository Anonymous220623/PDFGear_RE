// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.PresetColor
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class PresetColor
{
  private string colorValue;

  internal string Value
  {
    get => this.colorValue;
    set => this.colorValue = value;
  }

  internal PresetColor Clone()
  {
    PresetColor presetColor = (PresetColor) this.MemberwiseClone();
    if (this.colorValue != null)
      presetColor.colorValue = this.colorValue;
    return presetColor;
  }
}

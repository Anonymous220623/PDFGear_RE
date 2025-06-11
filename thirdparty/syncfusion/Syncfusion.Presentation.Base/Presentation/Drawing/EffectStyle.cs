// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.EffectStyle
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class EffectStyle
{
  private EffectProperties _effectProperties;
  private Dictionary<string, Stream> _preservedElements;

  public EffectStyle(Syncfusion.Presentation.Presentation presentation)
  {
    this._effectProperties = new EffectProperties(presentation);
  }

  internal EffectProperties EffectProperties
  {
    get => this._effectProperties;
    set => this._effectProperties = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._effectProperties != null)
      this._effectProperties.ClearAll();
    if (this._preservedElements == null)
      return;
    foreach (KeyValuePair<string, Stream> preservedElement in this._preservedElements)
      preservedElement.Value.Dispose();
    this._preservedElements.Clear();
    this._preservedElements = (Dictionary<string, Stream>) null;
  }

  public EffectStyle Clone()
  {
    EffectStyle effectStyle = (EffectStyle) this.MemberwiseClone();
    effectStyle._effectProperties = this._effectProperties.Clone();
    if (this._preservedElements != null)
      effectStyle._preservedElements = Helper.CloneDictionary(this._preservedElements);
    return effectStyle;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._effectProperties.SetParent(presentation);
  }
}

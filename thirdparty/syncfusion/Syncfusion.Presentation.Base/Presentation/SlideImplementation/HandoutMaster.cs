// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.HandoutMaster
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Themes;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class HandoutMaster : BaseSlide
{
  private Theme _themeCollection;
  internal new Dictionary<string, string> ColorMap;

  internal HandoutMaster(Syncfusion.Presentation.Presentation presentation)
    : base(presentation)
  {
    this._themeCollection = new Theme(this);
    this.ColorMap = new Dictionary<string, string>();
  }

  internal Theme ThemeCollection
  {
    get => this._themeCollection;
    set => this._themeCollection = value;
  }

  internal override void Close()
  {
    base.Close();
    if (this._themeCollection != null)
    {
      this._themeCollection.Close();
      this._themeCollection = (Theme) null;
    }
    if (this.ColorMap == null)
      return;
    this.ColorMap.Clear();
    this.ColorMap = (Dictionary<string, string>) null;
  }

  public HandoutMaster Clone()
  {
    HandoutMaster newParent = (HandoutMaster) this.MemberwiseClone();
    this.Clone((BaseSlide) newParent);
    newParent._themeCollection = this._themeCollection.Clone();
    newParent._themeCollection.SetParent((BaseSlide) newParent);
    newParent.ColorMap = Helper.CloneDictionary(this.ColorMap);
    return newParent;
  }
}

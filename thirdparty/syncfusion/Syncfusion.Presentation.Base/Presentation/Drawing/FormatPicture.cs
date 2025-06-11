// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.FormatPicture
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class FormatPicture
{
  private int _bottomCrop;
  private int _leftCrop;
  private int _rightCrop;
  private int _topCrop;
  private List<Stream> _list;

  internal int ObtainBottomCrop() => this._bottomCrop;

  internal int ObtainLeftCrop() => this._leftCrop;

  internal int ObtainRightCrop() => this._rightCrop;

  internal int ObtainTopCrop() => this._topCrop;

  internal void AssignLeftCrop(int value) => this._leftCrop = value;

  internal void AssignBottomCrop(int value) => this._bottomCrop = value;

  internal void AssignRightCrop(int value) => this._rightCrop = value;

  internal void AssignTopCrop(int value) => this._topCrop = value;

  internal double BottomCrop
  {
    get => (double) this._bottomCrop / 1000.0;
    set => this._bottomCrop = (int) value;
  }

  internal double LeftCrop
  {
    get => (double) this._leftCrop / 1000.0;
    set => this._leftCrop = (int) value;
  }

  internal double RightCrop
  {
    get => (double) this._rightCrop / 1000.0;
    set => this._rightCrop = (int) value;
  }

  internal double TopCrop
  {
    get => (double) this._topCrop / 1000.0;
    set => this._topCrop = (int) value;
  }

  internal List<Stream> BlipList
  {
    get => this._list;
    set => this._list = value;
  }

  internal void Close()
  {
    if (this._list == null)
      return;
    foreach (Stream stream in this._list)
      stream?.Dispose();
    this._list.Clear();
    this._list = (List<Stream>) null;
  }

  public FormatPicture Clone()
  {
    FormatPicture formatPicture = (FormatPicture) this.MemberwiseClone();
    if (this._list != null)
      formatPicture._list = Helper.CloneList(this._list);
    return formatPicture;
  }
}

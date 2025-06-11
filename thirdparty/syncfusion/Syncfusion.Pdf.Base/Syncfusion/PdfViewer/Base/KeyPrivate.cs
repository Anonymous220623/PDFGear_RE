// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.KeyPrivate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class KeyPrivate : PostScriptObj
{
  private readonly KeyProperty<PostScriptArray> subrs;
  private readonly KeyProperty<PostScriptArray> otherSubrs;
  private readonly Dictionary<int, byte[]> subroutines;

  public PostScriptArray Subrs => this.subrs.GetValue();

  public PostScriptArray OtherSubrs => this.otherSubrs.GetValue();

  public KeyPrivate()
  {
    this.subrs = this.CreateProperty<PostScriptArray>(new KeyPropertyDescriptor()
    {
      Name = nameof (Subrs)
    });
    this.otherSubrs = this.CreateProperty<PostScriptArray>(new KeyPropertyDescriptor()
    {
      Name = nameof (OtherSubrs)
    });
    this.subroutines = new Dictionary<int, byte[]>();
  }

  public byte[] GetSubr(int index)
  {
    byte[] byteArray;
    if (!this.subroutines.TryGetValue(index, out byteArray))
    {
      byteArray = this.Subrs.GetElementAs<PostScriptStrHelper>(index).ToByteArray();
      this.subroutines[index] = byteArray;
    }
    return byteArray;
  }
}

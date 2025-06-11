// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPrivateType1Format
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPrivateType1Format : SystemFontPostScriptObject
{
  private readonly SystemFontProperty<SystemFontPostScriptArray> subrs;
  private readonly SystemFontProperty<SystemFontPostScriptArray> otherSubrs;
  private readonly Dictionary<int, byte[]> subroutines;

  public SystemFontPostScriptArray Subrs => this.subrs.GetValue();

  public SystemFontPostScriptArray OtherSubrs => this.otherSubrs.GetValue();

  public SystemFontPrivateType1Format()
  {
    this.subrs = this.CreateProperty<SystemFontPostScriptArray>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (Subrs)
    });
    this.otherSubrs = this.CreateProperty<SystemFontPostScriptArray>(new SystemFontPropertyDescriptor()
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
      byteArray = this.Subrs.GetElementAs<SystemFontPostScriptString>(index).ToByteArray();
      this.subroutines[index] = byteArray;
    }
    return byteArray;
  }
}

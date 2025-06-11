// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JBIG2BaseFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class JBIG2BaseFlags
{
  protected internal int flagsAsInt;
  protected internal IDictionary flags = (IDictionary) new Dictionary<string, int>();

  public int GetFlagValue(string key) => ((int?) this.flags[(object) key]).Value;

  internal abstract void setFlags(int flagsAsInt);
}

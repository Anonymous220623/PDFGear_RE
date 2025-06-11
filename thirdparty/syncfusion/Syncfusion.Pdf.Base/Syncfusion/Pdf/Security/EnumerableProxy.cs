// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EnumerableProxy
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EnumerableProxy : IEnumerable
{
  private readonly IEnumerable m_value;

  internal EnumerableProxy(IEnumerable value) => this.m_value = value;

  public IEnumerator GetEnumerator() => this.m_value.GetEnumerator();
}

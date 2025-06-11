// Decompiled with JetBrains decompiler
// Type: Ionic.SelectionCriterion
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using Ionic.Zip;
using System.Diagnostics;

#nullable disable
namespace Ionic;

internal abstract class SelectionCriterion
{
  internal virtual bool Verbose { get; set; }

  internal abstract bool Evaluate(string filename);

  [Conditional("SelectorTrace")]
  protected static void CriterionTrace(string format, params object[] args)
  {
  }

  internal abstract bool Evaluate(ZipEntry entry);
}

// Decompiled with JetBrains decompiler
// Type: Ionic.SizeCriterion
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Ionic;

internal class SizeCriterion : SelectionCriterion
{
  internal ComparisonOperator Operator;
  internal long Size;

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("size ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.Size.ToString());
    return stringBuilder.ToString();
  }

  internal override bool Evaluate(string filename) => this._Evaluate(new FileInfo(filename).Length);

  private bool _Evaluate(long Length)
  {
    switch (this.Operator)
    {
      case ComparisonOperator.GreaterThan:
        return Length > this.Size;
      case ComparisonOperator.GreaterThanOrEqualTo:
        return Length >= this.Size;
      case ComparisonOperator.LesserThan:
        return Length < this.Size;
      case ComparisonOperator.LesserThanOrEqualTo:
        return Length <= this.Size;
      case ComparisonOperator.EqualTo:
        return Length == this.Size;
      case ComparisonOperator.NotEqualTo:
        return Length != this.Size;
      default:
        throw new ArgumentException("Operator");
    }
  }

  internal override bool Evaluate(ZipEntry entry) => this._Evaluate(entry.UncompressedSize);
}

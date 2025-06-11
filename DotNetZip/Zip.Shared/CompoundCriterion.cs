// Decompiled with JetBrains decompiler
// Type: Ionic.CompoundCriterion
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using Ionic.Zip;
using System;
using System.Text;

#nullable disable
namespace Ionic;

internal class CompoundCriterion : SelectionCriterion
{
  internal LogicalConjunction Conjunction;
  internal SelectionCriterion Left;
  private SelectionCriterion _Right;

  internal SelectionCriterion Right
  {
    get => this._Right;
    set
    {
      this._Right = value;
      if (value == null)
      {
        this.Conjunction = LogicalConjunction.NONE;
      }
      else
      {
        if (this.Conjunction != LogicalConjunction.NONE)
          return;
        this.Conjunction = LogicalConjunction.AND;
      }
    }
  }

  internal override bool Evaluate(string filename)
  {
    bool flag = this.Left.Evaluate(filename);
    switch (this.Conjunction)
    {
      case LogicalConjunction.AND:
        if (flag)
        {
          flag = this.Right.Evaluate(filename);
          break;
        }
        break;
      case LogicalConjunction.OR:
        if (!flag)
        {
          flag = this.Right.Evaluate(filename);
          break;
        }
        break;
      case LogicalConjunction.XOR:
        flag ^= this.Right.Evaluate(filename);
        break;
      default:
        throw new ArgumentException("Conjunction");
    }
    return flag;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("(").Append(this.Left != null ? this.Left.ToString() : "null").Append(" ").Append(this.Conjunction.ToString()).Append(" ").Append(this.Right != null ? this.Right.ToString() : "null").Append(")");
    return stringBuilder.ToString();
  }

  internal override bool Evaluate(ZipEntry entry)
  {
    bool flag = this.Left.Evaluate(entry);
    switch (this.Conjunction)
    {
      case LogicalConjunction.AND:
        if (flag)
        {
          flag = this.Right.Evaluate(entry);
          break;
        }
        break;
      case LogicalConjunction.OR:
        if (!flag)
        {
          flag = this.Right.Evaluate(entry);
          break;
        }
        break;
      case LogicalConjunction.XOR:
        flag ^= this.Right.Evaluate(entry);
        break;
    }
    return flag;
  }
}

// Decompiled with JetBrains decompiler
// Type: Ionic.AttributesCriterion
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Ionic;

internal class AttributesCriterion : SelectionCriterion
{
  private FileAttributes _Attributes;
  internal ComparisonOperator Operator;

  internal string AttributeString
  {
    get
    {
      string attributeString = "";
      if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes) 0)
        attributeString += "H";
      if ((this._Attributes & FileAttributes.System) != (FileAttributes) 0)
        attributeString += "S";
      if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes) 0)
        attributeString += "R";
      if ((this._Attributes & FileAttributes.Archive) != (FileAttributes) 0)
        attributeString += "A";
      if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes) 0)
        attributeString += "L";
      if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes) 0)
        attributeString += "I";
      return attributeString;
    }
    set
    {
      this._Attributes = FileAttributes.Normal;
      foreach (char ch in value.ToUpper())
      {
        switch (ch)
        {
          case 'A':
            if ((this._Attributes & FileAttributes.Archive) != (FileAttributes) 0)
              throw new ArgumentException($"Repeated flag. ({ch})", nameof (value));
            this._Attributes |= FileAttributes.Archive;
            break;
          case 'H':
            if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes) 0)
              throw new ArgumentException($"Repeated flag. ({ch})", nameof (value));
            this._Attributes |= FileAttributes.Hidden;
            break;
          case 'I':
            if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes) 0)
              throw new ArgumentException($"Repeated flag. ({ch})", nameof (value));
            this._Attributes |= FileAttributes.NotContentIndexed;
            break;
          case 'L':
            if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes) 0)
              throw new ArgumentException($"Repeated flag. ({ch})", nameof (value));
            this._Attributes |= FileAttributes.ReparsePoint;
            break;
          case 'R':
            if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes) 0)
              throw new ArgumentException($"Repeated flag. ({ch})", nameof (value));
            this._Attributes |= FileAttributes.ReadOnly;
            break;
          case 'S':
            if ((this._Attributes & FileAttributes.System) != (FileAttributes) 0)
              throw new ArgumentException($"Repeated flag. ({ch})", nameof (value));
            this._Attributes |= FileAttributes.System;
            break;
          default:
            throw new ArgumentException(value);
        }
      }
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("attributes ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.AttributeString);
    return stringBuilder.ToString();
  }

  private bool _EvaluateOne(FileAttributes fileAttrs, FileAttributes criterionAttrs)
  {
    return (this._Attributes & criterionAttrs) != criterionAttrs || (fileAttrs & criterionAttrs) == criterionAttrs;
  }

  internal override bool Evaluate(string filename)
  {
    return Directory.Exists(filename) ? this.Operator != ComparisonOperator.EqualTo : this._Evaluate(File.GetAttributes(filename));
  }

  private bool _Evaluate(FileAttributes fileAttrs)
  {
    bool flag = this._EvaluateOne(fileAttrs, FileAttributes.Hidden);
    if (flag)
      flag = this._EvaluateOne(fileAttrs, FileAttributes.System);
    if (flag)
      flag = this._EvaluateOne(fileAttrs, FileAttributes.ReadOnly);
    if (flag)
      flag = this._EvaluateOne(fileAttrs, FileAttributes.Archive);
    if (flag)
      flag = this._EvaluateOne(fileAttrs, FileAttributes.NotContentIndexed);
    if (flag)
      flag = this._EvaluateOne(fileAttrs, FileAttributes.ReparsePoint);
    if (this.Operator != ComparisonOperator.EqualTo)
      flag = !flag;
    return flag;
  }

  internal override bool Evaluate(ZipEntry entry) => this._Evaluate(entry.Attributes);
}

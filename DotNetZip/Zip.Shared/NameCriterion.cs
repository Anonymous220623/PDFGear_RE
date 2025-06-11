// Decompiled with JetBrains decompiler
// Type: Ionic.NameCriterion
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using Ionic.Zip;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Ionic;

internal class NameCriterion : SelectionCriterion
{
  private Regex _re;
  private string _regexString;
  internal ComparisonOperator Operator;
  private string _MatchingFileSpec;

  internal virtual string MatchingFileSpec
  {
    set
    {
      if (Directory.Exists(value))
        this._MatchingFileSpec = $".{Path.DirectorySeparatorChar.ToString()}{value}{Path.DirectorySeparatorChar.ToString()}*.*";
      else
        this._MatchingFileSpec = value;
      this._regexString = $"^{Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*").Replace("\\*", ".*").Replace("\\?", "[^\\\\\\.]")}$";
      this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("name ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" '").Append(this._MatchingFileSpec).Append("'");
    return stringBuilder.ToString();
  }

  internal override bool Evaluate(string filename) => this._Evaluate(filename);

  private bool _Evaluate(string fullpath)
  {
    bool flag = this._re.IsMatch(this._MatchingFileSpec.IndexOf(Path.DirectorySeparatorChar) == -1 ? Path.GetFileName(fullpath) : fullpath);
    if (this.Operator != ComparisonOperator.EqualTo)
      flag = !flag;
    return flag;
  }

  internal override bool Evaluate(ZipEntry entry)
  {
    return this._Evaluate(entry.FileName.Replace(Path.DirectorySeparatorChar == '/' ? '\\' : '/', Path.DirectorySeparatorChar));
  }
}

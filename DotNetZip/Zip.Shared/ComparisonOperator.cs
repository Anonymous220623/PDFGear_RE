// Decompiled with JetBrains decompiler
// Type: Ionic.ComparisonOperator
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System.ComponentModel;

#nullable disable
namespace Ionic;

internal enum ComparisonOperator
{
  [Description(">")] GreaterThan,
  [Description(">=")] GreaterThanOrEqualTo,
  [Description("<")] LesserThan,
  [Description("<=")] LesserThanOrEqualTo,
  [Description("=")] EqualTo,
  [Description("!=")] NotEqualTo,
}

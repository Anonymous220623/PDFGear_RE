// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.ExceptionStringTable
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace HandyControl.Expression.Drawing;

[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class ExceptionStringTable
{
  private static ResourceManager ResourceMan;

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture { get; set; }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (ExceptionStringTable.ResourceMan == null)
        ExceptionStringTable.ResourceMan = new ResourceManager("Microsoft.Expression.Drawing.ExceptionStringTable", typeof (ExceptionStringTable).Assembly);
      return ExceptionStringTable.ResourceMan;
    }
  }

  internal static string TypeNotSupported
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (TypeNotSupported), ExceptionStringTable.Culture);
    }
  }
}

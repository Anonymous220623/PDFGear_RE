// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ExceptionUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace pdfeditor.Utils;

public static class ExceptionUtils
{
  public static string CreateUnhandledExceptionMessage(this Exception exception)
  {
    if (exception == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("Time: ").AppendLine(DateTime.Now.ToString()).Append("Message: ").AppendLine(exception.Message ?? "").Append("StackTrace: ").AppendLine(GetStackTrace(exception)).Append("ExceptionType: ").AppendLine(exception.GetType().FullName).Append("Exception: ").AppendLine(exception.ToString());
    foreach (Exception ex in (IEnumerable<Exception>) ExpandException(exception))
      stringBuilder.Append('\n').AppendLine("InnerException: ").Append("\tMessage: ").AppendLine(ex.Message ?? "").Append("\tStackTrace: ").AppendLine(GetStackTrace(ex)).Append("\tExceptionType: ").AppendLine(ex.GetType().FullName).Append("\tException: ").AppendLine(ex.ToString());
    return stringBuilder.ToString();

    static IReadOnlyCollection<Exception> ExpandException(Exception ex)
    {
      if (ex.InnerException == null && !(ex is AggregateException))
        return (IReadOnlyCollection<Exception>) Array.Empty<Exception>();
      List<Exception> list = new List<Exception>();
      ExpandExceptionCore(list, ex);
      return (IReadOnlyCollection<Exception>) list;
    }

    static void ExpandExceptionCore(List<Exception> list, Exception ex)
    {
      if (ex == null)
        return;
      if (ex.InnerException != null)
      {
        list.Add(ex.InnerException);
        ExpandExceptionCore(list, ex.InnerException);
      }
      if (!(ex is AggregateException aggregateException))
        return;
      foreach (Exception innerException in aggregateException.InnerExceptions)
      {
        list.Add(innerException);
        ExpandExceptionCore(list, innerException);
      }
    }

    string GetStackTrace(Exception ex)
    {
      return ex.Data != null && ex.Data.Contains((object) "OriginalStackTrace") && ex.Data[(object) "OriginalStackTrace"] is string str ? str : exception.StackTrace ?? "";
    }
  }
}

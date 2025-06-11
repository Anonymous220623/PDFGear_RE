// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PopupAnnotationWrapperExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net.Annotations;
using System;
using System.Collections.Generic;

#nullable disable
namespace pdfeditor.Utils;

public static class PopupAnnotationWrapperExtensions
{
  private static Dictionary<string, bool> popupIsOpenCache = new Dictionary<string, bool>();

  public static void SetIsOpen(PdfPopupAnnotation annot, bool value)
  {
    if (annot?.Page?.Document == null)
      throw new ArgumentNullException(nameof (annot));
    string name = annot.Parent?.Name;
    if (string.IsNullOrEmpty(name))
      return;
    string key = $"{annot.Page.Document.Handle.ToInt64():X2}_{annot.Page.PageIndex}_{name}";
    PopupAnnotationWrapperExtensions.popupIsOpenCache[key] = value;
  }

  public static bool GetIsOpen(PdfPopupAnnotation annot)
  {
    if (annot?.Page?.Document == null)
      throw new ArgumentNullException(nameof (annot));
    string name = annot.Parent?.Name;
    if (!string.IsNullOrEmpty(name))
    {
      string key = $"{annot.Page.Document.Handle.ToInt64():X2}_{annot.Page.PageIndex}_{name}";
      bool isOpen;
      if (PopupAnnotationWrapperExtensions.popupIsOpenCache.TryGetValue(key, out isOpen))
        return isOpen;
    }
    return annot.IsOpen;
  }
}

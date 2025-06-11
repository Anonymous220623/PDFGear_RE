// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.TextViewProxy
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

internal class TextViewProxy
{
  private static readonly Type typeITextView;
  private static readonly Type typeTextViewBase;
  private static Func<object, TextPointer, TextPointer, Geometry> getTightBoundingGeometryFromTextPositionsFunc;
  private static Func<object, bool> validateFunc;

  public static bool IsSupported { get; }

  static TextViewProxy()
  {
    try
    {
      TextViewProxy.typeITextView = typeof (FlowDocument).Assembly?.GetType("System.Windows.Documents.ITextView");
    }
    catch
    {
    }
    try
    {
      TextViewProxy.typeTextViewBase = typeof (FlowDocument).Assembly?.GetType("MS.Internal.Documents.TextViewBase");
    }
    catch
    {
    }
    TextViewProxy.IsSupported = TextViewProxy.typeITextView != (Type) null && TextViewProxy.typeTextViewBase != (Type) null;
  }

  private TextViewProxy(object rawObject, Dispatcher dispatcher)
  {
    this.RawObject = rawObject;
    this.Dispatcher = dispatcher;
  }

  public object RawObject { get; }

  public Dispatcher Dispatcher { get; }

  public Geometry GetTightBoundingGeometryFromTextPositions(
    TextPointer startPosition,
    TextPointer endPosition)
  {
    if (TextViewProxy.getTightBoundingGeometryFromTextPositionsFunc == null)
      TextViewProxy.getTightBoundingGeometryFromTextPositionsFunc = ReflectionHelper.BuildMethodFunction<object, TextPointer, TextPointer, Geometry>(TextViewProxy.typeITextView.GetMethod(nameof (GetTightBoundingGeometryFromTextPositions)), new Type[2]
      {
        typeof (TextPointer),
        typeof (TextPointer)
      });
    return TextViewProxy.getTightBoundingGeometryFromTextPositionsFunc(this.RawObject, startPosition, endPosition);
  }

  public bool Validate()
  {
    if (TextViewProxy.validateFunc == null)
      TextViewProxy.validateFunc = ReflectionHelper.BuildMethodFunction<object, bool>(TextViewProxy.typeTextViewBase.GetMethod(nameof (Validate), BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), Array.Empty<ParameterModifier>()));
    return TextViewProxy.validateFunc(this.RawObject);
  }

  public static TextViewProxy Create(RichTextBox rtb)
  {
    if (!TextViewProxy.IsSupported)
      return (TextViewProxy) null;
    if (rtb == null)
      throw new ArgumentNullException(nameof (rtb));
    try
    {
      IServiceProvider content;
      int num;
      if (VisualTreeHelper.GetChild((DependencyObject) rtb, 0) is FrameworkElement child && child.FindName("PART_ContentHost") is ScrollViewer name)
      {
        content = name.Content as IServiceProvider;
        num = content != null ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        if (TextViewProxy.typeITextView != (Type) null)
          return new TextViewProxy(content.GetService(TextViewProxy.typeITextView), rtb.Dispatcher);
      }
    }
    catch
    {
    }
    return (TextViewProxy) null;
  }
}

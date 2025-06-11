// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ModernMessageBox
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom.MessageBoxHelper;
using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace CommomLib.Commom;

public class ModernMessageBox
{
  public static MessageBoxResult Show(ModernMessageBoxOptions options)
  {
    return ModernMessageBox.ShowCore(options, (ModernMessageBoxCloser) null);
  }

  public static MessageBoxResult Show(
    ModernMessageBoxOptions options,
    ModernMessageBoxCloser closer)
  {
    return ModernMessageBox.ShowCore(options, closer);
  }

  public static MessageBoxResult Show(
    string messageBoxText,
    string caption = "",
    MessageBoxButton button = MessageBoxButton.OK,
    MessageBoxResult defaultResult = MessageBoxResult.None,
    CultureInfo cultureInfo = null,
    bool isButtonReversed = false)
  {
    return ModernMessageBox.ShowCore((Window) null, messageBoxText, caption, button, defaultResult, cultureInfo, isButtonReversed);
  }

  public static MessageBoxResult Show(
    Window owner,
    string messageBoxText,
    string caption = "",
    MessageBoxButton button = MessageBoxButton.OK,
    MessageBoxResult defaultResult = MessageBoxResult.None,
    CultureInfo cultureInfo = null,
    bool isButtonReversed = false)
  {
    return ModernMessageBox.ShowCore(owner, messageBoxText, caption, button, defaultResult, cultureInfo, isButtonReversed);
  }

  private static MessageBoxResult ShowCore(
    Window owner,
    string messageBoxText,
    string caption,
    MessageBoxButton button,
    MessageBoxResult defaultResult,
    CultureInfo cultureInfo,
    bool isButtonReversed)
  {
    return ModernMessageBox.ShowCore(new ModernMessageBoxOptions()
    {
      Owner = owner,
      Caption = caption,
      MessageBoxContent = (object) messageBoxText,
      Button = button,
      DefaultResult = defaultResult,
      CultureInfo = cultureInfo,
      UIOverrides = {
        IsButtonsReversed = isButtonReversed
      }
    }, (ModernMessageBoxCloser) null);
  }

  private static MessageBoxResult ShowCore(
    ModernMessageBoxOptions options,
    ModernMessageBoxCloser closer)
  {
    MessageBoxButton messageBoxButton = options != null ? options.Button : throw new ArgumentNullException(nameof (options));
    CommomLib.Controls.ModernMessageBox modernMessageBox = new CommomLib.Controls.ModernMessageBox(options);
    if (closer != null)
      closer.Owner = closer.Owner == null ? modernMessageBox : throw new ArgumentException("ModernMessageBoxCloser");
    modernMessageBox.ShowDialog();
    if (closer != null)
      closer.Owner = (CommomLib.Controls.ModernMessageBox) null;
    bool? dialogResultInternal = modernMessageBox.DialogResultInternal;
    switch (messageBoxButton)
    {
      case MessageBoxButton.OK:
        return MessageBoxResult.OK;
      case MessageBoxButton.OKCancel:
        return !dialogResultInternal.GetValueOrDefault() ? MessageBoxResult.Cancel : MessageBoxResult.OK;
      case MessageBoxButton.YesNoCancel:
        if (dialogResultInternal.GetValueOrDefault())
          return MessageBoxResult.Yes;
        bool? nullable = dialogResultInternal;
        bool flag = false;
        return !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? MessageBoxResult.Cancel : MessageBoxResult.No;
      case MessageBoxButton.YesNo:
        return !dialogResultInternal.GetValueOrDefault() ? MessageBoxResult.No : MessageBoxResult.Yes;
      default:
        return MessageBoxResult.None;
    }
  }
}

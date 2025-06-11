// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.ExceptionStringTable
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.ComponentModel;
using System.Globalization;
using System.Resources;

#nullable disable
namespace HandyControl.Interactivity;

internal class ExceptionStringTable
{
  private static ResourceManager ResourceMan;

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      return ExceptionStringTable.ResourceMan ?? (ExceptionStringTable.ResourceMan = new ResourceManager(nameof (ExceptionStringTable), typeof (ExceptionStringTable).Assembly));
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture { get; set; }

  internal static string CannotHostBehaviorCollectionMultipleTimesExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (CannotHostBehaviorCollectionMultipleTimesExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string CannotHostBehaviorMultipleTimesExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (CannotHostBehaviorMultipleTimesExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string CannotHostTriggerActionMultipleTimesExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (CannotHostTriggerActionMultipleTimesExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string CannotHostTriggerCollectionMultipleTimesExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (CannotHostTriggerCollectionMultipleTimesExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string CannotHostTriggerMultipleTimesExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (CannotHostTriggerMultipleTimesExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string CommandDoesNotExistOnBehaviorWarningMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (CommandDoesNotExistOnBehaviorWarningMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string DuplicateItemInCollectionExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (DuplicateItemInCollectionExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string EventTriggerBaseInvalidEventExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (EventTriggerBaseInvalidEventExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string EventTriggerCannotFindEventNameExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (EventTriggerCannotFindEventNameExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string RetargetedTypeConstraintViolatedExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (RetargetedTypeConstraintViolatedExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string TypeConstraintViolatedExceptionMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (TypeConstraintViolatedExceptionMessage), ExceptionStringTable.Culture);
    }
  }

  internal static string UnableToResolveTargetNameWarningMessage
  {
    get
    {
      return ExceptionStringTable.ResourceManager.GetString(nameof (UnableToResolveTargetNameWarningMessage), ExceptionStringTable.Culture);
    }
  }
}

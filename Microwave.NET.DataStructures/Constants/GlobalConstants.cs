using System;
using System.Collections.Generic;
using System.Text;

namespace Microwave.NET.DataStructures.Constants;

public static class GlobalConstants
{
    public const int MaxPowerLevel = 10;
    public const int MinPowerLevel = 1;

    public const int MaxTimerInSeconds = 120; // 2 minutos
    public const int MinTimerInSeconds = 1;

    public const int QuickStartTimerDefault = 30;
    public const int QuickStartPowerDefault = 10;
}

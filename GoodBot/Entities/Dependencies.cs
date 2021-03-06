﻿using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GoodBot.Entities
{
    internal class Dependencies
    {
		internal Config Config { get; set; }
		internal InteractivityModule Interactivity { get; set; }
        internal StartTimes StartTimes { get; set; }
        internal CancellationTokenSource Cts { get; set; }
    }
}

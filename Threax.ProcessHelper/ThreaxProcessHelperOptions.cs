﻿using System;

namespace Threax.ProcessHelper;

public class ThreaxProcessHelperOptions
{
    /// <summary>
    /// Further decorate the process runner. Can be null to have no modifications.
    /// </summary>
    public Func<IServiceProvider, IProcessRunner, IProcessRunner>? DecorateProcessRunner { get; set; }
}

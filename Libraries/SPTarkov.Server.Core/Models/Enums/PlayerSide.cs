﻿using SPTarkov.Server.Core.Utils.Json.Converters;

namespace SPTarkov.Server.Core.Models.Enums;

[EftListEnumConverter]
public enum PlayerSide
{
    Usec = 1,
    Bear = 2,
    Savage = 4
}

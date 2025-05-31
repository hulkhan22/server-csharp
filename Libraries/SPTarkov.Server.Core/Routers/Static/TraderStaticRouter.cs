﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Utils;

namespace SPTarkov.Server.Core.Routers.Static;

[Injectable]
public class TraderStaticRouter : StaticRouter
{
    public TraderStaticRouter(
        JsonUtil jsonUtil,
        TraderCallbacks traderCallbacks
    ) : base(
        jsonUtil,
        [
            new RouteAction(
                "/client/trading/api/traderSettings",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await traderCallbacks.GetTraderSettings(url, info as EmptyRequestData, sessionID)
            ),
            new RouteAction(
                "/singleplayer/moddedTraders",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await traderCallbacks.GetModdedTraderData(url, info as EmptyRequestData, sessionID)
            )
        ]
    )
    {
    }
}

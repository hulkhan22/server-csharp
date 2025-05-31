﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Utils;

namespace SPTarkov.Server.Core.Routers.Dynamic;

[Injectable]
public class TraderDynamicRouter : DynamicRouter
{
    public TraderDynamicRouter(
        JsonUtil jsonUtil,
        TraderCallbacks traderCallbacks
    ) : base(
        jsonUtil,
        [
            new RouteAction(
                "/client/trading/api/getTrader/",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await traderCallbacks.GetTrader(url, info as EmptyRequestData, sessionID)
            ),
            new RouteAction(
                "/client/trading/api/getTraderAssort/",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await traderCallbacks.GetAssort(url, info as EmptyRequestData, sessionID)
            )
        ]
    )
    {
    }
}

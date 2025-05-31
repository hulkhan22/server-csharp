﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Request;
using SPTarkov.Server.Core.Utils;

namespace SPTarkov.Server.Core.Routers.Static;

[Injectable]
public class NotifierStaticRouter : StaticRouter
{
    public NotifierStaticRouter(
        JsonUtil jsonUtil,
        NotifierCallbacks notifierCallbacks
    ) : base(
        jsonUtil,
        [
            new RouteAction(
                "/client/notifier/channel/create",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await notifierCallbacks.CreateNotifierChannel(url, info as EmptyRequestData, sessionID)
            ),
            new RouteAction(
                "/client/game/profile/select",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await notifierCallbacks.SelectProfile(url, info as UIDRequestData, sessionID),
                typeof(UIDRequestData)
            )
        ]
    )
    {
    }
}

﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Insurance;
using SPTarkov.Server.Core.Utils;

namespace SPTarkov.Server.Core.Routers.Static;

[Injectable]
public class InsuranceStaticRouter : StaticRouter
{
    public InsuranceStaticRouter(
        JsonUtil jsonUtil,
        InsuranceCallbacks insuranceCallbacks
    ) : base(
        jsonUtil,
        [
            new RouteAction(
                "/client/insurance/items/list/cost",
                async (
                    url,
                    info,
                    sessionID,
                    output
                ) => await insuranceCallbacks.GetInsuranceCost(url, info as GetInsuranceCostRequestData, sessionID),
                typeof(GetInsuranceCostRequestData)
            )
        ]
    )
    {
    }
}

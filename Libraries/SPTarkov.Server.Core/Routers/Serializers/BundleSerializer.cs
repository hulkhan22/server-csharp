﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Loaders;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;

namespace SPTarkov.Server.Core.Routers.Serializers;

[Injectable]
public class BundleSerializer(
    ISptLogger<BundleSerializer> logger,
    BundleLoader bundleLoader,
    HttpFileUtil httpFileUtil
) : ISerializer
{
    public async Task Serialize(string sessionID, HttpRequest req, HttpResponse resp, object? body)
    {
        var key = req.Path.Value.Split("/bundle/")[1];
        var bundle = bundleLoader.GetBundle(key);
        if (bundle == null)
        {
            return;
        }

        logger.Info($"[BUNDLE]: {req.Path.Value}");
        if (bundle.ModPath == null)
        {
            logger.Error($"Mod: {key} lacks a modPath property, skipped loading");
            return;
        }

        var bundlePath = Path.Join(bundle.ModPath, "/bundles/", bundle.FileName);
        await httpFileUtil.SendFile(resp, bundlePath);
    }

    public bool CanHandle(string route)
    {
        return route == "BUNDLE";
    }
}

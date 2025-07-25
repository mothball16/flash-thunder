﻿using fennecs;
using FlashThunder.GameLogic.CameraControl.Components;
using FlashThunder.GameLogic.CameraControl.Requests;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.CameraControl.Handlers;

internal sealed class CameraChangeHandler : IDisposable
{
    private readonly Stream<WorldCamera> _activeCameraQuery;
    private readonly List<IDisposable> _subscriptions;
    public CameraChangeHandler(World world)
    {
        _subscriptions = [
            world.Subscribe<CamTranslationRequest>(OnCamTranslation)
        ];
        _activeCameraQuery = world.Query<WorldCamera>().Stream();
    }

    public void OnCamTranslation(CamTranslationRequest msg)
    {
        _activeCameraQuery.For((ref WorldCamera worldCamera) =>
        {
            switch (msg.CamOp)
            {
                case CamOperationType.Smooth:
                    switch (msg.OffsetType)
                    {
                        case CamOffsetType.Absolute:
                            worldCamera.Target = new Vector2(msg.X, msg.Y);
                            break;
                        case CamOffsetType.Relative:
                            worldCamera.Target += new Vector2(msg.X, msg.Y);
                            break;
                        case CamOffsetType.RelativeSingleFrame:
                            worldCamera.Jitter += new Vector2(msg.X, msg.Y);
                            break;
                    }
                    break;
                case CamOperationType.Immediate:
                    switch (msg.OffsetType)
                    {
                        case CamOffsetType.Absolute:
                            worldCamera.Position = new Vector2(msg.X, msg.Y);
                            worldCamera.Target = new Vector2(msg.X, msg.Y);
                            break;
                        case CamOffsetType.Relative:
                            worldCamera.Position += new Vector2(msg.X, msg.Y);
                            worldCamera.Target += new Vector2(msg.X, msg.Y);
                            break;
                        case CamOffsetType.RelativeSingleFrame:
                            Logger.Warn("RelativeSingleFrame camera operations not supported in " +
                                "immediate mode. Using smooth camop logic.");
                            worldCamera.Jitter += new Vector2(msg.X, msg.Y);
                            break;
                    }
                    break;
            }
        });
    }

    public void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
    }
}

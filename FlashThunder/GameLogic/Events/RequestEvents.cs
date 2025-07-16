using fennecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Events;

internal readonly record struct NextTurnRequestEvent;

internal readonly record struct SpawnPrefabRequestEvent(string Name, Action<Entity> Callback = null);
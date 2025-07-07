using FlashThunder.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Defs;

internal readonly record struct KeybindDef(
    InputType InputType,
    string Identifier,
    GameAction Action
    );

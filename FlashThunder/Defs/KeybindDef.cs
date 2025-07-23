using FlashThunder.Enums;

namespace FlashThunder.Defs;

internal readonly record struct KeybindDef(
    InputType InputType,
    string Identifier,
    GameAction Action
    );

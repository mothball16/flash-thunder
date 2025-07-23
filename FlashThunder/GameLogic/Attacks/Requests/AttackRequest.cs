using FlashThunder.GameLogic.Attacks.Data;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Attacks.Requests;
internal struct AttackRequest
{
    public List<AttackData> Attacks { get; set; }
}

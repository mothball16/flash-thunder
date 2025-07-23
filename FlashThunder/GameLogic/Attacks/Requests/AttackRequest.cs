using FlashThunder.GameLogic.AttackLogic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Attacks.Requests;
internal struct AttackRequest
{
    public List<AttackData> Attacks { get; set; }
}

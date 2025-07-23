using fennecs;
using FlashThunder.Factories;
using FlashThunder.GameLogic.Team;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.Utilities;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.TeamLogic.Services
{
    internal sealed class TeamService
    {
        private readonly World _world;
        private readonly EntityFactory _factory;
        private readonly Dictionary<string, Entity> teamsByName;
        private readonly Dictionary<string, List<Entity>> teamsByFaction;

        public TeamService(World world, EntityFactory factory)
        {
            _world = world;
            _factory = factory;
            teamsByName = [];
            teamsByFaction = [];
        }

        public Entity CreateTeam(string teamName, string factionName, bool canControl)
        {
            if (teamsByName.ContainsKey(teamName))
            {
                Logger.Error($"Team with name {teamName} already exists! Aborting action.");
                return default;
            }

            // create the entity
            var team = _factory.CreateTeamBundle(new TeamTag(teamName), new Faction(factionName));
            if (canControl)
                team.Add<IsPlayerControllable>();

            // assign to lookups
            teamsByName[teamName] = team;
            if (!teamsByFaction.TryGetValue(factionName, out var factionTeams))
            {
                factionTeams = [];
                teamsByFaction[factionName] = factionTeams;
            }
            factionTeams.Add(team);

            return team;
        }

        public void RemoveTeam(string teamName)
        {
            if(!teamsByName.TryGetValue(teamName, out var teamEntity))
            {
                Logger.Error($"Team with name {teamName} does not exist! Aborting action.");
            }

            var teamFaction = teamEntity.Ref<Faction>().Name;

            // remove from lookups
            teamsByName.Remove(teamName);

            var teamsOfFaction = teamsByFaction[teamFaction];
            teamsOfFaction.Remove(teamEntity);
            if(teamsOfFaction.Count == 0)
                teamsByFaction.Remove(teamFaction);

            // fix entities to unassign
            var query = _world.Query<FromTeam>()
                .Has<FromTeam>(teamEntity).Stream();

            query.For((ref FromTeam fromTeam) =>
            {
                Logger.Error("HEY!!! There should be some behavior for this. I haven't done this yet");
                fromTeam = default; // unassign the team
            });

            // physically remove the entity
            teamEntity.Despawn();
        }

        public bool TryGetTeam(string name, out Entity team)
            => teamsByName.TryGetValue(name, out team);

        /// <summary>
        /// Create a relation between the entity and the respective team entity by name.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="name"></param>
        public void AssignTeam(Entity e, string name)
        {
            if(!TryGetTeam(name, out var team))
            {
                Logger.Error($"Cannot assign team {name}. Team does not exist.");
                return;
            }
            e.Add<FromTeam>(team);
        }
    }
}

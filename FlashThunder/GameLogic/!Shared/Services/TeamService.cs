using fennecs;
using FlashThunder.Factories;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.Utilities;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Team.Services;

internal sealed class TeamService
{
    private readonly World _world;
    private readonly EntityFactory _factory;
    private readonly Dictionary<string, Entity> _teamsByName;
    private readonly Dictionary<string, List<Entity>> _teamsByFaction;
    private readonly Stream<TeamTag> _entitiesOnATeam;

    public TeamService(World world, EntityFactory factory)
    {
        _world = world;
        _factory = factory;
        _entitiesOnATeam = world.Query<TeamTag>().Stream();
        _teamsByName = [];
        _teamsByFaction = [];
    }

    public Entity CreateTeam(string teamName, string factionName, bool canControl)
    {
        if (_teamsByName.ContainsKey(teamName))
        {
            Logger.Error($"Team with name {teamName} already exists! Aborting action.");
            return default;
        }

        // create the entity
        var team = _factory.CreateTeamBundle(new TeamTag(teamName), new Faction(factionName));
        if (canControl)
            team.Add<IsPlayerControllable>();

        // assign to lookups
        _teamsByName[teamName] = team;
        if (!_teamsByFaction.TryGetValue(factionName, out var factionTeams))
        {
            factionTeams = [];
            _teamsByFaction[factionName] = factionTeams;
        }
        factionTeams.Add(team);

        return team;
    }

    public void RemoveTeam(string teamName)
    {
        if(!_teamsByName.TryGetValue(teamName, out var teamEntity))
        {
            Logger.Error($"Team with name {teamName} does not exist! Aborting action.");
        }

        var teamFaction = teamEntity.Ref<Faction>().Name;

        // remove from lookups
        _teamsByName.Remove(teamName);

        var teamsOfFaction = _teamsByFaction[teamFaction];
        teamsOfFaction.Remove(teamEntity);
        if(teamsOfFaction.Count == 0)
            _teamsByFaction.Remove(teamFaction);

        // fix entities to unassign

        _entitiesOnATeam.For((ref TeamTag teamTag) =>
        {
            if(teamTag.Team == teamName)
            {
                teamTag.Team = "TBA";
            }
        });

        // physically remove the entity
        teamEntity.Despawn();
    }

    public bool TryGetTeam(string name, out Entity team)
        => _teamsByName.TryGetValue(name, out team);

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
        e.Add<TeamTag>(team);
        e.Add(new TeamTag(name));
    }
}

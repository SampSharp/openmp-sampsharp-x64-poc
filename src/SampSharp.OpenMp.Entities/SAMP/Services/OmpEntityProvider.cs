﻿using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class OmpEntityProvider(SampSharpEnvironment omp, IEntityManager entityManager) : IOmpEntityProvider
{
    private readonly IVehiclesComponent _vehicles = omp.Components.QueryComponent<IVehiclesComponent>();
    private readonly IObjectsComponent _objects = omp.Components.QueryComponent<IObjectsComponent>();
    private readonly IGangZonesComponent _gangZones = omp.Components.QueryComponent<IGangZonesComponent>();
    private readonly IActorsComponent _actors = omp.Components.QueryComponent<IActorsComponent>();
    private readonly IPickupsComponent _pickups = omp.Components.QueryComponent<IPickupsComponent>();
    private readonly ITextDrawsComponent _textDraws = omp.Components.QueryComponent<ITextDrawsComponent>();
    private readonly IMenusComponent _menus = omp.Components.QueryComponent<IMenusComponent>();
    private readonly ITextLabelsComponent _textLabels = omp.Components.QueryComponent<ITextLabelsComponent>();
    private readonly IPlayerPool _players = omp.Core.GetPlayers();

    public EntityId GetEntity(IActor actor)
    {
        return GetComponent(actor)?.Entity ?? default;
    }

    public EntityId GetEntity(IGangZone gangZone)
    {
        return GetComponent(gangZone)?.Entity ?? default;
    }

    public EntityId GetEntity(IMenu menu)
    {
        return GetComponent(menu)?.Entity ?? default;
    }

    public EntityId GetEntity(IObject @object)
    {
        return GetComponent(@object)?.Entity ?? default;
    }

    public EntityId GetEntity(IPickup pickup)
    {
        return GetComponent(pickup)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayer player)
    {
        return GetComponent(player)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayerObject playerObject, IPlayer player = default)
    {
        return GetComponent(playerObject, player)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayerTextDraw playerTextDraw, IPlayer player = default)
    {
        return GetComponent(playerTextDraw, player)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayerTextLabel playerTextLabel, IPlayer player = default)
    {
        return GetComponent(playerTextLabel, player)?.Entity ?? default;
    }

    public EntityId GetEntity(ITextDraw textDraw)
    {
        return GetComponent(textDraw)?.Entity ?? default;
    }

    public EntityId GetEntity(ITextLabel textLabel)
    {
        return GetComponent(textLabel)?.Entity ?? default;
    }

    public EntityId GetEntity(IVehicle vehicle)
    {
        return GetComponent(vehicle)?.Entity ?? default;
    }

    public Actor? GetComponent(IActor actor)
    {
        if (actor == null)
        {
            return null;
        }
        var ext = actor.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {

            var component = entityManager.AddComponent<Actor>(EntityId.NewEntityId(), _actors, actor);
            ext = new ComponentExtension(component);
            actor.AddExtension(ext);

            return component;
        }
        return (Actor)ext.Component;
    }

    public GangZone? GetComponent(IGangZone gangZone)
    {
        if (gangZone == null)
        {
            return null;
        }
        var ext = gangZone.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {

            var component = entityManager.AddComponent<GangZone>(EntityId.NewEntityId(), _gangZones, gangZone);
            ext = new ComponentExtension(component);
            gangZone.AddExtension(ext);

            return component;
        }
        return (GangZone)ext.Component;
    }

    public Menu? GetComponent(IMenu menu)
    {
        if (menu == null)
        {
            return null;
        }
        var ext = menu.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            // don't know the title of the menu (which cannot be retrieved through open.mp api) - cannot create a component for the foreign entity.
            return null;
        }
        return (Menu)ext.Component;
    }

    public GlobalObject? GetComponent(IObject @object)
    {
        if (@object == null)
        {
            return null;
        }
        var ext = @object.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {

            var component = entityManager.AddComponent<GlobalObject>(EntityId.NewEntityId(), _objects, @object);
            ext = new ComponentExtension(component);
            @object.AddExtension(ext);

            return component;
        }
        return (GlobalObject)ext.Component;
    }

    public Pickup? GetComponent(IPickup pickup)
    {
        if (pickup == null)
        {
            return null;
        }
        var ext = pickup.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = entityManager.AddComponent<Pickup>(EntityId.NewEntityId(), _pickups, pickup);
            ext = new ComponentExtension(component);
            pickup.AddExtension(ext);

            return component;
        }
        return (Pickup)ext.Component;
    }

    public Player? GetComponent(IPlayer player)
    {
        if (player == null)
        {
            return null;
        }
        var ext = player.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = entityManager.AddComponent<Player>(EntityId.NewEntityId(), this, player);
            ext = new ComponentExtension(component);
            player.AddExtension(ext);
            return component;
        }
        return (Player)ext.Component;
    }

    public PlayerObject? GetComponent(IPlayerObject playerObject, IPlayer player = default)
    {
        if (playerObject == null)
        {
            return null;
        }
        var ext = playerObject.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            if (player == null)
            {
                // don't know for which player this object is created - cannot create a component for the foreign entity.
                return null;
            }

            var data = player.QueryExtension<IPlayerObjectData>();
            
            var component = entityManager.AddComponent<PlayerObject>(EntityId.NewEntityId(), data, playerObject);
            ext = new ComponentExtension(component);
            player.AddExtension(ext);
            return component;
        }
        return (PlayerObject)ext.Component;
    }

    public PlayerTextDraw? GetComponent(IPlayerTextDraw playerTextDraw, IPlayer player = default)
    {
        if (playerTextDraw == null)
        {
            return null;
        }
        var ext = playerTextDraw.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            if (player == null)
            {
                // don't know for which player this text draw is created - cannot create a component for the foreign entity.
                return null;
            }

            var data = player.QueryExtension<IPlayerTextDrawData>();
            
            var component = entityManager.AddComponent<PlayerTextDraw>(EntityId.NewEntityId(), data, playerTextDraw);
            ext = new ComponentExtension(component);
            player.AddExtension(ext);
            return component;
        }

        return (PlayerTextDraw)ext.Component;
    }

    public PlayerTextLabel? GetComponent(IPlayerTextLabel playerTextLabel, IPlayer player = default)
    {
        if (playerTextLabel == null)
        {
            return null;
        }

        var ext = playerTextLabel.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            if (player == null)
            {
                // don't know for which player this text label is created - cannot create a component for the foreign entity.
                return null;
            }

            var data = player.QueryExtension<IPlayerTextLabelData>();
            
            var component = entityManager.AddComponent<PlayerTextLabel>(EntityId.NewEntityId(), this, data, playerTextLabel);
            ext = new ComponentExtension(component);
            player.AddExtension(ext);
            return component;
        }

        return (PlayerTextLabel)ext.Component;
    }

    public TextDraw? GetComponent(ITextDraw textDraw)
    {
        if (textDraw == null)
        {
            return null;
        }
        var ext = textDraw.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = entityManager.AddComponent<TextDraw>(EntityId.NewEntityId(), _textDraws, textDraw);
            ext = new ComponentExtension(component);
            textDraw.AddExtension(ext);

            return component;
        }

        return (TextDraw)ext.Component;
    }

    public TextLabel? GetComponent(ITextLabel textLabel)
    {
        if (textLabel == null)
        {
            return null;
        }
        var ext = textLabel.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = entityManager.AddComponent<TextLabel>(EntityId.NewEntityId(), this, _textLabels, textLabel);
            ext = new ComponentExtension(component);
            textLabel.AddExtension(ext);

            return component;
        }

        return (TextLabel)ext.Component;
    }

    public Vehicle? GetComponent(IVehicle vehicle)
    {
        if (vehicle == null)
        {
            return null;
        }

        var ext = vehicle.TryGetExtension<ComponentExtension>();

        if (ext == null)
        {
            var component = entityManager.AddComponent<Vehicle>(EntityId.NewEntityId(), _vehicles, vehicle);
            ext = new ComponentExtension(component);
            vehicle.AddExtension(ext);

            return component;
        }

        return (Vehicle)ext.Component;
    }

    public Actor? GetActor(int id)
    {
        return GetComponent(_actors.AsPool().Get(id));
    }

    public GangZone? GetGangZone(int id)
    {
        return GetComponent(_gangZones.AsPool().Get(id));
    }

    public Pickup? GetPickup(int id)
    {
        return GetComponent(_pickups.AsPool().Get(id));
    }

    public Player? GetPlayer(int id)
    {
        return GetComponent(_players.Get(id));
    }

    public PlayerObject? GetPlayerObject(IPlayer player, int id)
    {
        var data = player.QueryExtension<IPlayerObjectData>();
        return GetComponent(data.Get(id), player);
    }

    public PlayerTextDraw? GetPlayerTextDraw(IPlayer player, int id)
    {
        var data = player.QueryExtension<IPlayerTextDrawData>();
        return GetComponent(data.Get(id), player);
    }

    public PlayerTextLabel? GetPlayerTextLabel(IPlayer player, int id)
    {
        var data = player.QueryExtension<IPlayerTextLabelData>();
        return GetComponent(data.Get(id), player);
    }

    public TextDraw? GetTextDraw(int id)
    {
        return GetComponent(_textDraws.AsPool().Get(id));
    }

    public TextLabel? GetTextLabel(int id)
    {
        return GetComponent(_textLabels.AsPool().Get(id));
    }

    public Vehicle? GetVehicle(int id)
    {
        return GetComponent(_vehicles.AsPool().Get(id));
    }

    public GlobalObject? GetObject(int id)
    {
        return GetComponent(_objects.AsPool().Get(id));
    }

    public Menu? GetMenu(int id)
    {
        return GetComponent(_menus.AsPool().Get(id));
    }
}

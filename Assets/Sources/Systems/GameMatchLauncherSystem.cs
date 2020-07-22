﻿using InsaneOne.EcsRts.Storing;
using InsaneOne.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    public class GameMatchLauncherSystem : IEcsInitSystem
    {
        readonly EcsWorld world;
        readonly GameStartData startData;

        readonly EcsFilter<PlayerComponent> playerFilter = null;

        SpawnPoint[] spawnPoints;

        public void Init()
        {
            spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();

            SetupPlayers();
        }

        void SetupPlayers()
        {
            for (var i = 0; i < startData.StartPlayersCount; i++)
            {
                ref var playerComponent = ref world.NewEntity().Get<PlayerComponent>();

                playerComponent.Id = i;
                playerComponent.Resources = startData.StartMoney;
                playerComponent.Color = startData.PlayerColors[i];
                
                SpawnPlayerUnit(i);
            }
        }

        void SpawnPlayerUnit(int playerId)
        {
            for (var i = 0; i < spawnPoints.Length; i++)
            {
                var point = spawnPoints[i];
                if (point.PlayerId == playerId)
                {
                    ref var spawnEvent = ref world.NewEntity().Get<SpawnUnitEvent>();

                    spawnEvent.Position = point.transform.position;
                    spawnEvent.OwnerPlayerId = point.PlayerId;
                    spawnEvent.UnitToSpawnData = startData.StartUnit;

                    GameObject.Destroy(point);
                }
            }
        }
    }
}
    
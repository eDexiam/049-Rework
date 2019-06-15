using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace PlagueRework
{
	class RoundEventHandler : IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerSetRole, IEventHandlerPlayerDie
	{
		private readonly PlagueRework plugin;

		public RoundEventHandler(PlagueRework plugin) => this.plugin = plugin;

        private bool RoundInProgress = false;
        private int zombies = 0;
        public Player plaguecache = null;


        private IEnumerator<float> Heal()
        {
            yield return Timing.WaitForSeconds(2f);
            while(RoundInProgress == true)
            {
                if (plaguecache == null) continue;
                yield return Timing.WaitForSeconds(1f);
                if(plugin.GetConfigInt("049_maximumhealpersecond") > zombies && plugin.GetConfigInt("049_maximumhealpersecond") != 0)
                {
                    if(plaguecache.GetHealth() < plugin.GetConfigInt("049_maximumhptoheal")) {
                        plaguecache.AddHealth(zombies);
                    }
                } else if (plugin.GetConfigInt("049_maximumhealpersecond") < zombies && plugin.GetConfigInt("049_maximumhealpersecond") != 0) {
                    plaguecache.AddHealth(plugin.GetConfigInt("049_maximumhealpersecond"));
                }
                foreach(Player player in plugin.Server.GetPlayers())
                {
                    if(Vector.Distance(player.GetPosition(), plaguecache.GetPosition()) <= plugin.GetConfigInt("049_healradius"))
                    {
                        if(player.TeamRole.Role == Role.SCP_173 && player.GetHealth() < player.TeamRole.MaxHP)
                        {
                            player.AddHealth(zombies);
                        }
                        if (player.TeamRole.Role == Role.SCP_106 && player.GetHealth() < player.TeamRole.MaxHP)
                        {
                            player.AddHealth(zombies);
                        }
                        if (player.TeamRole.Role == Role.SCP_096 && player.GetHealth() < player.TeamRole.MaxHP)
                        {
                            player.AddHealth(zombies);
                        }
                        if (player.TeamRole.Role == Role.SCP_939_53 && player.GetHealth() < player.TeamRole.MaxHP || player.TeamRole.Role == Role.SCP_939_89 && player.GetHealth() < player.TeamRole.MaxHP)
                        {
                            player.AddHealth(zombies);
                        }
                        if (player.TeamRole.Role == Role.SCP_049_2 && player.GetHealth() < player.TeamRole.MaxHP)
                        {
                            player.AddHealth(zombies);
                        }
                        if (player.TeamRole.Role == Role.TUTORIAL && player.GetHealth() < player.TeamRole.MaxHP)
                        {
                            player.AddHealth(zombies);
                        }
                    }
                }
            }
        }

        private IEnumerator<float> ZombieTimer()
        {
            yield return Timing.WaitForSeconds(2f);
            while(RoundInProgress == true)
            {
                yield return Timing.WaitForSeconds(5f);
                CheckZombies();
            }
        }

        private IEnumerator<float> Plaguecache()
        {
            yield return Timing.WaitForSeconds(5f);
            foreach (Player player in plugin.Server.GetPlayers())
            {
                if(player.TeamRole.Role == Role.SCP_049)
                {
                    plaguecache = player;
                }
            }
            Timing.RunCoroutine(Heal());
            Timing.RunCoroutine(ZombieTimer());
        }

		public void OnRoundEnd(RoundEndEvent ev)
		{
            RoundInProgress = false;
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
            RoundInProgress = true;
            zombies = 0;
            plaguecache = null;
            Timing.RunCoroutine(Plaguecache());
        }

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            CheckZombies();
        }

        private void CheckZombies()
        {
            int zombiecount = 0;
            foreach (Player player in plugin.Server.GetPlayers()) // Updating zombie count
            {
                if (player.TeamRole.Role == Role.SCP_049_2 && player.GetRankName() == "SCP-966")
                {
                    zombiecount += 1;
                }
            }
            zombies = zombiecount;
        }

        private IEnumerator<float> RespawnZombie(PlayerDeathEvent ev)
        {
            yield return Timing.WaitForSeconds(0.1f);
            ev.Player.ChangeRole(Role.SCP_049_2);
            ev.Player.Teleport(ev.Killer.GetPosition());
        }

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if(ev.DamageTypeVar == DamageType.SCP_049)
            {
                Timing.RunCoroutine(RespawnZombie(ev));
                ev.SpawnRagdoll = false;
            }
            if(ev.Player.TeamRole.Role == Role.SCP_049)
            {
                plaguecache = null;
            } 
        }
    }
}

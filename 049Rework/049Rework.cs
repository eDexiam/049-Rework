using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.Lang;
using Smod2.Piping;

namespace PlagueRework
{
	[PluginDetails(
		author = "lucasboss45",
		name = "049 Rework",
		description = "A rework for 049",
		id = "lucas.plaguerework",
		version = "1.0",
		SmodMajor = 3,
		SmodMinor = 3,
		SmodRevision = 1
		)]
	public class PlagueRework : Plugin
	{
		public override void OnDisable()
		{
			this.Info("049 Rework disabled");
		}

		public override void OnEnable()
		{
			this.Info("049 Rework enabled");
		}
		
		public override void Register()
		{
			
			this.AddEventHandlers(new RoundEventHandler(this));
            this.AddConfig(new ConfigSetting("049_maximumhptoheal", 1000, true, "The limit of HP 049 can be healed to."));
            this.AddConfig(new ConfigSetting("049_maximumhealpersecond", 5, true, "Maximum HP per second 049 can be healed"));
            this.AddConfig(new ConfigSetting("049_healradius", 5, true, "Radius where SCP-049 heals SCPs."));
            this.AddConfig(new ConfigSetting("049_hptohealotherscps", 1, true, "The amount of HP to give to other SCPs when near SCP-049"));
		}
	}
}

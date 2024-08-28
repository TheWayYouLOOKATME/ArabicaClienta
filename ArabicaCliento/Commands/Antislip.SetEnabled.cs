using ArabicaCliento.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace ArabicaCliento.Commands;

[AnyCommand]
public class AntislipSetEnabled: IConsoleCommand
{
    public string Command => "arabica.anti_slip.set_enabled";
    public string Description => "Enable or disable antislip";
    public string Help => "arabica.anti_slip.set_enabled <bool>";
    
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError("Invalid argument count.");
            return;
        }

        if (!bool.TryParse(args[0], out var enabled))
        {
            shell.WriteError($"Can't parse {args[0]} as bool");
            return;
        }

        var sys = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ArabicaAntiSlipSystem>();
        sys.SetEnabled(enabled);
    }
}
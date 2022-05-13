// pypupy - 2022

namespace pypkg.Commands
{
    public class Command
    {
        public string Name = "Command";
        public async static Task<CommandStatus> Execute() { return CommandStatus.Success; }
    }
}

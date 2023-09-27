namespace Projector.Core
{
    public class CommandResult
    {
        /* PROPERTIES */
        public bool IsSuccessful => Errors == null || Errors.Count == 0;
        public List<string> Errors { get; set; }
        public object Result { get; set; }

        /* METHODS */
        public static CommandResult Error(params string[] errors)
        {
            return new CommandResult
            {
                Errors = new List<string>(errors)
            };
        }

        public static CommandResult Success(object data = null)
        {
            return new CommandResult {
                Result = data
            };
        }
    }
}

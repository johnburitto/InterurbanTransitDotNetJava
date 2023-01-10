namespace Security.Configurations
{
    public class Authorization
    {
        public enum Role
        {
            User, 
            Operator,
            Administrator
        }

        public const Role DEFAULT_ROLE = Role.User;
    }
}

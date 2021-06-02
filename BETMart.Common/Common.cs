namespace BETMart.Common
{
    public class Common
    {
        public class ContentType
        {
            public const string Json = "application/json";
        }
        public class APIEndpoint
        {
            public const string Order = "/api/Order";
            public const string Product = "/api/Product";
            public const string Login = "/Token";
            public const string Register = "/Register";
        }

        public enum Roles
        {
            SuperAdmin,
            Basic
        }
    }
}

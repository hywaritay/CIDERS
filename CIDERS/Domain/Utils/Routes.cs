namespace CIDERS.Domain.Utils;
public static class Routes
{
    public static class ApiRoute
    {
        private const string PrefixApi = "api";

        public static class Home
        {
            public const string Base = "/api";
            public const string Homepage = "";
            public const string Tobban = "tobban";
            public const string Toregular = "toregular";
        }

        public static class Security
        {
            private const string PrefixSecurity = PrefixApi + "";

            public static class Auth
            {
                public const string Base = PrefixSecurity + "/auth";
                public const string Login = "login";
                public const string Register = "register";
            }

            public static class Ldap
            {
                public const string Base = PrefixSecurity + "/ldap";
                public const string Login = "login";
            }
        }

        public static class Secure
        {
            private const string PrefixSecure = PrefixApi + "/secure";

            public static class Admin
            {
                private const string PrefixSecureAdmin = PrefixSecure + "/admin";

                public static class Branch
                {
                    public const string Base = PrefixSecureAdmin + "/branch";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string Delete = "delete";
                    public const string State = "state";
                }

                public static class User
                {
                    public const string Base = PrefixSecureAdmin + "/user";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string UpdateSecret = "updatesecret";
                    public const string Delete = "delete";
                    public const string State = "state";
                }

                public static class Rank
                {
                    public const string Base = PrefixSecureAdmin + "/rank";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string Delete = "delete";
                    public const string State = "state";
                }
                public static class Dept
                {
                    public const string Base = PrefixSecureAdmin + "/dept";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string Delete = "delete";
                    public const string State = "state";
                }
                public static class Location
                {
                    public const string Base = PrefixSecureAdmin + "/location";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string Delete = "delete";
                    public const string State = "state";
                }
                public static class Employee
                {
                    public const string Base = PrefixSecureAdmin + "/emp";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string Delete = "delete";
                    public const string State = "state";
                }


                public static class Permission
                {
                    public const string Base = PrefixSecureAdmin + "/permission";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string Create = "create";
                    public const string Update = "update";
                    public const string Delete = "delete";
                    public const string State = "state";
                }
            }

            public static class App
            {
                private const string PrefixSecureApp = PrefixSecure + "/app";

                public static class Account
                {
                    public const string Base = PrefixSecureApp + "/account";
                    public const string Balance = "balance";
                    public const string List = "list";
                    public const string Detail = "detail";
                    public const string TellerTillAccount = "tellertillaccount";
                }

                public static class Transaction
                {
                    public const string Base = PrefixSecureApp + "/transaction";
                    public const string Find = "find";
                    public const string FindByCode = "findbycode";
                    public const string FindByReference = "findbyreference";
                    public const string Post = "post";
                }

                public static class Branch
                {
                    public const string Base = PrefixSecureApp + "/branch";
                    public const string All = "all";
                    public const string Find = "find";
                    public const string FindByCode = "findbycode";
                }

                public static class Token
                {
                    public const string Base = PrefixSecureApp + "/token";
                    public const string Generate = "generate";
                    public const string Validate = "validate";
                }
            }
        }
    }
}
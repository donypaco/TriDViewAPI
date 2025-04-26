using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Configurations
{
    public class AppSettings
    {
        public JwtSettings Jwt { get; set; }
        public ConnectionStringsSettings ConnectionStrings { get; set; }
        public FeatureManagementSettings FeatureManagement { get; set; }
        public class JwtSettings
        {
            [Required]
            public string Key { get; set; }

            [Required]
            public string Issuer { get; set; }

            [Required]
            public string Audience { get; set; }
        }
        public class ConnectionStringsSettings
        {
            [Required]
            public string DbConnection { get; set; }
        }
        public class FeatureManagementSettings
        {
            public string NewFeature { get; set; }
            public string AnotherFeature { get; set; }
        }
        public string LogoDirectoryPath { get; set; }
        public string ProductsDirectoryPath { get; set; }

    }
}

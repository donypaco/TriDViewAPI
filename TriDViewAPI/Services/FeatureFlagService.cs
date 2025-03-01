using Flagsmith;

namespace TriDViewAPI.Services
{

    public class FeatureFlagService
    {
        private readonly IFlagsmithClient _flagsmithClient;
        
        public FeatureFlagService(IFlagsmithClient flagsmithClient)
        {
            _flagsmithClient = new FlagsmithClient(new FlagsmithConfiguration
            {
                ApiUrl = "https://api.flagsmith.com/api/v1/",
                EnvironmentKey = "your-environment-key"
            });
        }
        public bool IsFeatureEnabled(string featureName)
        {
            return true;
            //var flags = _flagsmithClient.GetEnvironmentFlags();
            //return flags.IsFeatureEnabled(featureName);
        }

    }
}

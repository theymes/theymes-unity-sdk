using System.Collections.Generic;

namespace Theymes
{
    public class TheymesConfig
    {
        public string language { get; set; }
        public string signedMetadataToken { get; set; }
        public TheymesPlayer player { get; set; }
        public List<string> tags { get; set; }
        public Dictionary<string, object> fields { get; set; }
    }
}
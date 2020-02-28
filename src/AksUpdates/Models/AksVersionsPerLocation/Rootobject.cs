using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates.Models.AksVersionsPerLocation
{
    public class Rootobject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public Orchestrator[] orchestrators { get; set; }
    }

    public class Orchestrator
    {
        public string orchestratorType { get; set; }
        public string orchestratorVersion { get; set; }
        public Upgrade[] upgrades { get; set; }
        public bool _default { get; set; }
        public bool isPreview { get; set; }
    }

    public class Upgrade
    {
        public string orchestratorType { get; set; }
        public string orchestratorVersion { get; set; }
        public bool isPreview { get; set; }
    }
}



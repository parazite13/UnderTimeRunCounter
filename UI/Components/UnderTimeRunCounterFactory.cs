using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class UnderTimeRunCounterFactory : IComponentFactory
    {
        public string ComponentName => "Under Time Run Counter";

        public string Description => "Displays the amount of runs that are below a given time.";

        public ComponentCategory Category => ComponentCategory.Information;

        public IComponent Create(LiveSplitState state) => new UnderTimeRunCounterComponent(state);

        public string UpdateName => ComponentName;

        public string XMLURL => "";

        public string UpdateURL => "";

        public Version Version => Version.Parse("1.0.0");
    }
}

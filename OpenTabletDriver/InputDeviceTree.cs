using System.Collections.Generic;
using System.Linq;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace OpenTabletDriver
{
    public class InputDeviceTree
    {
        public InputDeviceTree(TabletConfiguration configuration, IList<InputDevice> inputDevices)
        {
            Properties = configuration;
            InputDevices = inputDevices;
        }

        IList<InputDevice> inputDevices;

        public TabletConfiguration Properties { protected set; get; }
        public IList<InputDevice> InputDevices
        {
            protected set
            {
                this.inputDevices = value;
                foreach (var dev in InputDevices)
                    dev.Report += HandleReport;
            }
            get => this.inputDevices;
        }

        /// <summary>
        /// The active output mode at the end of the data pipeline for all data to be processed.
        /// </summary>
        public IOutputMode OutputMode { set; get; }

        public TabletReference CreateReference() => new TabletReference(Properties, InputDevices.Select(c => c.Identifier));

        private void HandleReport(object sender, IDeviceReport report)
        {
            OutputMode?.Read(report);
        }

        public static implicit operator TabletReference(InputDeviceTree deviceGroup) => deviceGroup.CreateReference();
    }
}
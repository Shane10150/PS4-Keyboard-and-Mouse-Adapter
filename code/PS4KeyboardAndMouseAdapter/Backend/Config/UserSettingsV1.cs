﻿using Newtonsoft.Json;
using Pizza.KeyboardAndMouseAdapter.Backend.Mappings;
using Serilog;
using SFML.Window;
using System.Collections.Generic;

namespace Pizza.KeyboardAndMouseAdapter.Backend.Config
{
    public class UserSettingsV1
    {
        ////////////////////////////////////////////////////////////////////////////
        /// Static props
        ////////////////////////////////////////////////////////////////////////////
        private static readonly ILogger StaticLogger = Log.ForContext(typeof(UserSettingsV1));

        ////////////////////////////////////////////////////////////////////////////
        /// Instance props
        ////////////////////////////////////////////////////////////////////////////
        public Dictionary<VirtualKey, Keyboard.Key> Mappings { get; set; } = new Dictionary<VirtualKey, Keyboard.Key>();

        public int AnalogStickLowerRange { get; set; } = 40;
        public int AnalogStickUpperRange { get; set; } = 95;

        public bool MouseControlsL3 { get; set; } = false;
        public bool MouseControlsR3 { get; set; } = false;

        public double MouseDistanceLowerRange { get; set; } = 5;
        public double MouseDistanceUpperRange { get; set; } = VideoMode.DesktopMode.Width / 20f;
        public double MouseMaxDistance { get; set; } = VideoMode.DesktopMode.Width / 2f;

        public int MousePollingRate { get; set; }

        public double MouseXAxisSensitivityMax { get; set; } = 100;
        public double MouseXAxisSensitivityModifier { get; set; } = 2;

        public double MouseYAxisSensitivityMax { get; set; } = 100;
        public double MouseYAxisSensitivityModifier { get; set; } = 1;

        public double XYRatio { get; set; } = 1;

        ////////////////////////////////////////////////////////////////////////////
        /// Static Methods
        ////////////////////////////////////////////////////////////////////////////

        private static void AddManualMouseMapping(UserSettingsV2 newSettings, VirtualKey vk, MouseButton mouseButton)
        {
            if (!newSettings.MappingsContainsKey(vk))
            {
                newSettings.Mappings[vk] = new PhysicalKeyGroup();
            }

            PhysicalKey pk = new PhysicalKey();
            pk.MouseValue = mouseButton;

            newSettings.Mappings[vk].PhysicalKeys.Add(pk);
        }

        public static PhysicalKeyGroup GetPhysicalKeyGroup(PhysicalKey pk)
        {
            PhysicalKeyGroup pkg = new PhysicalKeyGroup();
            pkg.PhysicalKeys.Add(pk);
            return pkg;
        }

        public static UserSettingsV2 ImportValues(string json)
        {
            StaticLogger.Information("UserSettings_1_0_11.ImportValues()");

            UserSettingsV1 legacySettings = JsonConvert.DeserializeObject<UserSettingsV1>(json);
            UserSettingsV2 newSettings = new UserSettingsV2();

            newSettings.AnalogStickLowerRange = legacySettings.AnalogStickLowerRange;
            newSettings.AnalogStickUpperRange = legacySettings.AnalogStickUpperRange;

            newSettings.MouseControlsL3 = legacySettings.MouseControlsL3;
            newSettings.MouseControlsR3 = legacySettings.MouseControlsR3;

            newSettings.MouseDistanceLowerRange = legacySettings.MouseDistanceLowerRange;
            newSettings.MouseDistanceUpperRange = legacySettings.MouseDistanceUpperRange;
            newSettings.MouseMaxDistance = legacySettings.MouseMaxDistance;

            newSettings.MousePollingRate = legacySettings.MousePollingRate;

            newSettings.MouseXAxisSensitivityAimModifier = legacySettings.MouseXAxisSensitivityModifier;
            newSettings.MouseXAxisSensitivityLookModifier = legacySettings.MouseXAxisSensitivityModifier;
            newSettings.MouseXAxisSensitivityMax = legacySettings.MouseXAxisSensitivityMax;

            newSettings.MouseYAxisSensitivityAimModifier = legacySettings.MouseYAxisSensitivityModifier;
            newSettings.MouseYAxisSensitivityLookModifier = legacySettings.MouseYAxisSensitivityModifier;
            newSettings.MouseYAxisSensitivityMax = legacySettings.MouseYAxisSensitivityMax;

            newSettings.XYRatio = legacySettings.XYRatio;


            foreach (VirtualKey key in legacySettings.Mappings.Keys)
            {
                PhysicalKey pk = new PhysicalKey();
                pk.KeyboardValue = legacySettings.Mappings[key];

                newSettings.Mappings[key] = GetPhysicalKeyGroup(pk);
            }

            // Now readd the mouse bindings that didnt exist as config in 1.0.11
            AddManualMouseMapping(newSettings, VirtualKey.L2, MouseButton.Right);
            AddManualMouseMapping(newSettings, VirtualKey.R2, MouseButton.Left);

            newSettings.Version_2_0_0_OrGreater = true;

            return newSettings;
        }

    }
}

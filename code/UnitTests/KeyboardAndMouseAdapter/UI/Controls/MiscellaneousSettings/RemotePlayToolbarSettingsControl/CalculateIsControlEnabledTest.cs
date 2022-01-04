﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pizza.Common;
using PS4KeyboardAndMouseAdapter.Config;
using PS4KeyboardAndMouseAdapter.UI;
using PS4KeyboardAndMouseAdapter.UI.Controls.MiscellaneousSettings;

namespace UnitTests.KeyboardAndMouseAdapter.UI.Controls.RemotePlayToolbarSettingsControlTest
{

    [TestClass]
    public class CalculateIsControlEnabledTest
    {

        [TestMethod]
        public void WhenEmulationMode__ONLY_PROCESS_INJECTION__ShouldAssumeRemotePlayProcessInjection()
        {
            ApplicationSettings.GetInstance().EmulationMode = EmulationConstants.ONLY_PROCESS_INJECTION;

            RemotePlayToolbarSettingsControl classUnderTest = new RemotePlayToolbarSettingsControl();
            classUnderTest.CalculateIsControlEnabled();

            Assert.AreEqual(UIConstants.VISIBILITY_COLLAPSED, classUnderTest.testonly_GetElement_TextBlock_Warning().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_TextBlock_Description().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_Toggle().Visibility);
        }

        [TestMethod]
        public void WhenEmulationMode__ONLY_VIGEM__ShouldAssumeRemotePlayProcessInjection()
        {
            ApplicationSettings.GetInstance().EmulationMode = EmulationConstants.ONLY_VIGEM;

            RemotePlayToolbarSettingsControl classUnderTest = new RemotePlayToolbarSettingsControl();
            classUnderTest.CalculateIsControlEnabled();

            Assert.AreEqual(UIConstants.VISIBILITY_COLLAPSED, classUnderTest.testonly_GetElement_TextBlock_Description().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_COLLAPSED, classUnderTest.testonly_GetElement_Toggle().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_TextBlock_Warning().Visibility);
        }

        [TestMethod]
        public void WhenEmulationMode__Unknown__ShouldAssumeRemotePlayProcessInjection()
        {
            ApplicationSettings.GetInstance().EmulationMode = -1;

            RemotePlayToolbarSettingsControl classUnderTest = new RemotePlayToolbarSettingsControl();
            classUnderTest.CalculateIsControlEnabled();

            Assert.AreEqual(UIConstants.VISIBILITY_COLLAPSED, classUnderTest.testonly_GetElement_TextBlock_Warning().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_TextBlock_Description().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_Toggle().Visibility);
        }

        [TestMethod]
        public void WhenEmulationMode__VIGEM_AND_PROCESS_INJECTION__ShouldAssumeRemotePlayProcessInjection()
        {
            ApplicationSettings.GetInstance().EmulationMode = EmulationConstants.VIGEM_AND_PROCESS_INJECTION;

            RemotePlayToolbarSettingsControl classUnderTest = new RemotePlayToolbarSettingsControl();
            classUnderTest.CalculateIsControlEnabled();

            Assert.AreEqual(UIConstants.VISIBILITY_COLLAPSED, classUnderTest.testonly_GetElement_TextBlock_Warning().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_TextBlock_Description().Visibility);
            Assert.AreEqual(UIConstants.VISIBILITY_VISIBLE, classUnderTest.testonly_GetElement_Toggle().Visibility);
        }

    }
}

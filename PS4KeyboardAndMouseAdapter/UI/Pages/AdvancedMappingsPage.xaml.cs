﻿using System;
using System.Windows;
using System.Windows.Controls;
using PS4KeyboardAndMouseAdapter.Config;

namespace PS4KeyboardAndMouseAdapter.UI.Pages
{
    public partial class AdvancedMappingsPage : UserControl
    {
        UserSettings Settings;

        public AdvancedMappingsPage()
        {
            InitializeComponent();

            Settings = UserSettings.GetInstance();

            addStuff();
        }

        private void addStuff()
        {
            var virtualKeys = KeyUtility.GetVirtualKeyValues();
            foreach (VirtualKey vk in virtualKeys)
            {

                TextBlock textblock = new TextBlock() {
                    Text = string.Format("{0}", vk),
                };
                mappingHolder.Children.Add(textblock);

                if (Settings.Mappings[vk] != null)
                {
                    Button button = new Button();

                    if (Settings.Mappings[vk].KeyboardValue != SFML.Window.Keyboard.Key.Unknown)
                    {
                        button.Content = string.Format("value '{0}'", Settings.Mappings[vk]);
                    }
                    mappingHolder.Children.Add(button);
                }

            }

        }

        private void GotFocusLocal(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("AdvancedMappingsPage.GotFocusLocal()");
            InstanceSettings.BroadcastRefresh();
            UserSettings.BroadcastRefresh();
        }

    }
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Spark
{
	/// <summary>
	/// Interaction logic for OverlaysConfigWindow.xaml
	/// </summary>
	public partial class OverlaysConfigWindow : UserControl
	{
		public Visibility ManualInputVisible { get => SparkSettings.instance.overlaysTeamSource == 0 ? Visibility.Visible : Visibility.Collapsed; }
		private bool init;
		public OverlaysConfigWindow()
		{
			InitializeComponent();
			LoadCasterSettings();
			init = true;
		}
		private void OpenOverlaysMainPage(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo("http://localhost:6724/") { UseShellExecute = true });
				e.Handled = true;
			}
			catch (Exception ex)
			{
				Logger.LogRow(Logger.LogType.Error, ex.ToString());
			}
		}

		private void SwapTeamSettings(object sender, RoutedEventArgs e)
		{
			(ManualTeamNameOrange.Text, ManualTeamNameBlue.Text) = (ManualTeamNameBlue.Text, ManualTeamNameOrange.Text);
			(ManualTeamLogoOrange.Text, ManualTeamLogoBlue.Text) = (ManualTeamLogoBlue.Text, ManualTeamLogoOrange.Text);

			SparkSettings.instance.overlaysManualTeamNameOrange = ManualTeamNameOrange.Text;
			SparkSettings.instance.overlaysManualTeamNameBlue = ManualTeamNameBlue.Text;
			SparkSettings.instance.overlaysManualTeamLogoOrange = ManualTeamLogoOrange.Text;
			SparkSettings.instance.overlaysManualTeamLogoBlue = ManualTeamLogoBlue.Text;
			Program.OverlayConfigChanged?.Invoke();
		}

		public void SetUIToSettings()
		{
			ManualTeamNameOrange.Text = SparkSettings.instance.overlaysManualTeamNameOrange;
			ManualTeamNameBlue.Text = SparkSettings.instance.overlaysManualTeamNameBlue;
			ManualTeamLogoOrange.Text = SparkSettings.instance.overlaysManualTeamLogoOrange;
			ManualTeamLogoBlue.Text = SparkSettings.instance.overlaysManualTeamLogoBlue;
		}

		private void TeamsDataSourceChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!init) return;
			ComboBox dropdown = (ComboBox)sender;
			ManualInputSettings.Visibility = dropdown.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
			Program.OverlayConfigChanged?.Invoke();
		}

		private void TeamNameChanged(object sender, TextChangedEventArgs e)
		{
			Program.OverlayConfigChanged?.Invoke();
		}

		private void LoadCasterSettings()
		{
			// Load number of casters
			if (SparkSettings.instance.casterPrefs.ContainsKey("num_casters"))
			{
				int numCasters = Convert.ToInt32(SparkSettings.instance.casterPrefs["num_casters"]);
				NumCastersComboBox.SelectedIndex = numCasters - 1;
			}
			else
			{
				NumCastersComboBox.SelectedIndex = 0; // Default to 1
			}

			// Load caster names and logos
			Caster1Name.Text = GetCasterValue("caster1_name");
			Caster1Logo.Text = GetCasterValue("caster1_logo");
			Caster2Name.Text = GetCasterValue("caster2_name");
			Caster2Logo.Text = GetCasterValue("caster2_logo");
			Caster3Name.Text = GetCasterValue("caster3_name");
			Caster3Logo.Text = GetCasterValue("caster3_logo");
			Caster4Name.Text = GetCasterValue("caster4_name");
			Caster4Logo.Text = GetCasterValue("caster4_logo");

			UpdateCasterPanelVisibility();
		}

		private string GetCasterValue(string key)
		{
			if (SparkSettings.instance.casterPrefs.ContainsKey(key))
			{
				return SparkSettings.instance.casterPrefs[key]?.ToString() ?? "";
			}
			return "";
		}

		private void NumCastersChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!init) return;

			int numCasters = NumCastersComboBox.SelectedIndex + 1;
			SparkSettings.instance.casterPrefs["num_casters"] = numCasters;

			UpdateCasterPanelVisibility();
			SaveCasterSettings();
		}

		private void UpdateCasterPanelVisibility()
		{
			int numCasters = NumCastersComboBox.SelectedIndex + 1;

			Caster1Panel.Visibility = numCasters >= 1 ? Visibility.Visible : Visibility.Collapsed;
			Caster2Panel.Visibility = numCasters >= 2 ? Visibility.Visible : Visibility.Collapsed;
			Caster3Panel.Visibility = numCasters >= 3 ? Visibility.Visible : Visibility.Collapsed;
			Caster4Panel.Visibility = numCasters >= 4 ? Visibility.Visible : Visibility.Collapsed;
		}

		private void CasterChanged(object sender, TextChangedEventArgs e)
		{
			if (!init) return;
			SaveCasterSettings();
		}

		private void SaveCasterSettings()
		{
			// Only save if values actually changed
			bool changed = false;

			if (!SparkSettings.instance.casterPrefs.ContainsKey("caster1_name") || SparkSettings.instance.casterPrefs["caster1_name"]?.ToString() != Caster1Name.Text) changed = true;
			if (!SparkSettings.instance.casterPrefs.ContainsKey("caster1_logo") || SparkSettings.instance.casterPrefs["caster1_logo"]?.ToString() != Caster1Logo.Text) changed = true;

			if (!changed)
			{
				// Check other casters quickly
				if (SparkSettings.instance.casterPrefs.GetValueOrDefault("caster2_name")?.ToString() != Caster2Name.Text ||
					SparkSettings.instance.casterPrefs.GetValueOrDefault("caster3_name")?.ToString() != Caster3Name.Text ||
					SparkSettings.instance.casterPrefs.GetValueOrDefault("caster4_name")?.ToString() != Caster4Name.Text)
				{
					changed = true;
				}
			}

			if (!changed)
			{
				Logger.LogRow(Logger.LogType.Info, "Caster settings unchanged, skipping save");
				return;
			}

			// Save all caster values to settings
			SparkSettings.instance.casterPrefs["caster1_name"] = Caster1Name.Text;
			SparkSettings.instance.casterPrefs["caster1_logo"] = Caster1Logo.Text;
			SparkSettings.instance.casterPrefs["caster2_name"] = Caster2Name.Text;
			SparkSettings.instance.casterPrefs["caster2_logo"] = Caster2Logo.Text;
			SparkSettings.instance.casterPrefs["caster3_name"] = Caster3Name.Text;
			SparkSettings.instance.casterPrefs["caster3_logo"] = Caster3Logo.Text;
			SparkSettings.instance.casterPrefs["caster4_name"] = Caster4Name.Text;
			SparkSettings.instance.casterPrefs["caster4_logo"] = Caster4Logo.Text;

			Logger.LogRow(Logger.LogType.Info, $"Saving caster settings from UI: num_casters={SparkSettings.instance.casterPrefs["num_casters"]}, caster1={Caster1Name.Text}");

			// Save to disk
			SparkSettings.instance.Save();

			// Trigger overlay config update via WebSocket
			Program.OverlayConfigChanged?.Invoke();
			Logger.LogRow(Logger.LogType.Info, "OverlayConfigChanged invoked from caster settings");
		}
	}
}

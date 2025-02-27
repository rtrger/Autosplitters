using System;
using System.Reflection;
using System.Windows.Forms;

namespace TR1
{
    public class ComponentSettings : UserControl
    {
        private GroupBox _modeSelect;
        public RadioButton ILModeButton;
        public RadioButton FullGameModeButton;
        public RadioButton DeathrunModeButton;
        public Label GameVersionLabel;
        public Label AutosplitterVersionLabel;
        public bool FullGame = true;
        public bool Deathrun = false;

        public ComponentSettings() => InitializeComponent();

        private void InitializeComponent()
        {
            _modeSelect = new GroupBox();
            ILModeButton = new RadioButton();
            FullGameModeButton = new RadioButton();
            DeathrunModeButton = new RadioButton();
            GameVersionLabel = new Label();
            AutosplitterVersionLabel = new Label();
            _modeSelect.SuspendLayout();
            SuspendLayout();

            // _modeSelect
            _modeSelect.Controls.Add(ILModeButton);
            _modeSelect.Controls.Add(FullGameModeButton);
            _modeSelect.Controls.Add(DeathrunModeButton);
            _modeSelect.Location = new System.Drawing.Point(4, 4);
            _modeSelect.Name = "_modeSelect";
            _modeSelect.Size = new System.Drawing.Size(297, 53);
            _modeSelect.TabIndex = 0;
            _modeSelect.TabStop = false;
            _modeSelect.Text = "Mode Selection";

            // ILModeButton
            ILModeButton.AutoSize = true;
            ILModeButton.Location = new System.Drawing.Point(84, 20);
            ILModeButton.Name = "ILModeButton";
            ILModeButton.Size = new System.Drawing.Size(139, 17);
            ILModeButton.TabIndex = 1;
            ILModeButton.Text = "IL or Section Run (RTA)";
            ILModeButton.UseVisualStyleBackColor = true;
            ILModeButton.CheckedChanged += ILModeButtonCheckedChanged;

            // FullGameModeButton
            FullGameModeButton.AutoSize = true;
            FullGameModeButton.Checked = true;
            FullGameModeButton.Location = new System.Drawing.Point(6, 20);
            FullGameModeButton.Name = "FullGameModeButton";
            FullGameModeButton.Size = new System.Drawing.Size(72, 17);
            FullGameModeButton.TabIndex = 0;
            FullGameModeButton.TabStop = true;
            FullGameModeButton.Text = "Full Game";
            FullGameModeButton.UseVisualStyleBackColor = true;
            FullGameModeButton.CheckedChanged += FullGameModeButtonCheckedChanged;

            // DeathrunModeButton
            DeathrunModeButton.AutoSize = true;
            DeathrunModeButton.Location = new System.Drawing.Point(225, 20);
            DeathrunModeButton.Name = "DeathrunModeButton";
            DeathrunModeButton.Size = new System.Drawing.Size(69, 17);
            DeathrunModeButton.TabIndex = 2;
            DeathrunModeButton.Text = "Deathrun";
            DeathrunModeButton.UseVisualStyleBackColor = true;
            DeathrunModeButton.CheckedChanged += DeathrunModeButtonCheckedChanged;

            // GameVersionLabel
            GameVersionLabel.AutoSize = true;
            GameVersionLabel.Location = new System.Drawing.Point(10, 64);
            GameVersionLabel.Name = "GameVersionLabel";
            GameVersionLabel.Size = new System.Drawing.Size(186, 13);
            GameVersionLabel.TabIndex = 1;
            GameVersionLabel.Text = "Game Version: Unknown/Undetected";

            // AutosplitterVersionLabel
            AutosplitterVersionLabel.AutoSize = true;
            AutosplitterVersionLabel.Location = new System.Drawing.Point(10, 87);
            AutosplitterVersionLabel.Name = "AutosplitterVersionLabel";
            AutosplitterVersionLabel.Size = new System.Drawing.Size(103, 13);
            AutosplitterVersionLabel.TabIndex = 2;
            AutosplitterVersionLabel.Text = "Autosplitter Version: " + Assembly.GetExecutingAssembly().GetName().Version;

            // ComponentSettings
            Controls.Add(AutosplitterVersionLabel);
            Controls.Add(GameVersionLabel);
            Controls.Add(_modeSelect);
            Name = "ComponentSettings";
            Size = new System.Drawing.Size(313, 110);
            _modeSelect.ResumeLayout(false);
            _modeSelect.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        internal void SetGameVersion(GameVersion? version)
        {
            string versionText = "";
            switch (version)
            {
                case GameVersion.DOSBox:
                    versionText = "DOSBox";
                    break;
                case GameVersion.ATI:
                    versionText = "TombATI";
                    break;
            }
            GameVersionLabel.Text = string.IsNullOrEmpty(versionText) ? "Game Version: Unknown/Undetected" : "Game Version: " + versionText;
        }

        private void FullGameModeButtonCheckedChanged(object sender, EventArgs e)
        {
            FullGame = true;
            Deathrun = false;
        }

        private void ILModeButtonCheckedChanged(object sender, EventArgs e)
        {
            FullGame = false;
            Deathrun = false;
        }

        private void DeathrunModeButtonCheckedChanged(object sender, EventArgs e)
        {
            FullGame = false;
            Deathrun = true;
        }
    }
}

using System;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public partial class UnderTimeRunCounterSettings : UserControl
    {
        public TimeSpan TargetTime { get; set; }
        public string LabelText { get; set; }

        public event Action OnUpdate;

        private bool isLoaded = false;

        public UnderTimeRunCounterSettings()
        {
            InitializeComponent();

            TargetTime = TimeSpan.Zero;
            LabelText = "Under Time Run Counter";
        }
 
        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
            TargetTime = SettingsHelper.ParseTimeSpan(element["TargetTime"]);
            LabelText = SettingsHelper.ParseString(element["LabelText"]);
            OnUpdate?.Invoke();
        }

        public XmlNode GetSettings(XmlDocument document)
        { 
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", "1.0") ^
            SettingsHelper.CreateSetting(document, parent, "TargetTime", TargetTime) ^
            SettingsHelper.CreateSetting(document, parent, "LabelText", LabelText);
        }

        private void timeHours_TextChanged(object sender, EventArgs e)
        {
            UpdateTargetTime();
        }

        private void timeMinutes_TextChanged(object sender, EventArgs e)
        {
            UpdateTargetTime();
        }

        private void timeSeconds_TextChanged(object sender, EventArgs e)
        {
            UpdateTargetTime();
        }

        private void UpdateTargetTime()
        {
            if(isLoaded)
            {
                if (!int.TryParse(timeHours.Text, out int hours))
                {
                    hours = 0;
                }
                if (!int.TryParse(timeMinutes.Text, out int minutes))
                {
                    minutes = 0;
                }
                if(!int.TryParse(timeSeconds.Text, out int seconds))
                {
                    seconds = 0;
                }
                TargetTime = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes) +  TimeSpan.FromSeconds(seconds);
                OnUpdate?.Invoke();
            }
        }

        private void labelText_TextChanged(object sender, EventArgs e)
        {
            LabelText = labelText.Text;
        }

        private void UnderTimeRunCounterSettings_Load(object sender, EventArgs e)
        {
            labelText.Text = LabelText;
            timeHours.Text = TargetTime.Hours.ToString();
            timeMinutes.Text = TargetTime.Minutes.ToString();
            timeSeconds.Text = TargetTime.Seconds.ToString();
            isLoaded = true;
        }
    }
}

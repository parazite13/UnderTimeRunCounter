using LiveSplit.Model;
using LiveSplit.TimeFormatters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public class UnderTimeRunCounterComponent : IComponent
    {
        protected InfoTimeComponent InternalComponent { get; set; }
        protected LiveSplitState CurrentState { get; set; }
        public UnderTimeRunCounterSettings Settings { get; set; }

        public string ComponentName => "Under Time Run Counter";

        public float VerticalHeight => InternalComponent.VerticalHeight;
        public float MinimumWidth => InternalComponent.MinimumWidth;
        public float HorizontalWidth => InternalComponent.HorizontalWidth;
        public float MinimumHeight => InternalComponent.MinimumHeight;

        public float PaddingTop => InternalComponent.PaddingTop;
        public float PaddingLeft => InternalComponent.PaddingLeft;
        public float PaddingBottom => InternalComponent.PaddingBottom;
        public float PaddingRight => InternalComponent.PaddingRight;

        public IDictionary<string, Action> ContextMenuControls => null;

        private RegularUnderTimeRunCounterTimeFormatter Formatter { get; set; }

        private int underTimeRunCounterValue = 0;

        public UnderTimeRunCounterComponent(LiveSplitState state)
        {
            Formatter = new RegularUnderTimeRunCounterTimeFormatter();
            InternalComponent = new InfoTimeComponent(ComponentName, null, Formatter);
            Settings = new UnderTimeRunCounterSettings();
            CurrentState = state;

            state.OnSplit += State_OnSplit;
            state.OnReset += State_OnReset;
            state.RunManuallyModified += State_RunManuallyModified;

            Settings.OnUpdate += Settings_OnUpdate;
            UpdateUnderTimeCounterValue(state);
        }

        private void State_RunManuallyModified(object sender, EventArgs e)
        {
            UpdateUnderTimeCounterValue((LiveSplitState)sender);
        }

        private void State_OnReset(object sender, TimerPhase value)
        {
            UpdateUnderTimeCounterValue((LiveSplitState)sender);
        }

        private void Settings_OnUpdate()
        {
            UpdateUnderTimeCounterValue(CurrentState);
        }

        private void State_OnSplit(object sender, EventArgs e)
        {
            UpdateUnderTimeCounterValue((LiveSplitState)sender);
        }

        public void Dispose()
        {
            CurrentState.OnSplit -= State_OnSplit;
            Settings.OnUpdate -= Settings_OnUpdate;
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            InternalComponent.NameLabel.HasShadow
                = InternalComponent.ValueLabel.HasShadow
                = state.LayoutSettings.DropShadows;

            InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;

            InternalComponent.DrawHorizontal(g, state, height, clipRegion);
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            InternalComponent.NameLabel.HasShadow
                = InternalComponent.ValueLabel.HasShadow
                = state.LayoutSettings.DropShadows;

            InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;

            InternalComponent.DrawVertical(g, state, width, clipRegion);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            return Settings;
        }

        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();

        public void SetSettings(XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            InternalComponent.InformationName = Settings.LabelText;
            InternalComponent.InformationValue = underTimeRunCounterValue.ToString();
            InternalComponent.Update(invalidator, state, width, height, mode);
        }

        private void UpdateUnderTimeCounterValue(LiveSplitState state)
        {
            var completedRunsId = state.Run[state.Run.Count - 1].SegmentHistory.Select(s => s.Key);
            underTimeRunCounterValue = state.Run.AttemptHistory
                .Where(a => completedRunsId.Contains(a.Index))
                .Count(a => a.Duration < Settings.TargetTime) 
                + Convert.ToInt32(state.CurrentPhase == TimerPhase.Ended && state.CurrentTime.RealTime < Settings.TargetTime);
        }

    }
}

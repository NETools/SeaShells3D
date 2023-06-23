using FrassekPicadoStrategies.Settings;
using FrassekPicadoStrategies.Strategies;
using FrassekPicadoStrategies.Strategies.Parameters.Runtime;
using FrassekPicadoStrategies.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace FrassekPicadoStrategies.View
{
    public partial class ParameterSettings : Form
    {
        private ShellVisualization _shellVisualization;
        private IShellStrategy[] _strategies = new IShellStrategy[]
        {
            new Picado(),
            new Frassek()
        };

        public ParameterSettings(ShellVisualization shellVisualization)
        {
            InitializeComponent();
            _shellVisualization = shellVisualization;
        }

        private void phiTracker_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.ShellStrategy.Parameters.Phi = PicadoPhi.Value;
            lblPhi.Text = PicadoPhi.Value + "";
        }

        private void muTracker_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.ShellStrategy.Parameters.Mu = PicadoMu.Value;
            lblMu.Text = PicadoMu.Value + "";
        }

        private void omegaTracker_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.ShellStrategy.Parameters.Omega = PicadoOmega.Value;
            lblOmega.Text = PicadoOmega.Value + "";
        }

        private void alphaTracker_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.ShellStrategy.Parameters.Alpha = PicadoAlpha.Value;
            lblAlpha.Text = PicadoAlpha.Value + "";

        }

        private void betaTracker_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.ShellStrategy.Parameters.Beta = PicadoBeta.Value;
            lblBeta.Text = PicadoBeta.Value + "";
        }

        private void dTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoD.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.D = rslt;
        }

        private void aTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoA.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.A = rslt;
        }

        private void majorTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoMinorAxis.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.MajorAxis = rslt;
        }

        private void minorTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoMajorAxis.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.MinorAxis = rslt;
        }

        private void lTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoL.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.L = rslt;
        }

        private void nTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoN.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.N = rslt;
        }

        private void w1Tb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoW1.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.W1 = rslt;
        }

        private void w2Tb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoW2.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.W2 = rslt;
        }

        private void pTb_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(PicadoP.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.P = rslt;
        }

        private void helicoSamplesTrackbar_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.MaximumHelicoSamples = MaximumHelicoSamples.Value;
            helicoLb.Text = MaximumHelicoSamples.Value + "";
        }

        private void ellipseSamplesTrackbar_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.MaximumEllipseSamples = MaximumEllipseSamples.Value;
            ellipseLb.Text = MaximumEllipseSamples.Value + "";
        }

        private void ParameterSettings_Load(object sender, EventArgs e)
        {

            _shellVisualization.PropertyChanged += OnPropertyChanged;

            _shellVisualization.SetShellStrategy(_strategies[0]);

            _shellVisualization.ShellStrategy.Parameters.D = 1;
            _shellVisualization.ShellStrategy.Parameters.Alpha = 87;
            _shellVisualization.ShellStrategy.Parameters.Beta = 7;
            _shellVisualization.ShellStrategy.Parameters.A = 90;
            _shellVisualization.ShellStrategy.Parameters.MajorAxis = 20;
            _shellVisualization.ShellStrategy.Parameters.MinorAxis = 20;
            _shellVisualization.ShellStrategy.Parameters.Phi = 45;
            _shellVisualization.ShellStrategy.Parameters.Mu = 5;
            _shellVisualization.ShellStrategy.Parameters.Omega = 1;
            _shellVisualization.ShellStrategy.Parameters.L = 0;
            _shellVisualization.ShellStrategy.Parameters.N = 40;
            _shellVisualization.ShellStrategy.Parameters.W1 = 180;
            _shellVisualization.ShellStrategy.Parameters.W2 = 0.4;
            _shellVisualization.ShellStrategy.Parameters.P = 40;



            //_shellVisualization.ShellStrategy.Parameters.R = 1;
            //_shellVisualization.ShellStrategy.Parameters.B = 1.6;
            //_shellVisualization.ShellStrategy.Parameters.C = 0;
            //_shellVisualization.ShellStrategy.Parameters.D1 = 1;
            //_shellVisualization.ShellStrategy.Parameters.W = 1;
            //_shellVisualization.ShellStrategy.Parameters.D = -1;
            //_shellVisualization.ShellStrategy.Parameters.M = 5e-5;
            //_shellVisualization.ShellStrategy.Parameters.L = .1;
            //_shellVisualization.ShellStrategy.Parameters.L1 = 32;
            //_shellVisualization.ShellStrategy.Parameters.Q = .05;
            //_shellVisualization.ShellStrategy.Parameters.Q1 = 32;

            _shellVisualization.Revolutions = 2;
            _shellVisualization.ThetaMin = -40;
            _shellVisualization.MaximumEllipseSamples = 100;
            _shellVisualization.MaximumHelicoSamples = 100;

            FillComboBox();

        }

        private void OnPropertyChanged(object value, PropertyChangedEventArgs e)
        {
            var fields = this.Controls.Find( e.PropertyName, true);
            if (fields.Length == 0)
                return;
            var fieldValue = fields[0];
            if(fieldValue is TextBox)
            {
                var txtb = fieldValue as TextBox;
                txtb.Text = value + "";
            }
            else if(fieldValue is TrackBar)
            {
                var trkb = fieldValue as TrackBar;
                dynamic numeric = value;
                trkb.Value = int.Parse(Math.Round((double)numeric) + "");
            }
        }

        struct ComboBoxItem
        {

            public string Name;
            public string File;
            public override string ToString()
            {
                return Name;
            }
        }

        private void FillComboBox()
        {
            var comboxBox = ((ComboBox)this.Controls.Find($"{_shellVisualization.ShellStrategy.ToString()}Cmbx", true)[0]);
            comboxBox.Items.Clear();
            foreach (var file in Directory.GetFiles(_shellVisualization.ShellStrategy + "\\"))
            {
                comboxBox.Items.Add(new ComboBoxItem()
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    File = file
                });
            }
        }
    
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex + 1 > _strategies.Length) return;
            _shellVisualization.SetShellStrategy(_strategies[tabControl1.SelectedIndex ]);
            FillComboBox();
        }

        private void ReadSettings()
        {
            IniSettingsReader reader = new IniSettingsReader(((ComboBoxItem)((ComboBox)this.Controls.Find($"{_shellVisualization.ShellStrategy.ToString()}Cmbx", true)[0]).SelectedItem).File);
            foreach (var pair in reader.GetGroup("Shell-Parameters"))
            {
                var reflected = _shellVisualization.ShellStrategy.Parameters as IReflection;
                reflected.SetValue(pair.Key, double.Parse(pair.Value));
            }

            foreach (var pair in reader.GetGroup("Renderer-Parameters"))
            {
                var reflected = _shellVisualization as IReflection;
                reflected.SetValue(pair.Key, double.Parse(pair.Value));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadSettings();
        }

        private void trackBarX_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.Translation = _shellVisualization.Translation + Vector3.UnitX * (trackbarX.Value - _shellVisualization.Translation.X);
            x.Text = trackbarX.Value + "";
        }
        private void trackBarY_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.Translation = _shellVisualization.Translation + Vector3.UnitY * (trackBarY.Value - _shellVisualization.Translation.Y); 
            y.Text = trackBarY.Value + "";
        }
        private void trackBarZ_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.Translation = _shellVisualization.Translation + Vector3.UnitZ * (trackBarZ.Value - _shellVisualization.Translation.Z);
            z.Text = trackBarZ.Value + "";
        }

        private void trackBarRX_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.RotationX = trackBarRX.Value;
            rx.Text = trackBarRX.Value + "";
        }

        private void trackBarRY_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.RotationY = trackBarRY.Value;
            ry.Text = trackBarRY.Value + "";
        }

        private void trackBarRZ_ValueChanged(object sender, EventArgs e)
        {
            _shellVisualization.RotationZ = trackBarRZ.Value;
            rz.Text = trackBarRZ.Value + "";
        }

        private void FrassekR_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekR.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.R = rslt;
        }

        private void FrassekB_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekB.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.B = rslt;
        }

        private void FrassekC_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekC.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.C = rslt;
        }

        private void FrassekD_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekD.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.D = rslt;
        }

        private void FrassekW_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekW.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.W = rslt;
        }

        private void FrassekD1_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekD1.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.D1 = rslt;
        }

        private void FrassekM_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekM.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.M = rslt;
        }

        private void FrassekL_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekL.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.L = rslt;
        }

        private void FrassekL1_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekL1.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.L1 = rslt;
        }

        private void FrassekQ_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekQ.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.Q = rslt;
        }

        private void FrassekQ1_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(FrassekQ1.Text, out double rslt))
                _shellVisualization.ShellStrategy.Parameters.Q1 = rslt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReadSettings();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _shellVisualization.ShowFloor = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _shellVisualization.ExportCommand();
        }

        private void MinTheta_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(ThetaMin.Text, out double value))
                _shellVisualization.ThetaMin = value;
        }

        private void Revolutions_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(Revolutions.Text, out double value))
                _shellVisualization.Revolutions = value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _shellVisualization.SaveSettingsCommand();
            FillComboBox();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _shellVisualization.SaveSettingsCommand();
            FillComboBox();
        }
    }
}

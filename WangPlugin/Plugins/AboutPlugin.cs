﻿using System;
using System.Windows.Forms;
using WangPlugin.GUI;

namespace WangPlugin.Plugins
{
    internal class AboutPlugin: WangPlugin
    {
        public override string Name => "关于";
        public override int Priority => 9;

        protected override void AddPluginControl(ToolStripDropDownItem modmenu)
        {
            var ctrl = new ToolStripMenuItem(Name)
            {
                Image = Properties.Resources.avatar
            };
            ctrl.Click += OpenForm;
            ctrl.Name = "关于";
            modmenu.DropDownItems.Add(ctrl);

        }

        private void OpenForm(object sender, EventArgs e)
        {

            var form = new AboutUI();
            form.ShowDialog();
        }
    }
}

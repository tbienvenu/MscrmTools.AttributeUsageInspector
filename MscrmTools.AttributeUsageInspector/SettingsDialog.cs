﻿using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MscrmTools.AttributeUsageInspector
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog(Settings settings)
        {
            InitializeComponent();

            Settings = settings;

            nudNumberOfRecordsPerCall.Value = settings.RecordsReturnedPerTrip >= nudNumberOfRecordsPerCall.Minimum &&
                                              settings.RecordsReturnedPerTrip <= nudNumberOfRecordsPerCall.Maximum
                ? settings.RecordsReturnedPerTrip
                : nudNumberOfRecordsPerCall.Minimum;

            nudNumberOfAttributesPerCall.Value = settings.AttributesReturnedPerTrip >= nudNumberOfAttributesPerCall.Minimum &&
                                                 settings.AttributesReturnedPerTrip <= nudNumberOfAttributesPerCall.Maximum
                ? settings.AttributesReturnedPerTrip
                : nudNumberOfAttributesPerCall.Minimum;

            chkFilterAttributes.Checked = settings.FilterAttributes;

            chkUseSQL.Checked = settings.UseSQLQuery;

            tbSQLConnectionString.Text = settings.SQLConnectionString;
            btnTestConnection.Enabled = chkUseSQL.Checked;
            lblCommandTimeout.Enabled = chkUseSQL.Checked;
            nudCommandTimeOut.Enabled = chkUseSQL.Checked;
        }

        public Settings Settings { get; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings.RecordsReturnedPerTrip = Convert.ToInt32(nudNumberOfRecordsPerCall.Value);
            Settings.AttributesReturnedPerTrip = Convert.ToInt32(nudNumberOfAttributesPerCall.Value);
            Settings.FilterAttributes = chkFilterAttributes.Checked;
            Settings.UseSQLQuery = chkUseSQL.Checked;
            Settings.SQLConnectionString = tbSQLConnectionString.Text;
            Settings.SQLCommandTimeout = Convert.ToInt32(nudCommandTimeOut.Value);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void chkUseSQL_CheckedChanged(object sender, EventArgs e)
        {
            tbSQLConnectionString.Enabled = chkUseSQL.Checked;
            btnTestConnection.Enabled = chkUseSQL.Checked;
            lblCommandTimeout.Enabled = chkUseSQL.Checked;
            nudCommandTimeOut.Enabled = chkUseSQL.Checked;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            testSqlConnection(tbSQLConnectionString.Text);
        }

        private static void testSqlConnection(string connectionstring)
        {
            try
            {
                using (var connection = new SqlConnection(connectionstring))
                {
                    var query = "select 1";

                    var command = new SqlCommand(query, connection);

                    connection.Open();

                    command.ExecuteScalar();
                    MessageBox.Show("Connection Ok", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
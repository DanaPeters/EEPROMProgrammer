using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEPROMProgrammer
{
    public partial class AddressForm : Form
    {
        private int _length;

        public int StartAddress
        {
            get => (int)startAddressNumericUpDown.Value;
        }

        public AddressForm(int startAddress, int length)
        {
            _length = length;
            InitializeComponent();
            startAddressNumericUpDown.Value = startAddress;
            startAddressNumericUpDown.Maximum = 0x10000 - _length;
            UpdateEndAddress();
        }

        private void startAddressNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateEndAddress();
        }

        private void UpdateEndAddress()
        {
            endAddressLabel.Text = $"{StartAddress + _length - 1:X4}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;

using ArduinoUploader;
using EEPROMProgrammer.Properties;

namespace EEPROMProgrammer
{
    public partial class MainForm : Form, IArduinoUploaderLogger, IProgress<double>
    {
        private enum StateEnum
        {
            None,
            AwaitingSerialPort,
            AwaitingSketchDetect,
            Ready,
            Writing,
            Verifying,
            Reading,
            LoadingSketch,
        };

        private const string SKETCH_FILENAME = "Sketch.hex";
        private const int MAX_RECORD_SIZE = 64;

        private ManagementEventWatcher _managementEventWatcher;
        private System.Windows.Forms.Timer _deviceChangeTimer;
        private SerialPort _serialPort;
        private string[] _comPorts;
        private DateTime _sketchDetectTime;
        private DateTime _stateChangeTime;
        private int _eepromSize;
        private int _eepromStartAddress;
        private bool _rom;
        private Thread _actionThread;
        private bool _abort;
        private byte[] _writeData;
        private byte[] _readData;
        private byte[] _verifyData;
        private string _readFilename;
        private StateEnum _state;
        private ArduinoSketchUploader _arduinoSketchUploader;
        private string _serialPortName;
        private bool _sketchLoadAttempted;

        private static int[] _eepromSizes =
        {
            8 * 1024,
            16 * 1024,
            32 * 1024,
            64 * 1024,
        };

        public MainForm()
        {
            InitializeComponent();
            versionToolStripStatusLabel.Text = $"Version {Application.ProductVersion}";
            LoadSettings();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _comPorts = SerialPort.GetPortNames();
            if (String.IsNullOrEmpty(Settings.Default.SerialPort) || !_comPorts.Contains(Settings.Default.SerialPort))
                SetState(StateEnum.AwaitingSerialPort);
            else
                OpenSerialPort(Settings.Default.SerialPort);
            InitializeSerialPortWatcher();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
            eepromSizeComboBox.SelectedIndex = Settings.Default.FlashSizeIndex;
            eepromStartAddressComboBox.SelectedIndex = Settings.Default.FlashStartAddressIndex;
        }

        private void SaveSettings()
        {
            Settings.Default.FlashSizeIndex = eepromSizeComboBox.SelectedIndex;
            Settings.Default.FlashStartAddressIndex = eepromStartAddressComboBox.SelectedIndex;
            Settings.Default.Save();
        }

        private void SetState(StateEnum state)
        {
            if (state == _state)
                return;
            _state = state;
            _stateChangeTime = DateTime.Now;
            RefreshState();
        }

        private void RefreshState()
        {
            switch (_state)
            {
                case StateEnum.None:
                    stateColorLabel.BackColor = Color.Transparent;
                    stateTextLabel.Text = "";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = true;
                    stopToolStripDropDownButton.Visible = false;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = false;
                    break;
                case StateEnum.AwaitingSerialPort:
                    _sketchLoadAttempted = false;
                    stateColorLabel.BackColor = Color.Red;
                    stateTextLabel.Text = "Waiting for Arduino Mega";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = true;
                    stopToolStripDropDownButton.Visible = false;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = false;
                    break;
                case StateEnum.AwaitingSketchDetect:
                    stateColorLabel.BackColor = Color.Yellow;
                    stateTextLabel.Text = "Waiting for EEPROM programmer sketch";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = true;
                    stopToolStripDropDownButton.Visible = false;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = false;
                    break;
                case StateEnum.Ready:
                    stateColorLabel.BackColor = Color.Lime;
                    stateTextLabel.Text = "Ready";
                    writeButton.Enabled = !_rom;
                    verifyButton.Enabled = true;
                    readButton.Enabled = true;
                    versionToolStripStatusLabel.Visible = true;
                    stopToolStripDropDownButton.Visible = false;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = false;
                    break;
                case StateEnum.Writing:
                    stateColorLabel.BackColor = Color.Lime;
                    stateTextLabel.Text = "Writing file to EEPROM";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = false;
                    stopToolStripDropDownButton.Visible = true;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = true;
                    break;
                case StateEnum.Verifying:
                    stateColorLabel.BackColor = Color.Lime;
                    stateTextLabel.Text = "Comparing EEPROM to file";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = false;
                    stopToolStripDropDownButton.Visible = true;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = true;
                    break;
                case StateEnum.Reading:
                    stateColorLabel.BackColor = Color.Lime;
                    stateTextLabel.Text = "Reading EEPROM to file";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = false;
                    stopToolStripDropDownButton.Visible = true;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = true;
                    break;
                case StateEnum.LoadingSketch:
                    stateColorLabel.BackColor = Color.Lime;
                    stateTextLabel.Text = "Loading EEPROM programmer sketch";
                    writeButton.Enabled = false;
                    verifyButton.Enabled = false;
                    readButton.Enabled = false;
                    versionToolStripStatusLabel.Visible = true;
                    stopToolStripDropDownButton.Visible = false;
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Visible = true;
                    break;
            }
        }

        private void InitializeSerialPortWatcher()
        {
            _deviceChangeTimer = new System.Windows.Forms.Timer();
            _deviceChangeTimer.Interval = 500;
            _deviceChangeTimer.Tick += _deviceChangeTimer_Tick;
            var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 1 OR EventType = 2");
            _managementEventWatcher = new ManagementEventWatcher(query);
            _managementEventWatcher.EventArrived += _managementEventWatcher_EventArrived;
            //(sender, eventArgs) => CheckForSerialPortChanges(eventArgs);
            _managementEventWatcher.Start();
        }

        private void _deviceChangeTimer_Tick(object sender, EventArgs e)
        {
            _deviceChangeTimer.Stop();
            CheckForSerialPortChanges();
        }

        private void _managementEventWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Debug.Print("Device changed event");
            Invoke(new Action(() =>
            {
                _deviceChangeTimer.Stop();
                _deviceChangeTimer.Start();
            }));
        }

        private void CheckForSerialPortChanges()
        {
            Debug.Print("Checking for serial port changes");
            var ports = SerialPort.GetPortNames();
            int added = 0;
            int removed = 0;
            foreach (string comPort in _comPorts)
            {
                if (!ports.Contains(comPort))
                {
                    Debug.Print("Port removed: {0}", comPort);
                    removed += 1;
                }
            }
            string lastAddedPort = null;
            foreach (var port in ports)
            {
                if (!_comPorts.Contains(port))
                {
                    Debug.Print("Port added: {0}", port);
                    lastAddedPort = port;
                    added += 1;
                }
            }
            if (added > 0 || removed > 0)
            {
                _comPorts = ports;
                if (added == 1)
                {
                    if (_serialPort == null && _state == StateEnum.AwaitingSerialPort)
                        OpenSerialPort(lastAddedPort);
                }
            }
        }

        private void OpenSerialPort(string portName)
        {
            var serialPort = new SafeSerialPort()
            {
                PortName = portName,
                BaudRate = 115200,
                DataBits = 8,
                StopBits = System.IO.Ports.StopBits.One,
                NewLine = "\r\n",
                ReadTimeout = 1000,
            };
            try
            {
                serialPort.Open();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Exception: {exception.Message}");
                if (serialPort.IsOpen)
                    serialPort.Close();
                SetState(StateEnum.AwaitingSerialPort);
                return;
            }
            _serialPort = serialPort;
            _serialPortName = portName;
            SetState(StateEnum.AwaitingSketchDetect);

        }

        private void CloseSerialPort()
        {
            _serialPort?.Close();
            _serialPort = null;
            CheckForSerialPortChanges();
        }

        private void pollTimer_Tick(object sender, EventArgs e)
        {
            bool serialPortOpen = _serialPort?.IsOpen ?? false;
            switch (_state)
            {
                case StateEnum.None:
                case StateEnum.AwaitingSerialPort:
                case StateEnum.LoadingSketch:
                    break;
                case StateEnum.Writing:
                case StateEnum.Verifying:
                case StateEnum.Reading:
                    if (!serialPortOpen)
                    {
                        SetState(StateEnum.AwaitingSerialPort);
                        CloseSerialPort();
                        break;
                    }
                    _sketchDetectTime = DateTime.Now;
                    break;
                case StateEnum.AwaitingSketchDetect:
                    if (!serialPortOpen)
                    {
                        SetState(StateEnum.AwaitingSerialPort);
                        CloseSerialPort();
                        break;
                    }
                    if (SecondsSinceScriptDetected() == 0)
                    {
                        SetState(StateEnum.Ready);
                        break;
                    }
                    if (!_sketchLoadAttempted && (DateTime.Now.Subtract(_stateChangeTime).TotalSeconds > 10))
                    {
                        _sketchLoadAttempted = true;
                        var loadSketchForm = new LoadSketchForm();
                        if (loadSketchForm.ShowDialog(this) == DialogResult.OK)
                        {
                            UploadSketch();
                        }
                    }
                    break;
                case StateEnum.Ready:
                    if (SecondsSinceScriptDetected() > 10)
                        SetState(StateEnum.AwaitingSketchDetect);
                    break;
            }
        }

        private int SecondsSinceScriptDetected()
        {
            if (_serialPort?.IsOpen ?? false)
            {
                string text = _serialPort.ReadExisting();
                if (text.Contains("EPv1\r\n"))
                {
                    Debug.WriteLine("Programmer sketch detected");
                    Settings.Default.SerialPort = _serialPortName;
                    _sketchDetectTime = DateTime.Now;
                    return 0;
                }
            }
            if (_sketchDetectTime == DateTime.MinValue)
                return int.MaxValue;
            return (int)DateTime.Now.Subtract(_sketchDetectTime).TotalSeconds;
        }

        private void UploadSketch()
        {
            string applicationDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            var arduinoSketchUploaderOptions = new ArduinoSketchUploaderOptions();
            arduinoSketchUploaderOptions.PortName = _serialPortName;
            arduinoSketchUploaderOptions.FileName = Path.Combine(applicationDirectory, SKETCH_FILENAME);
            arduinoSketchUploaderOptions.ArduinoModel = ArduinoUploader.Hardware.ArduinoModel.Mega2560;
            SetState(StateEnum.LoadingSketch);
            CloseSerialPort();
            _arduinoSketchUploader = new ArduinoSketchUploader(arduinoSketchUploaderOptions, this, this);
            _actionThread = new Thread(new ThreadStart(LoadSketchThread));
            _actionThread.Name = "LoadSketchThread";
            _actionThread.IsBackground = true;
            _abort = false;
            _actionThread.Start();
        }

        private void LoadSketchThread()
        {
            _arduinoSketchUploader.UploadSketch();
            BeginInvoke((MethodInvoker)delegate
            {
                SetState(StateEnum.AwaitingSketchDetect);
                OpenSerialPort(_serialPortName);
            });
        }

        void IArduinoUploaderLogger.Error(string message, Exception exception)
        {
            Debug.WriteLine($"Arduino upload error: {message}");
        }

        void IArduinoUploaderLogger.Warn(string message)
        {
            Debug.WriteLine($"Arduino upload warning: {message}");
        }

        void IArduinoUploaderLogger.Info(string message)
        {
            Debug.WriteLine($"Arduino upload info: {message}");
        }

        void IArduinoUploaderLogger.Debug(string message)
        {
            Debug.WriteLine($"Arduino upload debug: {message}");
        }

        void IArduinoUploaderLogger.Trace(string message)
        {
            Debug.WriteLine($"Arduino upload trace: {message}");
        }

        void IProgress<double>.Report(double value)
        {
            Debug.WriteLine($"Arduino upload progress: {value}");
            BeginInvoke((MethodInvoker)delegate
            {
                toolStripProgressBar.Value = (int)(value * 100);
            });
        }

        private void eepromSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eepromSizeComboBox.SelectedIndex < 0)
            {
                eepromStartAddressComboBox.SelectedIndex = -1;
                return;
            }
            bool rom = eepromSizeComboBox.SelectedIndex >= 4;
            if (rom != _rom)
            {
                _rom = rom;
                RefreshState();
            }
            int eepromSize = _eepromSizes[eepromSizeComboBox.SelectedIndex % 4];
            eepromStartAddressComboBox.Items.Clear();
            for (int i = 0; i < 0x10000; i += eepromSize)
            {
                eepromStartAddressComboBox.Items.Add($"{i:X4}");
            }
            eepromStartAddressComboBox.SelectedIndex = 0;
        }

        private void eepromStartAddressComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eepromStartAddressComboBox.SelectedIndex < 0)
            {
                eepromEndAddressLabel.Text = "";
                return;
            }
            _eepromSize = _eepromSizes[eepromSizeComboBox.SelectedIndex % 4];
            _eepromStartAddress = eepromStartAddressComboBox.SelectedIndex * _eepromSize;
            int eepromEndAddress = _eepromStartAddress + _eepromSize - 1;
            eepromEndAddressLabel.Text = $"{eepromEndAddress:X4}";
        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            string filename = GetDataFile();
            if (filename != null)
            {
                Write(filename);
            }
        }

        private void verifyButton_Click(object sender, EventArgs e)
        {
            string filename = GetDataFile();
            if (filename != null)
            {
                Verify(filename);
            }
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Intel Hex file (*.hex)|*.hex|Motorola SREC file (*.s19)|*.s19|Binary file (*.bin)|*.bin|All files (*.*)|*.*",
                DefaultExt = "hex"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Read(saveFileDialog.FileName);
            }
        }

        private void stopToolStripDropDownButton_Click(object sender, EventArgs e)
        {
            _abort = true;
        }

        private string GetDataFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Data files (*.hex;*.s19;*.bin)|*.hex;*.s19;*.bin|Intel Hex file (*.hex)|*.hex|Motorola SREC file (*.s19)|*.s19|Binary file (*.bin)|*.bin|All files (*.*)|*.*",
                DefaultExt = "hex"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return null;
            return openFileDialog.FileName;
        }

        private void Write(string filename)
        {
            var memorySegments = LoadMemorySegments(filename);
            if (memorySegments == null)
                return;
            var (lowAddress, highAddress) = GetAddressRange(memorySegments);
            string text = $"File address range: {lowAddress:X4} to {highAddress:X4}\r"
                + $"EEPROM address range: {_eepromStartAddress:X4} to {_eepromStartAddress + _eepromSize - 1:X4}\r\r";
            if (lowAddress > _eepromStartAddress + _eepromSize || highAddress < _eepromStartAddress)
            {
                text += "Error: no overlap between these ranges";
                MessageBox.Show(text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var messageBoxIcon = MessageBoxIcon.Information;
            if (lowAddress < _eepromStartAddress || highAddress > _eepromStartAddress + _eepromSize - 1)
            {
                text += "Warning: file data will not be completely written to EEPROM\r\r";
                messageBoxIcon = MessageBoxIcon.Warning;
            }
            text += "Continue?";
            var dialogResult = MessageBox.Show(text, this.Text, MessageBoxButtons.OKCancel, messageBoxIcon, MessageBoxDefaultButton.Button1);
            if (dialogResult == DialogResult.Cancel)
                return;
            _writeData = new byte[_eepromSize];
            LoadData(memorySegments, _eepromStartAddress, _writeData);
            SetState(StateEnum.Writing);
            _actionThread = new Thread(new ThreadStart(WriteThread));
            _actionThread.Name = "WriteThread";
            _actionThread.IsBackground = true;
            _abort = false;
            _actionThread.Start();
        }

        private MemorySegments LoadMemorySegments(string filename)
        {
            try
            {
                return LoadFile(filename, _eepromStartAddress);
            }
            catch (Exception exception)
            {
                ShowException(exception);
                return null;
            }
        }

        private static void LoadData(MemorySegments memorySegments, int startAddress, byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0xff;
            }
            foreach (var memorySegment in memorySegments)
            {
                for (int i = 0; i < memorySegment.Data.Length; i++)
                {
                    int address = memorySegment.StartAddress + i - startAddress;
                    if (address >= 0 && address < buffer.Length)
                    {
                        buffer[address] = memorySegment.Data[i];
                    }
                }
            }
        }

        private (int lowAddress, int highAddress) GetAddressRange(MemorySegments memorySegments)
        {
            int lowAddress = 0x10000;
            int highAddress = 0x0000;
            foreach (var memorySegment in memorySegments)
            {
                if (memorySegment.StartAddress < lowAddress)
                    lowAddress = memorySegment.StartAddress;
                if (memorySegment.StartAddress + memorySegment.Data.Length - 1 > highAddress)
                    highAddress = memorySegment.StartAddress + memorySegment.Data.Length - 1;
            }
            return (lowAddress, highAddress);
        }

        private void WriteThread()
        {
            _serialPort.ReadExisting();
            try
            {
                int previousPercent = 0;
                var hexString = new StringBuilder();
                int address = 0;
                while (address < _writeData.Length)
                {
                    if (_abort)
                        break;
                    hexString.Clear();
                    int recordSize = Math.Min(MAX_RECORD_SIZE, _writeData.Length - address);
                    int sum = recordSize + (address >> 8) + (address & 0xff);
                    for (int i = 0; i < recordSize; i++)
                    {
                        sum += _writeData[address + i];
                        hexString.Append($"{_writeData[address + i]:X2}");
                    }
                    int checksum = (-sum) & 0xff;
                    string command = $"W{recordSize:X2}{address:X4}{hexString}{checksum:X2}";
                    Debug.WriteLine($"Tx: {command}");
                    _serialPort.Write(command);
                    _serialPort.ReadLine();
                    string response = _serialPort.ReadLine();
                    Debug.WriteLine($"Rx: {response}");
                    if (!response.Contains("OK"))
                        throw new Exception($"Invalid response \"{response}\"");
                    address += recordSize;
                    int donePercent = 100 * address / _writeData.Length;
                    if (donePercent != previousPercent)
                    {
                        previousPercent = donePercent;
                        BeginInvoke((MethodInvoker)delegate
                        {
                            toolStripProgressBar.Value = donePercent;
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    SetState(StateEnum.Ready);
                    ShowException(exception);
                    return;
                });
            }
            BeginInvoke((MethodInvoker)delegate
            {
                SetState(StateEnum.Ready);
            });
        }

        private void Read(string filename)
        {
            _readFilename = filename;
            _readData = new byte[_eepromSize];
            for (int i = 0; i < _eepromSize; i++)
            {
                _readData[i] = 0xff;
            }
            SetState(StateEnum.Reading);
            _actionThread = new Thread(new ThreadStart(ReadOrVerifyThread));
            _actionThread.Name = "ReadThread";
            _actionThread.IsBackground = true;
            _abort = false;
            _actionThread.Start();
        }

        private void Verify(string filename)
        {
            var memorySegments = LoadMemorySegments(filename);
            if (memorySegments == null)
                return;
            _verifyData = new byte[_eepromSize];
            LoadData(memorySegments, _eepromStartAddress, _verifyData);
            _readData = new byte[_eepromSize];
            SetState(StateEnum.Verifying);
            _actionThread = new Thread(new ThreadStart(ReadOrVerifyThread));
            _actionThread.Name = "VerifyThread";
            _actionThread.IsBackground = true;
            _abort = false;
            _actionThread.Start();
        }

        private void ReadOrVerifyThread()
        {
            _serialPort.ReadExisting();
            try
            {
                int previousPercent = 0;
                var hexString = new StringBuilder();
                int address = 0;
                while (address < _readData.Length)
                {
                    if (_abort)
                        break;
                    int recordSize = Math.Min(MAX_RECORD_SIZE, _readData.Length - address);
                    string command = $"R{recordSize:X2}{address:X4}";
                    Debug.WriteLine($"Tx: {command}");
                    _serialPort.Write(command);
                    string response = _serialPort.ReadLine();
                    Debug.WriteLine($"Rx: {response}");
                    if (response.Length != recordSize * 2 + 2)
                        throw new Exception($"Invalid response \"{response}\"");
                    int sum = recordSize + (address >> 8) + (address & 0xff);
                    for (int i = 0; i < recordSize; i++)
                    {
                        byte b = byte.Parse(response.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
                        _readData[address + i] = b;
                        sum += b;
                    }
                    int checksum1 = (-sum) & 0xff;
                    int checksum2 = int.Parse(response.Substring(recordSize * 2, 2), NumberStyles.AllowHexSpecifier);
                    if (checksum1 != checksum2)
                        throw new Exception($"Checksum mismatch, received {checksum2:X02} expected {checksum1:X02}");
                    address += recordSize;
                    int donePercent = 100 * address / _readData.Length;
                    if (donePercent != previousPercent)
                    {
                        previousPercent = donePercent;
                        BeginInvoke((MethodInvoker)delegate
                        {
                            toolStripProgressBar.Value = donePercent;
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    SetState(StateEnum.Ready);
                    ShowException(exception);
                    return;
                });
            }
            BeginInvoke((MethodInvoker)delegate
            {
                if (_abort)
                {
                    SetState(StateEnum.Ready);
                }
                else if (_state == StateEnum.Reading)
                {
                    SetState(StateEnum.Ready);
                    SaveFile();
                }
                else if (_state == StateEnum.Verifying)
                {
                    SetState(StateEnum.Ready);
                    int diffCount = 0;
                    for (int i = 0; i < _readData.Length; i++)
                    {
                        if (_readData[i] != _verifyData[i])
                            diffCount += 1;
                    }
                    MessageBox.Show($"Verify found {(diffCount == 0 ? "no" : diffCount.ToString())} differences.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }

        private void SaveFile()
        {
            try
            {
                switch (Path.GetExtension(_readFilename).ToLower())
                {
                    case ".hex":
                        SaveIntelFile(_readFilename, _eepromStartAddress, _readData);
                        break;
                    case ".s19":
                        SaveMotorolaFile(_readFilename, _eepromStartAddress, _readData);
                        break;
                    case ".bin":
                        SaveBinaryFile(_readFilename, _readData);
                        break;
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        private static void SaveIntelFile(string filename, int startAddress, byte[] data)
        {
            using (var streamWriter = new StreamWriter(filename, false))
            {
                var hexString = new StringBuilder();
                int address = 0;
                while (address < data.Length)
                {
                    int recordSize = Math.Min(32, data.Length - address);
                    int fileAddress = startAddress + address;
                    int sum = recordSize + (fileAddress >> 8) + (fileAddress & 0xff);
                    hexString.Clear();
                    for (int i = 0; i < recordSize; i++)
                    {
                        byte b = data[address + i];
                        sum += b;
                        hexString.Append($"{b:X2}");
                    }
                    int checksum = (-sum) & 0xff;
                    streamWriter.WriteLine($":{recordSize:X2}{fileAddress:X4}00{hexString}{checksum:X2}");
                    address += recordSize;
                }
                streamWriter.WriteLine(":00000001FF");
            }
        }

        private static void SaveMotorolaFile(string filename, int startAddress, byte[] data)
        {
            using (var streamWriter = new StreamWriter(filename, false))
            {
                var hexString = new StringBuilder();
                int address = 0;
                while (address < data.Length)
                {
                    int recordSize = Math.Min(32, data.Length - address);
                    int fileAddress = startAddress + address;
                    int sum = recordSize + 3 + (fileAddress >> 8) + (fileAddress & 0xff);
                    hexString.Clear();
                    for (int i = 0; i < recordSize; i++)
                    {
                        byte b = data[address + i];
                        sum += b;
                        hexString.Append($"{b:X2}");
                    }
                    int checksum = (~sum) & 0xff;
                    streamWriter.WriteLine($"S1{(recordSize + 3):X2}{fileAddress:X4}{hexString}{checksum:X2}");
                    address += recordSize;
                }
                streamWriter.WriteLine("S9030000FC");
            }
        }

        private static void SaveBinaryFile(string filename, byte[] data)
        {
            using (var fileStream = File.OpenWrite(filename))
            {
                fileStream.Write(data, 0, data.Length);
            }
        }

        private static MemorySegments LoadFile(string filename, int eepromStartAddress)
        {
            switch (Path.GetExtension(filename).ToLower())
            {
                case ".hex":
                    return LoadIntelFile(filename);
                case ".s19":
                    return LoadMotorolaFile(filename);
                case ".bin":
                    byte[] data = LoadBinaryFile(filename);
                    var addressForm = new AddressForm(eepromStartAddress, data.Length);
                    if (addressForm.ShowDialog() == DialogResult.OK)
                    {
                        var memorySegments = new MemorySegments();
                        memorySegments.Add(new MemorySegment(addressForm.StartAddress, data));
                        return memorySegments;
                    }
                    return null;
            }
            throw new Exception($"Unrecognized extension \"{Path.GetExtension(filename)}\" (must be .hex, .s19, or .bin)");
        }

        private static MemorySegments LoadIntelFile(string filename)
        {
            var memorySegments = new MemorySegments();
            int startAddress = -1;
            int currentAddress = -1;
            List<byte> data = null;
            using (var streamReader = new StreamReader(filename))
            {
                int lineNumber = 0;
                while (true)
                {
                    lineNumber += 1;
                    string line = streamReader.ReadLine();
                    if (line == null)
                        throw new Exception("Line {lineNumber}: end-of-file record expected");
                    int startChar = line.IndexOf(':');
                    if (startChar < 0)
                        continue;
                    if (startChar > 0)
                        line = line.Substring(startChar);
                    int byteCount = int.Parse(line.Substring(1, 2), NumberStyles.HexNumber);
                    if (line.Length < (byteCount + 5) * 2 + 1)
                        throw new Exception($"Line {lineNumber}: too few data bytes");
                    int sum = 0;
                    for (int i = 0; i < byteCount + 5; i++)
                    {
                        sum += byte.Parse(line.Substring(1 + i * 2, 2), NumberStyles.AllowHexSpecifier);
                    }
                    if ((sum & 0xff) != 0)
                        throw new Exception($"Line {lineNumber}: checksum does not match");
                    int address = int.Parse(line.Substring(3, 4), NumberStyles.AllowHexSpecifier);
                    int recType = int.Parse(line.Substring(7, 2), NumberStyles.AllowHexSpecifier);
                    if (recType == 1) // end-of-file
                    {
                        if (data != null)
                            memorySegments.Add(new MemorySegment(startAddress, data.ToArray()));
                        break;
                    }
                    else if (recType == 4) // extended linear address (ignore)
                        continue;
                    else if (recType == 0) // data
                    {
                        if (currentAddress != address)
                        {
                            if (data != null)
                                memorySegments.Add(new MemorySegment(startAddress, data.ToArray()));
                            data = new List<byte>();
                            startAddress = currentAddress = address;
                        }
                        for (int i = 0; i < byteCount; i++)
                        {
                            data.Add(byte.Parse(line.Substring(9 + i * 2, 2), NumberStyles.AllowHexSpecifier));
                        }
                        currentAddress += byteCount;
                    }
                    else
                        throw new Exception($"Line {lineNumber}: unhandled record type {recType}");
                }
            }
            return memorySegments;
        }

        static readonly Regex _motorolaRegex = new Regex("^S(?<type>[0-9])(?<length>[0-9A-F]{2,2})(?<address>[0-9A-F]{4,4})(?<data>([0-9A-F]{2,2})*)(?<checksum>[0-9A-F]{2,2})$");

        private static MemorySegments LoadMotorolaFile(string filename)
        {
            var memorySegments = new MemorySegments();
            int startAddress = -1;
            int currentAddress = -1;
            List<byte> data = null;
            using (var streamReader = new StreamReader(filename))
            {
                int lineNumber = 0;
                while (true)
                {
                    lineNumber += 1;
                    string line = streamReader.ReadLine();
                    if (line == null)
                        throw new Exception($"Line {lineNumber}: end-of-file record expected");
                    var match = _motorolaRegex.Match(line);
                    if (!match.Success)
                        throw new Exception($"Line {lineNumber}: invalid format");
                    int recordType = int.Parse(match.Result("${type}"));
                    int byteCount = int.Parse(match.Result("${length}"), NumberStyles.AllowHexSpecifier) - 3;
                    int address = int.Parse(match.Result("${address}"), NumberStyles.AllowHexSpecifier);
                    string dataString = match.Result("${data}");
                    if (byteCount != dataString.Length / 2)
                        throw new Exception($"Line {lineNumber}: length does not match");
                    var recordData = new byte[byteCount];
                    int sum = byteCount + 3 + (address >> 8) + (address & 0xff);
                    for (int i = 0; i < byteCount; i++)
                    {
                        recordData[i] = byte.Parse(dataString.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
                        sum += recordData[i];
                    }
                    byte checksum1 = (byte)((~sum) & 0xff);
                    byte checksum2 = byte.Parse(match.Result("${checksum}"), NumberStyles.AllowHexSpecifier);
                    if (checksum1 != checksum2)
                        throw new Exception($"Line {lineNumber}: checksum does not match");
                    if (recordType == 9) // end-of-file
                    {
                        if (data != null)
                            memorySegments.Add(new MemorySegment(startAddress, data.ToArray()));
                        break;
                    }
                    else if (recordType == 1) // data
                    {
                        if (currentAddress != address)
                        {
                            if (data != null)
                                memorySegments.Add(new MemorySegment(startAddress, data.ToArray()));
                            data = new List<byte>();
                            startAddress = currentAddress = address;
                        }
                        data.AddRange(recordData);
                        currentAddress += byteCount;
                    }
                }
            }
            return memorySegments;
        }

        private static byte[] LoadBinaryFile(string filename)
        {
            using (var fileStream = File.OpenRead(filename))
            {
                int length = (int)fileStream.Length;
                var data = new byte[length];
                fileStream.Read(data, 0, length);
                return data;
            }
        }

        public static void ShowException(Exception exception, string header = null)
        {
            if (MessageBox.Show(String.Format(
                "{0}\r\r{1}\r\r{2}\r\rCopy debug information to the clipboard?",
                header == null ? "The following exception occurred:" : header,
                exception.GetType().ToString(), exception.Message.Substring(0, Math.Min(exception.Message.Length, 500))),
                String.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
            {
                var s = new StringBuilder();
                s.Append(exception.ToString());
                if (exception.InnerException != null)
                {
                    s.Append("\r\n\r\nInner exception:\r\n\r\n");
                    s.Append(exception.InnerException.ToString());
                }
                Clipboard.SetText(s.ToString());
            }
        }

    }
}

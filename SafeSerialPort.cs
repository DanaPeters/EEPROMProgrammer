using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEPROMProgrammer
{
    // Avoid problem that prevents serial port from being re-opened
    // if USB-to-serial cable unplugged while port is open
    // http://stackoverflow.com/questions/13408476

    public class SafeSerialPort : SerialPort
    {
        private Stream _baseStream;

        public new void Open()
        {
            base.Open();
            _baseStream = BaseStream;
            GC.SuppressFinalize(BaseStream);
        }

        public new void Dispose()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (base.Container != null))
            {
                base.Container.Dispose();
            }
            try
            {
                if (_baseStream != null && _baseStream.CanRead)
                {
                    _baseStream.Close();
                    GC.ReRegisterForFinalize(_baseStream);
                }
            }
            catch
            {
                // ignore exception
            }
            base.Dispose(disposing);
        }
    }

}

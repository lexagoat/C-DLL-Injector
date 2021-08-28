using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace DLL_Injector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Process[] PC = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
            comboBox1.Items.Clear();
            foreach (Process p in PC)
            {
                comboBox1.Items.Add(p.ProcessName);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private static string DLLP { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = @"C:\";
                OFD.Title = "DLL Inject etmek için konum secin";
                OFD.DefaultExt = "dll";
                OFD.Filter = "DLL Files (*.dll)|*.dll";
                OFD.CheckFileExists = true;
                OFD.CheckPathExists = true;
                OFD.ShowDialog();

                textBox1.Text = OFD.FileName;
                DLLP = OFD.FileName;
            }
            catch (Exception ed)
            {
                MessageBox.Show(ed.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DLLP = textBox1.Text;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Process[] PC = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
            comboBox1.Items.Clear();
            foreach (Process p in PC)
            {
                comboBox1.Items.Add(p.ProcessName);
            }
        }
        static readonly IntPtr INTPTR_ZERO = (IntPtr)0;
        private static uint _procid;
        private static int _procId;

        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr GetProcAdress(IntPtr hModule, string lprocName);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpBaseAdress, byte[] Buffer, uint size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAdress, byte[] Buffer, uint size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAdress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        public static int Inject(string PN, string DLLP)

        {
            //1 = dosya bulunamadı
            
            if (!File.Exists(DLLP)) { return 1; }

            uint procId = 0;
            Process[] _procs = Process.GetProcesses();
            for (int i = 0; i < _procs.Length; i++)
            {
                if (_procs[i].ProcessName == PN)
                {
                    _procid = (uint)_procs[i].Id;
                }
            }

            if (_procId == 0) { return 2; }


            
        }

        public bool SI(uint P, string DDLP)
        {
            IntPtr hndProc = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, P);

                if (hndProc != INTPTR_ZERO)
            {
                IntPtr lpAdress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)DLLP.Length, 0x1000 | 0x2000, 0x40);

                if (lpAdress == INTPTR_ZERO)
                {
                    return false;
                }

                byte[] bytes = Encoding.ASCII.GetBytes(DLLP);

                if (WriteProcessMemory(hndProc, lpAdress, bytes, (uint)bytes.Length, 0) == 0)
                {
                    return false;
                }

                CloseHandle(hndProc);

                return true;
            }
            return false;
        }

        private IntPtr VirtualAllocEx(IntPtr hndProc, IntPtr ıntPtr, IntPtr length, int v1, int v2)
        {
            throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int Result = Inject(comboBox1.Text, DLLP);

            if (Result == 1)
            {
                MessageBox.Show("Dosya Bulunamadı");
            }
            else if (Result == 2)
            {
                MessageBox.Show("Process Does Not Exist");
            }
            else if (Result == 3)
            {
                MessageBox.Show("Inject Başarısız");
            }
            else if (Result == 4)
            {
                MessageBox.Show("Inject Basarili");
            }
        }
    }
}

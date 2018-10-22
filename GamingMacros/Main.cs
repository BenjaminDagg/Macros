using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using WindowsInput.Native;
using WindowsInput;
using System.Data.SQLite;

 enum ShowWindowEnum
{
    Hide = 0,
    ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
    Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
    Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
    Restore = 9, ShowDefault = 10, ForceMinimized = 11
};

public static class DataAccess
{
    public static void InitializeDataBase()
    {
        using (SQLiteConnection db = new SQLiteConnection("DataSource=macros.db"))
        {
            db.Open();

            String tableCommand = "CREATE TABLE IF NOT " +
                "EXISTS Macros (Primary_Key INTEGER PRIMARY KEY, " +
                "Text_Entry NVARCHAR(2048) NULL)";
            SQLiteCommand createTable = new SQLiteCommand(tableCommand, db);
            createTable.ExecuteReader();
        }
    }

    public static void AddData(string inputText)
    {
        using (SQLiteConnection db = new SQLiteConnection("DataSource=macros.db"))
        {
            db.Open();

            SQLiteCommand insertCommand = new SQLiteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO Macros VALUES (NULL, @Entry);";
            insertCommand.Parameters.AddWithValue("@Entry", inputText);

            insertCommand.ExecuteReader();
            db.Close();
        }
    }

    public static List<String> GetData()
    {
        List<String> entries = new List<string>();

        using (SQLiteConnection db =
            new SQLiteConnection("DataSource=macros.db"))
        {
            db.Open();

            SQLiteCommand selectCommand = new SQLiteCommand
                ("SELECT Text_Entry from Macros", db);

            SQLiteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                entries.Add(query.GetString(0));
            }

            db.Close();
        }

        return entries;
    }
}

namespace GamingMacros
{
    public partial class Main : Form

    {
        private volatile bool macroRunning;
        private Process targetProcess = null;
        private Thread workerThread;
        private InputSimulator sendInput = new InputSimulator();
        private List<Process> programs = new List<Process>();
       

        public Main()
        {
            //add event to listen for mouse clicks
            MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(MouseWasClicked);

            InitializeComponent();

            DataAccess.InitializeDataBase();

            DataAccess.AddData("test");
            
            
            this.macroRunning = false;
            workerThread = new Thread(this.AFKWorker);
            workerThread.Priority = ThreadPriority.Lowest;

            //get all open programs on computer
            foreach(Process p in Process.GetProcesses())
            {
                bool unique = true;
                for (int i = 0; i < this.programs.Count;i++)
                {
                    if (this.programs[i].ProcessName == p.ProcessName)
                    {
                        unique = false;
                    }
                }
                if (unique)
                {
                    this.programs.Add(p);
                }
            }
            this.populateProgramBox();
            
        }

        
        //get current window in focus
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        
        //gets Process object by name
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //gets status of a key 0 for on 1 for off
        [DllImport("user32.dll")]
        internal static extern short GetKeyState(int keyCode);

        //simulates key presses
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //sends message to specified window pointer to by intptr
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);


        private void populateProgramBox()
        {
            

            //sort alphabetically
            for (int i = 0; i < this.programs.Count - 1;i++)
            {
                int min = i;
                for (int j = i + 1; j < this.programs.Count;j++)
                {
                    if (string.Compare(this.programs[j].ProcessName,this.programs[i].ProcessName) < 0)
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    Process temp = this.programs[i];
                    this.programs[i] = this.programs[min];
                    this.programs[min] = temp;
                }
            }

            foreach (Process p in this.programs)
            {

                this.ProgramListBox.Items.Add(p.ProcessName);
            }
        }


        /*
         * Returns Process object of currently focused on process
         * If no process found return null else return the process obj
         * */
        private Process GetForegroundProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();

            // The foreground window can be NULL in certain circumstances, 
            // such as when a window is losing activation.
            if (hwnd == null)
                return null;

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);

            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.Id == pid)
                    return p;
            }
            
            return null;
        }

        /*
         * The Afk macro algorithm
         * */
        private void AFKWorker()
        {
            const int KEY_INTERVAL = 200; //time to hold each key
            const int KEY_REPS = 3; //number of times to press each key

            //w a s d keys
            VirtualKeyCode[] keys = {VirtualKeyCode.VK_W,
                                      VirtualKeyCode.VK_A,
                                      VirtualKeyCode.VK_S,
                                      VirtualKeyCode.VK_D,};
            //keeps track of what index keys is on
            int counter = 0;

            

            //loop w,a,s,d
            while (this.macroRunning == true)
            {
                //end loop if button pressed
                if (!this.macroRunning)
                {
                    return;
                }

                //press current keys index for KEY_REPS time
                for (int i = 0; i < KEY_REPS && this.macroRunning == true; i++)
                {
                    if (!this.macroRunning)
                    {
                        return;
                    }

                    //hold key down for given interval
                    this.sendInput.Keyboard.KeyDown(keys[counter]);
                    Thread.Sleep(KEY_INTERVAL);
                    this.sendInput.Keyboard.KeyUp(keys[counter]);


                }

                //go to next element or return back to first
                counter = (counter + 1) % (keys.Length);



            }




        }


        /*
         * SImulates key press of given key code value
         * */
        private void PressKeyboardButton(Keys keyCode)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            keybd_event((byte)keyCode, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((byte)keyCode, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }


        //event triggered when key pressed on form
        private void Main_KeyPress(object sender, KeyEventArgs e)
        {
            //if numlocked enabled disable it
            bool NumLock = (GetKeyState((int)Keys.NumLock)) == 0 ? false : true;

            if (NumLock)
            {
                Console.WriteLine("numlcoked");
                this.PressKeyboardButton(Keys.NumLock);
            }
        }

        private void Main_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            //this.label1.Text = e.KeyChar.ToString();
        }


        /*
         * Event triggered when start button is pressed. Toggles
         * the macro ON/OFF
         * */
        private void StartBtn_Click(object sender, EventArgs e)
        {
            //macro is on
            this.label1.Text = this.macroRunning.ToString();
            if (this.macroRunning == true)
            {
                this.macroRunning = false;
                this.startBtn.Text = "Start";
                //stop worker thread
                this.endMacro();
                
            }
            //macro off
            else
            {

                

                this.macroRunning = true;
                this.startBtn.Text = "Stop";
                
                //start worker thread
                this.startMacro();
            }
        }

        //starts or resumes the worker thread to start macro algorithm
        private void startMacro()
        {

            

            if (this.workerThread.ThreadState == System.Threading.ThreadState.Suspended)
            {
                this.workerThread.Resume();
            }
            else
            {
                this.workerThread.Start();
            }
        }

        //ends the worker thread execution
        private void endMacro()
        {
            
            this.workerThread = new Thread(this.AFKWorker);
        }

        
        /*
         * Event triggered on global mouse clcik
         * Detects which process is in focus and sets the targetProces to that
         * */
        private void MouseWasClicked(object sender, EventArgs e)
        {
            Process p = this.GetForegroundProcessName();
            this.targetProcess = p;

            

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void programEnterBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.ProgramListBox.SelectedIndex;
            string selectedItem = this.ProgramListBox.SelectedItem.ToString();

            if (selectedIndex < 0 || selectedIndex > this.programs.Count)
            {
                return;
            }
            this.label1.Text = "Selected  " + selectedItem;
            Process selectedProcess = null;

            foreach(Process p in this.programs)
            {
                if (p.ProcessName == selectedItem)
                {
                    selectedProcess = p;
                }
            }

            if (selectedProcess != null)
            {
                Console.WriteLine("not null " + selectedProcess.ProcessName);
                this.targetProcess = selectedProcess;

                //shift focus to selected program
                //ShowWindow(this.targetProcess.Handle, ShowWindowEnum.ShowNormal);
                if (this.targetProcess.MainWindowHandle == IntPtr.Zero)
                {
                    // the window is hidden so try to restore it before setting focus.
                    ShowWindow(this.targetProcess.Handle, ShowWindowEnum.Restore);
                }

                // set user the focus to the window
                SetForegroundWindow(this.targetProcess.MainWindowHandle);
            }
            


        }

        private void ProgramSearchBox_TextChanged(object sender, EventArgs e)
        {
            string val = this.ProgramSearchBox.Text;

            this.ProgramListBox.Items.Clear();

            for (int i = 0; i < this.programs.Count;i++)
            {
                if (this.programs[i].ProcessName.ToLower().Contains(val))
                {
                    this.ProgramListBox.Items.Add(this.programs[i].ProcessName);
                }
            }
        }

        private void ProgramSearchBox_Click(object sender, EventArgs e)
        {
            string value = this.ProgramSearchBox.Text;

            if (value == "Search")
            {
                this.ProgramSearchBox.Text = "";
            }
        }

        private void StartMacroButton_Click(object sender, EventArgs e)
        {
            this.macroKeys.Visible = !this.macroKeys.Visible;

            List<string> m = DataAccess.GetData();

            if (m.Count > 0)
            {
                this.label1.Text = m[0];
            }
            else
            {
                this.label1.Text = "no data found";
            }
        }
    }
}

public static class MouseHook
{
    public static event EventHandler MouseAction = delegate { };

    public static void Start()
    {
        _hookID = SetHook(_proc);


    }
    public static void stop()
    {
        UnhookWindowsHookEx(_hookID);
    }

    private static LowLevelMouseProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    private static IntPtr SetHook(LowLevelMouseProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_MOUSE_LL, proc,
              GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(
      int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
        {
            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            MouseAction(null, new EventArgs());
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private const int WH_MOUSE_LL = 14;

    private enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
      LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
      IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);


}

public class WindowsAPI
{
    /// <summary>
    /// 
    /// </summary>
    public const uint WM_KEYDOWN = 0x100;

    /// <summary>
    /// 
    /// </summary>
    public const uint WM_KEYUP = 0x101;

    /// <summary>
    /// 
    /// </summary>
    public const uint WM_LBUTTONDOWN = 0x201;

    /// <summary>
    /// 
    /// </summary>
    public const uint WM_LBUTTONUP = 0x202;

    public const uint WM_CHAR = 0x102;

    /// <summary>
    /// 
    /// </summary>
    public const int MK_LBUTTON = 0x01;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_RETURN = 0x0d;

    public const int VK_ESCAPE = 0x1b;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_TAB = 0x09;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_LEFT = 0x25;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_UP = 0x26;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_RIGHT = 0x27;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_DOWN = 0x28;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_F5 = 0x74;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_F6 = 0x75;

    /// <summary>
    /// 
    /// </summary>
    public const int VK_F7 = 0x76;

    /// <summary>
    /// The GetForegroundWindow function returns a handle to the foreground window.
    /// </summary>
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      [Out()] byte[] lpBuffer,
      int dwSize,
      out int lpNumberOfBytesRead
     );

    public static void SwitchWindow(IntPtr windowHandle)
    {
        if (GetForegroundWindow() == windowHandle)
            return;

        IntPtr foregroundWindowHandle = GetForegroundWindow();
        uint currentThreadId = GetCurrentThreadId();
        uint temp;
        uint foregroundThreadId = GetWindowThreadProcessId(foregroundWindowHandle, out temp);
        AttachThreadInput(currentThreadId, foregroundThreadId, true);
        SetForegroundWindow(windowHandle);
        AttachThreadInput(currentThreadId, foregroundThreadId, false);

        while (GetForegroundWindow() != windowHandle)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hwndParent"></param>
    /// <param name="hwndChildAfter"></param>
    /// <param name="lpszClass"></param>
    /// <param name="lpszWindow"></param>
    /// <returns></returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="msg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
    public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern byte VkKeyScan(char ch);

    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, uint uMapType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IntPtr FindWindow(string name)
    {
        Process[] procs = Process.GetProcesses();

        foreach (Process proc in procs)
        {
            if (proc.MainWindowTitle == name)
            {
                return proc.MainWindowHandle;
            }
        }

        return IntPtr.Zero;
    }

    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="low"></param>
    /// <param name="high"></param>
    /// <returns></returns>
    public static int MakeLong(int low, int high)
    {
        return (high << 16) | (low & 0xffff);
    }

    [DllImport("User32.dll")]
    public static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);

    [DllImport("user32.dll")]
    public static extern IntPtr GetMessageExtraInfo();

    public const int INPUT_MOUSE = 0;
    public const int INPUT_KEYBOARD = 1;
    public const int INPUT_HARDWARE = 2;
    public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    public const uint KEYEVENTF_KEYUP = 0x0002;
    public const uint KEYEVENTF_UNICODE = 0x0004;
    public const uint KEYEVENTF_SCANCODE = 0x0008;
    public const uint XBUTTON1 = 0x0001;
    public const uint XBUTTON2 = 0x0002;
    public const uint MOUSEEVENTF_MOVE = 0x0001;
    public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    public const uint MOUSEEVENTF_LEFTUP = 0x0004;
    public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
    public const uint MOUSEEVENTF_XDOWN = 0x0080;
    public const uint MOUSEEVENTF_XUP = 0x0100;
    public const uint MOUSEEVENTF_WHEEL = 0x0800;
    public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
    public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
}

[StructLayout(LayoutKind.Sequential)]
public struct MOUSEINPUT
{
    int dx;
    int dy;
    uint mouseData;
    uint dwFlags;
    uint time;
    IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct KEYBDINPUT
{
    public ushort wVk;
    public ushort wScan;
    public uint dwFlags;
    public uint time;
    public IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct HARDWAREINPUT
{
    uint uMsg;
    ushort wParamL;
    ushort wParamH;
}

[StructLayout(LayoutKind.Explicit)]
public struct INPUT
{
    [FieldOffset(0)]
    public int type;
    [FieldOffset(4)] //*
    public MOUSEINPUT mi;
    [FieldOffset(4)] //*
    public KEYBDINPUT ki;
    [FieldOffset(4)] //*
    public HARDWAREINPUT hi;
}



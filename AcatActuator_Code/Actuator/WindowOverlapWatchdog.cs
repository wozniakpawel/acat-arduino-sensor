///////////////////////////////////////////////////////////////////////////
// <copyright file="WindowOverlapWatchdog.cs" company="Intel Corporation">
//
// Copyright (c) 2019 Intel Corporation 
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

/// <summary>
/// FILE FROM ACAT SOURCE CODE
/// MODIFICATION - If one of ACAT's windows is overlapping 
/// then don't try to grab focus from it
/// ACAT always has priority
/// </summary>
namespace ACAT_Arduino_Windows_App
{
    /// <summary>
    /// Keeps watch if the form is getting obscured by other windows and if so,
    /// brings it back on top. Based on a watchdog timer
    /// </summary>
    public class WindowOverlapWatchdog : IDisposable
    {
        protected Timer _timer;
        protected Form _window;
        protected bool _isPaused = false;
        protected bool _force;

        /// <summary>
        /// How often to check
        /// </summary>
        protected int _interval = 1500;

        /// <summary>
        /// WindowOverlapWatchdog constructor
        /// </summary>
        /// <param name="window">window to watch and make sure nothing overlaps it</param>
        /// <param name="force">force</param>
        /// <param name="timerInterval">timer interval to check for overlap</param>
        public WindowOverlapWatchdog(Form window, bool force = false, int timerInterval = 1500)
        {
            _force = force;
            _interval = timerInterval;
            _window = window;
            _timer = new Timer { Enabled = true, Interval = _interval };
            _timer.Tick += new EventHandler(timer_Tick);
            window.VisibleChanged += new EventHandler(window_VisibleChanged);
        }

        /// <summary>
        /// Start / stop watchdog timer
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        void window_VisibleChanged(object sender, EventArgs e)
        {
            if (_window == null || !_window.IsHandleCreated)
                return;

            if (!_isPaused)
            {
                startTimer();
            }
            else
            {
                stopTimer();
            }
        }

        /// <summary>
        /// Pause the timer (don't check for now)
        /// </summary>
        public void Pause()
        {
            stopTimer();
            _isPaused = true;
        }

        /// <summary>
        /// Resume the timer / checking for overlap
        /// </summary>
        public void Resume()
        {
            _isPaused = false;
            startTimer();
        }

        /// <summary>
        /// Stop watchdog timer
        /// </summary>
        void stopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// Start watchdog timer
        /// </summary>
        void startTimer()
        {
            if (_timer != null)
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// Stops the watchdog timer
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            _window.VisibleChanged -= new EventHandler(window_VisibleChanged);
            _window = null;
        }

        /// <summary>
        /// Checks if any part of the window is obscured and if so,
        /// brings it back on top
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event args</param>
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (isObscuredWindow())
                {
                    _window.Invoke(new MethodInvoker(delegate ()
                    {
                        _window.TopMost = false;
                        _window.TopMost = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("exception occured!  ex=" + ex.ToString());
            }
        }

        /// <summary>
        /// Checks if any part of the specified window is obscured by
        /// another window
        /// </summary>
        /// <param name="windowHandle">Window to check for</param>
        /// <returns>true if it is</returns>
        public bool isObscuredWindow()
        {
            if (_window == null || _window.Handle == IntPtr.Zero)
            {
                return false;
            }

            IntPtr windowHandle = _window.Handle;

            // to keep track of whether we have already visited this window
            var cache = new HashSet<IntPtr> { windowHandle };

            RECT windowRect;
            GetWindowRect(windowHandle, out windowRect);

            // now, step through all active windows and check for overlap
            while ((windowHandle = GetWindow(windowHandle, GW_HWNDPREV)) != IntPtr.Zero &&
                   !cache.Contains(windowHandle))
            {
                RECT rect;
                RECT intersection;

                cache.Add(windowHandle);

                bool isScanner = false;
                if (!_force && IsWindowVisible(windowHandle))
                {
                    try
                    {
                        Control ctl = Form.FromHandle(windowHandle);
                        isScanner = (ctl is Form);
                    }
                    catch (Exception ex)
                    {
                        isScanner = false;
                        System.Diagnostics.Trace.WriteLine("isScanner logic exception " + ex.ToString());
                    }
                }

                // MODIFICATION - If ACAT process, don't consider it if overlapping
                Process process = GetProcessForWindow(windowHandle);
                bool isAcatProcess = process.ProcessName.Contains("ACAT");
                if (isAcatProcess)
                {
                    continue;
                }
                    
                if (IsWindowVisible(windowHandle) &&
                    !IsMinimized(windowHandle) &&
                    !isScanner &&
                    GetWindowRect(windowHandle, out rect) &&
                    IntersectRect(out intersection, ref windowRect, ref rect))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Stuff to interact with different windows
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, [Out] out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IntersectRect([Out] out RECT lprcDst, [In] ref RECT lprcSrc1, [In] ref RECT lprcSrc2);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImportAttribute("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        static extern int GetWindowThreadProcessId([InAttribute()] IntPtr hWnd, out int lpdwProcessId);

        public const int GW_HWNDPREV = 3;
        const int GWL_STYLE = (-16);
        public const UInt32 WS_MINIMIZE = 0x20000000;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// Checks if window is minimzied or not
        /// </summary>
        /// <param name="handle">window to check if minimized or not</param>
        /// <returns></returns>
        static public bool IsMinimized(IntPtr handle)
        {
            int style = GetWindowLong(handle, GWL_STYLE);
            return ((style & WS_MINIMIZE) == WS_MINIMIZE);
        }

        /// <summary>
        /// Get process of window
        /// </summary>
        /// <param name="hwnd">window to get process for</param>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        static public Process GetProcessForWindow(IntPtr hwnd)
        {
            int pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return Process.GetProcessById(pid);
        }

    }
}

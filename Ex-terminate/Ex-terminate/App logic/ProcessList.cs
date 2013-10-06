using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_terminate.App_logic
{
    class ProcessList
    {

        Process[] procList;
        List<String> procNames = new List<String>();
        Process selProc;
        public List<Process> ProcList { get { update(); return procList.ToList(); } }
        public List<String> ProcNames { get { update(); return procNames; } }
        public Process SelectedProcess { get { return selProc; } }



        public ProcessList()
        {
            update();
        }

        public void setSelProc(int index)
        {
            selProc = procList[index];
        }

        public void update()
        {
            procList = Process.GetProcesses();
            procNames.Clear();
            foreach (Process p in procList)
            {
                procNames.Add(StringHandling.UppercaseFirst(p.ProcessName) + "  id: " + p.Id);
            }
        }

        public void printListToConsole()
        {
            foreach (Process p in procList)
            {
                Console.WriteLine("Process: {0} ID: {1}", p.ProcessName, p.Id);
            }
        }

        public void killProcess(Process proc)
        {
            if (proc == selProc)
            {
                selProc = null;
            }
            // Kill the process!
            proc.Kill();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LYForms.ViewModel
{
    public class MulitThreadManager : SingletonBase<MulitThreadManager>
    {
        /// <summary>
        /// 最大线程数量
        /// </summary>
        public int MaxThread { get; set; }

        /// <summary>
        /// 每个线程最大任务数
        /// </summary>
        public int MaxTaskCountPerThread { get; set; }

        /// <summary>
        /// 工作线程数组
        /// </summary>
        private WorkThread[] workThreads = new WorkThread[0];

        public Action<int> DoWork
        {
            private get;
            set;
        }
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <returns>启动线程是否成功</returns>
        public bool Start()
        {
            //结束上一次的线程

            workThreads = new WorkThread[MaxThread];
            for (int i = 0; i < MaxThread; i++)
            {
                WorkThread newThread = new WorkThread()
                {
                    ThreadID = i + 1,                      //线程编号,用于线程自己推导起始位置
                    TaskCount = MaxTaskCountPerThread,
                    DoWork = this.DoWork
                };
                workThreads[i] = newThread;
                newThread.Start();
                //System.Threading.Thread.Sleep(100);
            }
            return true;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="SleepTime">线程等待时间：单位ms</param>
        /// <returns>启动线程是否成功</returns>
        public bool Start(int SleepTime)
        {
            //结束上一次的线程

            workThreads = new WorkThread[MaxThread];
            for (int i = 0; i < MaxThread; i++)
            {
                WorkThread newThread = new WorkThread()
                {
                    ThreadID = i + 1,                      //线程编号,用于线程自己推导起始位置
                    TaskCount = MaxTaskCountPerThread,
                    DoWork = this.DoWork
                };
                workThreads[i] = newThread;
                newThread.Start();
                System.Threading.Thread.Sleep(SleepTime);
            }
            return true;
        }

        /// <summary>
        /// 停止所有工作线程
        /// </summary>
        public void Stop()
        {
            //首先发出停止指令
            foreach (WorkThread workthread in workThreads)
            {
                workthread.Stop();
            }
            //等待所有工作线程都完成
            bool isAllThreaWorkDone = false;
            while (!isAllThreaWorkDone)
            {
                isAllThreaWorkDone = IsWorkDone();
            }

        }

        /// <summary>
        /// 等待所有线程工作完成
        /// </summary>
        public bool IsWorkDone()
        {
            bool isAllThreaWorkDone = true;

            foreach (WorkThread workthread in workThreads)
            {
                if (workthread == null)
                    continue;
                isAllThreaWorkDone = workthread.IsWorkFinish();
                if (!isAllThreaWorkDone) break;
            }
            if (isAllThreaWorkDone)
            {
            }
            return isAllThreaWorkDone;
        }


  
    }

    class WorkThread
    {
        Thread workThread = null;

        /// <summary>
        /// 运行标志
        /// </summary>
        private bool runFlag = false;

        /// <summary>
        /// 工作完成标志
        /// </summary>
        private bool workOverFlag = false;

        /// <summary>
        /// 线程编号
        /// </summary>
        public int ThreadID { get; set; }
        /// <summary>
        /// 任务数量
        /// </summary>
        public int TaskCount { get; set; }

        public Action<int> DoWork { get; set; }

        /// <summary>
        /// 停止当前工作任务
        /// </summary>
        public void Stop()
        {
            runFlag = true;
        }

        /// <summary>
        /// 工作线程是否完成
        /// </summary>
        /// <returns></returns>
        public bool IsWorkFinish()
        {
            return workOverFlag;
        }

        /// <summary>
        /// 启动工作线程
        /// </summary>
        /// <param name="paras"></param>
        public void Start()
        {
            workThread = new Thread(StartWork);
            workThread.Start();
        }

        private void StartWork()
        {
            //初始化标志
            runFlag = true;
            workOverFlag = false;
            //计算负载
            int startpos = (ThreadID - 1) * TaskCount;
            int endpos = startpos + TaskCount;
            //调用方法
            try
            {
                //bool[] isOpen = new bool[Adapter.Instance.BwCount];
                for (int i = startpos; i < endpos; i++)
                {
                    //if (Helper.MeterDataHelper.Instance.Meter(i) != null)
                    //{
                    if (DoWork != null)
                    {
                        DateTime startTime = DateTime.Now;
                        bool openRet = true;
                        if (openRet)
                        {
                            //Comm.MessageController.Instance.AddMessage(String.Format("开始进行第{0}项工作任务", i + 1));
                            DoWork(i);
                            //Comm.MessageController.Instance.AddMessage(String.Format("已经完成第{0}项工作任务", i + 1));
                        }
                        else
                        {
                            ;
                        }

                        TimeSpan ts = DateTime.Now - startTime;
                        double rettime = ts.TotalMilliseconds;
                        Console.WriteLine("单次工作使用时间{0}ms", rettime);
                        //}
                    }
                    if (!runFlag)
                        break;
                }
            }
            catch { }
            finally
            {
                //恢复标志
                runFlag = false;
                workOverFlag = true;
            }
        }


    }

    public class SingletonBase<T>
    where T : new()
    {

        private static T instance;
        private static object syncRoot = new Object();
        /// <summary>
        /// 获取对象的单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new T();
                    }
                }
                return instance;
            }
        }


    }
}

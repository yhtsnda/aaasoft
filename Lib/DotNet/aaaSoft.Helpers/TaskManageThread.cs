using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace aaaSoft.Helpers
{
    public class TaskManagerThread<TaskType,TaskResultType>
    {
        private Thread trdManageTask = null;
        //任务处理器
        private ITaskHandller<TaskType, TaskResultType> taskHandller = null;
        //任务列表
        private Queue<TaskType> taskQueue = new Queue<TaskType>();
        //处理任务线程列表
        private List<Thread> handleTaskThreadList = new List<Thread>();
        // 正在工作的线程列表(即非空转的线程列表)
        private List<Thread> workingThreadList = new List<Thread>();
        // 期望线程数
        private Int32 expectThreadCount = 1;

        /// <summary>
        /// 任务队列是否已完成
        /// </summary>
        public Boolean IsTaskQueueCompleted = true;
        /// <summary>
        /// 最小线程数量
        /// </summary>
        public Int32 MinThreadCount = 1;
        /// <summary>
        /// 最大线程数量
        /// </summary>
        public Int32 MaxThreadCount = 10;
        /// <summary>
        /// 任务数阈值(当任务数达到此阈值时，线程数达到最大值)
        /// </summary>
        public Int32 TaskCountThreshold = 100;
        /// <summary>
        /// 是否自动控制线程数量
        /// </summary>
        public Boolean IsAutoControlThreadCount = true;

        /// <summary>
        /// 任务队列改变时
        /// </summary>
        public event EventHandler TaskQueueChanged;
        /// <summary>
        /// 任务队列完成时
        /// </summary>
        public event EventHandler TaskQueueCompleted;
        /// <summary>
        /// 任务执行完成时
        /// </summary>
        public event EventHandler<TaskCompletedEventArgs> TaskCompleted;
        
        /// <summary>
        /// 任务执行完成事件参数
        /// </summary>
        public class TaskCompletedEventArgs : EventArgs
        {
            public TaskType Task;
            public TaskResultType TaskResult;
        }

        public TaskManagerThread(ITaskHandller<TaskType, TaskResultType> taskHandller)
        {
            this.taskHandller = taskHandller;
            this.TaskQueueChanged += new EventHandler(TaskManagerThread_TaskQueueChanged);
            this.TaskCompleted += new EventHandler<TaskCompletedEventArgs>(TaskManagerThread_TaskCompleted);
        }

        //任务完成时，判断队列是否完成
        void TaskManagerThread_TaskCompleted(object sender, TaskManagerThread<TaskType, TaskResultType>.TaskCompletedEventArgs e)
        {
            Int32 taskQueueCount = GetTaskQueueCount();
            Int32 currentWorkingThreadCount = GetCurrentWorkingThreadCount();

            if (taskQueueCount == 0)
            {
                Console.WriteLine("待处理任务数为0，当前工作线程数量：" + currentWorkingThreadCount);
            }


            //如果当前任务队列中任务数量为0，并且当前正在工作的线程数量也为0，则说明任务队列已完成
            if (taskQueueCount == 0 && currentWorkingThreadCount == 0)
            {
                IsTaskQueueCompleted = true;
                if (TaskQueueCompleted != null)
                    TaskQueueCompleted.Invoke(this, e);                
            }
        }

        //任务队列改变时，自动调整期望线程数
        private void TaskManagerThread_TaskQueueChanged(object sender, EventArgs e)
        {
            if (!IsAutoControlThreadCount)
                return;

            Int32 queueCount = GetTaskQueueCount();
            if (queueCount > TaskCountThreshold)
            {
                expectThreadCount = MaxThreadCount;
            }
            else
            {
                expectThreadCount = (MaxThreadCount - MinThreadCount) * queueCount / TaskCountThreshold + MinThreadCount;
            }
        }

        /// <summary>
        /// 任务处理器接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ITaskHandller<TaskType, TaskResultType>
        {
            TaskResultType Handle(TaskType Task, TaskManagerThread<TaskType, TaskResultType> TaskManageThread);
        }

        public void Start()
        {
            trdManageTask = new Thread(taskManageThreadFunction);
            trdManageTask.Start();
        }

        public void Interrupt()
        {
            List<Thread> tmpList = new List<Thread>();
            tmpList.AddRange(handleTaskThreadList);
            foreach (Thread thread in tmpList)
            {
                thread.Interrupt();
            }
            trdManageTask.Interrupt();
        }

        /// <summary>
        /// 入队列
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue(TaskType t)
        {
            lock (taskQueue)
            {
                taskQueue.Enqueue(t);
                if (TaskQueueChanged != null)
                    TaskQueueChanged.Invoke(this, null);
            }
            IsTaskQueueCompleted = false;
        }

        /// <summary>
        /// 出队列
        /// </summary>
        /// <returns></returns>
        public TaskType Dequeue()
        {
            lock (taskQueue)
            {
                if (taskQueue.Count == 0)
                    return default(TaskType);

                TaskType t = taskQueue.Dequeue();
                if (t != null && TaskQueueChanged != null)
                    TaskQueueChanged.Invoke(this, null);
                return t;
            }
        }

        /// <summary>
        /// 得到任务队列中对象的数量
        /// </summary>
        /// <returns></returns>
        public Int32 GetTaskQueueCount()
        {
            lock (taskQueue)
            {
                return taskQueue.Count;
            }
        }

        /// <summary>
        /// 得到当前线程数量
        /// </summary>
        /// <returns></returns>
        public Int32 GetCurrentThreadCount()
        {
            return handleTaskThreadList.Count;
        }

        /// <summary>
        /// 得到当前正在工作的线程数量
        /// </summary>
        /// <returns></returns>
        public Int32 GetCurrentWorkingThreadCount()
        {
            return workingThreadList.Count;
        }

        //任务管理线程方法
        private void taskManageThreadFunction()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(100);
                    Int32 currentThreadCount = GetCurrentThreadCount();
                    
                    //如果当前线程数小于期望线程数
                    if (currentThreadCount < expectThreadCount)
                    {
                        Thread newThread = new Thread(handleTaskThreadFunction);
                        newThread.Start();
                        handleTaskThreadList.Add(newThread);
                    }
                }
            }
            catch (ThreadInterruptedException ex)
            {

            }
            catch (Exception ex)
            {
                Debug.Print("CheckSiteThread线程出错：" + ex);
            }
        }

        //处理任务线程方法
        public void handleTaskThreadFunction()
        {
            Thread currentThread = Thread.CurrentThread;
            try
            {
                while (true)
                {
                    Thread.Sleep(100);

                    lock (handleTaskThreadList)
                    {
                        Int32 currentThreadCount = GetCurrentThreadCount();
                        //如果当前线程数大于期望线程数
                        if (currentThreadCount > expectThreadCount)
                            break;
                    }

                    //取出任务
                    TaskType task = this.Dequeue();
                    if (task == null) continue;

                    workingThreadList.Add(currentThread);

                    //执行任务
                    TaskResultType taskResult = taskHandller.Handle(task, this);

                    lock (workingThreadList)
                    {
                        if (workingThreadList.Contains(currentThread))
                            workingThreadList.Remove(currentThread);
                    }

                    //触发任务执行完成事件
                    if (TaskCompleted != null)
                        TaskCompleted(this, new TaskCompletedEventArgs() { Task = task, TaskResult = taskResult });
                }
            }
            catch (ThreadInterruptedException ex)
            {
                //当前线程被其他线程中断
            }
            catch (Exception ex)
            {
                Debug.Print("任务管理线程出错：" + ex);
            }
            finally
            {
                lock (handleTaskThreadList)
                {
                    if (handleTaskThreadList.Contains(currentThread))
                        handleTaskThreadList.Remove(currentThread);
                }
                lock (workingThreadList)
                {
                    if (workingThreadList.Contains(currentThread))
                        workingThreadList.Remove(currentThread);
                }
            }
        }
    }
}

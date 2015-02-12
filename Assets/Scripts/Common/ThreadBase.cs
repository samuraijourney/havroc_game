using UnityEngine;
using System.Collections;

public class ThreadBase 
{
    private bool m_isDone = false;
    private object m_handle = new object();
    private System.Threading.Thread m_thread = null;

    public delegate void ThreadFunction();

    public virtual void Start(ThreadFunction threadFunction)
    {
        m_thread = new System.Threading.Thread(() => Run(threadFunction));
        m_thread.Start();
    }

    public virtual void Abort()
    {
        m_thread.Abort();
    }

    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (m_handle)
            {
                tmp = m_isDone;
            }
            return tmp;
        }
        set
        {
            lock (m_handle)
            {
                m_isDone = value;
            }
        }
    }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }

    private void Run(ThreadFunction threadFunction)
    {
        threadFunction();
        IsDone = true;
    }

    protected virtual void OnFinished() { }
}

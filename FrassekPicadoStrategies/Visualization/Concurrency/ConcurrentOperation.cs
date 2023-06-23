using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Visualization.Concurrency
{
    internal class ConcurrentOperation 
    {
        private ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private HashSet<Action> _alreadyExecutingActions = new HashSet<Action>();

        private int _releaseCounts = 0;
        private Semaphore _queueSemaphore = new(0, 1);

        private Action _whenDone;
        public ConcurrentOperation()
        {
            Thread backgroundThread = new Thread(() =>
            {
                while (true)
                {
                    _queueSemaphore.WaitOne();
                    while (_queue.TryDequeue(out Action action))
                    {
                        action();
                        if (_alreadyExecutingActions.Contains(action))
                            _alreadyExecutingActions.Remove(action);
                    }
                    _whenDone?.Invoke();
                    _releaseCounts = 0;
                }
            });
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        public void Add(Action action)
        {
            if (!_alreadyExecutingActions.Contains(action))
            {
                _queue.Enqueue(action);
                _alreadyExecutingActions.Add(action);
            }
            if (_releaseCounts == 0)
            {
                _releaseCounts++;
                _queueSemaphore.Release();
            }
        }

        public void WhenDone(Action action)
        {
            _whenDone = action;
        }
    }
}

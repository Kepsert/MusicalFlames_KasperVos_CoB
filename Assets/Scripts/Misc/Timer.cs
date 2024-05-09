using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class Timer : MonoBehaviour
    {
        public static Timer Instance;

        private List<TimerModel> _timers = new();
        private List<TimerModel> _unscaledTimers = new();
        List<int> _testList = new List<int>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        public void RemoveTimer(Guid guid)
        {
            _timers.RemoveAll(t => t.Guid == guid);
        }

        public void RemoveUnscaledTimer(Guid guid)
        {
            _unscaledTimers.RemoveAll(t => t.Guid == guid);
        }

        public Guid AddTimer(float time, Action onComplete)
        {
            var guid = Guid.NewGuid();

            _timers.Add(new TimerModel()
            {
                Time = time,
                OnComplete = onComplete,
                Guid = guid
            });



            return guid;
        }

        public Guid AddUnscaledTimer(float time, Action onComplete)
        {
            var guid = Guid.NewGuid();
            _unscaledTimers.Add(new TimerModel()
            {
                Time = time,
                OnComplete = onComplete,
                Guid = guid
            });

            return guid;
        }

        private void Update()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].ElapsedTime += Time.deltaTime;
                if (_timers[i].ElapsedTime >= _timers[i].Time)
                {
                    _timers[i].IsDone = true;
                    _timers[i].OnComplete();
                }
            }

            for (int i = 0; i < _unscaledTimers.Count; i++)
            {
                _unscaledTimers[i].ElapsedTime += Time.unscaledDeltaTime;
                if (_unscaledTimers[i].ElapsedTime >= _unscaledTimers[i].Time)
                {
                    _unscaledTimers[i].IsDone = true;
                    _unscaledTimers[i].OnComplete();
                }
            }

            _timers.RemoveAll(t => t.IsDone);
            _unscaledTimers.RemoveAll(t => t.IsDone);
        }

        public void ClearTimers()
        {
            _timers.Clear();
        }

        public void ClearUnscaledTimers()
        {
            _unscaledTimers.Clear();
        }

        public class TimerModel
        {
            public float Time { get; set; }
            public float ElapsedTime { get; set; }
            public Action OnComplete { get; set; }
            public bool IsDone { get; set; }
            public Guid Guid { get; set; }
        }
    }
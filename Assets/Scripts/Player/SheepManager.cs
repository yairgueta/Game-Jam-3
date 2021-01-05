using System.Collections.Generic;
using Cycles;

namespace Player
{
    public class SheepManager : Singleton<SheepManager>
    {
        internal List<Sheep> sheeps;

        protected override void Awake()
        {
            base.Awake();
            sheeps = new List<Sheep>();
        }

        private void Start()
        {
            CyclesManager.Instance.onNightTimeEnter.AddListener(() => sheeps.ForEach(s => s.Sleep()));
            CyclesManager.Instance.onNightTimeExit.AddListener(()=> sheeps.ForEach(s => s.WakeUp()));
        }
    }
}
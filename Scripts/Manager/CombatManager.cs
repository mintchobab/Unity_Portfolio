using System;

namespace lsy
{
    public class CombatManager : IManager
    {
        public bool IsCombating { get; private set; }

        public event Action onStartedCombat;
        public event Action onEndedCombat;


        public void Init()
        {

        }


        public void StartCombat()
        {
            IsCombating = true;
            onStartedCombat?.Invoke();
        }


        public void EndCombat()
        {
            IsCombating = false;
            onEndedCombat?.Invoke();
        }
    }
}

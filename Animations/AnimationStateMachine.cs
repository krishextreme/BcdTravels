using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ledger.Animations
{
    public class AnimationStateMachine : IAsyncStateMachine
    {
        public int State;
        public AsyncVoidMethodBuilder Builder;
        public OpacityAnimator Instance;

        public void MoveNext()
        {
            try
            {
                if (State == -1)
                {
                    // Start the animation
                    Instance.AnimateOpacityAsync();
                }
            }
            catch (Exception ex)
            {
                Builder.SetException(ex);
                return;
            }

            Builder.SetResult();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Builder.SetStateMachine(stateMachine);
        }
    }
}

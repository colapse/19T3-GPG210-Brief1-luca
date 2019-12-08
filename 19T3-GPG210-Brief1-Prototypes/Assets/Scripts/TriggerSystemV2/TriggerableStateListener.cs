namespace TriggerSystemV2
{
    public interface ITriggerableStateListener
    {
        void HandleTriggerableStateChanged(Triggerable triggerable, bool state);
    }
}
using System;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util
{
    public abstract class ShouldSaveCondition
    {
        public abstract bool ShouldSave(Type __instance);
    }

    public class DefaultShouldSaveConditon : ShouldSaveCondition
    {
        public override bool ShouldSave(Type __instance)
        {
            return true;
        }
    }

    public class PruneWorkOrderSaveConditon : ShouldSaveCondition
    {
        public override bool ShouldSave(Type __instance)
        {
            return false;
        }
    }


}
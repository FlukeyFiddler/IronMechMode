using System;

namespace nl.flukeyfiddler.bt.IronMechMode.Util
{
    public abstract class ShouldSaveCondition
    {
        public abstract bool ShouldSave(object __instance);
    }

    public class DefaultShouldSaveConditon : ShouldSaveCondition
    {
        public override bool ShouldSave(object __instance)
        {
            Logger.Line("in shouldsave for: " + __instance.GetType());
            return true;
        }
    }

    public class PruneWorkOrderSaveConditon : ShouldSaveCondition
    {
        public override bool ShouldSave(object __instance)
        {
            Logger.Line("in shouldsave for: " + __instance.GetType());
            return false;
        }
    }


}
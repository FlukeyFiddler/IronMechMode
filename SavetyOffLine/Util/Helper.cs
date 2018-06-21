using Harmony;
using System;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util
{
    public static class Helper
    {
        public static bool IsIronManCampaign<Type>(Type instanceHavingIronmanProperty)
        {
            var ironManProperty = Traverse.Create(instanceHavingIronmanProperty).Property("IsIronmanCampaign");

            return ironManProperty.FieldExists() && ironManProperty.GetValue<bool>();
        }
    }
}

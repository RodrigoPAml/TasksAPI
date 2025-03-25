using System.ComponentModel;

namespace Domain.Enums
{
    /// <summary>
    /// Profile type enum
    /// </summary>
    public enum ProfileTypeEnum
    {
        [Description("User")]
        User,

        [Description("Admin")]
        Admin
    }
}

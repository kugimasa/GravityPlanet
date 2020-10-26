using System;

namespace ItemSystem
{
    public interface IItem
    {
        string ItemID { get; }
        void Get();
        /// <remarks>引数にはItemのID入る</remarks>
        event EventHandler<string> OnGet;
    }
}

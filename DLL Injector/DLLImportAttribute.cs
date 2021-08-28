using System;

namespace DLL_Injector
{
    internal class DLLImportAttribute : Attribute
    {
        private string v;

        public DLLImportAttribute(string v, bool SetLastError)
        {
            this.v = v;
            this.SetLastError = SetLastError;
        }

        public bool SetLastError { get; set; }
    }
}
namespace AzureImageToolsTest.Domain
{
    public struct DiskFileStoreRoot
    {
        private readonly string _value;

        public DiskFileStoreRoot(string value) 
        {
            this._value = value;
        }

        public static implicit operator string(DiskFileStoreRoot diskFileStoreRoot)
        {
            return diskFileStoreRoot._value;
        }
    }
}
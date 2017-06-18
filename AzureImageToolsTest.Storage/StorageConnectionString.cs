namespace AzureImageToolsTest.Storage
{
    public struct StorageConnectionString
    {
        private readonly string _value;

        public StorageConnectionString(string value) 
        {
            this._value = value;
        }

        public static implicit operator string(StorageConnectionString diskFileStoreRoot)
        {
            return diskFileStoreRoot._value;
        }
    }
}
namespace AzureImageToolsTest.CognitiveServices
{
    public struct FaceApiKey
    {
        private readonly string _value;

        public FaceApiKey(string value)
        {
            this._value = value;
        }

        public static implicit operator string(FaceApiKey diskFileStoreRoot)
        {
            return diskFileStoreRoot._value;
        }
    }
}

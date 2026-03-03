namespace Theymes
{
    public class InitializeOptions
    {
        public string apiDomain { get; set; }
        public InitializeWebOptions web { get; set; }
        public InitializeAndroidOptions android { get; set; }
    }

    public class InitializeWebOptions
    {
        public string canvasSelector { get; set; }
        public string nonce { get; set; }
    }

    public class InitializeAndroidOptions
    {
        public int? orientation { get; set; }
    }
}

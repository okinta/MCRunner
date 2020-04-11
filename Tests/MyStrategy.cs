using PowerLanguage;
using PowerLanguage.Strategy;

namespace Tests
{
    class MyStrategy : SignalObject
    {
        [Input]
        public bool CreateCalled { get; set; } = false;

        [Input]
        public bool StartCalcCalled { get; set; } = false;

        [Input]
        public bool CalcBarCalled { get; set; } = false;

        public double CurrentHigh
        {
            get
            {
                return Bars.HighValue;
            }
        }

        public MyStrategy(object _ctx) : base(_ctx)
        {
        }

        protected override void Create()
        {
            CreateCalled = true;
        }

        protected override void StartCalc()
        {
            StartCalcCalled = true;
        }

        protected override void CalcBar()
        {
            CalcBarCalled = true;
        }
    }
}

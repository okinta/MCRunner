using PowerLanguage;
using System;
using System.Collections.Immutable;

namespace MCRunner.Instruments
{
    /// <summary>
    /// Represents the bars of a chart.
    /// </summary>
    public class Bars : IInstrument
    {
        /// <summary>
        /// Read-only property. Returns current bar number.
        /// </summary>
        public int CurrentBar
        {
            get
            {
                var index = bars.Count - 1;
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                return index;
            }
        }

        /// <summary>
        /// Read-only property. Provides an interface for accessing symbol information.
        /// </summary>
        public IInstrumentSettings Info => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an interface for accessing information reflected
        /// in Status Line.
        /// </summary>
        public IStatusLine StatusLine => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns series current bar's High value.
        /// </summary>
        public double HighValue
        {
            get
            {
                return bars[CurrentBar].High;
            }
        }

        /// <summary>
        /// Read-only property.Returns series current bar’s Low value.
        /// </summary>
        public double LowValue
        {
            get
            {
                return bars[CurrentBar].Low;
            }
        }

        /// <summary>
        /// Read-only property.Returns series current bar’s Open value.
        /// </summary>
        public double OpenValue
        {
            get
            {
                return bars[CurrentBar].Open;
            }
        }

        /// <summary>
        /// Read-only property.Returns series current bar’s Close value.
        /// </summary>
        public double CloseValue
        {
            get
            {
                return bars[CurrentBar].Close;
            }
        }

        /// <summary>
        /// Read-only property. Returns series current bar’s Volume value.
        /// </summary>
        public double VolumeValue
        {
            get
            {
                return bars[CurrentBar].Volume;
            }
        }

        /// <summary>
        /// Read-only property. Returns series current bar’s Ticks value.
        /// </summary>
        public double TicksValue => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns series current bar’s Up Ticks value.
        /// </summary>
        public double UpTicksValue => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns series current bar’s Down Ticks value.
        /// </summary>
        public double DownTicksValue => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns series current bar’s Open Interest value.
        /// </summary>
        public double OpenIntValue => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns current bar’s time.
        /// </summary>
        public DateTime TimeValue
        {
            get
            {
                return bars[CurrentBar].Time;
            }
        }

        /// <summary>
        /// Read-only property. Returns the last bar’s time.
        /// </summary>
        public DateTime LastBarTime
        {
            get
            {
                var lastBar = CurrentBar - 1;
                if (lastBar < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                return bars[lastBar].Time;
            }
        }

        /// <summary>
        /// Read-only property. Indicates whether current bar is the last bar on the chart.
        /// </summary>
        public bool LastBarOnChart => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Indicates whether current bar is the last bar in the
        /// SessionObject.
        /// </summary>
        public bool LastBarInSession => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns value of a point(pip).
        /// </summary>
        public double Point => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns current bar’s status: None = -1, Open = 0, Inside
        ///  = 1, Close = 2.
        /// </summary>
        public EBarState Status => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an interface for accessing Sessions information.
        /// </summary>
        public IROList<SessionObject> Sessions => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an interface for accessing any bar in the series.
        /// </summary>
        public ISeriesSymbolDataRand FullSymbolData => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an interface for accessing Depth Of Market data.
        /// </summary>
        public IDOMData DOM => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns the request for data describing the instrument.
        /// </summary>
        public InstrumentDataRequest Request => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns time of the last update of the current bar if Bar
        /// Magnifier mode is on. If Bar Magnifier mode is off, returns the time of the
        /// current bar (as the Time property).
        /// </summary>
        public DateTime BarUpdateTime => throw new NotImplementedException();

        /// <summary>
        /// Read-only property.
        /// This keyword can be used to distinguish between the bars with the same date and
        /// time stamps.
        /// For tick and volume-based charts: Returns the tick index within a second. For
        /// resolutions higher than 1 Tick returns the index of the last tick within the
        /// bar. For time-based charts with resolutions of 1 sec or more: Not supported.
        /// Returns 0.
        /// Real-time ticks stored in the data base are being assigned the last 31 bit -
        /// the identifier of realtime affiliation. In order to get the TickID value without
        /// real-time identifier one needs to calculate the remainder from TickID value
        /// division by 0x80000000, or execute an operation of bitwise "AND" with
        /// 0x7FFFFFFF value.
        /// uint _TrueTickID = Bars.TickIDValue % 0x80000000; or uint
        /// _TrueTickID = Bars.TickIDValue & 0x7FFFFFFF;
        /// TickIDValue value can be obtained for the current bar only.
        /// </summary>
        public uint TickIDValue => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an array for accessing DateTime bars, previous to
        /// the current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<DateTime> Time { get; protected set; }

        /// <summary>
        /// Read-only property. Provides an array for accessing High bars previous to the
        /// current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> High { get; protected set; }

        /// <summary>
        /// Read-only property. Provides an array for accessing Low bars, previous to the
        /// current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> Low { get; protected set; }

        /// <summary>
        /// Read-only property. Provides an array for accessing Open bars, previous to the
        /// current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> Open { get; protected set; }

        /// <summary>
        /// Read-only property. Provides an array for accessing Close bars, previous to the
        /// current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> Close { get; protected set; }

        /// <summary>
        /// Read-only property. Provides an array for accessing Volume bars, previous to
        /// the current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> Volume { get; protected set; }

        /// <summary>
        /// Read-only property. Provides an array for accessing Tick bars,previous to the
        /// current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> Ticks => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an array for accessing UpTick bars, previous to
        /// the current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> UpTicks => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an array for accessing DownTick bars, previous to
        /// the current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> DownTicks => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Provides an array for accessing Open Interest bars, previous
        /// to the current one. The array index is number of bars back (Bars ago).
        /// </summary>
        public ISeries<double> OpenInt => throw new NotImplementedException();

        /// <summary>
        /// Holds Bar data in a thread-safe manner.
        /// </summary>
        protected ImmutableList<Bar> bars = ImmutableList<Bar>.Empty;

        /// <summary>
        /// Instantiates the instance.
        /// </summary>
        public Bars()
        {
            ReloadBarSeries();
        }

        /// <summary>
        /// Converts this instance into a datastream item that can be loaded into
        /// StrategyRunner.
        /// </summary>
        /// <returns>A ImmutableList datastream with this Bars instance as its
        /// single source of data.</returns>
        public ImmutableList<IInstrument> ToSingleDataStream()
        {
            return ImmutableList<IInstrument>.Empty.Add(this);
        }

        /// <summary>
        /// Reloads all the BarSeries. Must be called whenever the underlying bars data
        /// is updated.
        /// </summary>
        protected void ReloadBarSeries()
        {
            Close = new BarSeries<double>(bars, "Close");
            High = new BarSeries<double>(bars, "High");
            Low = new BarSeries<double>(bars, "Low");
            Open = new BarSeries<double>(bars, "Open");
            Time = new BarSeries<DateTime>(bars, "Time");
            Volume = new BarSeries<double>(bars, "Volume");
        }
    }
}
